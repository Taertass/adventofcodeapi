using System;
using System.Collections.Generic;

namespace AdventOfCodeApi.Models
{
    public class AppOption
    {
        public TimeSpan PollingIntervale { get; set; }

        public TimeSpan FirstTimeOfDayToLoad { get; set; }

        public TimeSpan LastTimeOfDayToLoad { get; set; }

        public List<int> DaysToLoad { get; set; }

        public string AocApiAddress { get; set; }

        public string SessionId { get; set; }

    }
}
