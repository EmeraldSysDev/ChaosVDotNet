using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using LemonUI;
using LemonUI.Menus;

using ChaosVDotNet.Effects;

namespace ChaosVDotNet
{
    public class Main : Script
    {
        private ObjectPool pool = new ObjectPool();
        private EffectManager effectManager;
        public Main()
        {
            Tick += MainTick;
            effectManager = new EffectManager();
            Thread();
        }

        protected void MainTick(object sender, EventArgs e)
        {
            pool.Process();
        }

        protected void Thread()
        {
            NativeMenu menu = new NativeMenu("ChaosVDotNet");
            pool.Add(menu);

            
        }
    }
}
