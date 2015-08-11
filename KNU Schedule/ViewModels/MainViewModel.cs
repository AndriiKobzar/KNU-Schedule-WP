using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using KNU_Schedule.Resources;
using System.Collections.Generic;
using KNU_Schedule.Logic;

namespace KNU_Schedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string[] days = new string[5] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };
        string[] periodTimes = new string[5] {"8:40 - 10:15","10:35 - 12:10","12:20 - 13:55","14:05 - 15:40"," " };

        public MainViewModel()
        {
            this.Days = new ObservableCollection<DayViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<DayViewModel> Days 
        { get; private set;
        }
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
            Days = new ObservableCollection<DayViewModel>();
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
                Days.Add(new DayViewModel() { Header = days[i], Subjects = subj,  });
            }
            this.IsDataLoaded = true;
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