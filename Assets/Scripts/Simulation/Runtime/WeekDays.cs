using System;

namespace Simulation.Runtime
{
    [Flags] //1010000 <=> Monday and Wednesday
   public enum WeekDays : byte
    {
        Monday    = 1 << 0,
        Tuesday   = 1 << 1,
        Wednesday = 1 << 2,
        Thursday  = 1 << 3,
        Friday    = 1 << 4,
        Saturday  = 1 << 5,
        Sunday    = 1 << 6,
        
        All      = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday,
        WorkDays = Monday | Tuesday | Wednesday | Thursday | Friday
    }
}
