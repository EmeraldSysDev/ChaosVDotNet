using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class AttackPlayer : Effect
    {
        private readonly List<Ped> Peds = new List<Ped>();
        private RelationshipGroup group;

        public AttackPlayer() : base("effect_pedsattackplr", "All Peds Attack Player", EffectType.Ped, true, false)
        {
            OnStart += _OnStart;
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            group = World.AddRelationshipGroup("_ATTACK_PLAYER");
        }

        private void _OnTick(object sender, EventArgs e)
        {
            Player player = Game.Player;
            Ped playerPed = player.Character;
            RelationshipGroup playerGroup = playerPed.RelationshipGroup;

            if (group != null) group.SetRelationshipBetweenGroups(playerGroup, Relationship.Hate, true);

            Ped[] peds = World.GetAllPeds();
            foreach (Ped ped in peds)
            {
                if (!ped.IsPlayer)
                {
                    if (ped.IsInGroup && ped.RelationshipGroup == playerGroup)
                    {
                        ped.PedGroup.Remove(ped);
                    }

                    ped.RelationshipGroup = group;

                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 5, true);
                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 46, true);

                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 2, true);

                    ped.Task.FightAgainst(playerPed);
                    Peds.Add(ped);
                }
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            foreach (Ped ped in Peds)
            {
                if (ped.Exists())
                {
                    ped.RelationshipGroup = null;

                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 5, false);
                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 46, false);
                    Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 2, false);

                    ped.Task.ClearAllImmediately();
                }
            }

            Peds.Clear();
        }
    }
}
