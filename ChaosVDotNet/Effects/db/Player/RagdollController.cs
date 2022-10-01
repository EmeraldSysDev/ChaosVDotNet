using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class RagdollController
    {
        private static readonly Random Rand = new Random();

        internal class Ragdoll : Effect
        {
            public Ragdoll() : base("effect_ragdoll", "Ragdoll", EffectType.Player, false, false)
            {
                OnStart += _OnStart;
            }

            private void _OnStart(object sender, EventArgs e)
            {
                Player player = Game.Player;
                Ped playerPed = player.Character;

                playerPed.Task.ClearAllImmediately();

                Function.Call(Hash.SET_PED_TO_RAGDOLL, playerPed, 10000, 10000, 0, true, true, false);
            }
        }

        internal class FakeRagdoll : Effect
        {
            public FakeRagdoll() : base("effect_ragdoll_fake", "Fake Ragdoll", EffectType.Player, false, false)
            {
                OnStart += _OnStart;
            }

            private void _OnStart(object sender, EventArgs e)
            {
                Player player = Game.Player;
                Ped playerPed = player.Character;
                Vehicle playerVeh = playerPed.CurrentVehicle;

                int count = 500;

                if (--count > 0)
                {
                    Wait(0);
                }

                playerPed.Task.ClearAllImmediately();

                Function.Call(Hash.SET_PED_TO_RAGDOLL, playerPed, 45000, 45000, 0, true, true, false);

                // Delay teleport/cancel ragdoll
                //Wait(Rand.Next(5000, 15000));
                count = Rand.Next(5000, 15000);
                if (--count > 0)
                {
                    Wait(0);
                }

                Function.Call(Hash.SET_PED_TO_RAGDOLL, playerPed, 1, 1, 1, true, true, false);

                if (playerVeh != null && playerVeh.Exists())
                {
                    // Set ped back into vehicle
                    if (playerVeh.IsSeatFree(VehicleSeat.Driver))
                    {
                        playerPed.SetIntoVehicle(playerVeh, VehicleSeat.Driver);
                    }
                    else
                    {
                        playerPed.SetIntoVehicle(playerVeh, VehicleSeat.Any);
                    }
                }
            }
        }
    }
}
