using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using KNU_Schedule.ViewModels;
using System.IO.IsolatedStorage;
using KNU_Schedule.Resources;

namespace KNU_Schedule
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(AppResources.GROUP_ID))
                NavigationService.Navigate(new Uri("/Pages/TimetablePage.xaml", UriKind.Relative));
            else App.ViewModel.LoadFaculties();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            //string groupName = (GroupPicker.SelectedItem as GroupViewModel).GroupName;
            //if (!IsolatedStorageSettings.ApplicationSettings.Contains(AppResources.GROUP_ID))
            //    IsolatedStorageSettings.ApplicationSettings.Add(AppResources.GROUP_ID, App.ViewModel.IdByName(groupName));
            //else
            //    IsolatedStorageSettings.ApplicationSettings[AppResources.GROUP_ID] = App.ViewModel.IdByName(groupName);
            NavigationService.Navigate(new Uri("/Pages/TimetablePage.xaml", UriKind.Relative));
        }

        private void FacultyPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FacultyPicker.SelectedItem != null)
                App.ViewModel.LoadGroups((FacultyPicker.SelectedItem as FacultyViewModel).ID);
        }

        private void coursePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (coursePicker.SelectedItem != null)
                App.ViewModel.ShowGroups((coursePicker.SelectedItem as CourseViewModel).CourseName);
        }
    }
}