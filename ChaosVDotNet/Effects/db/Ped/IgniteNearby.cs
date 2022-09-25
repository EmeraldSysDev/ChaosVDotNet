using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class IgniteNearby : Effect
    {
        public IgniteNearby() : base("effect_igniteall", "Ignite All Nearby", EffectType.Ped, false, false)
        {
            OnStart += _OnStart;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (!ped.IsPlayer)
                {
                    Function.Call(Hash.START_ENTITY_FIRE, ped);
                }
            }
        }
    }
}
