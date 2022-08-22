using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosVDotNet.Effects
{
    public class UnloadArgs
    {
        public Effect Unloaded { get; }

        internal UnloadArgs(Effect unloaded)
        {
            Unloaded = unloaded;
        }
    }
}
