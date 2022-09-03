using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class Earthquake : Effect
    {
        public Earthquake() : base("Earthquake", EffectType.Misc, true, false)
        {
            OnStop += _OnStop;
            OnTick += _OnTick;
        }

        private void _OnStop(object sender, EventArgs e)
        {
            GameplayCamera.StopShaking();
        }

        private void _OnTick(object sender, EventArgs e)
        {
            GameplayCamera.Shake(CameraShake.LargeExplosion, 0.05f);
            float shook = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, -9.0f, 7.0f);

            List<Entity> entities = new List<Entity>();

            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                Model model = veh.Model;

                if (!model.IsHelicopter && !model.IsPlane)
                {
                    entities.Add(veh);
                }
            }

            Prop[] props = World.GetAllProps();
            foreach (Prop prop in props)
            {
                entities.Add(prop);
            }

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (!ped.IsPlayer)
                {
                    entities.Add(ped);
                }
            }

            foreach (Entity entity in entities)
            {
                Function.Call(Hash.APPLY_FORCE_TO_ENTITY, entity, 1, 0, 0, shook, .0f, .0f, .0f, 0, true, true, true, false, true);
            }
        }
    }
}
