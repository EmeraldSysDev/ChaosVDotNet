using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class GravityField : Effect
    {
        public GravityField() : base("effect_gravfield", "Gravity Field", EffectType.Player, true, false)
        {
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Player player = Game.Player;
            Ped playerPed = player.Character;

            List<Entity> entities = new List<Entity>();

            //playerPed.IsInvincible = true;

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (ped != playerPed)
                {
                    entities.Add(ped);
                }
            }

            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                if (!playerPed.IsInVehicle(veh))
                {
                    entities.Add(veh);
                }
            }

            Prop[] props = World.GetAllProps();
            foreach (Prop prop in props)
            {
                entities.Add(prop);
            }

            Vector3 playerCoord = playerPed.Position;
            int count = 10;

            foreach (Entity entity in entities)
            {
                float startDist = 50;
                float maxForceDist = 1;
                float maxForce = 80;

                Vector3 entityCoord = entity.Position;

                float dist = World.GetDistance(playerCoord, entityCoord);

                if (dist < startDist)
                {
                    if (Function.Call<bool>(Hash.IS_ENTITY_A_PED, entity))
                    {
                        Ped ped = (Ped)entity;
                        if (!ped.IsRagdoll)
                        {
                            Function.Call(Hash.SET_PED_TO_RAGDOLL, ped, 5000, 5000, 0, true, true, false);
                        }
                    }

                    float forceDist = Math.Min(Math.Max(0.0f, (startDist - dist)), maxForceDist);
                    float force = (forceDist / maxForceDist) * maxForce;

                    float x = (entityCoord.X - playerCoord.X) * -1.0f;
                    float y = (entityCoord.Y - playerCoord.Y) * -1.0f;
                    float z = (entityCoord.Z - playerCoord.Z) * -1.0f;
                    Function.Call(Hash.APPLY_FORCE_TO_ENTITY, entity, 3, x, y, z, 0, 0, 0, false, false, true, true, false, true);

                    if (Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, entity))
                    {
                        entity.IsInvincible = true;
                    }

                    if (--count <= 0)
                    {
                        Wait(0);
                        count = 10;
                    }
                }
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            Player player = Game.Player;
            Ped playerPed = player.Character;

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (ped != playerPed)
                {
                    ped.IsInvincible = false;
                }
            }

            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                if (!playerPed.IsInVehicle(veh))
                {
                    veh.IsInvincible = false;
                }
            }
        }
    }
}
