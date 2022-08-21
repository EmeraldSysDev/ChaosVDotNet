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
        public enum EffectType
        {
            Test = -1,
            Player,
            Ped,
            Vehicle,
            Misc
        }

        public string EffectName { get; }
        public EffectType Type { get; }
        private bool Continuous = false;
        private bool Running = false;

        public event EventHandler OnStart;
        public event EventHandler OnStop;
        public event EventHandler OnTick;

        public Effect(string Name, EffectType Type, bool isContinuous = false, bool isRunning = true)
        {
            EffectName = Name;
            this.Type = Type;
            Continuous = isContinuous;
            Running = isRunning;

            Debug.WriteLine("Thread start!");
            if (isContinuous)
            {
                Tick += (s, e) =>
                {
                    if (Running)
                    {
                        OnTick?.Invoke(this, EventArgs.Empty);
                    }
                };
            }
        }

        public void Start()
        {
            if (!Running)
            {
                Running = true;
                OnStart?.Invoke(this, EventArgs.Empty);
                if (!Continuous) Stop();
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

        public bool IsContinuous()
        {
            return Continuous;
        }
    }
}
