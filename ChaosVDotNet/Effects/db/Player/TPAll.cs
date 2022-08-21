using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;
using GTA.Math;

namespace ChaosVDotNet.Effects.db.Player
{
    internal class TPAll : Effect
    {
        public TPAll() : base("Teleport Everything to Player", EffectType.Player, false, false)
        {
            OnStart += _OnStart;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            List<Entity> entities = new List<Entity>();
            int maxEntities = 20;

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (maxEntities == 10)
                {
                    break;
                }

                maxEntities--;

                if (!ped.IsPlayer && !Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, ped))
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, ped, true, true);
                    entities.Add(ped);
                }
            }

            Ped playerPed = Game.Player.Character;

            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                if (maxEntities == 0)
                {
                    break;
                }

                maxEntities--;

                if ((!playerPed.IsInVehicle() || veh != playerPed.CurrentVehicle) && !Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, veh))
                {
                    Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, veh, true, true);
                    entities.Add(veh);
                }
            }

            Vector3 playerPos = playerPed.Position;

            foreach (Entity entity in entities)
            {
                Function.Call(Hash.SET_ENTITY_COORDS, entity, playerPos.X, playerPos.Y, playerPos.Z, false, false, false, false);
            }

            Wait(0);

            foreach (Entity entity in entities)
            {
                Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, entity, false, false);
                entity.MarkAsNoLongerNeeded();
            }
        }
    }
}
