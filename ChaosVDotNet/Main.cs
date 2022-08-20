using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GTA;
using LemonUI;
using LemonUI.Menus;

using ChaosVDotNet.Effects;

namespace ChaosVDotNet
{
    public class Main : Script
    {
        private ObjectPool pool = new ObjectPool();
        private NativeMenu mainMenu;

        private EffectManager effectManager = InstantiateScript<EffectManager>();
        public Main()
        {
            Tick += MainTick;
            KeyDown += OnKeyDown;
            Thread();
        }

        protected void MainTick(object sender, EventArgs e)
        {
            pool.Process();
        }

        protected void Thread()
        {
            mainMenu = new NativeMenu("ChaosVDotNet");
            pool.Add(mainMenu);

            List<Effect> loadedInit = effectManager.LoadAll();

            foreach (Effect eff in loadedInit)
            {
                NativeCheckboxItem effectCheckbox = new NativeCheckboxItem(eff.EffectName, eff.isRunning());
                effectCheckbox.CheckboxChanged += (s, e) =>
                {
                    Debug.WriteLine("Checkbox changed");
                    if (effectCheckbox.Checked)
                    {
                        Debug.WriteLine("Starting");
                        eff.Start();
                    }
                    else
                    {
                        Debug.WriteLine("Stopping");
                        eff.Stop();
                    }
                };
                mainMenu.Add(effectCheckbox);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.X && mainMenu != null && !mainMenu.Visible)
            {
                mainMenu.Visible = true;
            }
        }
    }
}
