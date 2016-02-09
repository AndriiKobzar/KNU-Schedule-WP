using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNU_Schedule.Logic
{
    public class KSSubject
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public List<KSTeacher> Teachers { get; set; }
        public List<KSClassroom> Classrooms { get; set; }
    }
}
