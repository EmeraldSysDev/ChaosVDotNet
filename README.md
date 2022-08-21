# ChaosVDotNet

A replica of Chaos Mod V but in .NET.

## Adding new effects

You can easily integrate your own effects using class files.

1. Create a new .cs file in the appropriate folder under `ChaosVDotNet/Effects/db` with a fitting name

Layout of the file should look like this:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class Generic : Effect
    {
        public Generic() : base("Generic Effect", EffectType.Test, false, false)
        {
            OnStart += _OnStart;
            OnStop += _OnStop;
            OnTick += _OnTick;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            // Effect start code goes here
        }

        private void _OnStop(object sender, EventArgs e)
        {
            // Effect stop code goes here
        }

        private void _OnTick(object sender, EventArgs e)
        {
            // Effect tick code goes here
        }
    }
}
```

Your effect will automatically be loaded.