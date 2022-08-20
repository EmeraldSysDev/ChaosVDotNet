using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosVDotNet.Effects
{
    internal class Effect
    {
        public string Name { get; }
        private bool Running = false;
        public Effect(string Name, bool isRunning = true)
        {
            this.Name = Name;
            Running = isRunning;
        }

        public void Stop()
        {
            Running = false;
        }
    }
}
