using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;

namespace ChaosVDotNet.Effects
{
    public class EffectUtil
    {
        public static bool IsVehicleBraking(Vehicle veh)
        {
            if (veh != null && veh.Exists())
            {
                return veh.BrakePower > 0.0f;
            }

            return false;
        }

        public static double NextInRange(Random rand, double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }
    }
}
