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
