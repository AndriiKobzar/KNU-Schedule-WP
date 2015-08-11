using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KNU_Schedule.Logic;
using KNU_Schedule.Resources;

namespace KNU_Schedule
{
    public partial class TimetablePage : PhoneApplicationPage
    {

        int currentItem = 0;
               
        public TimetablePage()
        {
            InitializeComponent();
            pivot.DataContext = App.ViewModel.Days;
            switch(DateTime.Now.DayOfWeek) // show timetable for today
            {
                case DayOfWeek.Tuesday: pivot.SelectedIndex = 1; break;
                case DayOfWeek.Wednesday: pivot.SelectedIndex = 2; break;
                case DayOfWeek.Thursday: pivot.SelectedIndex = 3; break;
                case DayOfWeek.Friday: pivot.SelectedIndex = 4; break;
                default: pivot.SelectedIndex = 0; break; // Monday or weekend
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentItem = pivot.SelectedIndex;
        }

        private void SaveAppBarButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.LoadData();
            this.pivot.DataContext = App.ViewModel.Days;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            KSConnector connector = new KSConnector(App.Timetable,"22");
            connector.DownloadStarted += () => {  };
            connector.DownloadEnded += () =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        DownloadingBar.Visibility = Visibility.Collapsed;
                        App.ViewModel.LoadData();
                        this.pivot.DataContext = App.ViewModel.Days;
                    });
            };
            connector.CreateTimetable();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).SelectedIndex = -1;
        }
    }
}