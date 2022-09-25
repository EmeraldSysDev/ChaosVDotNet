using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTA;

namespace ChaosVDotNet.Effects.db
{
    internal class PropModels : Effect
    {
        private readonly Dictionary<Vehicle, Prop> vehPropsMap;
        private readonly List<Model> availPropModels;

        public PropModels() : base("effect_propmodels", "Prop Models", EffectType.Vehicle, true, false)
        {
            vehPropsMap = new Dictionary<Vehicle, Prop>();
            availPropModels = new List<Model>();
            OnTick += _OnTick;
            OnStop += _OnStop;
        }

        private void _OnTick(object sender, EventArgs e)
        {

        }

        private void _OnStop(object sender, EventArgs e)
        {

        }
    }
}
