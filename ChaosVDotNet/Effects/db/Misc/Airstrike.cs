using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class Airstrike : Effect
    {
        private static readonly Random Rand = new Random();
        private static int lastAirStrike = 0;
        private static WeaponAsset airstrikeModel;

        public Airstrike() : base("Airstrike Inbound", EffectType.Misc, true, false)
        {
            OnStart += _OnStart;
            OnStop += _OnStop;
            OnTick += _OnTick;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            airstrikeModel = new WeaponAsset(Game.GenerateHash("WEAPON_AIRSTRIKE_ROCKET"));
        }

        private void _OnStop(object sender, EventArgs e)
        {
            airstrikeModel.MarkAsNoLongerNeeded();
        }

        private static Vector3 getRandomOffsetCoord(Vector3 startCoord, float max)
        {
            int randomX = Rand.Next((int)-max, (int)max);
            int randomY = Rand.Next((int)-max, (int)max);
            int randomZ = Rand.Next((int)-max, (int)max);

            return new Vector3(startCoord.X + randomX, startCoord.Y + randomY, startCoord.Z + randomZ);
        }

        private void _OnTick(object sender, EventArgs e)
        {
            airstrikeModel.Request(0);
            int currentTime = Game.GameTime;
            if ((currentTime - lastAirStrike) > 1000)
            {
                lastAirStrike = currentTime;
                Ped player = Game.Player.Character;
                Vector3 playerPos = player.Position;
                Vector3 startPos = getRandomOffsetCoord(playerPos, 10);
                Vector3 targetPos = getRandomOffsetCoord(playerPos, 50);

                float groundZ = 0;
                unsafe
                {
                    Function.Call(Hash.GET_GROUND_Z_FOR_3D_COORD, targetPos.X, targetPos.Y, targetPos.Z, &groundZ, false, false);
                }
                Yield();
                if (groundZ != 0)
                {
                    Vector3 startPosNew = new Vector3(startPos.X, startPos.Y, startPos.Z + 200);
                    Vector3 targetPosNew = new Vector3(targetPos.X, targetPos.Y, groundZ);

                    World.ShootBullet(startPosNew, targetPosNew, null, airstrikeModel, 200, 5000);
                }
            }
        }
    }
}
