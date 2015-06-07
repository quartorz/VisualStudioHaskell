using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.VisualStudioHaskell.Options
{
    class GeneralOptionsPage : HaskellOptionsPage
    {
        private GeneralOptionsControl _window;

        public GeneralOptionsPage()
            : base("Geneal")
        {
        }

        protected override System.Windows.Forms.IWin32Window Window
        {
            get
            {
                if (_window == null)
                {
                    _window = new GeneralOptionsControl();
                    // TODO: call LoadSettingsFromStorage
                }
                return _window;
            }
        }

        // TODO: create ResetSettings, LoadSettingsFromStorage, and SaveSettingsToStorage
        // see Microsoft.PythonTools.Options.PythonGeneralOptionsPage
    }
}
