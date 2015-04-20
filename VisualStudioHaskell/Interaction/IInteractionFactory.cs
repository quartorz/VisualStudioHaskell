﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Repl;
using Microsoft.VisualStudioTools;

namespace Company.VisualStudioHaskell.Interaction
{
    interface IInteractionFactory : IDisposable
    {
        string Description
        {
            get;
        }

        InteractionConfiguration Configuration
        {
            get;
        }

        IInteraction CreateInterpreter();
    }
}
