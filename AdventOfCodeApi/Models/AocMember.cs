using System.Collections.Generic;

namespace AdventOfCodeApi.Models
{
    public class AocMember
    {
        public int LocalScore { get; set; }

        public int Stars { get; set; }

        public string Id { get; set; }

        public object MyProperty { get; set; }

        public int GlobalScore { get; set; }

        public List<AocCompletionDayLevel> CompletionDayLevels { get; set; }

        public int LastStarTs { get; internal set; }

        public string Name { get; internal set; }

        public AocMember()
        {
            CompletionDayLevels = new List<AocCompletionDayLevel>();
        }
    }
}
