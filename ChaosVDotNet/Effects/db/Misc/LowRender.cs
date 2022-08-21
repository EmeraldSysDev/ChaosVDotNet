using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class LowRender : Effect
    {
        public LowRender() : base("Low Render", EffectType.Misc, true, false)
        {
            OnTick += _OnTick;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Function.Call(Hash.OVERRIDE_LODSCALE_THIS_FRAME, .04f);

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (!ped.IsPlayer && !Function.Call<bool>(Hash.IS_ENTITY_A_MISSION_ENTITY, ped))
                {
                    Function.Call(Hash.FORCE_PED_MOTION_STATE, ped, 0xbac0f10b, 0, 0, 0);
                }
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            Function.Call(Hash.OVERRIDE_LODSCALE_THIS_FRAME, 1.0f);
        }
    }
}
