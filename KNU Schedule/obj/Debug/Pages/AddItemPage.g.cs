﻿#pragma checksum "D:\Study\KNU Schedule\KNU Schedule\Pages\AddItemPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B24F76A45D2785204A5645BCE2C8AADF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace KNU_Schedule {
    
    
    public partial class AddItemPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox SubjectName;
        
        internal System.Windows.Controls.TextBox TeacherName;
        
        internal System.Windows.Controls.TextBox Room;
        
        internal System.Windows.Controls.TextBox NumberOfPeriod;
        
        internal System.Windows.Controls.Button SubmitBtn;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/KNU%20Schedule;component/Pages/AddItemPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.SubjectName = ((System.Windows.Controls.TextBox)(this.FindName("SubjectName")));
            this.TeacherName = ((System.Windows.Controls.TextBox)(this.FindName("TeacherName")));
            this.Room = ((System.Windows.Controls.TextBox)(this.FindName("Room")));
            this.NumberOfPeriod = ((System.Windows.Controls.TextBox)(this.FindName("NumberOfPeriod")));
            this.SubmitBtn = ((System.Windows.Controls.Button)(this.FindName("SubmitBtn")));
        }
    }
}
