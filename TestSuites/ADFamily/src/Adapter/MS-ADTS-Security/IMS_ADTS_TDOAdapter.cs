// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using ProtocolMessageStructures;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    /// <summary>
    /// IMS_ADTS_SecurityAdapter interface consists of the model actions which
    /// are used for modeling and validating behaviors related to TDO Operations.
    /// The interface also inherits IAdapter
    /// Methods used for TDO operations are defined in this class
    /// </summary>
    public interface IMS_ADTS_TDOAdapter : IAdapter
    {

        /// <summary>
        /// InitializeOSVersion deals with setting the OS Version for the test-pass.
        /// InitializeOSVersion is called only once during any test-pass
        /// </summary>
        /// <param name="serverVersion">OS Version for the test pass</param>
        /// <param name="forestFuncLevel">Forest Functional Level for the test pass</param>
        void InitializeOSVersion(ServerVersion serverVersion,
                                 ForestFunctionalLevel forestFuncLevel);

        /// <summary>
        /// SetTrustedDomainObject is a top level client action which maps to
        /// LDAP/LSAD calls in the adapter, which creates and sets the attributes
        /// for a TDO Object
        /// </summary>
        /// <param name="nameAttribute">name of the trusted object</param>
        /// <param name="tdoObject">attributes of TDO are stored</param>
        /// <returns>bool</returns>
        bool SetInformationTDO(string nameAttribute,
                               TRUSTED_DOMAIN_OBJECT tdoObject);

        /// <summary>
        /// CreateTrustedAccounts is used for storing inter domain trust information.
        /// Information is required when trustDirection equals to:
        /// TRUST_DIRECTION_INBOUND or TRUST_DIRECTION_BIDIRECTIONAL 
        /// </summary>
        /// <param name="nameAttribute">name of the trusted object</param>
        /// <param name="interDomainAccInfo">for storing the interdomain trust
        /// information</param>
        void CreateTrustedAccounts(string nameAttribute,
                                   InterDomain_Trust_Info interDomainAccInfo);

        /// <summary>
        /// TrustedForestInformation is used for storing Trusted Forest Info Records
        /// when the trust attribute of the TDO is TRUST_ATTRIBUTE_FOREST_TRANSITIVE
        /// </summary>
        /// <param name="nameAttribute">name of the TDO</param>
        /// <param name="trustedForestInfo">Trust Forest Information Records</param>
        /// <returns>bool to indicate the action result</returns>
        bool TrustedForestInformation(string nameAttribute,
                                      Trusted_Forest_Information trustedForestInfo);

        /// <summary>
        /// TDOOperation is mapped to LDAP call which will check the TDO operations after
        /// the TDO attributes are set. The method seeks to model all the behaviors related
        /// to TDO attributes and features. TDO attributes will be changed one by one and 
        /// its behaviors are modeled and validated as per MS-ADTS TD Section: 7.1.6.
        /// Attributes of a TDO object are already stored as state variables.
        /// Assumption Enforced: Trusted Domain Object must be created prior to calling this method.
        ///                      Then the behavior is noted and validated in the implementation.
        /// In parameter nameAttribute will identify the Trusted Domain Object
        /// </summary>
        /// <param name="nameAttribute">name attribute of the TDO</param>
        /// <returns>bool</returns>
        bool TDOOperation(string nameAttribute);

        /// <summary>
        /// DeleteTDO is a top-level client action which is mapped to MS-LSAD
        /// call LsarDeleteTrustedDomain in adapter. Used for deleting a TDO.
        /// </summary>
        void DeleteTDO();
    }
}
