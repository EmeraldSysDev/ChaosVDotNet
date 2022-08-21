/*
    ChaosVDotNet (Effect.cs)
    Copyright (C) 2022 Ryan Omasta

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

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
