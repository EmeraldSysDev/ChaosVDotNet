using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosVDotNet.Effects
{
    public class LogArgs
    {
        public LogVerbosity Level { get; }
        public string Message { get; }

        internal LogArgs(string Message, LogVerbosity Level = LogVerbosity.Info)
        {
            this.Level = Level;
            this.Message = Message;
        }
    }
}
