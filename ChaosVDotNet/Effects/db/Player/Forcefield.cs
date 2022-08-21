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
    internal class Forcefield : Effect
    {
        public Forcefield() : base("Forcefield", EffectType.Player, true, false)
        {
            OnTick += _OnTick;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Ped player = Game.Player.Character;
            List<Entity> entities = new List<Entity>();

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (ped != player)
                {
                    entities.Add(ped);
                }
            }

            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                if (!player.IsInVehicle(veh))
                {
                    entities.Add(veh);
                }
            }

            Prop[] props = World.GetAllProps();
            foreach (Prop prop in props)
            {
                entities.Add(prop);
            }

            Vector3 playerCoord = player.Position;
            foreach (Entity entity in entities)
            {
                const float startDist = 15;
                const float maxForceDist = 10;
                const float maxForce = 100;
                Vector3 entityCoord = entity.Position;
                float dist = World.GetDistance(playerCoord, entityCoord);

                if (dist < startDist)
                {
                    bool isPed = Function.Call<bool>(Hash.IS_ENTITY_A_PED, entity);
                    if (isPed)
                    {
                        Ped ped = (Ped)entity;
                        if (!ped.IsRagdoll)
                        {
                            Function.Call(Hash.SET_PED_TO_RAGDOLL, ped, 5000, 5000, 0, true, true, false);
                        }
                    }
                    float forceDist = Math.Min(Math.Max(0.0f, (startDist - dist)), maxForceDist);
                    float force = (forceDist / maxForceDist) * maxForce;
                    Vector3 direction = entityCoord - playerCoord;
                    entity.ApplyForce(direction, Vector3.Zero, ForceType.MaxForceRot2);
                }
            }
        }
    }
}
