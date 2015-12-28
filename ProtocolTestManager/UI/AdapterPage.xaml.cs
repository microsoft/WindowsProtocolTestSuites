// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Protocols.TestManager.UI
{
    /// <summary>
    /// Interaction logic for AdapterPage.xaml
    /// </summary>
    public partial class AdapterPage : Page
    {
        public AdapterPage()
        {
            InitializeComponent();
        }
        public void SetDatasource(IEnumerable obj)
        {
            AdaptersList.ItemsSource = obj;
        }
    }
}
