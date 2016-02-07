using System.IO.IsolatedStorage;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using KNU_Schedule.Resources;
using System;
using System.Windows;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace KNU_Schedule.Logic
{
    public class KSSchedule
    {
        List<KSSubject>[] timetable = new List<KSSubject>[5];
        public KSSchedule()
        {
            for (int i = 0; i < 5; i++)
            {
                timetable[i] = new List<KSSubject>();
            }
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
                App.ViewModel.LoadGroups();
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
            for (int i = 0; i < timetable.GetLength(1); i++ )
            {
                if (timetable[day][i] != null) return true;
            }
            return false;
        }

        internal void Clear()
        {
            for (int i = 0; i < 5; i++)
            {
                timetable[i] = new List<KSSubject>();
            }
        }
    }
    public class KSScheduleResult
    {
        public int Status { get; set; }
        public int Error { get; set; }
        //first level-for security and standartization
        //second level is for week data
        //third level is for day data
        //fourth level is for subject data
        public List<Dictionary<string,Dictionary<string,List<List<KSSubject>>>>> Result;
    }

}