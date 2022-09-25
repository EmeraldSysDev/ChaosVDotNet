using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class FullAccel : Effect
    {
        public FullAccel() : base("effect_fullaccel", "Full Acceleration", EffectType.Vehicle, true, false)
        {
            OnTick += _OnTick;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                Model vehModel = veh.Model;
                bool vehBraking = veh.BrakePower > 0.0f;

                if (!vehBraking && (veh.IsOnAllWheels || veh.IsPlane || veh.IsHelicopter))
                {
                    veh.ForwardSpeed = Function.Call<float>(Hash.GET_VEHICLE_MODEL_ESTIMATED_MAX_SPEED, vehModel) * 2;
                }
            }
        }
    }
}
