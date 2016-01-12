// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
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

namespace Microsoft.Protocols.TestManager.KerberosPlugin
{

    public partial class DetectionResultControl : UserControl
    {
        public DetectionResultControl()
        {
            InitializeComponent();
        }

        #region Properties

        private DetectionInfo info = null;
        private const string commonInfoDescription = "Common Information";  
        private const string localDomainDescription  = "Local Domain Information";        
        private const string localDCDescription  = "Local DC Information"; 
        private const string localClientDescription  = "Local Client Information"; 
        private const string localAPDescription  = "Local Application Server Information";
        private const string localUsersDescription = "Local Domain Users Information";
        private const string trustDomainDescription = "Trust Domain Information"; 
        private const string trustDCDescription  = "Trust DC Information"; 
        private const string trustAPDescription  = "Trust Application Server Information";
        private const string trustUsersDescription = "Trust Domain Users Information";

        private ResultItemMap commonItems  = new ResultItemMap(){Header = commonInfoDescription, Description = commonInfoDescription};
        private ResultItemMap localDomainCommonItems  = new ResultItemMap(){Header = localDomainDescription, Description = localDomainDescription};
        private ResultItemMap trustDomainCommonItems = new ResultItemMap() { Header = trustDomainDescription, Description = trustDomainDescription };        
            
        private ResultItemMap localDCItems  = new ResultItemMap(){Header = localDCDescription, Description = localDCDescription};
        private ResultItemMap localClientItems  = new ResultItemMap(){Header = localClientDescription, Description = localClientDescription};
        private ResultItemMap localAPItems  = new ResultItemMap(){Header = localAPDescription, Description = localAPDescription};
        private ResultItemMap localUsesItems = new ResultItemMap() { Header = localUsersDescription, Description = localUsersDescription }; 

        private ResultItemMap trustDCItems  = new ResultItemMap(){Header = trustDCDescription, Description = trustDCDescription};
        private ResultItemMap trustAPItems  = new ResultItemMap(){Header = trustAPDescription, Description = trustAPDescription};
        private ResultItemMap trustUsesItems = new ResultItemMap() { Header = trustUsersDescription, Description = trustUsersDescription }; 

        private List<ResultItemMap> resultItemMapList = new List<ResultItemMap>();

        #endregion

        #region Private functions

        private void AddUsers()
        {
            AddUserList(ref this.localUsesItems, this.info.localUsers,this.info.localDomain);

            if (this.info.trustType != KerberosTrustType.NoTrust)
            {
                AddUserList(ref this.trustUsesItems, this.info.trustUsers,this.info.trustDomain);
            }
        }
        private void AddUserList(ref ResultItemMap resultMap,  Dictionary<string, User> users, DomainInfo domain)
        {
            
            foreach (string key in users.Keys)
            {
                User user = users[key];
                try
                {   
                    //detect the user in domain 
                    //if cannot get the user information, set unsupported result.

                    string userName = ServerHelper.GetAccountAttribute(user.Name, "Users", "sAMAccountName", domain.Name, domain.Admin, domain.AdminPassword);                    
                    
                    if (string.IsNullOrEmpty(userName))
                    {
                        AddResultItem(ref resultMap, key + " User Name: ", user.Name, DetectResult.UnSupported);

                        //cannot detect the password, assume the default value
                        AddResultItem(ref resultMap, key + " User Password: ", user.Password, DetectResult.UnSupported);

                        if (user.Salt != null)
                        {
                            // to do: send as req to get salt value.
                            AddResultItem(ref resultMap, key + " User Salt: ", user.Salt, DetectResult.UnSupported);
                        }
                    }
                    else
                    {
                        AddResultItem(ref resultMap, key + " User Name: ", user.Name, DetectResult.Supported);
                        AddResultItem(ref resultMap, key + " User Password: ", user.Password, DetectResult.Supported);

                        if (user.Salt != null)
                        {
                            // to do: send as req to get salt value.
                            AddResultItem(ref resultMap, key + " User Salt: ", user.Salt, DetectResult.Supported);
                        }
                    }
                }
                catch
                {
                    AddResultItem(ref resultMap, key + " User Name: ", "", DetectResult.DetectFail);
                    AddResultItem(ref resultMap, key + " User Password: ", "", DetectResult.DetectFail);                                    
                  
                }

                try
                {
                    string spn = ServerHelper.GetAccountAttribute(user.Name, "Users", "servicePrincipalName", domain.Name, domain.Admin, domain.AdminPassword);
                    if (user.ServiceName != null)
                    {
                        if (!string.IsNullOrEmpty(spn))
                        {
                            user.ServiceName = spn;
                            AddResultItem(ref resultMap, key + " User ServiceName: ", user.ServiceName, DetectResult.Supported);

                        }
                        else
                        {
                            AddResultItem(ref resultMap, key + " User ServiceName: ", user.ServiceName, DetectResult.UnSupported);
                        }
                    }  
                }
                catch
                {
                    if (user.ServiceName != null)
                    {
                        AddResultItem(ref resultMap, key + " User ServiceName: ", "", DetectResult.DetectFail);
                    }
                    
                }
            }            
        }

