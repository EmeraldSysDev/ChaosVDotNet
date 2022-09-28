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
        public string Id { get; }
        public string Name { get; }
        public EffectType Type { get; }
        private bool Continuous = false;
        private bool Running = false;

        // Internal events
        internal event EventHandler OnStart;
        internal event EventHandler OnStop;
        internal event EventHandler OnTick;

        // Logging events
        public event LogEventHandler OnLog;

        public Effect(string Id, string Name, EffectType Type, bool isContinuous = false, bool isRunning = true)
        {
            this.Id = Id;
            this.Name = Name;
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

        public void Log(string msg)
        {
            OnLog?.Invoke(this, new LogArgs(msg));
        }

        public void Log(string msg, LogVerbosity level)
        {
            OnLog?.Invoke(this, new LogArgs(msg, level));
        }

        public void Start()
        {
            if (!Running)
            {
                Running = true;
                OnStart?.Invoke(this, EventArgs.Empty);
                Log("Started");
                if (!Continuous) Stop();
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
                OnStop?.Invoke(this, EventArgs.Empty);
                Log("Stopped");
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
