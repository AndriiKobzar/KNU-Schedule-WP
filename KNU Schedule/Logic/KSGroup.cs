using Newtonsoft.Json;
using System.Collections.Generic;

namespace KNU_Schedule.Logic
{
    public class KSGroupResponse
    {
        public int Status;// { get; set; }
        public int Error;// { get; set; }

        public List<Dictionary<string, List<KSGroup>>> Result;// { get; set; }
    }
    public class KSGroup
    {
        public int Id;// { get; set; }
        public string Name;// { get; set; }
    }
}
