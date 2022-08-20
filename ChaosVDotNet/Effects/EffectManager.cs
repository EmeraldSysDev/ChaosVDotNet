using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChaosVDotNet.Effects
{
    public class EffectManager
    {
        public EffectManager()
        {

        }

        public void LoadEffects()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Effect))).ToArray();
            foreach (Type t in types)
            {
                object eff = Activator.CreateInstance(t);
            }
        }
    }
}
