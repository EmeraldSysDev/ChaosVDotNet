using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;
using GTA.Math;
using GTA.Native;

namespace ChaosVDotNet.Effects.db
{
    internal class PropHunt : Effect
    {
        private readonly Random Rand;
        private readonly Dictionary<Ped, Prop> pedPropsMap;
        private readonly List<Model> availablePropModels;

        public PropHunt() : base("effect_prophunt", "Prop Hunt", EffectType.Ped, true, false)
        {
            Rand = new Random();
            pedPropsMap = new Dictionary<Ped, Prop>();
            availablePropModels = new List<Model>();
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private static Vector3 GetPropOffset(Model propModel)
        {
            (Vector3 propMin, Vector3 propMax) = propModel.Dimensions;
            return (((propMax - propMin) / 2.0f) + propMin) * -1.0f;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            int lastModelsUpdateTick = 0;
            int currentTick = Game.GameTime;
            if ((currentTick - lastModelsUpdateTick) > 1000 || !availablePropModels.Any())
            {
                lastModelsUpdateTick = currentTick;

                availablePropModels.Clear();

                Entity[] props = World.GetAllProps();
                foreach (Entity prop in props)
                {
                    Model model = prop.Model;
                    (Vector3 min, Vector3 max) = model.Dimensions;
                    float modelSize = (max - min).Length();

                    if (modelSize > 0.3f && modelSize <= 6.0f)
                    {
                        availablePropModels.Add(model);
                    }
                }
            }

            if (availablePropModels.Any())
            {
                int Count = 10;
                Ped[] peds = World.GetAllPeds();
                foreach (Ped ped in peds)
                {
                    if (ped.Exists() && !ped.IsPlayer && !pedPropsMap.Any(item => item.Key == ped))
                    {
                        Model propModel = availablePropModels[Rand.Next(0, availablePropModels.Count - 1)];

                        Prop prop = Function.Call<Prop>(Hash.CREATE_OBJECT, propModel.Hash, 0.0f, 0.0f, 0.0f, true, false, false);

                        prop.IsCollisionEnabled = false;
                        Function.Call(Hash.SET_ENTITY_COMPLETELY_DISABLE_COLLISION, prop, false, false);

                        Function.Call(Hash.SET_ENTITY_VISIBLE, ped, false, 0);

                        Vector3 offset = GetPropOffset(propModel);
                        Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, ped, -1, offset.X, offset.Y, offset.Z, 0.0f, 0.0f, 0.0f, false, false, false, false, 0, true);
                        Function.Call(Hash.SET_ENTITY_VISIBLE, prop, true, 0);

                        pedPropsMap[ped] = prop;

                        if (--Count == 0)
                        {
                            Count = 10;
                            Wait(0);
                        }
                    }
                }
            }

            int lastPropPedsCheckTick = 0;
            currentTick = Game.GameTime;
            if ((currentTick - lastPropPedsCheckTick) > 500)
            {
                lastPropPedsCheckTick = currentTick;

                int Count = 20;
                List<Ped> pedsToRemove = new List<Ped>();
                foreach (KeyValuePair<Ped, Prop> kvp in pedPropsMap)
                {
                    Ped ped = kvp.Key;
                    Prop prop = kvp.Value;
                    if (!ped.Exists())
                    {
                        if (prop.Exists())
                        {
                            prop.Delete();
                        }

                        pedsToRemove.Add(ped);
                        continue;
                    }
                    else if (ped.IsPlayer)
                    {
                        Function.Call(Hash.SET_ENTITY_VISIBLE, ped, true, 0);

                        if (prop.Exists())
                        {
                            prop.Delete();
                        }

                        pedsToRemove.Add(ped);
                        continue;
                    }
                    else if (!prop.Exists())
                    {
                        Function.Call(Hash.SET_ENTITY_VISIBLE, ped, true, 0);
                        pedsToRemove.Add(ped);
                        continue;
                    }
                    else
                    {
                        Function.Call(Hash.SET_ENTITY_VISIBLE, ped, false, 0);

                        if (!prop.IsAttachedTo(ped))
                        {
                            Vector3 offset = GetPropOffset(prop.Model);
                            Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, ped, -1, offset.X, offset.Y, offset.Z, 0.0f, 0.0f, 0.0f, false, false, false, false, 0, true);
                        }

                        Function.Call(Hash.SET_ENTITY_VISIBLE, prop, true, 0);
                    }

                    if (--Count == 0)
                    {
                        Count = 20;
                        Wait(0);
                    }
                }

                foreach (Ped ped in pedsToRemove)
                {
                    pedPropsMap.Remove(ped);

                    if (--Count == 0)
                    {
                        Count = 20;
                        Wait(0);
                    }
                }

                pedsToRemove.Clear();
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            foreach (KeyValuePair<Ped, Prop> kvp in pedPropsMap)
            {
                Ped ped = kvp.Key;
                Prop prop = kvp.Value;

                if (ped.Exists())
                {
                    Function.Call(Hash.SET_ENTITY_VISIBLE, ped, true, 0);
                }

                if (prop.Exists())
                {
                    prop.Delete();
                }
            }

            pedPropsMap.Clear();
        }
    }
}
