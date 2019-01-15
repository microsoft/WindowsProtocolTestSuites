//------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//------------------------------------------------------------------------------

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
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestManager.BranchCachePlugin
{
    public partial class DetectionResultControl : UserControl
    {
        /// <summary>
        /// Construct
        /// </summary>
        public DetectionResultControl()
        {
            InitializeComponent();
        }

        #region Properties

        private DetectionInfo info = null;

        private const string shareHashGenerationSuportedDecription = "\"Hash Supported\" is the share flags returned in Tree Connect response by server.";
        private const string versionSupportedDescription = "\"BranchCache Version Supported\" is the selected one in SRV_READ_HASH response by server.";        

        private ResultItemMap hashSuportedItems = new ResultItemMap() { Header = "Share hash generation supported", Description = shareHashGenerationSuportedDecription };
        private ResultItemMap versionSupportedItems = new ResultItemMap() { Header = "Branch cache version supported", Description = versionSupportedDescription };      

        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions
        private void AddShareHashInfo(ShareHashGeneration shareHashGeneration)
        {
            AddResultItem(ref this.hashSuportedItems, shareHashGeneration.ToString(), this.info.ShareInformation.shareHashGeneration.HasFlag(shareHashGeneration) ? DetectResult.Supported : DetectResult.UnSupported);
        }

        private void AddBranchCacheVersionInfo(BranchCacheVersion version)
        {
            AddResultItem(ref this.versionSupportedItems, version.ToString(), this.info.VersionInformation.branchCacheVersion.HasFlag(version)? DetectResult.Supported:DetectResult.UnSupported);
        }

        private void AddResultItem(ref ResultItemMap resultItemMap, string name, DetectResult result)
        {
            string imagePath = string.Empty;
            switch (result)
            {
                case DetectResult.Supported:
                    imagePath = "/BranchCachePlugin;component/Icons/supported.png";
                    break;
                case DetectResult.UnSupported:
                    imagePath = "/BranchCachePlugin;component/Icons/unsupported.png";
                    break;
                case DetectResult.DetectFail:
                    imagePath = "/BranchCachePlugin;component/Icons/undetected.png";
                    break;
                default:
                    break;
            }
            ResultItem item = new ResultItem(result,imagePath,name);
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

                if (tempItem.DetectedResult == DetectResult.UnSupported)
                {
                    this.ItemDescription.Text = tempItem.Name + " is failed to detect or not supported.";
                    return;
                }
                if (!info.DetectExceptions.ContainsKey(tempItem.Name))
                {
                    this.ItemDescription.Text = tempItem.Name + " is detected.";
                    return;
                }
                string log = info.DetectExceptions[tempItem.Name];
                if (!string.IsNullOrEmpty(log))
                {
                    this.ItemDescription.Text = log;
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

        /// <summary>
        /// Load the detected information and bind to the data source
        /// </summary>
        /// <param name="detectionInfo"></param>
        public void LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            this.info = detectionInfo;

            //Add/Update detected share hash generation supported info
            AddShareHashInfo(ShareHashGeneration.V1Enabled);
            AddShareHashInfo(ShareHashGeneration.V2Enabled);

            //Add/Update detected branch cache version supported info
            AddBranchCacheVersionInfo(BranchCacheVersion.BranchCacheVersion1);
            AddBranchCacheVersionInfo(BranchCacheVersion.BranchCacheVersion2);

            //Bind the data to the control
            resultItemMapList.Add(versionSupportedItems);
            resultItemMapList.Add(hashSuportedItems);
            ResultMapList.ItemsSource = resultItemMapList;
        }
    }
}
