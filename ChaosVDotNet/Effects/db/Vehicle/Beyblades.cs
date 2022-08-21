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
    internal class Beyblades : Effect
    {
        public Beyblades() : base("Beyblades", EffectType.Vehicle, true, false)
        {
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnTick(object sender, EventArgs e)
        {
            int Count = 5;
            float Force = 100;
            float VeloMulti = 3;

            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                if (veh != null)
                {
                    Ped pedOnSeat = veh.GetPedOnSeat(VehicleSeat.Driver);
                    bool willBeyblade = (veh.IsSeatFree(VehicleSeat.Driver) || pedOnSeat == null) ? true : !pedOnSeat.IsPlayer;

                    if (willBeyblade)
                    {
                        Debug.WriteLine("WillBeyblade");

                        Vector3 zero = Vector3.Zero;
                        Vector3 rot1 = new Vector3(4, 0, 0);
                        Vector3 rot2 = new Vector3(-4, 0, 0);

                        Debug.WriteLine("Vectors");

                        Function.Call(Hash.APPLY_FORCE_TO_ENTITY, veh, 3, Force, zero.X, zero.Y, zero.Z, rot1.X, rot1.Y, rot1.Z, true, true, true, true, true);
                        Function.Call(Hash.APPLY_FORCE_TO_ENTITY, veh, 3, -Force, zero.X, zero.Y, zero.Z, rot2.X, rot2.Y, rot2.Z, true, true, true, true, true);
                        veh.IsInvincible = true;
                        Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, veh, true);

                        Debug.WriteLine("Applying force");

                        if (veh.Speed < 10)
                        {
                            Debug.WriteLine("Forcing speed");
                            Vector3 Velo = Function.Call<Vector3>(Hash.GET_ENTITY_SPEED_VECTOR, veh, true);
                            Velo.X *= VeloMulti;
                            Velo.Y *= VeloMulti;
                            veh.Velocity = Velo;
                        }
                        else
                        {
                            Debug.WriteLine("Not forcing speed");
                            Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, veh, false);
                        }

                        if (--Count == 0)
                        {
                            Debug.WriteLine("Resetting count");
                            Count = 5;
                            Wait(0);
                        }
                    }
                }
            }
        }

        private void _OnStop(object sender, EventArgs e)
        {
            Vehicle[] vehs = World.GetAllVehicles();
            foreach (Vehicle veh in vehs)
            {
                veh.IsInvincible = false;
                Function.Call(Hash.SET_VEHICLE_REDUCE_GRIP, veh, false);
            }
        }
    }
}
