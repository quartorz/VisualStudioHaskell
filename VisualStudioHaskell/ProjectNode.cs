using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudioTools.Project;

namespace Company.VisualStudioHaskell
{
    class ProjectNode : CommonProjectNode
    {
        private VisualStudioHaskellPackage _package;

        public ProjectNode(VisualStudioHaskellPackage package)
            : base(package, Utilities.GetImageList(typeof(ProjectNode).Assembly.GetManifestResourceStream(Constants.ProjectImageList)))
        {
            _package = package;
        }

        protected override NodeProperties CreatePropertiesObject()
        {
            return new ProjectNodeProperties(this);
        }

        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            return new[]{
                typeof(Project.TestPropertyPage).GUID
            };
        }

        #region Abstract property implementation (incomplete)

        public override Guid ProjectGuid
        {
            get { return GuidList.guidVisualStudioHaskellProjectFactory; }
        }
        public override string ProjectType
        {
            get { return "Haskell Project"; }
        }
        internal override string IssueTrackerUrl
        {
            get { return ""; }
        }
        protected override Stream ProjectIconsImageStripStream
        {
            get { return typeof(ProjectNode).Assembly.GetManifestResourceStream("Company.VisualStudioHaskell.Project.Resources.imagelis.bmp"); }
        }

        #endregion

        #region Abstract method implementation (incomplete)

        public override Type GetProjectFactoryType()
        {
            return typeof(ProjectFactory);
        }

        public override Type GetEditorFactoryType()
        {
            return typeof(Editor.EditorFactory);
        }

        public override string GetProjectName()
        {
            return "";
        }

        public override string GetFormatList()
        {
            return "";
        }

        public override Type GetGeneralPropertyPageType()
        {
            return null;
        }

        public override Type GetLibraryManagerType()
        {
            return typeof(Navigation.HaskellLibraryManager);
        }

        public override IProjectLauncher/*!*/ GetLauncher()
        {
            return null;
        }

        #endregion
    }
}
