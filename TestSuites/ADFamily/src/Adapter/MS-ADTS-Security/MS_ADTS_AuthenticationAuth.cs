// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using System.Security.AccessControl;

using ProtocolMessageStructures;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    /// <summary>
    /// MS_ADTS_AuthenticationAuth contains implementation of the methods defined 
    /// in IMS_ADTS_AuthenticationAuth interface. The class also inherits ManagedAdapterBase
    /// Methods used for Authentication mechanism, authorization, Password modify operations
    /// are implemented in this class
    /// </summary>
    public partial class MS_ADTS_AuthenticationAuth : ADCommonServerAdapter, IMS_ADTS_AuthenticationAuth
    {

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            adtsRequirementsValidation.Initialize(testSite);
            PdcDN = Utilities.ParseDomainName(PrimaryDomainDnsName);
            PdcFqdn = PDCNetbiosName + "." + PrimaryDomainDnsName;
        }

        #region Global variables

        /// <summary>
        /// Specifies the version of OS from config.cord file
        /// </summary>
        Common.ServerVersion testServerVersion;

        /// <summary>
        /// Specifies the type of AD from config file
        /// </summary>
        ADTypes adTestType;
                   

        /// <summary>
        /// Variable defines the authentication mechanism
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public authenticationMech strAuthMech;
        /// <summary>
        /// Variable defines the port number
        /// </summary>
        private Port enumPortNum = Port.LDAP_PORT;
        /// <summary>
        /// Variable defines the return code
        /// </summary>
        public errorstatus strResult = errorstatus.success;
        /// <summary>
        /// Variable defines the user name
        /// </summary>
        public string user = string.Empty;
        /// <summary>
        /// Variable defines the user password
        /// </summary>
        public string userPassword = string.Empty;

        /// <summary>
        /// used to invoke the methods to validate the requirements.
        /// </summary>
        MS_ADTS_SecurityRequirementsValidator adtsRequirementsValidation = new MS_ADTS_SecurityRequirementsValidator();

        public string PdcDN;

        public string PdcFqdn;

        #endregion
        
        #region Event
        /// <summary>
        /// SicilyPacakageResponse is an event which receives data from the adapter
        /// It signifies whether the package delivery scheme as requested through
        /// SicilyNegotiate is successful. The authentication scheme as passed from
        /// SicilyNegotiate is supported by the Server
        /// </summary>
        public event QueryPackageStatus SicilyPacakageResponse;
        #endregion
        
        #region InitializeSession

        /// <summary>
        /// InitializeSession
        /// This action is used to model the method InitializeSession.
        /// This method is used to do the initial settings before calling bind operations.
        /// InitializeSession is called only once during one test-pass and is the pre-requisite
        /// for other calls
        /// </summary>
        /// <param name="packageDiscovery">packageDiscovery set to true signifies that
        /// the client has got out-of-band knowledge of the supported Authentication
        /// protocols</param>
        /// <param name="serverVersion">serverVersion stores the Server version used for 
        /// the test pass</param>
        /// <param name="adType">Stores whether the Active Directory is of type DS or LDS</param>
        public void InitializeSession(bool packageDiscovery,
                                      Common.ServerVersion serverVersion,
                                      ADTypes adType)
        {
            //Default short of Doc.
            TestClassBase.BaseTestSite.DefaultProtocolDocShortName = "MS-ADTS-Security";

            //Assigning the server version value from config.cord.
            testServerVersion = serverVersion;

            //Assigning the server version value from config.cord. 
            adTestType = adType;

            if (PDCOSVersion == ServerVersion.NonWin)
            {
                Site.Log.Add(LogEntryKind.Comment, "Run Windows Server 2008 R2 test cases for Non-Windows platform instead.");
            }


            //initialize settings
            adtsRequirementsValidation.InitializeSession();

            TestClassBase.BaseTestSite.Log.Add(LogEntryKind.Comment, "The TestSuite is running with" + PDCOSVersion.ToString());

        }

        #endregion
        
        #region SimpleBind

        /// <summary>
        /// SimpleBind
        /// This action is used for doing simple authentication.
        /// This method is used for authenticating the Domain user and anonymous user
        /// on both regular and protected LDAP ports
        /// </summary>
        /// <param name="userName">Contains username in Domain</param>
        /// <param name="passWord">Contains the password to the username</param>
        /// <param name="portNum">Contains the port number over which the bind will accomplish</param>
        /// <param name="enableTLS">This variable i used to state when we are using TLS </param>
        /// <returns>Returns Success if the method is successful
        ///  Returns InvalidCredentials if the passed in credentials are invalid</returns>
        public errorstatus SimpleBind(name userName,
                                      Password passWord,
                                      Port portNum,
                                      bool enableTLS)
        {
                    //Assigning Authorization mechanism to Bind
                    strAuthMech = authenticationMech.simple;

                    //Assigning port number .
                    enumPortNum = portNum;

                    //Valid nameMapsMoreThanOneObject user and valid password
                    if ((userName == name.nameMapsMoreThanOneObject) && (passWord == Password.validPassword))
                    {
                        //name maps more than one object.
                        //To validate if name maps more than object
                        user = MS_ADTS_SecurityRequirementsValidator.NameMapsMorethanOneObject;

                        //Create  an AD User.
                        ADTSHelper.CreateActiveDirUser(PdcFqdn, userName, ClientUserPassword, PdcDN);
                        //Change the attribute
                        ADTSHelper.ModifyOperation(PdcFqdn, userName, adTestType, ClientUserName, ClientUserPassword, PrimaryDomainDnsName, PDCOSVersion);
                    }

                    else if ((userName == name.nameMapsMoreThanOneObject) && (passWord == Password.invalidPassword))
                    {
                        //name maps more than one object.
                        user = MS_ADTS_SecurityRequirementsValidator.NameMapsMorethanOneObject;
                        //Invalid password
                        userPassword = MS_ADTS_SecurityRequirementsValidator.InvalidPassword;
                    }
                    //if invalid user name
                    else if (userName == name.nonexistUserName)
                    {
                        //get from config file
                        user = MS_ADTS_SecurityRequirementsValidator.NonExistUserName;
                    }
                    //valid user
                    else if (userName == name.validUserName)
                    {
                        //get the Current username from config file
                        user = ClientUserName;

                    }
                    //Anonymous user
                    else if (userName == name.anonymousUser)
                    {
                        //Empty user name and Empty password
                        //Anonymous user should have (null,null) credentials
                        //Setting the credentials to null
                        user = null;
                    }
                    //invalid password
                    if ((passWord == Password.invalidPassword) && (userName != name.anonymousUser))
                    {
                        //get from config file
                        userPassword = MS_ADTS_SecurityRequirementsValidator.InvalidPassword;
                    }

                    if ((passWord == Password.invalidPassword) && (userName == name.anonymousUser))
                    {
                        //Anonymous user passowrd.
                        userPassword = null;
                    }

                    else if (passWord == Password.validPassword)
                    {
                        //get from config file
                        userPassword = ClientUserPassword;

                        if (userName == name.anonymousUser)
                        {
                            //anonymous user password.
                            userPassword = null;
                        }
                    }

                    //SimpleBind Authentication 
                    strResult = adtsRequirementsValidation.SimpleBind(PdcFqdn, (uint)enumPortNum, user, userPassword, enableTLS, adTestType);

                    return strResult;
        }

        #endregion

        #region FastBind

        /// <summary>
        /// Fast bind
        /// This action is used for doing fast authentication.
        /// This method is used for authentication the Domain user
        /// on regular LDAP ports
        /// </summary>
        /// <param name="userName">Contains username in the domain</param>
        /// <param name="passWord">Contains the password to the username</param>
        /// <param name="portNum">Contains the port number over which the bind will accomplish</param>
        /// <param name="enableTLS">This variable i used to state when we are using TLS</param>
        /// <returns>Returns Success if the method is successful</returns>
        /// <returns>Returns InvalidCredentials if the passed in credentials are invalid</returns>
        public errorstatus FastBind(name userName,
                                    Password passWord,
                                    Port portNum,
                                    bool enableTLS)
        {
            //Assigning Authorization mechanism to Bind
            strAuthMech = authenticationMech.simple;

            //Assigning port number .
            enumPortNum = portNum;


            if (userName == name.nonexistUserName)
            {
                //get from config file
                user = MS_ADTS_SecurityRequirementsValidator.NonExistUserName;
            }
            else if (userName == name.validUserName)
            {
                //Current user
                user = ClientUserName;
            }
            if (passWord == Password.invalidPassword)
            {
                //get from config file
                userPassword = MS_ADTS_SecurityRequirementsValidator.InvalidPassword;
            }
            else if (passWord == Password.validPassword)
            {
                userPassword = ClientUserPassword;
            }

            //SimpleBind Authentication 
            strResult = adtsRequirementsValidation.FastBind(PdcFqdn, (uint)enumPortNum, user, userPassword, enableTLS);

            return strResult;

        }

        #endregion

        #region SASLAuthentication

        /// <summary>
        /// The following authentication mechanisms are covered under SASL authentication:
        ///     GSS-SPNEGO
        /// 	GSSAPI
        /// 	External
        /// 	Digest-MD5/
        /// This action is used to do SPNEGOBind, GSSAPI, External or Digest-MD5
        /// authentication as per authMech passed from InitializeSession.
        /// This method is used to authenticate the Domain user 
        /// on both regular and protected LDAP ports
        /// </summary>
        /// <param name="userName">Contains username in Domain</param>
        /// <param name="passWord">Contains the password to the username</param>
        /// <param name="saslMech">Specifies the SASL Mechanism preferred</param>
        /// <param name="portNum">Contains the port number over which the bind will accomplish</param>
        /// <param name="enableTLS">This variable i used to state when we are using TLS </param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidCreadentials if the passed in credentials are invalid</returns>
        public errorstatus SASLAuthentication(name userName,
                                              SASLChoice saslMech,
                                              Password passWord,
                                              Port portNum,
                                              bool enableTLS)
        {
          
                    //get the port number
                    enumPortNum = portNum;

                    //specifies invalidSPN.
                    bool invalidSPN = false;

                    if (userName == name.nonexistUserName)
                    {
                        //get from config file
                        user = MS_ADTS_SecurityRequirementsValidator.NonExistUserName;
                    }
                    else if (userName == name.validUserName)
                    {
                        //Current user
                        user = ClientUserName;
                    }
                    if (passWord == Password.invalidPassword)
                    {
                        //get from config file
                        userPassword = MS_ADTS_SecurityRequirementsValidator.InvalidPassword;
                    }
                    else if (passWord == Password.validPassword)
                    {
                        userPassword = ClientUserPassword;
                    }

                    //SASL Bind
                    strResult = adtsRequirementsValidation.SASLBind(PdcFqdn, 
                                                                    (uint)enumPortNum,
                                                                    user,
                                                                    userPassword,
                                                                    enableTLS,
                                                                    saslMech,
                                                                    invalidSPN);

                    //returned result 
                    return strResult;
              
        }
        #endregion

        #region MutualAuthBind 
        /// <summary>
        /// MutualAuthBind
        /// This action is used to do Mutual authentication.
        /// This method is used to authenticate the Domain user 
        /// on both regular and protected LDAP ports between client and server mutually
        /// </summary>
        /// <param name="userName">Contains username in Domain</param>
        /// <param name="passWord">Contains the password to the username</param>
        /// <param name="validSPN">This variable i used to state when we are using valid SPN </param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidCredentials if the passed in credentials are invalid</returns>
        public errorstatus MutualAuthBind(name userName,
                                          Password passWord,
                                          bool validSPN)
        {
                    if (userName == name.nonexistUserName)
                    {
                        //get from config file
                        user = MS_ADTS_SecurityRequirementsValidator.NonExistUserName;
                    }
                    else if (userName == name.validUserName)
                    {
                        //Current user
                        user = ClientUserName;
                    }
                    if (passWord == Password.invalidPassword)
                    {
                        //get from config file
                        userPassword = MS_ADTS_SecurityRequirementsValidator.InvalidPassword;
                    }
                    else if (passWord == Password.validPassword)
                    {
                        userPassword = ClientUserPassword;
                    }

                    //Mutual Bind
                    strResult = adtsRequirementsValidation.MutualBind(user, userPassword, validSPN);

                    //returned result 
                    return strResult;
               
        }

        #endregion

        #region SicilyNegotiate
        /// <summary>
        /// SicilyNegotiate request
        /// For clients who do not have an out-of-band knowledge about the authentication
        /// schemes supported by the Server, the Client should sent discover the 
        /// Package Delivery mechanism supported. The mechanism should be passed as part
        /// of method input in order to check if that mechanism is supported by the Server.
        /// For Windows it is always NTLM
        /// </summary>
        /// <param name="authProtocol">specifies the authentication protocol name
        /// which the client checks if supported by the server</param>
        public void SicilyNegotiate(string authProtocol)
        {
            SicilyPacakageResponse(true);
        }
        #endregion

        #region SicilyBind
        /// <summary>
        /// SicilyBind 
        /// This method is used for modeling behaviors pertaining to
        /// SicilyBind. Similar to SASL GSSSPNEGO type of binding
        /// </summary>
        /// <param name="userName">Contains username in Domain</param>
        /// <param name="passWord">Contains the password to the username</param>
        /// <param name="portNum">Contains the port number over which the bind will accomplish</param>
        /// <param name="enableTLS">This variable is used to state when we are using TLS </param>
        /// <returns>Returns Success if the method is successful
        /// Returns InvalidCredentials if the passed in credentials are invalid</returns>
        public errorstatus SicilyBind(name userName,
                                      Password passWord,
                                      Port portNum,
                                      bool enableTLS)
        {

            //Assigning Authorization mechanism to Bind
            strAuthMech = authenticationMech.sicily;

            //Assigning port number .
            enumPortNum = portNum;


            //if invalid user name
            if (userName == name.nonexistUserName)
            {
                //get from config file
                user = MS_ADTS_SecurityRequirementsValidator.NonExistUserName;
            }
            //valid user
            else if (userName == name.validUserName)
            {
                //Current user
                user = ClientUserName;

            }

            //invalid password
            if (passWord == Password.invalidPassword)
            {
                //get from config file
                userPassword = MS_ADTS_SecurityRequirementsValidator.InvalidPassword;
            }
            else if (passWord == Password.validPassword)
            {
                //get from config file
                userPassword = ClientUserPassword;

                if (userName == name.anonymousUser)
                {
                    //anonymous user password.
                    userPassword = null;
                }
            }

            //SicilyBind Authentication 
            strResult = adtsRequirementsValidation.SicilyBind(PdcFqdn, (uint)enumPortNum, user, userPassword, enableTLS);

            return strResult;
        }
        #endregion

        #region AuthorizationCheck
        /// <summary>
        /// AuthorizationCheck
        /// This method is used to check if the user is authorized to access the objects's
        /// attribute. Accessing of object's specific attributes is dependent on the accessRights 
        /// and ctrlAccessRights bit
        /// </summary>
        /// <param name="accessRights">specifies the access rights granted on the user for
        /// the object's specific attribute</param>
        /// <param name="ctrlAccessRights">specifies the control access rights on the user 
        /// for the object's specific attribute</param>
        /// <param name="attribCheck">specifies the attribute of the object.Depending 
        /// upon the access control bits set, we will observe if the object is accessible</param>
        /// <param name="fUserPwdSupport">specifies the whether the password Change operation is supported or not</param>
        /// <returns>errorstatus</returns>
        public errorstatus AuthorizationCheck(AccessRights accessRights,
                                              ControlAccessRights ctrlAccessRights,
                                              AttribsToCheck attribCheck, 
                                              bool fUserPwdSupport)
        {
              #region valid Credentials
                              
              return AuthorizationRequirements(accessRights, ctrlAccessRights, attribCheck,fUserPwdSupport);
                    
              #endregion
        }

        #region AuthorizationRequirements
        /// <summary>
        /// This method validates the requirements AD Authorization
        /// </summary>
        /// <param name="accessRights">specifies the ActiveDirectoryRight</param>
        /// <param name="ctrlAccessRights">specifies the Control access right</param>
        /// <param name="attribCheck">specifies attribute to validate</param>
        /// <param name="fUserPwdSupport">specifies the whether the password Change operation is supported or not</param>
        /// <returns>returns the status</returns>
        private errorstatus AuthorizationRequirements(AccessRights accessRights,
                                                      ControlAccessRights ctrlAccessRights,
                                                      AttribsToCheck attribCheck, 
                                                      bool fUserPwdSupport)
        {

        

            #region nTSecurityDescriptoRequirementsValidation

            //Checking nTSecurityDescriptor
            if (attribCheck == AttribsToCheck.nTSecurityDescriptor)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidatenTSecurityDescriptor(accessRights, attribCheck);

            }


            #endregion
                        
            #region msDS_QuotaEffective

            if (attribCheck == AttribsToCheck.msDS_QuotaEffective)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidatemsDS_QuotaEffectiveAttribute(accessRights, ctrlAccessRights, attribCheck);

            }

            #endregion

            #region msDS_QuotaUsed
            if (attribCheck == AttribsToCheck.msDS_QuotaUsed)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidatemsDS_QuotaUsedAttribute(accessRights, ctrlAccessRights, attribCheck);

            }

            #endregion

            #region passwordChange attribute

            if (attribCheck == AttribsToCheck.userPassword)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateUserPasswordAttribute(attribCheck, accessRights, ctrlAccessRights, fUserPwdSupport);


            }
            
            if (attribCheck == AttribsToCheck.nTSecurityDescriptor)
            {

                return adtsRequirementsValidation.ValidatenTSecurityDescriptor(accessRights, attribCheck);

            }
            #endregion

            #region NtdsQuotaRequirements
            
            if (ctrlAccessRights == ControlAccessRights.DS_Query_Self_Quota)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidatemsDS_QuotaUsedAttribute(accessRights, ctrlAccessRights, attribCheck);

            }
            
            #endregion
            
            #region MsDS_ReplAttributeMetaDataAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.msDS_ReplAttributeMetaData)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateMsDS_ReplAttributeMetaData(accessRights, ctrlAccessRights, attribCheck);

            }
            #endregion
            
            #region msDS-ReplValueMetaDataRequirementsValidation

            //Checking msDS_ReplValueMetaData
            if (attribCheck == AttribsToCheck.msDS_ReplValueMetaData)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateMsDS_ReplValueMetaData(accessRights, ctrlAccessRights, attribCheck);


            }
            
            #endregion

           
                                              
            #region msDS_NCReplInboundNeighborsAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.msDS_NCReplInboundNeighbors)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateMsDS_NCReplInboundNeighborsAttribute(accessRights, ctrlAccessRights, attribCheck);

            }
            
            #endregion

            #region ValidateMsDS_NCReplOutboundNeighborsAttributeAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.msDS_NCReplOutboundNeighbors)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateMsDS_NCReplOutboundNeighborsAttribute(accessRights, ctrlAccessRights, attribCheck);

            }

            #endregion
            
            #region msDS_NCReplCursorsAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.msDS_NCReplCursors)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateMsDS_NCReplCursor(accessRights, ctrlAccessRights, attribCheck);

            }


            #endregion

            #region servicePrincipleNameAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.servicePrincipleName)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateServicePrincipalName(accessRights, ctrlAccessRights, attribCheck);

            }


            #endregion
            
            #region dnsHostNameAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.dnsHostName)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateDNSHostname(accessRights, ctrlAccessRights, attribCheck);

            }



              #endregion

            #region writeDACLOperationeAttributeRequirementsValidation

            //Checking msDS_ReplAttributeMetaData
            if (attribCheck == AttribsToCheck.writeDACLOperation)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidatewriteDACLOperation(accessRights, ctrlAccessRights, attribCheck);

            }

            #endregion

            #region MoveOperationValidation

            if (attribCheck == AttribsToCheck.moveOperation)
            {
                //return the status of the Validation.
                return adtsRequirementsValidation.ValidateMoveOperation(accessRights, ctrlAccessRights, attribCheck);
            }
            #endregion

             return errorstatus.failure;
        }

        #endregion



        #endregion


        #region PasswordChangeOperation

        /// <summary>
        /// PasswordChangeOperation deals with modifying a password when the 
        /// old and new passwords are given
        /// </summary>
        /// <param name="vdel">old password</param>
        /// <param name="vadd">new password</param>
        /// <param name="fAllowOverNonSecure">fAllowPasswordOperationsOverNonSecureConnection flag of
        /// dsHeuristics attribute bit</param>
        /// <returns>errorstatus</returns>
        public errorstatus PasswordChangeOperation(Password vdel,
                                                   string vadd,
                                                   bool fAllowOverNonSecure)
        {




            #region Valid Credentials

            //password can be changed or set if the user has valid credentials only
                    // valid user and valid password credentials can only change the password
            if ((user == ClientUserName) &&
                        (userPassword == ClientUserPassword))
                    {
                         return adtsRequirementsValidation.ValidatePasswordChangeOperation(vdel, fAllowOverNonSecure, adTestType);

                    }
                    #endregion

                  

                    return errorstatus.success;
                
               
        }
        #endregion

        #region AdminPasswordReset
        /// <summary>
        /// AdminPasswordReset deals with administrative change of a password.
        /// The old password is not required.
        /// </summary>
        /// <param name="vrep">new password</param>
        /// <param name="fAllowOverNonSecure">fAllowPasswordOperationsOverNonSecureConnection flag of
        /// dsHeuristics attribute bit</param>
        /// <returns>errorstatus</returns>
        public errorstatus AdminPasswordReset(string vrep,
                                              bool fAllowOverNonSecure)
        {
                                      
                 return adtsRequirementsValidation.ValidateAdminPasswordReset(true, adTestType);  
              
        }
        #endregion
        
       
    }
}
