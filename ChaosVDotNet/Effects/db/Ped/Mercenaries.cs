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
    internal class Mercenaries : Effect
    {
        private static Random Rand = new Random();

        internal class EnemyGroup
        {
            public Vehicle vehicle { get; set; }
            public List<Ped> peds { get; }

            public EnemyGroup()
            {
                peds = new List<Ped>();
            }
        }

        private static Model model;
        private static WeaponHash microSmgHash;
        private static RelationshipGroup relGroup;
        private static EnemyGroup heliGroup;
        private static EnemyGroup mesaGroup;

        public Mercenaries() : base("Mercenaries", EffectType.Ped, true, false)
        {
            OnStart += _OnStart;
            OnStop += _OnStop;
            OnTick += _OnTick;
        }

        private static Vector3 getRandomOffsetCoord(Vector3 startCoord, float min, float max)
        {
            Vector3 randomCoord = Vector3.Zero;

            if ((Rand.Next(0, 2) % 2) == 0)
            {
                randomCoord.X = startCoord.X + Rand.Next((int)min, (int)max);
            }
            else
            {
                randomCoord.X = startCoord.X - Rand.Next((int)min, (int)max);
            }

            if ((Rand.Next(0, 2) % 2) == 0)
            {
                randomCoord.Y = startCoord.Y + Rand.Next((int)min, (int)max);
            }
            else
            {
                randomCoord.Y = startCoord.Y - Rand.Next((int)min, (int)max);
            }

            randomCoord.Z = startCoord.Z;
            return randomCoord;
        }

        private static void FillVehicleWithPeds(Vehicle veh, Ped playerPed, RelationshipGroup relGroup, Model model, WeaponHash weaponHash, List<Ped> list, bool canExitVeh)
        {
            for (int seatPos = -1; seatPos < 3; seatPos++)
            {
                Ped ped = Function.Call<Ped>(Hash.CREATE_PED_INSIDE_VEHICLE, veh, -1, model.Hash, seatPos, true, false);
                ped.BlockPermanentEvents = true;

                ped.RelationshipGroup = relGroup;
                ped.HearingRange = 9999.0f;

                Function.Call(Hash.GIVE_WEAPON_TO_PED, ped, weaponHash, 9999, true, true);
                ped.Accuracy = 50;

                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 0, true);
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 1, true);
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 2, true);
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 3, canExitVeh);
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 5, true);
                Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 46, true);

                Function.Call(Hash.REGISTER_TARGET, ped, playerPed);
                ped.Task.FightAgainst(playerPed);
                list.Add(ped);
                Wait(0);
            }
        }

        private static void spawnBuzzard()
        {
            Ped playerPed = Game.Player.Character;
            Vector3 playerPos = playerPed.Position;
            Model buzzard = new Model(VehicleHash.Buzzard);
            Vector3 spawnPoint = getRandomOffsetCoord(playerPos, 200, 250);
            float xDiff = playerPos.X - spawnPoint.X;
            float yDiff = playerPos.Y - spawnPoint.Y;
            float heading = Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, xDiff, yDiff);
            heliGroup = new EnemyGroup();
            heliGroup.vehicle = Function.Call<Vehicle>(Hash.CREATE_VEHICLE, buzzard.Hash, spawnPoint.X, spawnPoint.Y, spawnPoint.Z + 50, heading, true, false, false);
            Function.Call(Hash.SET_VEHICLE_COLOURS, heliGroup.vehicle, 0, 0);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, heliGroup.vehicle, true, true, true);
            heliGroup.vehicle.ForwardSpeed = 0;
            Function.Call(Hash.SET_VEHICLE_CHEAT_POWER_INCREASE, heliGroup.vehicle, 2);
            FillVehicleWithPeds(heliGroup.vehicle, playerPed, relGroup, model, microSmgHash, heliGroup.peds, false);
        }

        private static void spawnMesa()
        {
            Ped playerPed = Game.Player.Character;
            Vector3 playerPos = playerPed.Position;
            Vector3 spawnPoint;

            unsafe
            {
                int nodeId;
                if (!Function.Call<bool>(Hash.GET_RANDOM_VEHICLE_NODE, playerPos.X, playerPos.Y, playerPos.Z, 150, false, false, false, &spawnPoint, &nodeId))
                {
                    spawnPoint = getRandomOffsetCoord(playerPos, 50, 50);
                    float groundZ;
                    if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, spawnPoint.X, spawnPoint.Y, spawnPoint.Z, &groundZ, false, false))
                    {
                        spawnPoint.Z = groundZ;
                    }
                }
            }

            float xDiff = playerPos.X - spawnPoint.X;
            float yDiff = playerPos.Y - spawnPoint.Y;
            float heading = Function.Call<float>(Hash.GET_HEADING_FROM_VECTOR_2D, xDiff, yDiff);

            VehicleHash mesaHash = VehicleHash.Mesa3;
            mesaGroup = new EnemyGroup();

            mesaGroup.vehicle = Function.Call<Vehicle>(Hash.CREATE_VEHICLE, mesaHash, spawnPoint.X, spawnPoint.Y, spawnPoint.Z + 5, heading, true, false, false);
            Function.Call(Hash.SET_VEHICLE_ON_GROUND_PROPERLY, mesaGroup.vehicle, 5.0f);
            Function.Call(Hash.SET_VEHICLE_COLOURS, mesaGroup.vehicle, 0, 0);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, mesaGroup.vehicle, true, true, true);
            Function.Call(Hash.SET_VEHICLE_CHEAT_POWER_INCREASE, mesaGroup.vehicle, 2);
            FillVehicleWithPeds(mesaGroup.vehicle, playerPed, relGroup, model, microSmgHash, mesaGroup.peds, true);
        }

        private void _OnStart(object sender, EventArgs e)
        {
            Ped playerPed = Game.Player.Character;
            Vector3 playerPos = playerPed.Position;

            model = new Model(PedHash.MerryWeatherCutscene);
            microSmgHash = WeaponHash.MicroSMG;

            RelationshipGroup playerGroup = playerPed.RelationshipGroup;
            relGroup = World.AddRelationshipGroup("_HOSTILE_MERRYWEATHER");
            relGroup.SetRelationshipBetweenGroups(playerGroup, Relationship.Hate, true);
            relGroup.SetRelationshipBetweenGroups(relGroup, Relationship.Companion);

            spawnBuzzard();
            spawnMesa();
        }

        private void _OnStop(object sender, EventArgs e)
        {
            heliGroup.vehicle.MarkAsNoLongerNeeded();
            foreach (Ped ped in heliGroup.peds)
            {
                ped.MarkAsNoLongerNeeded();
            }
            mesaGroup.vehicle.MarkAsNoLongerNeeded();
            foreach (Ped ped in mesaGroup.peds)
            {
                ped.MarkAsNoLongerNeeded();
            }
        }

        private bool checkPedsAlive(List<Ped> list)
        {
            bool allDead = true;
            Ped player = Game.Player.Character;
            foreach (Ped ped in list)
            {
                if (!ped.Exists() || Function.Call<bool>(Hash.IS_PED_DEAD_OR_DYING, ped, false))
                {
                    Function.Call(Hash.SET_ENTITY_HEALTH, ped, 0, false);
                    ped.MarkAsNoLongerNeeded();
                }
                else
                {
                    allDead = false;
                    Vector3 playerPos = player.Position;
                    Vector3 enemyPos = ped.Position;

                    if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, playerPos.X, playerPos.Y, playerPos.Z,
                        enemyPos.X, enemyPos.Y, enemyPos.Z, false) > 350)
                    {
                        Function.Call(Hash.SET_ENTITY_HEALTH, ped, 0, false);
                        ped.MarkAsNoLongerNeeded();
                    }
                    else
                    {
                        ped.Task.FightAgainst(player);
                    }
                }
            }

            return allDead;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            bool allHeliDead = checkPedsAlive(heliGroup.peds);
            if (allHeliDead)
            {
                heliGroup.peds.Clear();
                heliGroup.vehicle.MarkAsNoLongerNeeded();
                spawnBuzzard();
            }

            bool allVanDead = checkPedsAlive(mesaGroup.peds);
            if (allVanDead)
            {
                mesaGroup.peds.Clear();
                mesaGroup.vehicle.MarkAsNoLongerNeeded();
                spawnMesa();
            }
        }
    }
}
