// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Protocols.TestTools;

using ProtocolMessageStructures;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    /// <summary>
    /// QueryPackageStatus conveys whether the package discovery mechanism is successful
    /// The authentication scheme is passed as part of SicilyNegotiate request. This gives the
    /// response that whether the Server supports the specified authentication scheme.
    /// </summary>
    /// <param name="packageAllowed">bool parameter which signifies whether
    /// the queried authentication scheme is supported by server for SicilyBind</param>
    public delegate void QueryPackageStatus(bool packageAllowed);

    /// <summary>
    /// IMS_ADTS_AuthenticationAuth interface consists of the model actions which
    /// are used for modeling and validating behaviors related to Authentication
    /// mechanisms, Authorization, Password Change operations.
    /// The interface also inherits IAdapter
    /// Methods used for modeling and validating behaviors related to Authentication
    /// mechanisms, Authorization, Password Change operations are defined in this Interface
    /// </summary>
    public interface IMS_ADTS_AuthenticationAuth : IAdapter
    {
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
        void InitializeSession(bool packageDiscovery,
                               Common.ServerVersion serverVersion,
                               ADTypes adType);

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
        errorstatus SimpleBind(name userName,
                               Password passWord,
                               Port portNum,
                               bool enableTLS);


        /// <summary>
        /// Fast bind
        /// This action is used for doing fast authentication.
        /// This method is used for authentication the Domain user
        /// on regular LDAP ports
        /// </summary>
        /// <param name="username">Contains username in the domain</param>
        /// <param name="passWord">Contains the password to the username</param>
        /// <param name="portNum">Contains the port number over which the bind will accomplish</param>
        /// <param name="enableTLS">This variable i used to state when we are using TLS</param>
        /// <returns>Returns Success if the method is successful</returns>
        /// <returns>Returns InvalidCredentials if the passed in credentials are invalid</returns>
        errorstatus FastBind(name username,
                             Password passWord,
                             Port portNum,
                             bool enableTLS);



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
        /// Returns InvalidCredentials if the passed in credentials are invalid</returns>
        errorstatus SASLAuthentication(name userName,
                                       SASLChoice saslMech,
                                       Password passWord,
                                       Port portNum,
                                       bool enableTLS);

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
        errorstatus MutualAuthBind(name userName,
                                   Password passWord,
                                   bool validSPN);
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
        void SicilyNegotiate(string authProtocol);

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
        errorstatus SicilyBind(name userName,
                               Password passWord,
                               Port portNum,
                               bool enableTLS);


        /// <summary>
        /// PasswordChangeOperation deals with modifying a password when the 
        /// old and new passwords are given
        /// </summary>
        /// <param name="vdel">old password</param>
        /// <param name="vadd">new password</param>
        /// <param name="fAllowOverNonSecure">fAllowPasswordOperationsOverNonSecureConnection flag of
        /// dsHeuristics attribute bit</param>
        /// <returns>errorstatus</returns>
        errorstatus PasswordChangeOperation(Password vdel,
                                            string vadd,
                                            bool fAllowOverNonSecure);

        /// <summary>
        /// AdminPasswordReset deals with administrative change of a password.
        /// The old password is not required.
        /// </summary>
        /// <param name="vrep">new password</param>
        /// <param name="fAllowOverNonSecure">fAllowPasswordOperationsOverNonSecureConnection flag of
        /// dsHeuristics attribute bit</param>
        /// <returns>errorstatus</returns>
        errorstatus AdminPasswordReset(string vrep,
                                       bool fAllowOverNonSecure);

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
        errorstatus AuthorizationCheck(AccessRights accessRights,
                                       ControlAccessRights ctrlAccessRights,
                                       AttribsToCheck attribCheck,
                                       bool fUserPwdSupport);

        /// <summary>
        /// SicilyPacakageResponse is an event which receives data from the adapter
        /// It signifies whether the package delivery scheme as requested through
        /// SicilyNegotiate is successful. The authentication scheme as passed from
        /// SicilyNegotiate is supported by the Server
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event QueryPackageStatus SicilyPacakageResponse;
    }
}
