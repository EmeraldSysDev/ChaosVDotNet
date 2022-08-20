using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GTA;

namespace ChaosVDotNet.Effects
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class Effect : Script
    {
        public string EffectName { get; }
        private bool Running = false;

        public event EventHandler OnStart;
        public event EventHandler OnStop;
        public event EventHandler OnTick;
        public Effect(string Name, bool isRunning = true)
        {
            EffectName = Name;
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

        public bool isRunning()
        {
            return Running;
        }

        protected void Thread()
        {
            while (Running)
            {
                OnTick?.Invoke(this, EventArgs.Empty);
                Wait(500);
            }
        }
    }
}
