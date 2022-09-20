# ChaosVDotNet

A replica of Chaos Mod V but in .NET.
<a name="readme-top"></a>

<!-- NEW EFFECTS -->
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

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Any contributions you make are **greatly appreciated**.

If you have an effect to add that is chaotic enough to be included or any bugfixes to patch, please fork the repository and create a pull request. You can also simply open an issue with the tag "enhancement". If you have found a bug, you can simply open an issue with the tag "bug".
Don't forget to give this project a star. Thanks!

1. Fork the project
2. Create your feature branch (`git checkout -b patch-1`)
3. Commit your changes to the feature branch (`git commit -m 'Add some code'`)
4. Push to the feature branch (`git push origin patch-1`)
5. Open a PR

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
## Roadmap

- [x] Effect manager for loading and unloading effects
- [ ] All effects ported from original ChaosModV
- [ ] Greatly reduce lag when loading effects

See the [open issues](https://github.com/EmeraldSysDev/ChaosVDotNet/issues) for a full list of proposed features (and known issues).

<!-- LICENSE -->
## License

Copyright (c) 2022 Ryan Omasta - ryand@emeraldsys.xyz
All rights reserved.

Distributed under the GPL-3.0 License (GNU General Public License v3.0). See `LICENSE` for more information. All files except for effect files will have a header explaining `LICENSE` information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>