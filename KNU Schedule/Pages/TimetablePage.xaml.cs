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
        
        Uri addUri = new Uri("/Pages/AddItemPage.xaml", UriKind.Relative);
        
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

        private void AddAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(addUri);
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentItem = pivot.SelectedIndex;
        }

        private void SaveAppBarButton_Click(object sender, EventArgs e)
        {
            App.Timetable.Save();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);            
            if(e.NavigationMode == NavigationMode.Back)
            {
                if (PhoneApplicationService.Current.State.ContainsKey(AppResources.Period) && PhoneApplicationService.Current.State.ContainsKey(AppResources.Subject))
                {
                    int period = (int)PhoneApplicationService.Current.State[AppResources.Period];
                    KSSubject subject = PhoneApplicationService.Current.State[AppResources.Subject] as KSSubject;

                    App.Timetable[currentItem, period-1] = subject;
                    App.ViewModel.LoadData();
                    this.pivot.DataContext = App.ViewModel.Days;
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).SelectedIndex = -1;
        }
    }
}