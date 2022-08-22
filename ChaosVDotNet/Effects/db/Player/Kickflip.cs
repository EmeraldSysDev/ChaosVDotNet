using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class Kickflip : Effect
    {
        public Kickflip() : base("Kickflip", EffectType.Player, false, false)
        {
            OnStart += _OnStart;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            Ped player = Game.Player.Character;
            Entity toFlip;

            if (player.IsInVehicle())
            {
                toFlip = player.CurrentVehicle;
            }
            else
            {
                toFlip = player;
                player.CanRagdoll = true;
                Function.Call(Hash.SET_PED_TO_RAGDOLL, player, 200, 0, 0, true, true, false);
            }

            Wait(0);
            if (toFlip != null)
            {
                Function.Call(Hash.APPLY_FORCE_TO_ENTITY, toFlip, 1, 0, 0, 10, 2, 0, 0, 0, true, true, true, false, true);
            }
        }
    }
}
