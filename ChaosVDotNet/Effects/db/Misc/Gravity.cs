using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class Gravity
    {
        private static void _OnStop(object sender, EventArgs e)
        {
            World.GravityLevel = 9.8f;
        }

        internal class VeryLowGravity : Effect
        {
            public VeryLowGravity() : base("effect_gravity_low", "Very Low Gravity", EffectType.Misc, true, false)
            {
                OnTick += _OnTick;
                OnStop += _OnStop;
            }

            private void _OnTick(object sender, EventArgs e)
            {
                World.GravityLevel = 0.1f;
            }
        }

        internal class InsaneGravity : Effect
        {
            public InsaneGravity() : base("effect_gravity_insane", "Insane Gravity", EffectType.Misc, true, false)
            {
                OnTick += _OnTick;
                OnStop += _OnStop;
            }

            private void _OnTick(object sender, EventArgs e)
            {
                World.GravityLevel = 200.0f;

                Ped[] peds = World.GetAllPeds();
                foreach (Ped ped in peds)
                {
                    if (!ped.IsInVehicle())
                    {
                        Function.Call(Hash.SET_PED_TO_RAGDOLL, ped, 1000, 1000, 0, true, true, false);
                        Function.Call(Hash.APPLY_FORCE_TO_ENTITY_CENTER_OF_MASS, ped, 0, 0, 0, -75.0f, false, false, true, false);
                    }
                }

                Prop[] props = World.GetAllProps();
                foreach (Prop prop in props)
                {
                    Function.Call(Hash.APPLY_FORCE_TO_ENTITY_CENTER_OF_MASS, prop, 0, 0, 0, -200.0f, false, false, true, false);
                }
            }
        }
    }
}
