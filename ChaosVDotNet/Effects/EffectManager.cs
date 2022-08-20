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

        public List<Effect> LoadAll()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Effect))).ToArray();
            foreach (Type t in types)
            {
                Effect eff = (Effect)InstantiateScript(t);
                Loaded.Add(eff);
            }

            return Loaded;
        }

        public List<Effect> GetLoaded()
        {
            return Loaded;
        }
    }
}