        private void AddEndpoints()
        {
            if (
                this.info.localDC != null
                && !string.IsNullOrEmpty(this.info.localDC.ComputerName )
                && !string.IsNullOrEmpty(this.info.localDC.FQDN)
                && !string.IsNullOrEmpty(this.info.localDC.DefaultServiceName)
               // && !string.IsNullOrEmpty(this.info.localDC.Password)
                //&& !string.IsNullOrEmpty(this.info.localDC.Port)
                )
            {
                AddEndpoint(ref this.localDCItems, this.info.localDC, DetectResult.Supported);
            }
            else
            {
                AddEndpoint(ref this.localDCItems, this.info.localDC, DetectResult.UnSupported);
            }

            if (
                this.info.localClient != null
                && !string.IsNullOrEmpty(this.info.localClient.ComputerName )
                && !string.IsNullOrEmpty(this.info.localClient.FQDN)
                && !string.IsNullOrEmpty(this.info.localClient.DefaultServiceName)
               // && !string.IsNullOrEmpty(this.info.localClient.Password)
               // && !string.IsNullOrEmpty(this.info.localClient.Port)
                )
            {
                AddEndpoint(ref this.localClientItems, this.info.localClient, DetectResult.Supported);
            }
            else
            {
                AddEndpoint(ref this.localClientItems, this.info.localClient, DetectResult.UnSupported);
            }

            if (
                this.info.localAP != null
                && !string.IsNullOrEmpty(this.info.localAP.ComputerName )
                && !string.IsNullOrEmpty(this.info.localAP.FQDN)
                && !string.IsNullOrEmpty(this.info.localAP.DefaultServiceName)
              //  && !string.IsNullOrEmpty(this.info.localAP.Password)
                //&& !string.IsNullOrEmpty(this.info.localAP.Port)
                )            
            {
                AddEndpoint(ref this.localAPItems, this.info.localAP, DetectResult.Supported);
            }
            else
            {
                AddEndpoint(ref this.localAPItems, this.info.localAP, DetectResult.UnSupported);
            }

            if (
               this.info.localDC != null
               && this.info.localDC.ldapService != null
               && !string.IsNullOrEmpty(this.info.localDC.ldapService.LdapServiceName)
               && !string.IsNullOrEmpty(this.info.localDC.ldapService.GssToken)
               && !string.IsNullOrEmpty(this.info.localDC.ldapService.Port)
               )
            {
                AddLdapServer(ref this.localDCItems, this.info.localDC, DetectResult.Supported);
            }
            else
            {
                AddLdapServer(ref this.localDCItems, this.info.localDC, DetectResult.UnSupported);
            }            

            if (
              this.info.localAP != null
              && this.info.localAP.smb2Service != null
              && !string.IsNullOrEmpty(this.info.localAP.smb2Service.SMB2ServiceName)
              && !string.IsNullOrEmpty(this.info.localAP.smb2Service.SMB2Dialect)
              )
            {
                AddSmb2Server(ref this.localAPItems, this.info.localAP, DetectResult.Supported);
                
            }
            else
            {
                AddSmb2Server(ref this.localAPItems, this.info.localAP, DetectResult.UnSupported);
            }

            if (
              this.info.localAP != null
              && this.info.localAP.httpService != null          
              && !string.IsNullOrEmpty(this.info.localAP.httpService.HttpServiceName)
              )
            {
                AddHttpServer(ref this.localAPItems, this.info.localAP, DetectResult.Supported);

            }
            else
            {
                AddHttpServer(ref this.localAPItems, this.info.localAP, DetectResult.UnSupported);
            }
        
            AddOtherService(ref this.localAPItems, "AuthNotRequired", this.info.localAP.authNotReqService, this.info.localDomain);            
            AddOtherService(ref this.localAPItems, "localResource01", this.info.localAP.localResourceService1, this.info.localDomain);
            AddOtherService(ref this.localAPItems, "localResource02", this.info.localAP.localResourceService2, this.info.localDomain);

            if (this.info.trustType != KerberosTrustType.NoTrust)
            {
                if(
                     this.info.trustDC != null
                    && !string.IsNullOrEmpty(this.info.trustDC.ComputerName)
                    && !string.IsNullOrEmpty(this.info.trustDC.FQDN)
                    && !string.IsNullOrEmpty(this.info.trustDC.DefaultServiceName)                   
                    )
                {
                    AddEndpoint(ref this.trustDCItems, this.info.trustDC, DetectResult.Supported);
                }
                else
                {
                    AddEndpoint(ref this.trustDCItems, this.info.trustDC, DetectResult.UnSupported);
                }

                if (
                     this.info.trustAP != null
                    && !string.IsNullOrEmpty(this.info.trustAP.ComputerName)
                    && !string.IsNullOrEmpty(this.info.trustAP.FQDN)
                    && !string.IsNullOrEmpty(this.info.trustAP.DefaultServiceName)
                
                    )
                {
                    AddEndpoint(ref this.trustAPItems, this.info.trustAP, DetectResult.Supported);
                }
                else
                {
                    AddEndpoint(ref this.trustAPItems, this.info.trustAP, DetectResult.UnSupported);
                }

                if (
                     this.info.trustAP != null
                    && (this.info.trustAP.smb2Service != null)
                    && !string.IsNullOrEmpty(this.info.trustAP.smb2Service.SMB2Dialect)
                    && !string.IsNullOrEmpty(this.info.trustAP.smb2Service.SMB2ServiceName)
                    )
                {
                    AddSmb2Server(ref this.trustAPItems, this.info.trustAP, DetectResult.Supported);
                }
                else
                {
                    AddSmb2Server(ref this.trustAPItems, this.info.trustAP, DetectResult.UnSupported);
                }

                if (
                    this.info.trustAP != null
                   && (this.info.trustAP.httpService != null)                   
                   && !string.IsNullOrEmpty(this.info.trustAP.httpService.Uri)
                   )
                {
                    AddHttpServer(ref this.trustAPItems, this.info.trustAP, DetectResult.Supported);
                }
                else
                {
                    AddHttpServer(ref this.trustAPItems, this.info.trustAP, DetectResult.UnSupported);
                }

                if (
              this.info.trustDC != null
              && this.info.trustDC.ldapService != null
              && !string.IsNullOrEmpty(this.info.trustDC.ldapService.LdapServiceName)
              && !string.IsNullOrEmpty(this.info.trustDC.ldapService.GssToken)
              && !string.IsNullOrEmpty(this.info.trustDC.ldapService.Port)
              )
                {
                    AddLdapServer(ref this.trustDCItems, this.info.trustDC, DetectResult.Supported);
                }
                else
                {
                    AddLdapServer(ref this.trustDCItems, this.info.trustDC, DetectResult.UnSupported);
                }

           
            }
        }

