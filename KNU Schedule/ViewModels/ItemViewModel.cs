using KNU_Schedule.Logic;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace KNU_Schedule.ViewModels
{
    public class SubjectViewModel : INotifyPropertyChanged
    {
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (value != title)
                {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string teacher;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Teacher
        {
            get
            {
                return teacher;
            }
            set
            {
                if (value != teacher)
                {
                    teacher = value;
                    NotifyPropertyChanged("Teacher");
                }
            }
        }

        private string room;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string Room
        {
            get
            {
                return room;
            }
            set
            {
                if (value != room)
                {
                    room = value;
                    NotifyPropertyChanged("Room");
                }
            }
        }
        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                NotifyPropertyChanged("Time");
            }
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
    public class DayViewModel:INotifyPropertyChanged
    {
        string header;
        public string Header
        {
            get { return header; }
            set 
            { 
                if (header!=value)
                {
                    header = value;
                    NotifyPropertyChanged("Header");
                }
            }
        }
        ObservableCollection<SubjectViewModel> subjects;
        public ObservableCollection<SubjectViewModel> Subjects
        {
            get { return subjects; }
            set
            {
                if (subjects != value)
                {
                    subjects = value;
                    NotifyPropertyChanged("Subjects");
                }
            }
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
    public class FacultyViewModel:INotifyPropertyChanged
    {
        string name = "";
        public string FacultyName
        {
            get { return name; }
            set 
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        string id = null;
        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
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
        public override string ToString()
        {
            return this.name;
        }
    }
    public class GroupViewModel : INotifyPropertyChanged
    {
        string name = "";

        public string GroupName
        {
            get { return name; }
            set 
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
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

        public override string ToString()
        {
            return this.name;
        }
    }
}