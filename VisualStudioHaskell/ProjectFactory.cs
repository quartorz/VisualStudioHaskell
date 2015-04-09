﻿using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Company.VisualStudioHaskell
{
    [Guid(GuidList.guidVisualStudioHaskellProjectFactoryString)]
    class ProjectFactory : Microsoft.VisualStudio.Project.ProjectFactory
    {
        private VisualStudioHaskellPackage _package;

        public ProjectFactory(VisualStudioHaskellPackage package) :
            base(package)
        {
            Console.WriteLine("TestTestTest!");
            _package = package;
        }
        protected override Microsoft.VisualStudio.Project.ProjectNode CreateProject()
        {
            var project = new ProjectNode(_package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)_package).GetService(typeof(IOleServiceProvider)));
            return project;
        }
    }
}
