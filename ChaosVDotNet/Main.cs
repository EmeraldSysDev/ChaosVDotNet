﻿/*
    ChaosVDotNet (Main.cs)
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
        private readonly ObjectPool pool = new ObjectPool();
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

            List<Effect> loadedInit = effectManager.Load();

            foreach (Effect eff in loadedInit)
            {
                if (eff.Type != Effect.EffectType.Test)
                {
                    if (eff.IsContinuous())
                    {
                        NativeCheckboxItem effectCheckbox = new NativeCheckboxItem(eff.EffectName, eff.isRunning());
                        effectCheckbox.CheckboxChanged += (s, e) =>
                        {
                            if (effectCheckbox.Checked)
                            {
                                eff.Start();
                            }
                            else
                            {
                                eff.Stop();
                            }
                        };
                        mainMenu.Add(effectCheckbox);
                    }
                    else
                    {
                        NativeItem effectItem = new NativeItem(eff.EffectName);
                        effectItem.Activated += (s, e) =>
                        {
                            eff.Start();
                        };
                        mainMenu.Add(effectItem);
                    }
                }
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