        private void AddCommonInfo()
        {   
            //AddResultItem(ref this.commonItems, "TransportType: ", this.info.TransportType, DetectResult.Supported);
            //AddResultItem(ref this.commonItems, "TransportBufferSize: ", this.info.TransportBufferSize, DetectResult.Supported);
            //AddResultItem(ref this.commonItems, "IpVersion: ", this.info.ipVersion.ToString(), DetectResult.Supported);            
            //AddResultItem(ref this.commonItems, "IsKileImplemented: ", this.info.IsKileImplemented.ToString(), DetectResult.Supported);
            //AddResultItem(ref this.commonItems, "IsClaimSupported: ", this.info.IsClaimSupported.ToString(), DetectResult.Supported);
            //AddResultItem(ref this.commonItems, "SupportedOid: ", this.info.SupportedOid, DetectResult.Supported);
            AddResultItem(ref this.commonItems, "TrustType :", this.info.trustType.ToString(), DetectResult.Supported);
            if (this.info.trustType != KerberosTrustType.NoTrust)
            {
                AddResultItem(ref this.commonItems, "TrustPwd: ", this.info.TrustPwd, DetectResult.Supported);
            }
            else
            {
                AddResultItem(ref this.commonItems, "TrustPwd: ", this.info.TrustPwd, DetectResult.UnSupported);
            }
           
            DetectResult result = DetectResult.Supported;
            if (this.info.UseKKdcp == true)
            {
                if (!ServerHelper.URLExists(this.info.kkdcpInfo.KKDCPServerUrl))
                {
                    result = DetectResult.UnSupported;
                }
            }
            else
            {
                result = DetectResult.UnSupported;
            }
                
           
            AddResultItem(ref this.commonItems, "Use KKDCP: ", this.info.UseKKdcp.ToString(), DetectResult.Supported);
            AddResultItem(ref this.commonItems, "KKDCPServerUrl: ", this.info.kkdcpInfo.KKDCPServerUrl, result);
            //AddResultItem(ref this.commonItems, "KKDCPClientCertPath", this.info.kkdcpInfo.KKDCPClientCertPath, result);
            //AddResultItem(ref this.commonItems, "KKDCPClientCertPassword", this.info.kkdcpInfo.KKDCPClientCertPassword, result);
        }
        private void AddDomains()
        {
            if (
                !string.IsNullOrEmpty(this.info.localDomain.Name)
                && !string.IsNullOrEmpty(this.info.localDomain.Admin)
                && !string.IsNullOrEmpty(this.info.localDomain.AdminPassword)
                )
            {
                AddDomain(ref this.localDomainCommonItems, this.info.localDomain, DetectResult.Supported);
            }
            else
            {
                AddDomain(ref this.localDomainCommonItems, this.info.localDomain, DetectResult.UnSupported);
            }

            if (this.info.trustType != KerberosTrustType.NoTrust
                && !string.IsNullOrEmpty(this.info.trustDomain.Name)
                && !string.IsNullOrEmpty(this.info.trustDomain.Admin)
                && !string.IsNullOrEmpty(this.info.trustDomain.AdminPassword)
                )
            {                
                AddDomain(ref this.trustDomainCommonItems, this.info.trustDomain, DetectResult.Supported);
            }
            else
            {
                AddDomain(ref this.trustDomainCommonItems, this.info.trustDomain, DetectResult.UnSupported);
            }
            
        }

