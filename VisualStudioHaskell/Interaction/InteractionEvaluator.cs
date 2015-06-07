using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Repl;
using Company.VisualStudioHaskell.Interpreter;

namespace Company.VisualStudioHaskell.Interaction
{
    class InteractionEvaluator : IReplEvaluator
    {
        private IServiceProvider _serviceProvider;
        private IInterpreterFactory _interpreter;
        private IInterpreterOptionsService _interpreterService;
        private CommandProcessorThread _curListener;
        internal IReplWindow _window;

        public IInterpreterFactory Interpreter
        {
            get { return _interpreter; }
        }

        internal string DisplayName
        {
            get { return Interpreter != null ? Interpreter.Description : ""; }
        }
        

        public InteractionEvaluator(IInterpreterFactory interpreter, IServiceProvider serviceProvider, IInterpreterOptionsService interpreterService)
        {
            _interpreter = interpreter;
            _serviceProvider = serviceProvider;
            _interpreterService = interpreterService;
        }

        internal void EnsureConnected()
        {
            if (_curListener == null)
            {
                _serviceProvider.GetUIThread().Invoke(() =>
                {
                    if (_curListener == null)
                    {
                        Connect();
                    }
                });
            }
        }

        private void Connect()
        {
            // TODO: get ghci path from general options
            var processInfo = new ProcessStartInfo("ghci");

            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardInput = true;

            // TODO: read options

            var process = new Process();

            process.StartInfo = processInfo;
            process.Start();

            _curListener = new CommandProcessorThread(this, true, process);
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            if (_curListener != null)
            {
                _curListener.Dispose();
                _curListener = null;
            }
        }

        public Task<ExecutionResult> Reset()
        {
            Close();
            return ExecutionResult.Succeeded;
        }

        public Task<ExecutionResult> Initialize(IReplWindow window)
        {
            _window = window;
            Connect();
            return ExecutionResult.Succeeded;
        }

        public void AbortCommand()
        {
        }

        public void ActiveLanguageBufferChanged(ITextBuffer currentBuffer, ITextBuffer previousBuffer)
        {
        }

        public bool CanExecuteText(string text)
        {
            // TODO: check if text is executable
            return true;
        }

        public void ExecuteFile(string filename)
        {
        }

        public Task<ExecutionResult> ExecuteText(string text)
        {
            EnsureConnected();
            if (_curListener != null)
            {
                return _curListener.ExecuteText(text);
            }
            else
            {
                _window.WriteError("Current interactive window is disconnected." + Environment.NewLine);
            }
            return ExecutionResult.Failed;
        }

        public string FormatClipboard()
        {
            return "";
        }
    }

    internal class CommandProcessorThread : IDisposable
    {
        private readonly InteractionEvaluator _eval;

        private TaskCompletionSource<ExecutionResult> _completion;
        private object _completionLock = new object();

        private Action _deferredExecute;

        private readonly Process _process;
        private AutoResetEvent _completionResultEvent = new AutoResetEvent(false);

        //internal string _prompt1 = "Prelude> ", _prompt = "Prelude| ";

        private InteractionStreamReader _stdOutReader;
        private InteractionStreamReader _stdErrReader;
        private Thread _stdOutThread;
        private Thread _stdErrThread;

        private IReplWindow Window
        {
            get { return _eval._window; }
        }


        public  CommandProcessorThread(InteractionEvaluator evaluator, bool redirectOutput, Process process)
        {
            _eval = evaluator;
            _process = process;

            StartOutputThread(redirectOutput);
        }

        public void Dispose()
        {
            if (_process != null && !_process.HasExited)
            {
                try
                {
                    _process.Kill();
                }
                catch (InvalidOperationException)
                {
                }
                catch (Win32Exception)
                {
                }
            }

            if (_stdOutReader != null)
            {
                _stdOutReader.RequestStop();
            }

            if (_stdErrReader != null)
            {
                _stdErrReader.RequestStop();
            }
        }

        public Task<ExecutionResult> ExecuteText(string text)
        {
            _process.StandardInput.WriteLine(text);
            return ExecutionResult.Succeeded;
        }

        private void StartOutputThread(bool redirectOutput)
        {
            if (redirectOutput)
            {
                // _process.OutputDataReceived += StdOutReceived;
                // _process.ErrorDataReceived += StdErrReceived;

                _stdOutReader = new InteractionStreamReader(_process.StandardOutput);
                _stdOutReader.DataReceived += StdOutReceived;
                _stdOutThread = _stdOutReader.CreateThread();

                _stdErrReader = new InteractionStreamReader(_process.StandardError);
                _stdErrReader.DataReceived += StdErrReceived;
                _stdErrThread = _stdErrReader.CreateThread();

                _stdOutThread.Start();
                _stdErrThread.Start();
            }
        }

        private void StdOutReceived(object sender, string e)
        {
            if (e != null)
            {
                Window.WriteOutput(e + Environment.NewLine);
            }
        }

        private void StdErrReceived(object sender, string e)
        {
            if (e != null)
            {
                Window.WriteError(e + Environment.NewLine);
            }
        }
    }
}
