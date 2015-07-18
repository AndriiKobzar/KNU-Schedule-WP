using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using KNU_Schedule.Resources;

namespace KNU_Schedule
{
    public partial class AddItemPage : PhoneApplicationPage
    {
        public AddItemPage()
        {
            InitializeComponent();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectName.Text != "" && TeacherName.Text != "" && Room.Text != "" && NumberOfPeriod.Text != "")
            {
                KNU_Schedule.Logic.KSSubject subject = new Logic.KSSubject(SubjectName.Text, TeacherName.Text, Room.Text);
                PhoneApplicationService.Current.State[AppResources.Subject] = subject;
                PhoneApplicationService.Current.State[AppResources.Period] = int.Parse(NumberOfPeriod.Text);
                NavigationService.GoBack();
            }
            else MessageBox.Show("Введіть дані!");
        }
    }
}