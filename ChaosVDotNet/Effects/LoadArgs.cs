using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosVDotNet.Effects
{
    public class LoadArgs
    {
        public Effect Loaded { get; }

        internal LoadArgs(Effect loaded)
        {
            Loaded = loaded;
        }
    }
}
