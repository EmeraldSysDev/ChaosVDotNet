using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;

namespace ChaosVDotNet.Effects.db
{
    internal class RainingContainers : Effect
    {
        private static readonly Random Rand = new Random();
        private readonly List<int> containerHashes = new List<int>();
        private static int lastTick = 0;

        public RainingContainers() : base("effect_rainingboxes", "Raining Containers", EffectType.Misc, true, false)
        {
            int PropContainer02A = Game.GenerateHash("prop_container_02a");
            int PropContainer03A = Game.GenerateHash("prop_container_03a");
            int PropContainer01H = Game.GenerateHash("prop_container_01h");

            containerHashes.AddRange(new List<int>
            {
                PropContainer02A,
                PropContainer03A,
                PropContainer01H
            });

            OnTick += _OnTick;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            // TODO: Fix containers despawning immediately

            Player player = Game.Player;
            Ped playerPed = player.Character;

            Vector3 playerPos = playerPed.Position;
            Vector3 playerRot = playerPed.Rotation;

            int curTick = Game.GameTime;

            if (curTick > (lastTick + 500))
            {
                lastTick = curTick;

                float x = playerPos.X + Rand.Next(-100, 100);
                float y = playerPos.Y + Rand.Next(-100, 100);
                float z = playerPos.Z + Rand.Next(25, 50);

                Vector3 spawnPos = new Vector3(x, y, z);

                if (containerHashes.Count > 0)
                {
                    int modelHash = containerHashes[(int)EffectUtil.NextInRange(Rand, 0, containerHashes.Count - 1)];
                    //EffectUtil.CreateTempProp(modelHash, spawnPos);
                    Model model = new Model(modelHash);
                    Prop temp = World.CreateProp(model, spawnPos, playerRot, true, false);

                    temp.IsPersistent = true;

                    model.MarkAsNoLongerNeeded();

                    /* int count = 5000;
                    if (--count > 0)
                    {
                        Wait(0);
                    } */
                    Wait(15000);

                    temp.IsPersistent = false;
                    temp.MarkAsNoLongerNeeded();
                }
            }
        }
    }
}
