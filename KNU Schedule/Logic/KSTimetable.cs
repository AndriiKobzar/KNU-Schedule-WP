using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.Generic;

namespace KNU_Schedule.Logic
{
    public class KSSchedule
    {
        List<List<KSSubject>> timetable = new List<List<KSSubject>>();
        public KSSchedule()
        {
           
        }
        public KSSchedule(string groupKey):this()
        {
            
        }

        public KSSubject this[int day, int period]
        {
            get { return this.timetable[day][period]; }
            set 
            { 
                this.timetable[day][period] = value;
            }
        }
        public List<KSSubject> this[int day]
        {
            get { return this.timetable[day]; }
        }
        private void saveDataToIsolatedStorage(string data)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile("MyFile.store"))
                {
                    StreamWriter sw = new StreamWriter(rawStream);
                    sw.Write(data);
                    sw.Close();
                }
            }
        }
        private bool dayIsNotNull(int day)
        {
            for (int i = 0; i < timetable.Count; i++)
            {
                if (timetable[day][i] != null) return true;
            }
            return false;
        }

        internal void Clear()
        {
            timetable = new List<List<KSSubject>>(); 
        }
    }
    public class KSScheduleResult
    {
        public int Status { get; set; }
        public int Error { get; set; }
        //first level for security and standartization
        //second level is for week data: number of week -> 
        //third level is for day data
        //fourth level is for subject data
        public List<Dictionary<string,Dictionary<string,List<List<KSSubject>>>>> Result;
    }

}