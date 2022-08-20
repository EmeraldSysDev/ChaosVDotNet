using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Debug.WriteLine("Thread start!");
            Tick += (s, e) =>
            {
                if (Running)
                {
                    OnTick?.Invoke(this, EventArgs.Empty);
                }
            };
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
    }
}
