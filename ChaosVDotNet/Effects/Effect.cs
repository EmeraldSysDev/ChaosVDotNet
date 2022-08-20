using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;

namespace ChaosVDotNet.Effects
{
    internal class Effect
    {
        public string Name { get; }
        private bool Running = false;

        public event EventHandler OnStart;
        public event EventHandler OnStop;
        public event EventHandler OnTick;
        public Effect(string Name, bool isRunning = true)
        {
            this.Name = Name;
            Running = isRunning;
            Thread();
        }

        public void Start()
        {
            if (!Running)
            {
                Running = true;
                OnStart?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                OnStop?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void Thread()
        {
            while (Running)
            {
                OnTick?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
