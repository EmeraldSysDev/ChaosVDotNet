/*
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
        private readonly ObjectPool pool = new ObjectPool();
        private NativeMenu mainMenu;
        private readonly Dictionary<EffectType, NativeMenu> typeMenus = new Dictionary<EffectType, NativeMenu>();
        private NativeMenu debugMenu;

        private UpdateManager updateManager = InstantiateScript<UpdateManager>();
        private EffectManager effectManager = InstantiateScript<EffectManager>();
        public Main()
        {
            Tick += MainTick;
            KeyDown += OnKeyDown;
            effectManager.OnLoad += OnLoad;
            effectManager.OnUnload += OnUnload;
            Thread();
        }

        protected void MainTick(object sender, EventArgs e)
        {
            pool.Process();
        }

        protected void DebugNotif(string msg)
        {
            GTA.UI.Notification.Show($"~h~[ChaosVDotNet/Debug]~s~ {msg}");
        }

        protected void Thread()
        {
            mainMenu = new NativeMenu("ChaosVDotNet");
            pool.Add(mainMenu);

            string[] types = Enum.GetNames(typeof(EffectType));

            foreach (string type in types)
            {
                EffectType parsed = Util.ParseEnum<EffectType>(type);

                if (parsed != EffectType.Test)
                {
                    NativeMenu typeMenu = new NativeMenu(type);
                    mainMenu.AddSubMenu(typeMenu).Title = typeMenu.Title.Text;
                    pool.Add(typeMenu);

                    typeMenus.Add(parsed, typeMenu);
                }
            }

            effectManager.Load();

            debugMenu = new NativeMenu("Debug");
            mainMenu.AddSubMenu(debugMenu).Title = "Debug";
            pool.Add(debugMenu);

            NativeMenu debugVehicleMenu = new NativeMenu("Vehicle");
            debugMenu.AddSubMenu(debugVehicleMenu).Title = "Vehicle";
            pool.Add(debugVehicleMenu);

            NativeItem debugVehBrakeForce = new NativeItem("Brake Force");
            debugVehBrakeForce.Activated += (s, e) =>
            {
                Player player = Game.Player;
                Ped playerPed = player.Character;
                if (playerPed != null)
                {
                    if (playerPed.IsInVehicle())
                    {
                        Vehicle veh = playerPed.CurrentVehicle;
                        HandlingData handlingData = veh.HandlingData;
                        if (handlingData != null)
                        {
                            DebugNotif(handlingData.BrakeForce.ToString());
                        }
                    }
                }
            };
            debugVehicleMenu.Add(debugVehBrakeForce);

            NativeItem debugVehHBrakeForce = new NativeItem("Handbrake Force");
            debugVehHBrakeForce.Activated += (s, e) =>
            {
                Player player = Game.Player;
                Ped playerPed = player.Character;
                if (playerPed != null)
                {
                    if (playerPed.IsInVehicle())
                    {
                        Vehicle veh = playerPed.CurrentVehicle;
                        HandlingData handlingData = veh.HandlingData;
                        if (handlingData != null)
                        {
                            DebugNotif(handlingData.HandBrakeForce.ToString());
                        }
                    }
                }
            };
            debugVehicleMenu.Add(debugVehHBrakeForce);

            NativeItem debugLoad = new NativeItem("Load All");
            debugLoad.Activated += (s, e) =>
            {
                effectManager.Load();
            };
            debugMenu.Add(debugLoad);

            NativeItem debugUnload = new NativeItem("Unload All");
            debugUnload.Activated += (s, e) =>
            {
                effectManager.Unload();
            };
            debugMenu.Add(debugUnload);

            NativeItem debugCheckUpdates = new NativeItem("Check for Updates");
            debugCheckUpdates.Activated += (s, e) =>
            {
                Version loaded = updateManager.GetLoadedVersion();
                ReleaseModel latest = updateManager.GetLatestInfo();

                if (loaded < latest.Tag)
                    GTA.UI.Notification.Show($"~h~[ChaosVDotNet]~s~ A new version is available! You are running {loaded}, the latest available version is {latest.Tag}.");
                else if (loaded > latest.Tag)
                    GTA.UI.Notification.Show("~h~[ChaosVDotNet]~s~ You are running a version that has not yet been released to the public or has been built manually.");
                else
                    GTA.UI.Notification.Show("~h~[ChaosVDotNet]~s~ No new version is available.");
            };
            debugMenu.Add(debugCheckUpdates);
        }

        private void OnLoad(object sender, LoadArgs e)
        {
            Effect eff = e.Loaded;
            if (eff.Type != EffectType.Test)
            {
                if (typeMenus.ContainsKey(eff.Type))
                {
                    NativeMenu menu = typeMenus[eff.Type];
                    if (menu != null)
                    {
                        if (eff.IsContinuous())
                        {
                            NativeCheckboxItem effectCheckbox = new NativeCheckboxItem(eff.Name, eff.isRunning());
                            effectCheckbox.CheckboxChanged += (s, e2) =>
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
                            menu.Add(effectCheckbox);
                        }
                        else
                        {
                            NativeItem effectItem = new NativeItem(eff.Name);
                            effectItem.Activated += (s, e2) =>
                            {
                                eff.Start();
                            };
                            menu.Add(effectItem);
                        }
                    }
                }
            }
        }

        private NativeItem GetItemWithTitle(NativeMenu menu, string title)
        {
            NativeItem result = null;

            foreach (NativeItem item in menu.Items)
            {
                if (item.Title == title)
                {
                    result = item;
                    break;
                }
            }

            return result;
        }

        private void RemoveItemWithTitle(NativeMenu menu, string title)
        {
            NativeItem item = GetItemWithTitle(menu, title);
            if (item != null)
            {
                menu.Remove(item);
            }
        }

        private void OnUnload(object sender, UnloadArgs e)
        {
            Effect eff = e.Unloaded;
            if (eff.Type != EffectType.Test)
            {
                if (typeMenus.ContainsKey(eff.Type))
                {
                    NativeMenu menu = typeMenus[eff.Type];
                    if (menu != null)
                    {
                        RemoveItemWithTitle(menu, eff.Name);
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
