using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNU_Schedule.Logic
{
    public class KSSubject
    {
        int id = 0;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        string name = "Математичний аналіз";
        public string Name
        {
            get { return this.name; }
        }
        string lectureName = "Львов";
        public string LectureName
        {
            get { return this.lectureName; }
            set { this.lectureName = value; }
        }
        string room = "24";
        public string RoomName
        {
            get { return this.room; }
            set { this.room = value; }
        }
        string groupId;

        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        public KSSubject(string name, string teacher, string room)
        {
            this.name = name;
            this.lectureName = teacher;
            this.room = room;
        }
    }
}
