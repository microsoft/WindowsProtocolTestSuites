// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Protocols.TestManager.RDPClientPlugin
{

    public partial class DetectionResultControl : UserControl
    {
        public DetectionResultControl()
        {
            InitializeComponent();
        }

        #region Properties

        private DetectionInfo info = null;

        private const string protocolDescription = "\"Protocols\" use the Dynamic Virutal Channel. Execution Console tries to establish a Dynamic Virtual Channel to detect which protocol is supported in AutoDetection Phase";
        private const string featureDescription = "\"Features\" are found supported or not supported by analyzing the flags set in Client MCS Connect Initial PDU and Client Confirm Active PDU.";

        private ResultItemMap protocolItems = new ResultItemMap() { Header = "Protocols Support Info", Description = protocolDescription };
        private ResultItemMap featureItems = new ResultItemMap() { Header = "Features Support Info", Description = featureDescription };
        
        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions

        private void AddResultItem(ref ResultItemMap resultItemMap, string value, bool? result)
        {
            string imagePath = string.Empty;
            DetectResult detectResult = new DetectResult();
            switch (result)
            {
                case true:
                    imagePath = "/RDPClientPlugin;component/Icons/supported.png"; ;
                    detectResult = DetectResult.Supported;
                    break;
                case false:
                    imagePath = "/RDPClientPlugin;component/Icons/unsupported.png";
                    detectResult = DetectResult.UnSupported;
                    break;
                case null:
                    imagePath = "/RDPClientPlugin;component/Icons/undetected.png";
                    detectResult = DetectResult.DetectFail;
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = detectResult, ImageUrl = imagePath, Name = value };
            resultItemMap.ResultItemList.Add(item);
        }

        #endregion

        #region Private events

        private void MapSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox selectedMap = sender as ListBox;

            //Keep all map headers unselected
            selectedMap.UnselectAll();
        }

        private void ItemSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox selectedList = sender as ListBox;

            if (selectedList != null && selectedList.SelectedIndex != -1)
            {
                SetOtherItemsUnselected(sender);

                ResultItem tempItem = (ResultItem)selectedList.SelectedItem;

                if (tempItem.DetectedResult == DetectResult.UnSupported)
                {
                    this.ItemDescription.Text = tempItem.Name + " is found not supported after detection";
                    return;
                }
                else if (tempItem.DetectedResult == DetectResult.Supported)
                {
                    this.ItemDescription.Text = tempItem.Name + " is found supported after detection";
                    return;
                }
                else
                {
                    this.ItemDescription.Text = "Because of detection failure " + tempItem.Name + " is not detected successfully";
                    return;
                }
            }
        }

        private void SetOtherItemsUnselected(object sender)
        {
            ListBox selectedList = sender as ListBox;
            ResultItem selectedItem = (ResultItem)selectedList.SelectedItem;

            foreach (object obj in this.ResultMapList.Items)
            {
                //Find the controls in the DataTemplate with the help of VisualTreeHelper class
                ListBoxItem lbi = this.ResultMapList.ItemContainerGenerator.ContainerFromItem(obj) as ListBoxItem;
                ContentPresenter tempContentPresenter = FindVisualChild<ContentPresenter>(lbi);
                if (tempContentPresenter != null)
                {
                    DataTemplate tempDataTemplate = tempContentPresenter.ContentTemplate;
                    Expander mapHeader = tempDataTemplate.FindName("ResultMapHeader", tempContentPresenter) as Expander;
                    ListBox itemList = tempDataTemplate.FindName("ResultItemList", tempContentPresenter) as ListBox;
                    
                    //Keep the current selection
                    if (!itemList.Items.Contains(selectedItem))
                        itemList.UnselectAll();
                }
            }
        }

        private void ResultMapHeader_Collapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;

            foreach (object obj in this.ResultMapList.Items)
            {
                ListBoxItem lbi = this.ResultMapList.ItemContainerGenerator.ContainerFromItem(obj) as ListBoxItem;
                ContentPresenter tempContentPresenter = FindVisualChild<ContentPresenter>(lbi);
                if (tempContentPresenter != null)
                {
                    DataTemplate tempDataTemplate = tempContentPresenter.ContentTemplate;
                    Expander mapHeader = tempDataTemplate.FindName("ResultMapHeader", tempContentPresenter) as Expander;
                    ListBox itemList = tempDataTemplate.FindName("ResultItemList", tempContentPresenter) as ListBox;

                    //Find the target list and clear the selection
                    if (expander.Header == mapHeader.Header)
                    {
                        if (itemList.SelectedIndex != -1)
                            this.ItemDescription.Text = string.Empty;

                        itemList.UnselectAll();
                        break;
                    }
                }
            }
        }

        private void ResultMapHeader_MouseEnter(object sender, MouseEventArgs e)
        {
            Expander mapHeader = sender as Expander;
            foreach (ResultItemMap map in resultItemMapList)
            {
                if (map.Header == mapHeader.Header.ToString())
                {
                    this.ItemDescription.Visibility = System.Windows.Visibility.Collapsed;
                    this.MapDescription.Visibility = System.Windows.Visibility.Visible;
                    this.MapDescription.Text = map.Description;
                }
            }
        }

        private void ResultMapHeader_MouseLeave(object sender, MouseEventArgs e)
        {
            this.MapDescription.Visibility = System.Windows.Visibility.Collapsed;
            this.ItemDescription.Visibility = System.Windows.Visibility.Visible;
        }

        private void ResultMapList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.MapDescription.Visibility = System.Windows.Visibility.Collapsed;
            this.ItemDescription.Visibility = System.Windows.Visibility.Visible;
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = sender as ScrollViewer;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        #endregion

        public void LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            this.info = detectionInfo;

            // Add protocolItems
            AddResultItem(ref protocolItems, "MS-RDPBCGR", true);
            AddResultItem(ref protocolItems, "MS-RDPEDISP", detectionInfo.IsSupportRDPEDISP);
            AddResultItem(ref protocolItems, "MS-RDPEGFX", detectionInfo.IsSupportRDPEGFX);
            AddResultItem(ref protocolItems, "MS-RDPEI", detectionInfo.IsSupportRDPEI);
            AddResultItem(ref protocolItems, "MS-RDPEMT", detectionInfo.IsSupportRDPEMT);
            AddResultItem(ref protocolItems, "MS-RDPEUDP", detectionInfo.IsSupportRDPEUDP);
            AddResultItem(ref protocolItems, "MS-RDPEUSB", detectionInfo.IsSupportRDPEUSB);
            AddResultItem(ref protocolItems, "MS-RDPEVOR", detectionInfo.IsSupportRDPEVOR);
            AddResultItem(ref protocolItems, "MS-RDPRFX", detectionInfo.IsSupportRDPRFX);
            AddResultItem(ref protocolItems, "MS-RDPEFS (Used to test static virtual channel)", detectionInfo.IsSupportRDPEFS);

            // Add featuresItems

            AddResultItem(ref featureItems, "Auto Reconnect", detectionInfo.IsSupportAutoReconnect);
            AddResultItem(ref featureItems, "Server Redirection", detectionInfo.IsSupportServerRedirection);
            AddResultItem(ref featureItems, "Network Characteristics Detection", detectionInfo.IsSupportNetcharAutoDetect);
            
            AddResultItem(ref featureItems, "Connection Health Monitoring", detectionInfo.IsSupportHeartbeatPdu);
            AddResultItem(ref featureItems, "Static Virtual Channels", detectionInfo.IsSupportStaticVirtualChannel);
            AddResultItem(ref featureItems, "RDP-UDP Forward Error Correction reliable transport", detectionInfo.IsSupportTransportTypeUdpFECR);
            AddResultItem(ref featureItems, "RDP-UDP Forward Error Correction lossy transport", detectionInfo.IsSupportTransportTypeUdpFECL);
            this.resultItemMapList.Add(protocolItems);
            this.resultItemMapList.Add(featureItems);
            ResultMapList.ItemsSource = resultItemMapList;
        }
    }
}
