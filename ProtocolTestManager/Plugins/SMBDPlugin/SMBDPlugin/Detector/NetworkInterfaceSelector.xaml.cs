using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Protocols.TestManager.FileServerPlugin.Detector
{
    /// <summary>
    /// Interaction logic for NetworkInterfaceSelector.xaml
    /// </summary>
    public partial class NetworkInterfaceSelector : Window
    {
        public LocalNetworkInterfaceInformation[] LocalNetworkInterfaceInformation;

        private object selectedItem;



        public NetworkInterfaceSelector(LocalNetworkInterfaceInformation[] networkInterfaces)
        {
            LocalNetworkInterfaceInformation = networkInterfaces;
            InitializeComponent();
            Viewer.ItemsSource = LocalNetworkInterfaceInformation;
            OkButton.IsEnabled = false;
        }


        public new LocalNetworkInterfaceInformation ShowDialog()
        {
            var ret = base.ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                return selectedItem as LocalNetworkInterfaceInformation;
            }
            else
            {
                return null;
            }
        }

        private void Viewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OkButton.IsEnabled = true;
            selectedItem = Viewer.SelectedItem;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
