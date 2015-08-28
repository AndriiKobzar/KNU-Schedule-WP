using KNU_Schedule.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace KNU_Schedule.Logic
{

    public class KSController
    {
        public delegate void GroupDownloadResult(List<KSGroup> groups);


        public event Action ScheduleDownloadStarted = () => { };
        public event Action ScheduleDownloadEnded = () => { };
        public event Action ScheduleDownloadBreaked = () => { };

        public event Action GroupsDownloadStarted;
        public event Action GroupsDownloadBreaked;
        public event GroupDownloadResult GroupsDownloadEnded;

        KSTimetable timetable = new KSTimetable();
        HttpWebRequest request = null;

        private bool isDataLoaded = false;
        
        int time = 10;
        public KSController(KSTimetable timetable)
        {
            this.timetable = timetable;

        }


        public void CreateTimetable(string groupId)
        {
            timetable.Clear();
            DateTime now = DateTime.Now;
            DateTime prevMonday = now;
            while (prevMonday.DayOfWeek!=DayOfWeek.Monday)
            {
                prevMonday = prevMonday.AddDays(-1);
            }
            string start = string.Format("{0}-{1}-{2}",prevMonday.Year, prevMonday.Month, prevMonday.Day);
            DateTime nextMonday = now;
            if (now.DayOfWeek == DayOfWeek.Monday)
            {
                nextMonday.AddDays(7);
            }
            else
            {
                while (nextMonday.DayOfWeek != DayOfWeek.Monday)
                {
                    nextMonday = nextMonday.AddDays(1);
                }
            }
            string end = string.Format("{0}-{1}-{2}", nextMonday.Year, nextMonday.Month, nextMonday.Day); ;
            string requestUrl = string.Format("{0}?groupKey=\"{1}\"&lastEditDate=null&start={2}&end={3}", AppResources.ApiPath, groupId, start, end);
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl); //AppResources.ApiPath+"?groupKey=\"12\"&lastEditDate=null&start=2015-08-10&end=2015-08-16)"
            request.BeginGetResponse(receiveData, request);
            ScheduleDownloadStarted();
        }
        private void receiveData(IAsyncResult result)
        {
            isDataLoaded = true;
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);
                    string results;
                    using (var stream = new StreamReader(response.GetResponseStream()))
                    {
                        results = stream.ReadToEnd();
                    }
                    saveDataToIsolatedStorage(results);
                    List<KSSubject> list = JsonConvert.DeserializeObject<List<KSSubject>>(results);
                    response.Close();
                    foreach (KSSubject subject in list)
                    {
                        int numberOfDay = (int)subject.Date.DayOfWeek-1;
                        timetable[numberOfDay].Add(subject);
                    }
                    
                }
                catch (WebException e)
                {
                    ScheduleDownloadBreaked();
                    return;
                }
                
            }

            ScheduleDownloadEnded();
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
        public void GetGroupsList()
        {
            request = HttpWebRequest.CreateHttp(AppResources.ApiPath);
            request.BeginGetResponse(GroupResponse,request);
        }

        private void GroupResponse(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            if(request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(ar);
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        string result = sr.ReadToEnd();
                        List<string[]> groups = JsonConvert.DeserializeObject<List<string[]>>(result);
                        List<KSGroup> groupList = new List<KSGroup>();
                        foreach (string[] groupInfo in groups)
                        {
                            groupList.Add(new KSGroup() { Name = groupInfo[0], FacultyId = groupInfo[1], Id = groupInfo[2] });
                        }
                        GroupsDownloadEnded(groupList);
                    }
                }
                catch (WebException)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Неможливо завантажити дані. Перевірте підключення до мережі."));
                }
            }
        }
    }
}
