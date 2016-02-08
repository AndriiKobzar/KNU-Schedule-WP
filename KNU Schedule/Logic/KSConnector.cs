using KNU_Schedule.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;

namespace KNU_Schedule.Logic
{

    public class KSController
    {
        public delegate void GroupDownloadResult(List<Dictionary<string, List<KSGroup>>> groups);
        public delegate void FacultyDownloadResult(List<KSFaculty> faculties);

        public event Action ScheduleDownloadStarted = () => { };
        public event Action ScheduleDownloadEnded = () => { };
        public event Action ScheduleDownloadBreaked = () => { };

        public event Action GroupsDownloadStarted;
        public event Action GroupsDownloadBreaked;
        public event GroupDownloadResult GroupsDownloadEnded;

        public event Action FacultiesDownloadStarted = () => { };
        public event Action FacultiesDownloadBreaked = () => { };
        public event FacultyDownloadResult FacultiesDownloadEnded;

        KSSchedule timetable = new KSSchedule();
        HttpWebRequest request = null;

        public KSController(KSSchedule timetable)
        {
            this.timetable = timetable;
        }


        public void CreateTimetable(string groupId)
        {
            timetable.Clear();
            WebRequest request = WebRequest.CreateHttp(AppResources.ApiPath + groupId);
            request.BeginGetResponse(ScheduleResult, request);
            ScheduleDownloadStarted();
        }
        private void ScheduleResult(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    using (WebResponse response = request.EndGetResponse(result))
                    {
                        string results;
                        using (var stream = new StreamReader(response.GetResponseStream()))
                        {
                            results = stream.ReadToEnd();
                        }
                        saveDataToIsolatedStorage(results);
                        KSScheduleResult scheduleResult = JsonConvert.DeserializeObject<KSScheduleResult>(results);
                        var requestResult = scheduleResult.Result;
                    }

                }
                catch (WebException)
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
        public void GetGroupsList(int facultyId)
        {
            request = WebRequest.CreateHttp(AppResources.ApiPath + "groups?faculty=" + facultyId);
            request.BeginGetResponse(GroupRequestEnded, request);
            GroupsDownloadStarted();
        }
        private void GroupRequestEnded(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    using (WebResponse response = request.EndGetResponse(ar))
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            string result = sr.ReadToEnd();
                            KSGroupResponse groups = null;
                            groups = JsonConvert.DeserializeObject<KSGroupResponse>(result);

                            GroupsDownloadEnded(groups.Result);
                        }
                    }
                }
                catch (WebException)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Неможливо завантажити дані. Перевірте підключення до мережі."));
                    GroupsDownloadBreaked();
                }
            }
        }
        public void GetFaculties()
        {
            WebRequest request = WebRequest.CreateHttp(AppResources.ApiPath + "faculties");
            request.BeginGetResponse(GetFacultiesCallback, request);
        }

        private void GetFacultiesCallback(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    using (WebResponse response = request.EndGetResponse(ar))
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            string result = sr.ReadToEnd();
                            KSFacultyResponse groups = JsonConvert.DeserializeObject<KSFacultyResponse>(result);
                            Deployment.Current.Dispatcher.BeginInvoke(() => FacultiesDownloadEnded(groups.Result));
                        }
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
