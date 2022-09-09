using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class DrivebyPlayer : Effect
    {
        private readonly List<Ped> Peds = new List<Ped>();
        public DrivebyPlayer() : base("Peds Drive By Player", EffectType.Ped, true, false)
        {
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Ped playerPed = Game.Player.Character;
            WeaponHash weaponHash = WeaponHash.MachinePistol;

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (!Peds.Contains(ped))
                {
                    if (!ped.IsPlayer && ped.IsInVehicle())
                    {
                        ped.BlockPermanentEvents = true;

                        ped.Weapons.Give(weaponHash, 9999, true, true);
                        Function.Call(Hash.TASK_DRIVE_BY, ped, playerPed, 0, 0, 0, 0, (float)-1, 5, false, 0xC6EE6B4C);
                        Peds.Add(ped);
                    }
                }
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            foreach (Ped ped in Peds)
            {
                if (ped.Exists())
                {
                    ped.BlockPermanentEvents = false;

                    ped.Task.ClearAllImmediately();
                    ped.Weapons.RemoveAll();
                }
            }

            Peds.Clear();
        }
    }
}
