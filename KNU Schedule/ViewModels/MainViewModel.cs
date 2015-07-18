using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using KNU_Schedule.Resources;

namespace KNU_Schedule.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        string[] days = new string[5] { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };

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
                for (int j = 0; j < 4; j++)
                {
                    if (App.Timetable[i, j] == null)
                        subj.Add(null);
                    else subj.Add(new SubjectViewModel() { Teacher = App.Timetable[i, j].Teacher, Title = App.Timetable[i, j].Name, Room = App.Timetable[i, j].Room });
                }
                Days.Add(new DayViewModel() { Header = days[i], Subjects = subj });
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