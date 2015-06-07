using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudioTools;

namespace Company.VisualStudioHaskell
{
    static class Extensions
    {
        public static bool StartsWith(this StringBuilder builder, string value)
        {
            int i;
            for (i = 0; i < builder.Length && i < value.Length; i++)
            {
                if (builder[i] != value[i])
                {
                    return false;
                }
            }

            return i == value.Length;
        }

        public static bool EndsWith(this StringBuilder builder, string value)
        {
            if(builder.Length < value.Length)
                return false;

            for(int i = builder.Length - value.Length, j = 0; i + j < builder.Length; j++)
            {
                if (builder[i + j] != value[j])
                {
                    return false;
                }
            }

            return true;
        }

        internal static IComponentModel GetComponentModel(this IServiceProvider provider)
        {
            if (provider == null)
            {
                return null;
            }
            return (IComponentModel)provider.GetService(typeof(SComponentModel));
        }

        internal static IContentType GetHaskellContentType(this IServiceProvider provider)
        {
            return provider.GetComponentModel().GetService<IContentTypeRegistryService>().GetContentType(CoreConstants.ContentType);
        }

        internal static Service GetHaskellService(this IServiceProvider provider)
        {
            if (provider == null)
            {
                return null;
            }
            return (Service)provider.GetService(typeof(Service));
        }

        internal static IUIThread GetUIThread(this IServiceProvider provider)
        {
            var uiThread = (IUIThread) provider.GetService(typeof(IUIThread));
            if (uiThread == null)
            {
                // TODO: return NoOpUIThread
                Debug.Assert(VsShellUtilities.ShellIsShuttingDown);
                return null;
            }
            return uiThread;
        }
    }
}
