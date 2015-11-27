// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {

        public WelcomePage()
        {
            InitializeComponent();
        }

        public void LoadPage(Uri uri)
        {
            WebBrowserUserGuide.Url = uri;
        }

        private void Browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            DocumentTitle.Text = GetPropertyValue(WebBrowserUserGuide.Document, "Title");
            if (WebBrowserUserGuide.CanGoBack)
            {
                BackLinkBlock.Visibility = System.Windows.Visibility.Visible;
                BackLink.IsEnabled = true;
                BackLink.Focusable = true;

            }
            else
            {
                BackLinkBlock.Visibility = System.Windows.Visibility.Collapsed;
                BackLink.Focusable = false;
                BackLink.IsEnabled = false;
            }

        }
        private string GetPropertyValue(object obj, string propertyName)
        {
            Type objectType = obj.GetType();
            PropertyInfo propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo == null) return "";
            Type propertyType = propertyInfo.PropertyType;
            object o = propertyInfo.GetValue(obj, null);
            return o.ToString();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (WebBrowserUserGuide.CanGoBack)
            {
                WebBrowserUserGuide.GoBack();
            }
        }
    }
}
