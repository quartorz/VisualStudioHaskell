using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudioTools.Project;
using System.Windows.Forms;

namespace Company.VisualStudioHaskell.Project
{
    [Guid(GuidList.guidVisualStudioHaskellTestProperyPageString)]
    class TestPropertyPage : CommonPropertyPage
    {
        private readonly TestPropertyControl _control;

        public TestPropertyPage() {
            _control = new TestPropertyControl();
        }

        public override Control Control {
            get { return _control; }
        }

        public override void Apply()
        {
        }

        public override void LoadSettings()
        {
        }

        public override string Name
        {
            get { return "Test"; }
        }
    }
}
