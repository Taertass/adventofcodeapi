using System;

namespace AdventOfCodeApi.Models
{
    public class AocCompletionDayLevel
    {
        public DateTime CompletionTime { get; set; }
        public long GetStarTs { get; set; }
        public int Day { get; set; }
        public int StarNumber { get; set; }
    }
}
