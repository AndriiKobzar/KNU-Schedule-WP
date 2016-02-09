using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using KNU_Schedule.Logic;
using System.Windows;

namespace KNU_Schedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string[] daysTranslator = new string[7] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
        Dictionary<string, List<KSGroup>> groups = new Dictionary<string, List<KSGroup>>();
        Dictionary<string, List<List<KSSubject>>> weeksDictionary = new Dictionary<string, List<List<KSSubject>>>();
        IEnumerator<string> weeksEnumerator = null;

        public MainViewModel()
        {
            this.Days = new ObservableCollection<DayViewModel>();
            this.FacultyList = new ObservableCollection<FacultyViewModel>();
            this.GroupsList = new ObservableCollection<GroupViewModel>();
            this.CourseList = new ObservableCollection<CourseViewModel>();
            App.Connector.GroupsDownloadEnded += Connector_GroupsDownloadEnded;
            App.Connector.FacultiesDownloadEnded += Connector_FacultiesDownloadEnded;
            App.Connector.ScheduleDownloadEnded += Connector_ScheduleDownloadEnded;
            App.Connector.ScheduleDownloadBreaked += () =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show("Не вдалося завантажити розклад. Перевірте підключення до мережі."); });
            };
        }


        private ObservableCollection<GroupViewModel> groupslist;
        public ObservableCollection<GroupViewModel> GroupsList
        {
            get { return groupslist; }
            set { groupslist = value; NotifyPropertyChanged("GroupsList"); }
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

        public ObservableCollection<CourseViewModel> CourseList
        {
            get { return courses; }
            set
            {
                courses = value;
                NotifyPropertyChanged("CourseList");
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

        public void LoadSubjects(int groupId)
        {
            App.Connector.CreateTimetable(groupId);
        }

        private void Connector_ScheduleDownloadEnded(Dictionary<string, Dictionary<string, List<List<KSSubject>>>> schedule)
        {
            Days = new ObservableCollection<DayViewModel>();
            weeksDictionary = schedule["weeks"];
            weeksEnumerator = weeksDictionary.Keys.GetEnumerator();
            weeksEnumerator.MoveNext();
            ParseSchedule();
        }

        public void ChangeWeek()
        {
            ParseSchedule();
        }

        public void LoadGroups(int facultyId)
        {
            App.Connector.GetGroupsList(facultyId);
        }

        void Connector_GroupsDownloadEnded(List<Dictionary<string, List<KSGroup>>> result)
        {
            groups = new Dictionary<string, List<KSGroup>>();
            CourseList = new ObservableCollection<CourseViewModel>();
            GroupsList = new ObservableCollection<GroupViewModel>();
            if(result.Count!=0)
            groups = result[0];
            foreach (var item in groups)
            {
                CourseList.Add(new CourseViewModel() { CourseName = item.Key });
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
        private void ParseSchedule()
        {
            int dayCounter = 0;
            Days = new ObservableCollection<DayViewModel>();
            foreach (var dayData in weeksDictionary[weeksEnumerator.Current])
            {
                DayViewModel day = new DayViewModel() { Header = daysTranslator[dayCounter] };
                day.Subjects = new ObservableCollection<SubjectViewModel>();
                foreach (var subject in dayData)
                {
                    day.Subjects.Add(new SubjectViewModel()
                    {
                        Room = subject.Classrooms[0].ToString(),
                        Teacher = subject.Teachers[0].ToString(),
                        Title = subject.Name,
                        Time = DisplayPeriod(subject.Start, subject.End)
                    });
                }
                Days.Add(day);
                dayCounter++;
            }
            weeksEnumerator.MoveNext();
            if(weeksEnumerator.Current==null)
            {
                weeksEnumerator = weeksDictionary.Keys.GetEnumerator();
            }
        }
        //transfers from server method of keeping time to user-friendly
        private string DisplayPeriod(int start, int end)
        {
            int startHour = start / 60;
            int startMin = start % 60;
            int endHour = end / 60;
            int endMin = end % 60;
            return string.Format("{0}:{1} - {2}:{3}", startHour, startMin, endHour, endMin);
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