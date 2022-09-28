/*
    ChaosVDotNet (EffectManager.cs)
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using GTA;

namespace ChaosVDotNet.Effects
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class EffectManager : Script
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        [DllImport("kernel32.dll", EntryPoint = "FreeConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int FreeConsole();

        private readonly List<Effect> Loaded = new List<Effect>();
        public event LoadEventHandler OnLoad;
        public event UnloadEventHandler OnUnload;

        public EffectManager()
        {
            AllocConsole();
            Log("Hello! I have been initialized!");

            Aborted += (s, e) =>
            {
                Log("Goodbye!");
                FreeConsole();
            };
        }

        internal void Log(string msg)
        {
            Console.WriteLine($"[ChaosVDotNet/EffectManager] {msg}");
        }

        internal void OnLog(Effect eff, LogArgs e)
        {
            Console.WriteLine($"[ChaosVDotNet/{eff.Id}/{e.Level.ToString().ToUpper()}] {e.Message}");
        }

        /// <summary>
        /// Load <paramref name="eff"/> into the <see cref="EffectManager"/>.
        /// </summary>
        public void Load(Effect eff)
        {
            Effect loadedEff = (Effect)InstantiateScript(eff.GetType());
            Loaded.Add(loadedEff);
            OnLoad?.Invoke(this, new LoadArgs(loadedEff));
            loadedEff.OnLog += OnLog;
        }

        /// <summary>
        /// Load every <see cref="Effect"/> into the <see cref="EffectManager"/>.
        /// </summary>
        public List<Effect> Load()
        {
            if (Loaded.Any())
            {
                Unload();
                Wait(1000);
            }

            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Effect))).ToArray();
            foreach (Type t in types)
            {
                DateTime startTime, endTime;
                startTime = DateTime.Now;
                Log($"Loading {t.Name}");
                Effect eff = (Effect)InstantiateScript(t);
                Loaded.Add(eff);
                OnLoad?.Invoke(this, new LoadArgs(eff));
                eff.OnLog += OnLog;
                endTime = DateTime.Now;
                double elapsed = (endTime - startTime).TotalMilliseconds;
                Log($"Loaded {t.Name} in {elapsed} ms");
            }

            return Loaded;
        }

        /// <summary>
        /// Unload <paramref name="eff"/> from the <see cref="EffectManager"/>.
        /// </summary>
        public void Unload(Effect eff)
        {
            foreach (Effect loadedEff in Loaded)
            {
                if (loadedEff == eff)
                {
                    loadedEff.Abort();
                    Loaded.Remove(loadedEff);
                    OnUnload?.Invoke(this, new UnloadArgs(loadedEff));
                    loadedEff.OnLog -= OnLog;
                    break;
                }
            }
        }

        /// <summary>
        /// Unload every <see cref="Effect"/> in the <see cref="EffectManager"/>.
        /// </summary>
        public void Unload()
        {
            foreach (Effect eff in Loaded)
            {
                eff.Abort();
                OnUnload?.Invoke(this, new UnloadArgs(eff));
                eff.OnLog -= OnLog;
            }
            Loaded.Clear();
        }

        /// <summary>
        /// Get every loaded <see cref="Effect"/> in the <see cref="EffectManager"/>.
        /// </summary>
        public List<Effect> GetLoaded()
        {
            return Loaded;
        }

        /// <summary>
        /// Checks if <paramref name="eff"/> is loaded in the <see cref="EffectManager"/>.
        /// </summary>
        /// <param name="eff"></param>
        /// <returns></returns>
        public bool IsLoaded(Effect eff)
        {
            bool Result = false;

            foreach (Effect loadedEff in Loaded)
            {
                if (loadedEff == eff)
                {
                    Result = true;
                    break;
                }
            }

            return Result;
        }

        /// <summary>
        /// Gets all <see cref="Effect"/>s that are running in the <see cref="EffectManager"/>.
        /// </summary>
        /// <returns></returns>
        public List<Effect> GetRunning()
        {
            List<Effect> Running = new List<Effect>();

            foreach (Effect eff in Loaded)
            {
                if (eff.isRunning())
                {
                    Running.Add(eff);
                }
            }

            return Running;
        }
    }
}