        private void AddDomain(ref ResultItemMap resultMap, DomainInfo domain, DetectResult result)
        {
            AddResultItem(ref resultMap,"DomainName: ",domain.Name,result);
            AddResultItem(ref resultMap,"DomainAdmin: ",domain.Admin,result);
            AddResultItem(ref resultMap,"DomainAdminPwd: ",domain.AdminPassword,result);
            AddResultItem(ref resultMap, "DomainFunctionLevel: ", domain.FunctionalLevel, result);
        }

        private void AddEndpoint(ref ResultItemMap resultMap, ComputerInfo server, DetectResult result)
        {
            AddResultItem(ref resultMap,"ComputerName: ",server.ComputerName,result);
            AddResultItem(ref resultMap,"FQDN: ",server.FQDN,result);
            if (string.IsNullOrEmpty(server.IPv4))
            {
                AddResultItem(ref resultMap, "IP: ", server.IPv6, result);
            }
            else
            {
                AddResultItem(ref resultMap, "IP: ", server.IPv4, result);
            }
            AddResultItem(ref resultMap,"NetBIOS: ",server.NetBIOS,result);
           // AddResultItem(ref resultMap,"ComputerPwd: ",server.Password,result);
          //  AddResultItem(ref resultMap,"Port: ",server.Port,result);
            AddResultItem(ref resultMap,"ServiceSalt: ",server.ServiceSalt,result);
            AddResultItem(ref resultMap,"DefaultServiceName: ",server.DefaultServiceName,result);
            AddResultItem(ref resultMap, "IsWindows: ", server.IsWindows.ToString(), result);   

        }


