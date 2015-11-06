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
using System.IO.IsolatedStorage;

namespace KNU_Schedule
{
    public partial class TimetablePage : PhoneApplicationPage
    {

       //int currentItem = 0;
               
        public TimetablePage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
            //switch (DateTime.Now.DayOfWeek) // show timetable for this day
            //{
            //    case DayOfWeek.Tuesday: pivot.SelectedIndex = 1; break;
            //    case DayOfWeek.Wednesday: pivot.SelectedIndex = 2; break;
            //    case DayOfWeek.Thursday: pivot.SelectedIndex = 3; break;
            //    case DayOfWeek.Friday: pivot.SelectedIndex = 4; break;
            //    default: pivot.SelectedIndex = 0; break; // Monday or weekend
            //}
            
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // currentItem = pivot.SelectedIndex;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            KSController connector = App.Connector;
            connector.ScheduleDownloadBreaked += () => 
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => { MessageBox.Show("Не вдалося завантажити розклад. Перевірте підключення до мережі."); });
            };
            connector.ScheduleDownloadEnded += () =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        DownloadingBar.Visibility = Visibility.Collapsed;
                        App.ViewModel.LoadSubjects();
                        //this.pivot.DataContext = App.ViewModel.Days;
                    });
            };
       //     connector.CreateTimetable(IsolatedStorageSettings.ApplicationSettings[AppResources.GROUP_ID].ToString());
            connector.CreateTimetable("12");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).SelectedIndex = -1;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(AppResources.GROUP_ID);
            NavigationService.Navigate(new Uri("/Pages/MainPage.xaml", UriKind.Relative));
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}