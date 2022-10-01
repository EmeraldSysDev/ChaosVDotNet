using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class BrakeBoosting : Effect
    {
        private static Model blimpModel;

        public BrakeBoosting() : base("effect_brakeboost", "Brake Boosting", EffectType.Vehicle, true, false)
        {
            blimpModel = new Model(VehicleHash.Blimp);
            OnTick += _OnTick;
        }

        public void _OnTick(object sender, EventArgs e)
        {
            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                Model vehModel = veh.Model;
                VehicleClass vehClass = veh.ClassType;
                if (vehClass != VehicleClass.Helicopters && vehModel != blimpModel)
                {
                    // Is the vehicle occupied?
                    if (!veh.IsSeatFree(VehicleSeat.Driver))
                    {
                        // Does the driver exist?
                        Ped driver = veh.GetPedOnSeat(VehicleSeat.Driver);
                        if (driver != null && driver.Exists())
                        {
                            // Is the driver alive?
                            if (driver.IsAlive)
                            {
                                bool vehBraking = EffectUtil.IsVehicleBraking(veh); // Vehicle has brake power before proceeding
                                if (vehBraking)
                                {
                                    Function.Call(Hash.APPLY_FORCE_TO_ENTITY, veh, 0, 0.0f, 50.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0, true, true, true, false, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
