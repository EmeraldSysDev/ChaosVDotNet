using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;

namespace ChaosVDotNet.Effects.db
{
    internal class TestBraking : Effect
    {
        public TestBraking() : base("effect_testbraking", "Test Braking", EffectType.Vehicle, true, false)
        {
            OnStart += _OnStart;
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            Debug.WriteLine("Started!");
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Player player = Game.Player;
            Ped playerPed = player.Character;

            if (playerPed.IsInVehicle())
            {
                Vehicle veh = playerPed.CurrentVehicle;
                bool vehBraking = EffectUtil.IsVehicleBraking(veh);
                GTA.UI.Notification.Show(vehBraking.ToString());
            }

            Wait(3000);
        }

        private void _OnStop(object sender, EventArgs e)
        {
            Debug.WriteLine("Stopped!");
        }
    }
}
