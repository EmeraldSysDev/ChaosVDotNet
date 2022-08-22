using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;

namespace ChaosVDotNet.Effects.db
{
    internal class FlipAll : Effect
    {
        private static readonly Random Rand = new Random();

        public FlipAll() : base("Flip All Vehicles", EffectType.Vehicle, false, false)
        {
            OnStart += _OnStart;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                Vector3 vel = veh.Velocity;
                Vector3 rot = veh.Rotation;

                if (Rand.NextDouble() >= 0.5)
                {
                    Vector3 rotNew;

                    if (rot.X < 180.0f)
                    {
                        rotNew = new Vector3(rot.X + 180.0f, rot.Y, rot.Z);
                    }
                    else
                    {
                        rotNew = new Vector3(rot.X - 180.0f, rot.Y, rot.Z);
                    }

                    veh.Rotation = rotNew;
                }
                else
                {
                    Vector3 rotNew;

                    if (rot.Y < 180.0f)
                    {
                        rotNew = new Vector3(rot.X, rot.Y + 180.0f, rot.Z);
                    }
                    else
                    {
                        rotNew = new Vector3(rot.X, rot.Y - 180.0f, rot.Z);
                    }

                    veh.Rotation = rotNew;
                }

                veh.Velocity = vel;
            }
        }
    }
}
