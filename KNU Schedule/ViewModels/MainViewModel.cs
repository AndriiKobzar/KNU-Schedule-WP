using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using KNU_Schedule.Logic;

namespace KNU_Schedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string[] daysTranslator = new string[5] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };
        Dictionary<string, List<KSGroup>> groups = new Dictionary<string, List<KSGroup>>();
        public MainViewModel()
        {
            this.Days = new ObservableCollection<DayViewModel>();
            this.FacultyList = new ObservableCollection<FacultyViewModel>();
            this.GroupsList = new ObservableCollection<GroupViewModel>();
            App.Connector.GroupsDownloadEnded += Connector_GroupsDownloadEnded;
            App.Connector.FacultiesDownloadEnded += Connector_FacultiesDownloadEnded;
        }
        private ObservableCollection<GroupViewModel> groupslist;
        public ObservableCollection<GroupViewModel> GroupsList
        {
            get { return groupslist; }
            set { groupslist = value; }
        }

        ObservableCollection<DayViewModel> days = new ObservableCollection<DayViewModel>();
        public ObservableCollection<DayViewModel> Days
        {
            get { return days; }
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
        private ObservableCollection<CourseViewModel> courses;

        public ObservableCollection<CourseViewModel> Courses
        {
            get { return courses; }
            set
            {
                courses = value;
                NotifyPropertyChanged("Courses");
            }
        }


        internal void LoadFaculties()
        {
            App.Connector.GetFaculties();
        }

        private void Connector_FacultiesDownloadEnded(List<KSFaculty> faculties)
        {
            foreach (var faculty in faculties)
            {
                try
                {
                    FacultyList.Add(new FacultyViewModel() { FacultyName = faculty.Name, ID = faculty.Id });
                }
                catch (Exception)
                {

                }
            }
        }

        public void LoadSubjects()
        {
            Days = new ObservableCollection<DayViewModel>();
            for (int i = 0; i < 5; i++)
            {
                ObservableCollection<SubjectViewModel> subj = new ObservableCollection<SubjectViewModel>();

                foreach (KSSubject s in App.Timetable[i])
                {

                }
                Days.Add(new DayViewModel() { Header = daysTranslator[i], Subjects = subj });
            }
        }
        public void LoadGroups(int facultyId)
        {
            App.Connector.GetGroupsList(facultyId);
        }
        void Connector_GroupsDownloadEnded(List<Dictionary<string, List<KSGroup>>> result)
        {
            groups = result[0];
            foreach (var item in groups)
            {
                Courses.Add(new CourseViewModel() { CourseName = item.Key });
            }
        }
        public void ShowGroups(string course)
        {
            GroupsList = new ObservableCollection<GroupViewModel>();
            foreach(var item in groups[course])
            {
                GroupsList.Add(new GroupViewModel() { GroupName = item.Name, Id = item.Id });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}