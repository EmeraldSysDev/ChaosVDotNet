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
    internal class Teleport
    {
        private static void PlayerTo(Vector3 coords, bool noOffset = false)
        {
            Ped playerPed = Game.Player.Character;

            bool isInVeh = playerPed.IsInVehicle();
            bool isInFlyingVeh = playerPed.IsInFlyingVehicle;

            Vehicle playerVeh = playerPed.CurrentVehicle;
            Vector3 vel = isInVeh ? playerVeh.Velocity : playerPed.Velocity;
            float heading = isInVeh ? Function.Call<float>(Hash.GET_ENTITY_HEADING, playerVeh) : Function.Call<float>(Hash.GET_ENTITY_HEADING, playerPed);
            float groundHeight = playerVeh.HeightAboveGround;
            float forwardSpeed = 0.0f;

            if (isInVeh)
            {
                forwardSpeed = playerVeh.Speed;
            }

            if (noOffset)
            {
                if (isInVeh)
                {
                    float z = isInFlyingVeh ? coords.Z + groundHeight : coords.Z;
                    Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, playerVeh, coords.X, coords.Y, z, false, false, false);
                }
                else
                {
                    Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, playerPed, coords.X, coords.Y, coords.Z, false, false, false);
                }
            }
            else
            {
                if (isInVeh)
                {
                    float z = isInFlyingVeh ? coords.Z + groundHeight : coords.Z;
                    Function.Call(Hash.SET_ENTITY_COORDS, playerVeh, coords.X, coords.Y, z, false, false, false, false);
                }
                else
                {
                    Function.Call(Hash.SET_ENTITY_COORDS, playerPed, coords.X, coords.Y, coords.Z, false, false, false, false);
                }
            }

            if (isInVeh)
            {
                playerVeh.Heading = heading;
                playerVeh.Velocity = vel;
                playerVeh.ForwardSpeed = forwardSpeed;
            }
            else
            {
                playerPed.Heading = heading;
                playerPed.Velocity = vel;
            }
        }

        internal class Waypoint : Effect
        {
            public Waypoint() : base("effect_tp_waypoint", "Teleport to Waypoint", EffectType.Player, false, false)
            {
                OnStart += _OnStart;
            }

            private void _OnStart(object sender, EventArgs e)
            {
                if (Function.Call<bool>(Hash.IS_WAYPOINT_ACTIVE))
                {
                    PlayerTo(World.WaypointPosition);
                }
            }
        }
    }
}
