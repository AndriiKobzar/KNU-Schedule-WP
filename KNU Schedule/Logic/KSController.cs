using KNU_Schedule.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using Storage = System.IO.IsolatedStorage.IsolatedStorageSettings;
namespace KNU_Schedule.Logic
{
    public class KSController
    {
        string BACKUP = "backup";

        public delegate void GroupDownloadResult(List<Dictionary<string, List<KSGroup>>> groups);
        public delegate void FacultyDownloadResult(List<KSFaculty> faculties);
        public delegate void ScheduleDownloadResult(Dictionary<string, Dictionary<string, List<List<KSSubject>>>> schedule);

        public event Action ScheduleDownloadStarted = () => { };
        public event Action ScheduleDownloadBreaked = () => { };
        public event ScheduleDownloadResult ScheduleDownloadEnded = (schedule) => { };

        public event Action GroupsDownloadStarted = () => { };
        public event Action GroupsDownloadBreaked = () => { };
        public event GroupDownloadResult GroupsDownloadEnded;

        public event Action FacultiesDownloadStarted = () => { };
        public event Action FacultiesDownloadBreaked = () => { };
        public event FacultyDownloadResult FacultiesDownloadEnded;

        KSSchedule timetable = new KSSchedule();

        public KSController(KSSchedule timetable)
        {
            this.timetable = timetable;
        }

        public void CreateTimetable(int groupId)
        {
            timetable.Clear();
            WebRequest request = WebRequest.CreateHttp(AppResources.ApiPath + groupId);
            request.BeginGetResponse(ScheduleResult, request);
            Deployment.Current.Dispatcher.BeginInvoke(() => ScheduleDownloadStarted());
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
                            if (Storage.ApplicationSettings.Contains(BACKUP))
                            {
                                Storage.ApplicationSettings[BACKUP] = result;
                            }
                            else
                            {
                                Storage.ApplicationSettings.Add(BACKUP, result);
                            }
                        }
                        saveDataToIsolatedStorage(results);
                        KSScheduleResult scheduleResult = JsonConvert.DeserializeObject<KSScheduleResult>(results);
                        Deployment.Current.Dispatcher.BeginInvoke(()=>ScheduleDownloadEnded(scheduleResult.Result[0]));
                    }

                }
                catch (WebException)
                {
                    ScheduleDownloadBreaked();
                    return;
                }
            }
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
            WebRequest request = WebRequest.CreateHttp(AppResources.ApiPath + "groups?faculty=" + facultyId);
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

                            Deployment.Current.Dispatcher.BeginInvoke(()=>GroupsDownloadEnded(groups.Result));
                        }
                    }
                }
                catch (WebException)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => GroupsDownloadBreaked());
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