        private void AddSmb2Server(ref ResultItemMap resultMap, Server server, DetectResult result)
        {
            if (!string.IsNullOrEmpty(server.smb2Service.CBACShare))
            {
                AddResultItem(ref resultMap, "CBACShare: ", server.smb2Service.CBACShare, result);
            }
            else
            {
                AddResultItem(ref resultMap, "CBACShare: ", server.smb2Service.CBACShare, DetectResult.DetectFail);
            }

            if (!string.IsNullOrEmpty(server.smb2Service.DACShare))
            {
                AddResultItem(ref resultMap, "DACShare: ", server.smb2Service.DACShare, result);
            }
            else
            {
                AddResultItem(ref resultMap, "DACShare: ", server.smb2Service.DACShare, DetectResult.DetectFail);
            }
            if (!string.IsNullOrEmpty(server.smb2Service.SMB2Dialect))
            {
                AddResultItem(ref resultMap, "SMB2Dialect: ", server.smb2Service.SMB2Dialect, result);
            }
            else
            {
                AddResultItem(ref resultMap, "SMB2Dialect: ", server.smb2Service.SMB2Dialect, DetectResult.DetectFail);
            }
            if (!string.IsNullOrEmpty(server.smb2Service.SMB2ServiceName))
            {
                AddResultItem(ref resultMap, "SMB2ServiceName: ", server.smb2Service.SMB2ServiceName, result);
            }
            else
            {
                AddResultItem(ref resultMap, "SMB2ServiceName: ", server.smb2Service.SMB2ServiceName, DetectResult.DetectFail);
            }
        }

        private void AddHttpServer(ref ResultItemMap resultMap, Server server, DetectResult result)
        {
            if (!string.IsNullOrEmpty(server.httpService.HttpServiceName))
            {
                AddResultItem(ref resultMap, "HttpServiceName: ", server.httpService.HttpServiceName, result);
            }
            else
            {
                AddResultItem(ref resultMap, "HttpServiceName: ", server.httpService.HttpServiceName, DetectResult.DetectFail);
            }


            if (!string.IsNullOrEmpty(server.httpService.Uri))
            {
                AddResultItem(ref resultMap, "HttpUri: ", server.httpService.Uri, result);
            }
            else
            {
                AddResultItem(ref resultMap, "HttpUri: ", server.httpService.Uri, DetectResult.DetectFail);
            }
        }
        private void AddLdapServer(ref ResultItemMap resultMap, Server server, DetectResult result)
        {
            if (!string.IsNullOrEmpty(server.ldapService.LdapServiceName))
            {
                AddResultItem(ref resultMap, "LdapServiceName: ", server.ldapService.LdapServiceName, result);
            }
            else
            {
                AddResultItem(ref resultMap, "LdapServiceName: ", server.ldapService.LdapServiceName, DetectResult.DetectFail);
            }

            if (!string.IsNullOrEmpty(server.ldapService.Port))
            {
                AddResultItem(ref resultMap, "LdapPort: ", server.ldapService.Port, result);
            }
            else
            {
                AddResultItem(ref resultMap, "LdapPort: ", server.ldapService.Port, DetectResult.DetectFail);
            }
            if (!string.IsNullOrEmpty(server.ldapService.GssToken))
            {
                AddResultItem(ref resultMap, "LdapGssToken: ", server.ldapService.GssToken, result);
            }
            else
            {
                AddResultItem(ref resultMap, "LdapGssToken: ", server.ldapService.GssToken, DetectResult.DetectFail);
            }
        }

