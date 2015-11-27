// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestManager.Kernel;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for PrerequisitePage.xaml
    /// </summary>
    public partial class AutoDetectionPage
    {
        PrerequisitView prerequisits;
        public void SetPrerequisits(PrerequisitView prerequisits)
        {
            this.prerequisits = prerequisits;
            Title = prerequisits.Title;
            PrerequisitesGrid.DataContext = prerequisits;
        }
       
        List<Detector.DetectingItem> detectionSteps;
        public void SetDetectionSteps(List<Detector.DetectingItem> steps)
        {
            detectionSteps = steps;
            StepsListBox.ItemsSource = steps;
        }

        public void ResetSteps()
        {
            foreach (var item in detectionSteps)
                item.DetectingStatus = Detector.DetectingStatus.Pending;
        }

        public AutoDetectionPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ContentChanged != null) ContentChanged(this, new EventArgs());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContentChanged != null) ContentChanged(this, new EventArgs());
        }


        public event EventHandler ContentChanged;

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Pages.util.OpenDetectionLog();
        }
    }
}
