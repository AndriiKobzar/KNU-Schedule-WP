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
        string[] daysTranslator = new string[5] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };
        string[] periodTimes = new string[5] {"8:40 - 10:15","10:35 - 12:10","12:20 - 13:55","14:05 - 15:40"," " };
        Dictionary<string, List<KSGroup>> groups = new Dictionary<string,List<KSGroup>>();
        Dictionary<string, string> facultiesTranslator = new Dictionary<string, string>();
        public MainViewModel()
        {
            this.Days = new ObservableCollection<DayViewModel>();
            this.FacultyList = new ObservableCollection<FacultyViewModel>();
            this.GroupsList = new ObservableCollection<GroupViewModel>();

            facultiesTranslator.Add("history", "Історичний факультет");
            facultiesTranslator.Add("cybernetics", "Факультет кібернетики");
        }
        ObservableCollection<DayViewModel> days = new ObservableCollection<DayViewModel>();
        public ObservableCollection<DayViewModel> Days 
        { 
            get{return days;}
            private set
            {
                days = value;
                NotifyPropertyChanged("Days");
            } 
        }
        ObservableCollection<FacultyViewModel> facultyList = new ObservableCollection<FacultyViewModel>();
        public ObservableCollection<FacultyViewModel> FacultyList 
        { 
            get
            {
                return facultyList;
            }
            private set
            {
                facultyList = value;
                NotifyPropertyChanged("FacultyList");
            }
        }
        ObservableCollection<GroupViewModel> groupsList = new ObservableCollection<GroupViewModel>();
        public ObservableCollection<GroupViewModel> GroupsList 
        { 
            get
            {
                return groupsList;
            }
            private set
            {
                groupsList = value;
                NotifyPropertyChanged("GroupsList");
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadGroups()
        {
            App.Connector.GroupsDownloadStarted += () => { };
            App.Connector.GroupsDownloadEnded += Connector_GroupsDownloadEnded;
            App.Connector.FacultiesDownloadEnded += Connector_FacultiesDownloadEnded;
            //App.Connector.GetGroupsList(1);
            App.Connector.GetFaculties();
            FacultyList = new ObservableCollection<FacultyViewModel>();
            
            this.IsDataLoaded = true;
        }

        private void Connector_FacultiesDownloadEnded(List<KSFaculty> faculties)
        {
            
        }

        public void LoadSubjects()
        {
            Days = new ObservableCollection<DayViewModel>();
            for (int i = 0; i < 5; i++)
            {
                ObservableCollection<SubjectViewModel> subj = new ObservableCollection<SubjectViewModel>();
                int j = 0;
                foreach (KSSubject s in App.Timetable[i])
                {
                    //if (j < 5)
                    //    subj.Add(new SubjectViewModel() { Teacher = s.Teachers.For, Title = s.Name, Room = s.RoomName, Time = periodTimes[j++] });
                    //else
                    //    subj.Add(new SubjectViewModel() { Teacher = s.LectureName, Title = s.Name, Room = s.RoomName });
                }
                Days.Add(new DayViewModel() { Header = daysTranslator[i], Subjects = subj });
            }
        }
        void Connector_GroupsDownloadEnded(List<Dictionary<string, List<KSGroup>>> result)
        {
            groups = result[0];           
        }
        public void LoadGroups(string faculty)
        {
            GroupsList = new ObservableCollection<GroupViewModel>();
            //foreach(KSGroup group in groups)
            //{
            //     GroupsList.Add(new GroupViewModel() { GroupName = group.name });
            //}
        }
        public string IdByName(string groupName)
        {
            //foreach(KSGroup group in groups)
            //{
            //    if (group.Name == groupName) return group.Id;
            //}
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