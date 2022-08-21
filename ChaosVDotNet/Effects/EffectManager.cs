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
using System.Text;
using System.Threading.Tasks;

using GTA;

namespace ChaosVDotNet.Effects
{
    [ScriptAttributes(NoDefaultInstance = true)]
    public class EffectManager : Script
    {
        private readonly List<Effect> Loaded = new List<Effect>();
        public EffectManager() { }

        /// <summary>
        /// Load <paramref name="eff"/> into the <see cref="EffectManager"/>.
        /// </summary>
        public void Load(Effect eff)
        {
            Effect loadedEff = (Effect)InstantiateScript(eff.GetType());
            Loaded.Add(loadedEff);
        }

        /// <summary>
        /// Load every <see cref="Effect"/> into the <see cref="EffectManager"/>.
        /// </summary>
        public List<Effect> Load()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Effect))).ToArray();
            foreach (Type t in types)
            {
                Effect eff = (Effect)InstantiateScript(t);
                Loaded.Add(eff);
            }

            return Loaded;
        }

        /// <summary>
        /// Unload every <see cref="Effect"/> in the <see cref="EffectManager"/>. (WIP)
        /// </summary>
        public void UnloadAll() { }

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
    }
}
