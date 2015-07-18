using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNU_Schedule.Logic
{
    public class KSSubject
    {
        string name = "Математичний аналіз";
        public string Name
        {
            get { return this.name; }
        }
        string teacher = "Львов";
        public string Teacher
        {
            get { return this.teacher; }
        }
        string room = "24";
        public string Room
        {
            get { return this.room; }
        }
        public KSSubject(string name, string teacher, string room)
        {
            this.name = name;
            this.teacher = teacher;
            this.room = room;
        }
    }
}
