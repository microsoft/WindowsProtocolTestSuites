// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos;
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

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{

    public partial class DetectionResultControl : UserControl
    {
        public DetectionResultControl()
        {
            InitializeComponent();
        }

        public void LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            this.info = detectionInfo;

            // Add/Update detected Dialects
            AddDialect(this.info.smb2Info.MaxSupportedDialectRevision);

            // Add/Update detected Capablities
            AddCapability(Capabilities_Values.GLOBAL_CAP_DFS, "DFS (Distributed File System)");
            AddCapability(Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING, "Directory Leasing");
            AddCapability(Capabilities_Values.GLOBAL_CAP_ENCRYPTION, "Encryption");
            AddCapability(Capabilities_Values.GLOBAL_CAP_LARGE_MTU, "Large MTU (multi-credit operations)");
            AddCapability(Capabilities_Values.GLOBAL_CAP_LEASING, "Leasing");
            AddCapability(Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, "Multiple Channel");
            AddCapability(Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES, "Persistent Handle");

            // Add compression capabilities
            AddCompressionCapabilities(detectionInfo);

            // Add/Update detected IoCtl codes
            AddIoctlCode(CtlCode_Values.FSCTL_OFFLOAD_READ, this.info.F_CopyOffload[0]);
            AddIoctlCode(CtlCode_Values.FSCTL_OFFLOAD_WRITE, this.info.F_CopyOffload[1]);
            AddIoctlCode(CtlCode_Values.FSCTL_FILE_LEVEL_TRIM, this.info.F_FileLevelTrim);
            AddIoctlCode(CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION, this.info.F_IntegrityInfo[0]);
            AddIoctlCode(CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION, this.info.F_IntegrityInfo[1]);
            AddIoctlCode(CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY, this.info.F_ResilientHandle);
            AddIoctlCode(CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO, this.info.F_ValidateNegotiateInfo);
            AddIoctlCode(CtlCode_Values.FSCTL_SRV_ENUMERATE_SNAPSHOTS, this.info.F_EnumerateSnapShots);

            // Add/Update detected Create Contexts
            AddCreateContext(CreateContextTypeValue.SMB2_CREATE_APP_INSTANCE_ID, this.info.F_AppInstanceId);
            AddCreateContext(
                CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST,
                (info.F_HandleV1_BatchOplock == DetectResult.Supported || info.F_HandleV1_LeaseV1 == DetectResult.Supported) ?
                DetectResult.Supported : DetectResult.UnSupported);
            AddCreateContext(
                CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT,
                (info.F_HandleV1_BatchOplock == DetectResult.Supported || info.F_HandleV1_LeaseV1 == DetectResult.Supported) ?
                DetectResult.Supported : DetectResult.UnSupported);
            AddCreateContext(
                CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2,
                (info.F_HandleV2_BatchOplock == DetectResult.Supported
                || info.F_HandleV2_LeaseV1 == DetectResult.Supported
                || info.F_HandleV2_LeaseV2 == DetectResult.Supported) ?
                DetectResult.Supported : DetectResult.UnSupported);
            AddCreateContext(
                CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2,
                (info.F_HandleV2_BatchOplock == DetectResult.Supported
                || info.F_HandleV2_LeaseV1 == DetectResult.Supported
                || info.F_HandleV2_LeaseV2 == DetectResult.Supported) ?
                DetectResult.Supported : DetectResult.UnSupported);
            AddCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE, this.info.F_Leasing_V1);
            AddCreateContext(CreateContextTypeValue.SMB2_CREATE_REQUEST_LEASE_V2, this.info.F_Leasing_V2);

            // Add/Updata detected RSVD/SQOS version
            AddRSVD(info);
            AddSQOS(info);

            //Bind the data to the control
            resultItemMapList.Add(dialectsItems);
            resultItemMapList.Add(capabilitiesItems);
            resultItemMapList.Add(compressionItems);
            resultItemMapList.Add(ioctlCodesItems);
            resultItemMapList.Add(createContextsItems);
            resultItemMapList.Add(rsvdItems);
            resultItemMapList.Add(sqosItems);
            ResultMapList.ItemsSource = resultItemMapList;
        }

        #region Properties

        private DetectionInfo info = null;

        private const string dialectsDescription = "\"Max Supported Dialect\" is the selected one in the Negotiate Response by server when a Negotiate Request is sent to SUT with Dialects Smb2.002, Smb2.1, Smb3.0 and Smb3.02.";
        private const string capabilitiesDescription = "\"Capabilities\" are found supported or not supported by analyzing the flags set in Negotiate Response when a Negotiate Request is sent to SUT with all defined flags in TD set in Capabilities field.";
        private const string ioctlCodesDescription = "\"IoCtl Codes\" are found supported or not supported by analyzing IOCTL Responses when the following IOCTL Requests are sent to SUT.";
        private const string createContextsDescription = "\"Creat Contexts\" are found supported or not supported by analyzing Create Responses when the Create Requests with the following create contexts are sent to SUT.";
        private const string rsvdDescription = "\"RSVD Implementation\" is detected by sending Create Request with SVHDX_OPEN_DEVICE_CONTEXT\\SVHDX_OPEN_DEVICE_CONTEXT_V2.";
        private const string sqosDescription = "\"SQOS Implementation\" is detected by sending SQOS get status request.";
        private const string compressionDescription = "\"Supported SMB2 compression algorithms\" is detected by sending NEGOTIATE request with compression negotiate context when SMB2 dialect is greater than 3.1.1.";

        private ResultItemMap dialectsItems = new ResultItemMap() { Header = "Max Smb Version Supported", Description = dialectsDescription };
        private ResultItemMap capabilitiesItems = new ResultItemMap() { Header = "Capabilities", Description = capabilitiesDescription };
        private ResultItemMap ioctlCodesItems = new ResultItemMap() { Header = "IoCtl Codes", Description = ioctlCodesDescription };
        private ResultItemMap createContextsItems = new ResultItemMap() { Header = "Create Contexts", Description = createContextsDescription };

        private ResultItemMap rsvdItems = new ResultItemMap() { Header = "Remote Shared Virtual Disk (RSVD)", Description = rsvdDescription };
        private ResultItemMap sqosItems = new ResultItemMap() { Header = "Storage Quality of Service (SQOS)", Description = sqosDescription };

        private ResultItemMap compressionItems = new ResultItemMap() { Header = "SMB2 Compression Feature", Description = compressionDescription };

        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions

        private void AddDialect(DialectRevision dialect)
        {
            string maxSmbVersionSupported = string.Empty;
            switch (dialect)
            {
                case DialectRevision.Smb2002:
                    maxSmbVersionSupported = "Smb 2.002";
                    break;
                case DialectRevision.Smb21:
                    maxSmbVersionSupported = "Smb 2.1";
                    break;
                case DialectRevision.Smb30:
                    maxSmbVersionSupported = "Smb 3.0";
                    break;
                case DialectRevision.Smb302:
                    maxSmbVersionSupported = "Smb 3.02";
                    break;
                case DialectRevision.Smb311:
                    maxSmbVersionSupported = "Smb 3.1.1";
                    break;
                case DialectRevision.Smb2Wildcard:
                    break;
                case DialectRevision.Smb2Unknown:
                    break;
                default:
                    break;
            }
            AddResultItem(ref this.dialectsItems, maxSmbVersionSupported, DetectResult.Supported);
        }

        private void AddCapability(Capabilities_Values capabilityName, string featureName)
        {
            if (info.CheckHigherDialect(info.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb311) && capabilityName == Capabilities_Values.GLOBAL_CAP_ENCRYPTION)
            {
                AddResultItem(ref this.capabilitiesItems, featureName, info.smb2Info.SelectedCipherID > EncryptionAlgorithm.ENCRYPTION_NONE ? DetectResult.Supported : DetectResult.UnSupported);
            }
            else
            {
                AddResultItem(ref this.capabilitiesItems, featureName, this.info.smb2Info.SupportedCapabilities.HasFlag(capabilityName) ? DetectResult.Supported : DetectResult.UnSupported);
            }
        }

        private void AddIoctlCode(CtlCode_Values value, DetectResult result)
        {
            AddResultItem(ref this.ioctlCodesItems, value.ToString(), result);
        }

        private void AddCreateContext(CreateContextTypeValue value, DetectResult result)
        {
            AddResultItem(ref this.createContextsItems, value.ToString(), result);
        }

        private void AddResultItem(ref ResultItemMap resultItemMap, string value, DetectResult result)
        {
            string imagePath = string.Empty;
            switch (result)
            {
                case DetectResult.Supported:
                    imagePath = "/FileServerPlugin;component/Icons/supported.png";
                    break;
                case DetectResult.UnSupported:
                    imagePath = "/FileServerPlugin;component/Icons/unsupported.png";
                    break;
                case DetectResult.DetectFail:
                    imagePath = "/FileServerPlugin;component/Icons/undetected.png";
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = result, ImageUrl = imagePath, Name = value };
            resultItemMap.ResultItemList.Add(item);
        }

        private void AddRSVD(DetectionInfo info)
        {
            if (info.RsvdSupport == DetectResult.Supported)
            {
                if (info.RsvdVersion == RSVD_PROTOCOL_VERSION.RSVD_PROTOCOL_VERSION_1)
                {
                    AddResultItem(ref this.rsvdItems, "RSVD Protocol version 1", DetectResult.Supported);
                    AddResultItem(ref this.rsvdItems, "RSVD Protocol version 2", DetectResult.UnSupported);
                }
                else
                {
                    AddResultItem(ref this.rsvdItems, "RSVD Protocol version 1", DetectResult.Supported);
                    AddResultItem(ref this.rsvdItems, "RSVD Protocol version 2", DetectResult.Supported);
                }
            }
            // DetectResult.UnSupported and DetectResult.DetectFail
            else
            {
                AddResultItem(ref this.rsvdItems, "RSVD", info.RsvdSupport);
            }
        }

        private void AddSQOS(DetectionInfo info)
        {
            if (info.SqosSupport == DetectResult.Supported)
            {
                if (info.SqosVersion == SQOS_PROTOCOL_VERSION.Sqos10)
                {
                    AddResultItem(ref this.sqosItems, "SQOS dialect 1.0", DetectResult.Supported);
                    AddResultItem(ref this.sqosItems, "SQOS dialect 1.1", DetectResult.UnSupported);
                }
                else
                {
                    AddResultItem(ref this.sqosItems, "SQOS dialect 1.0", DetectResult.Supported);
                    AddResultItem(ref this.sqosItems, "SQOS dialect 1.1", DetectResult.Supported);
                }
            }
            // DetectResult.UnSupported and DetectResult.DetectFail
            else
            {
                AddResultItem(ref this.sqosItems, "SQOS", info.SqosSupport);
            }
        }

        private void AddCompressionCapabilities(DetectionInfo info)
        {
            var possibleCompressionAlogrithms = new CompressionAlgorithm[] { CompressionAlgorithm.LZ77, CompressionAlgorithm.LZ77Huffman, CompressionAlgorithm.LZNT1 };

            foreach (var compressionAlgorithm in possibleCompressionAlogrithms)
            {
                if (info.smb2Info.SupportedCompressionAlgorithms.Contains(compressionAlgorithm))
                {
                    AddResultItem(ref this.compressionItems, compressionAlgorithm.ToString(), DetectResult.Supported);
                }
                else
                {
                    AddResultItem(ref this.compressionItems, compressionAlgorithm.ToString(), DetectResult.UnSupported);
                }
            }
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
                    this.ItemDescription.Text = tempItem.Name + " is found not supported after detection";
                    return;
                }
                else if (tempItem.DetectedResult == DetectResult.DetectFail)
                {
                    this.ItemDescription.Text = "Detection failed";
                    return;
                }
                if (!info.detectExceptions.ContainsKey(tempItem.Name))
                {
                    this.ItemDescription.Text = tempItem.Name + " is found supported after detection";
                    return;
                }
                string log = info.detectExceptions[tempItem.Name];
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

    }
}
