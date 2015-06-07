using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;

namespace Company.VisualStudioHaskell.Options
{
    [ComVisible(true)]
    class HaskellOptionsPage : DialogPage
    {
        private readonly string _category;
        private const string _optionsKey = "Options";

        internal HaskellOptionsPage(string category)
        {
            _category = category;
        }

        internal Service HsService
        {
            get
            {
                return ((IServiceProvider)Site).GetHaskellService();
            }
        }

        internal IComponentModel ComponentModel
        {
            get
            {
                return ((IServiceProvider)Site).GetComponentModel();
            }
        }
    }
}
