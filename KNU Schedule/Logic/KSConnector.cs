using KNU_Schedule.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KNU_Schedule.Logic
{

    class KSConnector
    {
        KSTimetable timetable = new KSTimetable();
        string groupId;
        public delegate void Simple();
        public event Simple DownloadStarted;
        public event Simple DownloadEnded;
        public KSConnector(KSTimetable timetable, string groupId)
        {
            this.timetable = timetable;
            this.groupId = groupId;
        }
        public void CreateTimetable()
        {
            string requestUrl = string.Format("{0}?groupKey=\"{1}\"&lastEditDate=null&start={2}&end={3}", AppResources.ApiPath, groupId, "2015 - 08 - 10", "2015-08-16");
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl); //AppResources.ApiPath+"?groupKey=\"12\"&lastEditDate=null&start=2015-08-10&end=2015-08-16)"
            IAsyncResult res = request.BeginGetResponse(receiveData, request);
            DownloadStarted();
        }
        private void receiveData(IAsyncResult result)
        {
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
                        int numberOfDay = (int)subject.Date.DayOfWeek;
                        timetable[numberOfDay].Add(subject);
                    }
                    
                }
                catch (WebException e)
                {
                    return;
                }
                
            }
            DownloadEnded();
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
    }
}
