using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class PhoneRing : Effect
    {
        public PhoneRing() : base("Whose Phone Is Ringing?", EffectType.Ped, true, false)
        {
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                bool isRinging = Function.Call<bool>(Hash.IS_PED_RINGTONE_PLAYING, ped);
                if (!isRinging)
                {
                    Function.Call(Hash.PLAY_PED_RINGTONE, "Remote_Ring", ped, 1);
                }
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                Function.Call(Hash.STOP_PED_RINGTONE, ped);
            }
        }
    }
}
