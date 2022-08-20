using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosVDotNet.Effects.db
{
    public class TestEffect : Effect
    {
        public TestEffect() : base("TestEffect", false)
        {
            OnStart += _OnStart;
            OnStop += _OnStop;
        }

        private void _OnStart(object sender, EventArgs e)
        {
            Console.WriteLine("Started!");
        }

        private void _OnStop(object sender, EventArgs e)
        {
            Console.WriteLine("Stopped!");
        }
    }
}
