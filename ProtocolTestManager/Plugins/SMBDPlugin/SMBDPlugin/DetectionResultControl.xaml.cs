// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.SMBDPlugin.Detector;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Protocols.TestManager.SMBDPlugin
{

    public partial class DetectionResultControl : UserControl
    {
        public DetectionResultControl()
        {
            InitializeComponent();
        }

        public void LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            info = detectionInfo;

            AddDialect();
            AddSmb2TransportSupport();

            resultItemMapList.Add(dialectsItems);
            resultItemMapList.Add(smbdItems);

            ResultMapList.ItemsSource = resultItemMapList;
        }

        #region Properties

        private DetectionInfo info = null;

        private const string dialectsDescription = @"""SMB Version Supported"" lists the SMB dialects supported by SUT SMB2 over SMBD implementation and at least SMB dialect 3.0 is required.";
        private const string smbdDescription = @"""SMB2 over SMBD Feature Supported"" lists the feature supported by SUT SMB2 over SMBD implementation";


        private ResultItemMap dialectsItems = new ResultItemMap() { Header = "SMB Version Supported", Description = dialectsDescription };
        private ResultItemMap smbdItems = new ResultItemMap() { Header = "SMB2 over SMBD Feature Supported", Description = smbdDescription };

        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions
        private void AddDialect()
        {
            foreach (var dialect in info.SupportedSmbDialects)
            {
                string dialectName = null;
                switch (dialect)
                {
                    case DialectRevision.Smb30:
                        dialectName = "SMB 3.0";
                        break;
                    case DialectRevision.Smb302:
                        dialectName = "SMB 3.0.2";
                        break;
                    case DialectRevision.Smb311:
                        dialectName = "SMB 3.1.1";
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected dialect!");
                }
                AddResultItem(dialectsItems, dialectName, DetectResult.Supported);
            }
        }

        private void AddSmb2TransportSupport()
        {
            if (info.DriverNonRdmaNICIPAddress == null || info.SUTNonRdmaNICIPAddress == null)
            {
                AddResultItem(smbdItems, "Multiple Channels", DetectResult.DetectFail);
            }
            else
            {
                if (info.NonRDMATransportSupported && info.RDMATransportSupported)
                {
                    AddResultItem(smbdItems, "Multiple Channels", DetectResult.Supported);
                }
                else
                {
                    AddResultItem(smbdItems, "Multiple Channels", DetectResult.UnSupported);
                }
            }

            if (info.DriverRdmaNICIPAddress == null || info.SUTRdmaNICIPAddress == null)
            {
                AddResultItem(smbdItems, "RDMA Channel V1", DetectResult.DetectFail);
                AddResultItem(smbdItems, "RDMA Channel V1 Remote Invalidate", DetectResult.DetectFail);
            }
            else
            {
                if (info.RDMAChannelV1Supported)
                {
                    AddResultItem(smbdItems, "RDMA Channel V1", DetectResult.Supported);
                }
                else
                {
                    AddResultItem(smbdItems, "RDMA Channel V1", DetectResult.UnSupported);
                }

                if (info.RDMAChannelV1InvalidateSupported)
                {
                    AddResultItem(smbdItems, "RDMA Channel V1 Remote Invalidate", DetectResult.Supported);
                }
                else
                {
                    AddResultItem(smbdItems, "RDMA Channel V1 Remote Invalidate", DetectResult.UnSupported);
                }
            }

        }

        private void AddResultItem(ResultItemMap resultItemMap, string value, DetectResult result)
        {
            string imagePath = string.Empty;
            switch (result)
            {
                case DetectResult.Supported:
                    imagePath = "/SMBDPlugin;component/Icons/supported.png";
                    break;
                case DetectResult.UnSupported:
                    imagePath = "/SMBDPlugin;component/Icons/unsupported.png";
                    break;
                case DetectResult.DetectFail:
                    imagePath = "/SMBDPlugin;component/Icons/undetected.png";
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = result, ImageUrl = imagePath, Name = value };
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

            if (selectedList.SelectedIndex != -1)
            {
                SetOtherItemsUnselected(sender);

                ResultItem tempItem = (ResultItem)selectedList.SelectedItem;

                switch (tempItem.DetectedResult)
                {
                    case DetectResult.UnSupported:
                        ItemDescription.Text = String.Format("{0} is found not supported after detection", tempItem.Name);
                        break;
                    case DetectResult.Supported:
                        ItemDescription.Text = String.Format("{0} is found supported after detection", tempItem.Name);
                        break;
                    case DetectResult.DetectFail:
                        ItemDescription.Text = String.Format("Failed to detect {0}", tempItem.Name);
                        break;
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

    }
}
