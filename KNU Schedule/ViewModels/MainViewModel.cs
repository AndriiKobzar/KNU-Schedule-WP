using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using KNU_Schedule.Resources;
using System.Collections.Generic;
using KNU_Schedule.Logic;
using System.Windows;

namespace KNU_Schedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string[] days = new string[5] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };
        string[] periodTimes = new string[5] {"8:40 - 10:15","10:35 - 12:10","12:20 - 13:55","14:05 - 15:40"," " };
        List<KSGroup> groups = new List<KSGroup>();
        Dictionary<string, string> facultiesTranslator = new Dictionary<string, string>();
        public MainViewModel()
        {
            this.Days = new ObservableCollection<DayViewModel>();
            this.FacultyList = new ObservableCollection<FacultyViewModel>();
            this.GroupsList = new ObservableCollection<GroupViewModel>();

            facultiesTranslator.Add("history", "Історичний факультет");
            facultiesTranslator.Add("cybernetics", "Факультет кібернетики");
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<DayViewModel> Days { get; private set; }
        public ObservableCollection<FacultyViewModel> FacultyList { get; private set; }
        public ObservableCollection<GroupViewModel> GroupsList { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            App.Connector.GroupsDownloadStarted += ()=>{};
            App.Connector.GroupsDownloadEnded += Connector_GroupsDownloadEnded;
            App.Connector.GetGroupsList();
            Days = new ObservableCollection<DayViewModel>();
            FacultyList = new ObservableCollection<FacultyViewModel>();
            for (int i = 0; i < 5; i++ )
            {
                ObservableCollection<SubjectViewModel> subj = new ObservableCollection<SubjectViewModel>();
                int j = 0;
                foreach(KSSubject s in App.Timetable[i] )
                {
                    if (j<5) 
                        subj.Add(new SubjectViewModel() { Teacher = s.LectureName, Title = s.Name, Room = s.RoomName, Time = periodTimes[j++] });
                    else
                        subj.Add(new SubjectViewModel() { Teacher = s.LectureName, Title = s.Name, Room = s.RoomName });
                }
                Days.Add(new DayViewModel() { Header = days[i], Subjects = subj });
            }
            this.IsDataLoaded = true;
        }

        void Connector_GroupsDownloadEnded(List<KSGroup> result)
        {
            groups = result;
            List<string> faculties = new List<string>();
            foreach(KSGroup group in result)
            {
                if (!faculties.Contains(group.FacultyId)) faculties.Add(group.FacultyId);
            }
            foreach(string faculty in faculties)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => FacultyList.Add(new FacultyViewModel() { FacultyName=facultiesTranslator[faculty], ID=faculty}));     
            }
        }
        public void LoadGroups(string faculty)
        {
            GroupsList = new ObservableCollection<GroupViewModel>();
            foreach(KSGroup group in groups)
            {
                if (group.FacultyId.Equals(faculty)) GroupsList.Add(new GroupViewModel() { GroupName = group.Name });
            }
        }
        public string IdByName(string groupName)
        {
            foreach(KSGroup group in groups)
            {
                if (group.Name == groupName) return group.Id;
            }
            return null;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }
}