        private void AddOtherService(ref ResultItemMap resultMap, string computerName, OtherService service, DomainInfo domain)
        {
            try
            {
                ServerHelper.GetAccountAttribute(computerName, "Computers", "sAMAccountName",  domain.Name, domain.Admin, domain.AdminPassword);

                if (!string.IsNullOrEmpty(service.FQDN))
                {
                    AddResultItem(ref resultMap, computerName + " FQDN: ", service.FQDN, DetectResult.Supported);
                }
                if (!string.IsNullOrEmpty(service.NetBios))
                {
                    AddResultItem(ref resultMap, computerName + " NetBios: ", service.NetBios, DetectResult.Supported);
                }
                if (!string.IsNullOrEmpty(service.Password))
                {
                    AddResultItem(ref resultMap, computerName + " Password: ", service.Password, DetectResult.Supported);
                }
                if (!string.IsNullOrEmpty(service.DefaultServiceName))
                {
                    AddResultItem(ref resultMap, computerName + " DefaultServiceName: ", service.DefaultServiceName, DetectResult.Supported);
                }
                if (!string.IsNullOrEmpty(service.ServiceSalt))
                {
                    AddResultItem(ref resultMap, computerName + " ServiceSalt: ", service.ServiceSalt, DetectResult.Supported);
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(service.FQDN))
                {
                    AddResultItem(ref resultMap, computerName + " FQDN: ", service.FQDN, DetectResult.UnSupported);
                }
                if (!string.IsNullOrEmpty(service.NetBios))
                {
                    AddResultItem(ref resultMap, computerName + " NetBios: ", service.NetBios, DetectResult.UnSupported);
                }
                if (!string.IsNullOrEmpty(service.Password))
                {
                    AddResultItem(ref resultMap, computerName + " Password: ", service.Password, DetectResult.UnSupported);
                }
                if (!string.IsNullOrEmpty(service.DefaultServiceName))
                {
                    AddResultItem(ref resultMap, computerName + " DefaultServiceName: ", service.DefaultServiceName, DetectResult.UnSupported);
                }
                if (!string.IsNullOrEmpty(service.ServiceSalt))
                {
                    AddResultItem(ref resultMap, computerName + " ServiceSalt: ", service.ServiceSalt, DetectResult.UnSupported);
                }
            }                  

        }

        private void AddResultItem(ref ResultItemMap resultItemMap, string name, string value, DetectResult result)
        {
            string imagePath = string.Empty;
            switch (result)
            {
                case DetectResult.Supported:
                    imagePath = "/KerberosPlugin;component/Icons/supported.png";
                    break;
                case DetectResult.UnSupported:
                    imagePath = "/KerberosPlugin;component/Icons/unsupported.png";
                    break;
                case DetectResult.DetectFail:
                    imagePath = "/KerberosPlugin;component/Icons/undetected.png";
                    break;
                default:
                    break;
            }

            ResultItem item = new ResultItem() { DetectedResult = result, ImageUrl = imagePath, Name = name, Value = value };
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
                if (!info.detectExceptions.ContainsKey(tempItem.Name))
                {
                    this.ItemDescription.Text = tempItem.Name + " is detected.";
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

        public void LoadDetectionInfo(DetectionInfo detectionInfo)
        {
            this.info = detectionInfo;

            //add common info
            AddCommonInfo();

            //add dc info
            AddDomains();

            //add endpoints in network topology
            AddEndpoints();            

            //Bind the data to the control
            resultItemMapList.Add(this.commonItems);
            resultItemMapList.Add(this.localDomainCommonItems);            
            resultItemMapList.Add(this.localClientItems);
            resultItemMapList.Add(this.localDCItems);            
            resultItemMapList.Add(this.localAPItems);

            if (this.info.trustType != KerberosTrustType.NoTrust)
            {
                resultItemMapList.Add(this.trustDomainCommonItems);
                resultItemMapList.Add(this.trustDCItems);
                resultItemMapList.Add(this.trustAPItems);
            }

            ResultMapList.ItemsSource = resultItemMapList;
        }
    }
}
