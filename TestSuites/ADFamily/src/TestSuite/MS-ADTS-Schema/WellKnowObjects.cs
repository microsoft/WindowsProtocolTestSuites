// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ActiveDs;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Security.Principal;
using System.Text;

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
using System.DirectoryServices.Protocols;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Partial class for DataSchemaTestSuite
    /// This file is the source file for Validation of the TestCase34 and TestCase35.
    /// </summary>
    public partial class DataSchemaTestSuite
    {
        #region WellKnownObjects Implementation.
        /// <summary>
        /// This method validates the requirements under 
        /// WellKnownObjects Scenario 
        /// </summary> 
        public void ValidateWellKnownObjects()
        {
            //List for holding the wellKnownObjects of each NC.
            List<string> wellKnownObjects = new List<string>();
            PropertyValueCollection memeberAttr = null;
            //List<string> memberAttr = new List<string>();

            //Domain naming context
            DirectoryEntry domainEntry;
            if (!adAdapter.GetObjectByDN(adAdapter.rootDomainDN, out domainEntry))
            {
                DataSchemaSite.Assume.IsTrue(false, adAdapter.rootDomainDN + " Object is not found in server");
            }

            //Configuration naming context 
            DirectoryEntry configEntry;
            if (!adAdapter.GetObjectByDN("CN=Configuration," + adAdapter.rootDomainDN, out configEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Configuration,"
                    + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }

            DirectoryEntry child, childOfChild, childOfChildOfChild;
            DirectoryEntries childEntries;
            //Variables for storing the values of systemflags.
            string systemFlag;
            int systemFlagVal, isrecycle;
            //Cresting variable for the Property value of objectClasses and isCriticalSystemObjects
            PropertyValueCollection objectClasses, isCriticalSystemObjects;
            bool isObjectClass = false;
            bool isParent = false;
            bool isFlag = true;
            bool isObjectSid = false;
            bool isPresent = true;

            //WellKnownObjects for domain NC and config NC.
            Object wKnownDomainObjects = domainEntry.Properties["wellKnownObjects"].Value;
            Object wKnownConfigObjects = configEntry.Properties["wellKnownObjects"].Value;
            
            #region GUID value of well-known container's.
            ///For each of the well-known GUIDs of a given NC, the wellKnownObjects attribute on the NC
            ///Root object must contain a value such that the binary portion matches the well-known GUID.
            foreach (DNWithBinary wkObjects in (IEnumerable)wKnownDomainObjects)
            {
                byte[] bytes = (byte[])wkObjects.BinaryValue;
                string byteToHex = BitConverter.ToString(bytes);
                string dnOfWellKnownObj = wkObjects.DNString;
                byteToHex = byteToHex.Replace("-", String.Empty);
                //Checking whether wellknownobject is repeated or not.
                if (wellKnownObjects.Contains(byteToHex.ToUpper()))
                {
                    isFlag = false;
                }
                wellKnownObjects.Add(byteToHex.ToUpper());
                //Checking whether DN of each wellknowobject is exists or not.
                if (dnOfWellKnownObj == null)
                {
                    isPresent = false;
                }
            }

            //MS-ADTS-Schema_R687 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isPresent,
                687,
                @"Within each NC (excluding the schema NC) there are certain well-known system objects that can be 
                referred to using a well-known GUID.Domain and config NC root objects contain an attribute called 
                wellKnownObjects that lists the well-known system objects within that NC. Each value in this list is an
                Object(DN-Binary) value where the Binary portion is the well-known GUID in binary form and  the DN 
                portion is the DN of the object. The well-known GUID can be used in conjunction with the NC DN to refer 
                to the object.");

            Object owKnownDomainObjects = domainEntry.Properties["otherWellKnownObjects"].Value;
            //MS-ADTS-Schema_otherWellKnownObjects
            if (owKnownDomainObjects != null)
            {
                DataSchemaSite.Log.Add(LogEntryKind.CheckSucceeded,
                    @"In addition to the wellKnownObjects attribute, each NC root object may also contain an attribute called 
                    otherWellKnownObjects that lists other WKOs. Objects listed in the attribute otherWellKnownObjects can be 
                    referred to in the same way as those in the attribute wellKnownObjects.");
            }

            //MS-ADTS-Schema_R688 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isFlag,
                688,
                @"The wellKnownObjects attribute on the NC root object must contain a value such that the binary portion 
                matches the well-known GUID. There must be exactly one such value.");

            //MS-ADTS-Schema_R691 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("AA312825768811D1ADED00C04FD8D5CD"),
                691,
                @"The GUID value of computer container symbolic name of the well-known GUID is
                AA312825768811D1ADED00C04FD8D5CD.");

            //MS-ADTS-Schema_R692 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("18E2EA80684F11D2B9AA00C04F79F805"),
                692,
                @"The GUID value of Deleted Objects symbolic name of The GUID value of Well-Known GUID  
                is 18E2EA80684F11D2B9AA00C04F79F805.");

            //MS-ADTS-Schema_R693 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("A361B2FFFFD211D1AA4B00C04FD7D83A"),
                693,
                @"The GUID value of Domain Controllers container symbolic name of The GUID value of Well-Known GUID is 
                A361B2FFFFD211D1AA4B00C04FD7D83A.");

            //MS-ADTS-Schema_R694 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("22B70C67D56E4EFB91E9300FCA3DC1AA"),
                694,
                @"The GUID value of ForeignSecurityPrincipal container symbolic name of The GUID value of Well-Known 
                GUID is 22B70C67D56E4EFB91E9300FCA3DC1AA.");

            //MS-ADTS-Schema_R695 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("2FBAC1870ADE11D297C400C04FD8D5CD"),
                695,
                @"The GUID value of infrastructure container symbolic name of The GUID value of Well-Known GUID is 
                2FBAC1870ADE11D297C400C04FD8D5CD.");

            //MS-ADTS-Schema_R696 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("AB8153B7768811D1ADED00C04FD8D5CD"),
                696,
                @"The GUID value of LostAndFound container symbolic name of The GUID value of Well-Known GUID 
                is AB8153B7768811D1ADED00C04FD8D5CD.");

            //MS-ADTS-Schema_R697 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("F4BE92A4C777485E878E9421D53087DB"),
                697,
                @"The GUID value of Microsoft Program Data container symbloc name of The GUID value of Well-Known 
                GUID is F4BE92A4C777485E878E9421D53087DB.");

            //MS-ADTS-Schema_R698 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("6227F0AF1FC2410D8E3BB10615BB5B0F"),
                698,
                @"The GUID value of NTDS Quota Container symbolic name of The GUID value of Well-Known GUID is
                6227F0AF1FC2410D8E3BB10615BB5B0F.");

            //MS-ADTS-Schema_R699 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("09460C08AE1E4A4EA0F64AEE7DAA1E5A"),
                699,
                @"The GUID value of Program Data Container symbolic name of The GUID value of Well-Known GUID 
                is 09460C08AE1E4A4EA0F64AEE7DAA1E5A.");

            //MS-ADTS-Schema_R700 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("AB1D30F3768811D1ADED00C04FD8D5CD"),
                700,
                @"The GUID value of System Container symbolic name of The GUID value of Well-Known GUID 
                is AB1D30F3768811D1ADED00C04FD8D5CD.");

            //MS-ADTS-Schema_R701 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("A9D1CA15768811D1ADED00C04FD8D5CD"),
                701,
                @"The GUID value of User Container symbolic name of The GUID value of Well-Known GUID 
                is A9D1CA15768811D1ADED00C04FD8D5CD.");

            #endregion

            bool isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=LostAndFound");
            objectClasses = child.Properties["objectClass"];
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            //Verify MS-AD_Schema requirement:MS-ADTS_R102986
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                102986,
                "The isCriticalSystemObject attribute of the Lost and Found Container Well-Known Objects is TRUE.");

            //MS-ADTS-Schema_R703 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("lostAndFound"),
                703,
                "The objectClass attribute of the Lost and Found Container Well-Known Objects must be lostAndFound");

            if (!Utilities.IsObjectExist(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN,adAdapter.PDCNetbiosName,
                adAdapter.ADDSPortNum,adAdapter.DomainAdministratorName,adAdapter.DomainUserPassword))
            {
                //To create the Application NC in the Active Directory.
                AdLdapClient.Instance().ConnectAndBind(adAdapter.PDCNetbiosName,
                    adAdapter.PDCIPAddr, Convert.ToInt32(adAdapter.ADDSPortNum), adAdapter.DomainAdministratorName,
                    adAdapter.DomainUserPassword, adAdapter.PrimaryDomainDnsName,
                    AuthType.Basic | AuthType.Kerberos);
                List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
                attrs.Add(new DirectoryAttribute("instancetype:5"));
                attrs.Add(new DirectoryAttribute("objectclass:domainDNS"));
                AdLdapClient.Instance().AddObject(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, attrs, null);
                AdLdapClient.Instance().Unbind();
            }

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");
            // MS-ADTS-Schema_R704 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                704,
                @"The systemFlags attribute of the Lost and Found Container Well-Known Objects on domain NCs must be  
                either of FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            DirectoryEntry applicationEntry, applicationChild;
            if (!adAdapter.GetObjectByDN(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, out applicationEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    adAdapter.PartitionPath + "," + adAdapter.rootDomainDN
                    + " Object is not found in server");
            }
            applicationChild = applicationEntry.Children.Find("CN=LostAndFound");
            systemFlag = applicationChild.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");
            // MS-ADTS-Schema_R705 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                705,
                @"The systemFlags attribute of the Lost and Found Container Well-Known Objects on application NCs must 
                be  either of FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            ResultPropertyValueCollection objectClass = null, isDeleted = null, systemFlags = null,
                isCriticalSystemObject = null, isre = null;
            bool isDel = true;
            bool isRecyclenull = true;
            isCriticalSystem = true;
            DirectorySearcher ds = new DirectorySearcher(domainEntry, "(isDeleted=TRUE)");
            ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
            ds.Tombstone = true;
            using (SearchResultCollection src = ds.FindAll())
            {
                foreach (System.DirectoryServices.SearchResult sr in src)
                {
                    objectClass = sr.Properties["objectClass"];
                    isDeleted = sr.Properties["isDeleted"];
                    if (isDeleted != null)
                    {
                        if (isDeleted.Count > 0)
                        {
                            if (!(bool)isDeleted[0])
                            {
                                isDel = false;
                            }
                        }
                    }
                    //IsCriticalSystemObjects of Deleted Objects
                    isCriticalSystemObject = sr.Properties["isCriticalSystemObject"];
                    if (isCriticalSystemObject != null)
                    {
                        if (isCriticalSystemObject.Count > 0)
                        {
                            if (!(bool)isCriticalSystemObject[0])
                            {
                                isCriticalSystem = false;
                            }
                        }
                    }
                    //
                    isre = sr.Properties["isRecycled"];
                    isrecycle = isre.Count;
                    if (0 != isrecycle)
                    {
                        isRecyclenull = false;
                    }
                    systemFlags = sr.Properties["systemFlags"];
                    systemFlag = systemFlags[0].ToString();
                }
            }

            // MS-ADTS-Schema_R709 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains("container"),
                709,
                 "The objectClass attribute of the Deleted Objects Container Well-Known Objects must be container.");
            // MS-ADTS-Schema_R710 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isDel,
                710,
                "The isDeleted attribute of the Deleted Objects Container Well-Known Objects must be true.");

            // MS-ADTS-Schema_R711 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                711,
                @"The systemFlags attribute of the Deleted Objects  Well-Known Objects container must be
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R4536
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isRecyclenull,
                4536,
                @"[the replication metadata for the isDeleted attribute must show that the time at which the isDeleted 
                attribute was set to true is 9999-12-29] Furthermore, the isRecycled attribute must have no values.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R102992
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isCriticalSystem,
                102992,
                @"The isCriticalSystemObject attribute of the Deleted Objects Container Well-Known Objects is TRUE.");

            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=NTDS Quotas");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of NTDS Quotas
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            applicationChild = applicationEntry.Children.Find("CN=NTDS Quotas");
            PropertyValueCollection objectClassesOfApplicationNC = applicationChild.Properties["objectClass"];

            // MS-ADTS-Schema_R713 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("msDS-QuotaContainer")
                && objectClassesOfApplicationNC.Contains("msDS-QuotaContainer"),
                713,
                "The objectClass attribute of the NTDS Quotas Container Well-Known Objects must be msDS-QuotaContainer.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");

            // MS-ADTS-Schema_R714 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                714,
                "The systemFlags attribute of the NTDS Quotas Container Well-Known Objects must be FLAG_DISALLOW_DELETE");

            //Verify MS-AD_Schema requirement:MS-ADTS_R102995
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                102995,
                @"The isCriticalSystemObject attribute of the NTDS Quotas Container Well-Known Objects is TRUE.");

            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=Infrastructure");
            objectClasses = child.Properties["objectClass"];

            //IsCriticalSystemObjects of Infrastructure 
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R717 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("infrastructureUpdate"),
                717,
                "The objectClass attribute of the Infrastructure Well-Known Objects must be infrastructureUpdate.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

            // MS-ADTS-Schema_R718 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                718,
                @"The systemFlags attribute of the Infrastructure Well-Known Objects must be
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103000
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103000,
                @"The isCriticalSystemObject attribute of the Infrastructure Object Well-Known Objects is TRUE.");

            string fSMORoleOfInfra = child.Properties["fSMORoleOwner"].Value.ToString();
            string dnOfNTDS = string.Empty;
            using (configEntry)
            {
                ds = new DirectorySearcher(configEntry, "(CN=NTDS Settings)");
                ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                using (SearchResultCollection src = ds.FindAll())
                {
                    foreach (System.DirectoryServices.SearchResult sr in src)
                    {
                        ResultPropertyValueCollection distinguishedname;
                        distinguishedname = sr.Properties["distinguishedname"];
                        dnOfNTDS = distinguishedname[0].ToString();
                        break;
                    }
                }
            }
            // MS-ADTS-Schema_R719 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                fSMORoleOfInfra,
                dnOfNTDS,
                719,
                @"The fSMORoleOwner attribute of the Infrastructure Well-Known Objects must be
                the value refers to the nTDSDSA object of the DC that owns the Infrastructure FSMO role.");

            isCriticalSystem = false;
            child = domainEntry.Children.Find("OU=Domain Controllers");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of Domain Controllers 
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R721 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("organizationalUnit"),
                721,
                "The objectClass attribute of the Domain Controllers OU Well-Known Objects must be organizationalUnit.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");
            // MS-ADTS-Schema_R722 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                722,
                @"The systemFlags attribute of the Domain Controllers OU Well-Known Objects must be 
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103003
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103003,
                @"The isCriticalSystemObject attribute of the Domain Controllers OU Well-Known Objects is TRUE.");

            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=Users");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of Users 
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R724 
            DataSchemaSite.CaptureRequirementIfIsTrue
                (objectClasses.Contains("container"),
                724,
                "The objectClass attribute of the Users Container Well-Known Objects must be container.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

            // MS-ADTS-Schema_R725 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                725,
                @"The systemFlags attribute of the Users Container Well-Known Objects must be 
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103006
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103006,
                 "The isCriticalSystemObject attribute of the Users Container Well-Known Objects is TRUE.");

            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=Computers");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of Computers  
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R727 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                727,
                "The objectClass attribute of the Computers Container Well-Known Objects must be container.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");
            // MS-ADTS-Schema_R728 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                728,
                @"The systemFlags attribute of the Computers Container Well-Known Objects must be 
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103009
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103009,
                "The isCriticalSystemObject attribute of the Computers Container Well-Known Objects is TRUE.");

            child = domainEntry.Children.Find("CN=Program Data");
            objectClasses = child.Properties["objectClass"];
            // MS-ADTS-Schema_R729 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                729,
                "The objectClass attribute of the Program Data Container Well-Known Objects must be container.");

            systemFlag = (string)child.Properties["systemFlags"].Value;
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                null,
                systemFlag,
                730,
                "The systemFlags attribute of the Program Data Container Well-Known Objects must be 0.");
            
            child = domainEntry.Children.Find("CN=Managed Service Accounts");
            objectClasses = child.Properties["objectClass"];
            // MS-ADTS-Schema_Managed Service Accounts Container
            DataSchemaSite.Assert.IsTrue(objectClasses.Contains("container"),
                "The objectClass attribute of the Managed Service Accounts Container Well-Known Objects must be container.");

            systemFlag = (string)child.Properties["systemFlags"].Value;
            DataSchemaSite.Assert.AreEqual<string>(
                null,
                systemFlag,
                "The systemFlags attribute of the Managed Service Accounts Container Well-Known Objects must be 0.");
            
            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=ForeignSecurityPrincipals");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of ForeignSecurityPrincipals  
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R737 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                child != null,
                737,
                @"For the Foreign Security Principals Container Well-Known Objects the Parent must be domain NC root 
                on AD/DS.");

            // MS-ADTS-Schema_R739 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                739,
                @"The objectClass attribute of the Foreign Security Principals Container Well-Known Objects 
                must be container.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

            // MS-ADTS-Schema_R740 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                740,
                @"The systemFlags attribute of the Foreign Security Principals Container Well-Known Objects must be  
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103026
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103026,
                @"The isCriticalSystemObject attribute of the Foreign Security Principals Container Well-Known Objects 
                is TRUE.");

            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=System");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of System  
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R741 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                child != null,
                741,
                "For the System Container Well-Known Objects the Parent must be Domain NC root object.");

            // MS-ADTS-Schema_R742 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                742,
                "The objectClass attribute of the System Container Well-Known Objects must be container.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

            // MS-ADTS-Schema_R743 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                743,
                @"The systemFlags attribute of the System Container Well-Known Objects must be 
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103029
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103029,
                "The isCriticalSystemObject attribute of the System Container Well-Known Objects is TRUE.");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                childOfChild = child.Children.Find("CN=Password Settings Container");
                objectClasses = childOfChild.Properties["objectClass"];

                // MS-ADTS-Schema_R747 
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    childOfChild != null,
                    747,
                    "For the Password Settings Container Well-Known Objects the Parent must be System container.");

                // MS-ADTS-Schema_R748 
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClasses.Contains("msDS-PasswordSettingsContainer"),
                    748,
                    @"The objectClass attribute of the Password Settings Container Well-Known  
                    Objects must be msDS-PasswordSettingsContainer.");

                systemFlag = childOfChild.Properties["systemFlags"].Value.ToString();
                systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

                // MS-ADTS-Schema_R749 
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    systemFlagVal.ToString(),
                    systemFlag,
                    749,
                    @"The systemFlags attribute of Password Settings Container Well-Known Objects must be 
                    FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");
            }
            isCriticalSystem = false;
            child = domainEntry.Children.Find("CN=System");
            childOfChild = child.Children.Find("CN=AdminSDHolder");
            objectClasses = childOfChild.Properties["objectClass"];
            //IsCriticalSystemObjects of AdminSDHolder  
            isCriticalSystemObjects = childOfChild.Properties["isCriticalSystemObject"];

            //Verify MS-AD_Schema requirement:MS-ADTS_R103078
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103078,
                @"The isCriticalSystemObject attribute of AdminSDHolder Well-Known Objects is TRUE.");
            //
            // MS-ADTS-Schema_R790 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childOfChild != null,
                790,
                "For the AdminSDHolder Other Well-Known Objects the Parent must be System container.");

            // MS-ADTS-Schema_R791 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                791,
                "The objectClass attribute of AdminSDHolder Other Well-Known Objects must be container.");

            systemFlag = childOfChild.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

            // MS-ADTS-Schema_R792 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                792,
                @"The systemFlags attribute of AdminSDHolder Well-Known Objects must be 
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            isCriticalSystem = false;
            childOfChild = child.Children.Find("CN=Default Domain Policy");
            objectClasses = childOfChild.Properties["objectClass"];
            //IsCriticalSystemObjects of Default Domain Policy  
            isCriticalSystemObjects = childOfChild.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R793 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childOfChild != null,
                793,
                "For the Default Domain Policy Container Other Well-Known Objects the Parent must be System Container.");
            // MS-ADTS-Schema_R794 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("domainPolicy"),
                794,
                @"The objectClass attribute of Default Domain Policy Container Other Well-Known Objects must 
                be domainPolicy.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103080
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103080,
                @"The isCriticalSystemObject attribute of Default Domain Policy Container Well-Known Objects is TRUE.");

            childOfChild = child.Children.Find("CN=Server");
            objectClasses = childOfChild.Properties["objectClass"];

            // MS-ADTS-Schema_R795 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childOfChild != null,
                795,
                "For the Sam Server Other Well-Known Objects the Parent must be System Container.");

            // MS-ADTS-Schema_R796 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("samServer"),
                796,
                "The objectClass attribute of Sam Server Other Well-Known Objects must be samServer.");

            if (childOfChild.Properties["systemFlags"].Value != null)
                systemFlag = childOfChild.Properties["systemFlags"].Value.ToString();
            else
                systemFlag = String.Empty;

            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");
            if (serverOS == OSVersion.WinSvr2012)
            {
                // MS-ADTS-Schema_R797 
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    String.Empty,
                    systemFlag,
                    797,
                    @"Domain controllers running Windows Server 2012 operating system do not create the systemFlags attribute on the Sam Server object");
            }
            else
            {
                // MS-ADTS-Schema_R797 
                DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                    systemFlagVal.ToString(),
                    systemFlag,
                    797,
                    @"The systemFlags attribute of Sam Server Other Well-Known Objects must be FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");
            }

            //From child which contains the system container of domain NC we are finding the DomainUpdates by 
            //searching the childs of system container.
            childOfChild = child.Children.Find("CN=DomainUpdates");
            objectClasses = childOfChild.Properties["objectClass"];

            // MS-ADTS-Schema_R798 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childOfChild != null,
                798,
                "For the Domain Updates Container Other Well-Known Objects the Parent must be System Container.");

            //Checking whether objectClass of DomainUpdates container consists of class container or not.
            // MS-ADTS-Schema_R799 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                799,
                "The objectClass attribute of Domain Updates Container Other Well-Known Objects must be container.");

            //ChildOfChild directory entry is now Domain Updates
            childOfChildOfChild = childOfChild.Children.Find("CN=Operations");
            objectClasses = childOfChildOfChild.Properties["objectClass"];

            // MS-ADTS-Schema_R800 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childOfChildOfChild != null,
                800,
                "For the Operations Container Other Well-Known Objects the Parent must be Domain Updates Container.");
            // MS-ADTS-Schema_R801 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                801,
                "The objectClass attribute of Operations Container Other Well-Known Objects must be container.");

            childOfChildOfChild = childOfChild.Children.Find("CN=Windows2003Update");
            objectClasses = childOfChildOfChild.Properties["objectClass"];

            // MS-ADTS-Schema_R802 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                childOfChildOfChild != null,
                802,
                @"For the Windows2003Updates Container Other Well-Known Objects the Parent must be Domain 
                Updates Container.");

            // MS-ADTS-Schema_R803 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                803,
                "The objectClass attribute of Windows2003Updates Container Other Well-Known Objects must be container.");

            if (serverOS >= OSVersion.WinSvr2008)
            {
                childOfChildOfChild = childOfChild.Children.Find("CN=ActiveDirectoryUpdate");
                objectClasses = childOfChildOfChild.Properties["objectClass"];

                // MS-ADTS-Schema_R805 
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    childOfChildOfChild != null,
                    805,
                    @"For the ActiveDirectoryUpdate Container Other Well-Known Objects the Parent must be Domain 
                    Updates Container.");

                // MS-ADTS-Schema_R806 
                DataSchemaSite.CaptureRequirementIfIsTrue(
                    objectClasses.Contains("container"),
                    806,
                    @"The objectClass attribute of ActiveDirectoryUpdate Container Other Well-Known Objects  
                    must be container.");
            }

            child = domainEntry.Children.Find("CN=Builtin");
            objectClasses = child.Properties["objectClass"];

            // MS-ADTS-Schema_R752 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                child != null,
                752,
                @"For the Builtin Container Well-Known Objects the 
                Parent must be domain NC root.");

            // MS-ADTS-Schema_R753 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("builtinDomain"),
                753,
                "The objectClass attribute of the Builtin Container Well-Known Objects must be builtinDomain.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");

            // MS-ADTS-Schema_R754 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                754,
                @"The systemFlags attribute of the Builtin Container Well-Known Objects must be 
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE .");

            byte[] objSidBuiltIn = (byte[])child.Properties["objectSid"].Value;
            SecurityIdentifier sid = new SecurityIdentifier(objSidBuiltIn, 0);
            string objectSidBuiltIn = sid.ToString();
            string childEntrySid = string.Empty;
            string primaryGroupToken = string.Empty;
            isParent = isObjectClass = isFlag = isObjectSid = true;
            bool isGroupType = true;
            childEntries = child.Children;
            foreach (DirectoryEntry childEntry in childEntries)
            {
                //Checking whether the parent is "Builtin" or not
                if (!childEntry.Parent.Name.Equals("CN=Builtin"))
                {
                    isParent = false;
                }
                //Checking whether the objectClass is "group" or not
                objectClasses = childEntry.Properties["objectClass"];
                if (!objectClasses.Contains((object)"group"))
                {
                    isObjectClass = false;
                }
                //Checking whether the "systemFlags" attribute is 
                //"FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE" or not
                systemFlag = childEntry.Properties["systemFlags"].Value.ToString();
                if (!systemFlag.Equals(systemFlagVal.ToString()))
                {
                    isFlag = false;
                }
                byte[] objSid = (byte[])childEntry.Properties["objectSid"].Value;
                SecurityIdentifier sidChild = new SecurityIdentifier(objSid, 0);
                childEntrySid = sidChild.ToString();
                childEntry.RefreshCache(new string[] { "primaryGroupToken" });
                primaryGroupToken = childEntry.Properties["primaryGroupToken"].Value.ToString();
                if (childEntrySid != (objectSidBuiltIn + "-" + primaryGroupToken))
                {
                    isObjectSid = false;
                }
                PropertyValueCollection groupType = childEntry.Properties["groupType"];
                GroupTypeFlags gType = (GroupTypeFlags)Convert.ToInt32(groupType.Value);

                // MS-ADTS-Schema_ValidateGroupTypeExclusiveness
                DataSchemaSite.Assert.IsTrue(
                    GroupType.ValidateFlags(gType),
                    @"The flag GROUP_TYPE_BUILTIN_LOCAL_GROUP is reserved for use by the system, and can be set in combination with other flags on system-created Builtin objects. 
                   Otherwise, the flags GROUP_TYPE_ACCOUNT_GROUP, GROUP_TYPE_RESOURCE_GROUP, GROUP_TYPE_UNIVERSAL_GROUP, GROUP_TYPE_APP_BASIC_GROUP, and GROUP_TYPE_APP_QUERY_GROUP are mutually exclusive, 
                   and only one value must be set. The flag GROUP_TYPE_SECURITY_ENABLED can be combined using a bitwise OR with flags GROUP_TYPE_BUILTIN_LOCAL_GROUP, GROUP_TYPE_ACCOUNT_GROUP, 
                   GROUP_TYPE_RESOURCE_GROUP, and GROUP_TYPE_UNIVERSAL_GROUP.");

                if (gType != (GroupTypeFlags.GROUP_TYPE_BUILTIN_LOCAL_GROUP | GroupTypeFlags.GROUP_TYPE_RESOURCE_GROUP | GroupTypeFlags.GROUP_TYPE_SECURITY_ENABLED))
                {
                    isGroupType = false;
                }
            }

            // MS-ADTS-Schema_R756 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isParent,
                756,
                @"For each child of the Builtin Container Well-Known Objects the  
                Parent must be Builtin container.");

            // MS-ADTS-Schema_R757 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectClass,
                757,
                @"The objectClass attribute for each child of the 
                Builtin Container  Well-Known Objects must be 'group'.");

            // MS-ADTS-Schema_R758 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isObjectSid,
                758,
                @"The objectSid attribute for each child of the
                Builtin Container  Well-Known Objects must be combination of built-in domain SID and RID.");

            // MS-ADTS-Schema_R759 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isFlag,
                759,
                @"The systemFlags attribute for each child of the Builtin Container  Well-Known Objects must be  
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE .");

            // MS-ADTS-Schema_R760 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isGroupType,
                760,
                @"The groupType attribute for each child of the Builtin Container  Well-Known Objects must be 
                GROUP_TYPE_BUILTIN_LOCAL_GROUP | GROUP_TYPE_RESOURCE_GROUP | GROUP_TYPE_SECURITY_ENABLED.");

            childOfChild = child.Children.Find("CN=Administrators");
            memeberAttr = childOfChild.Properties["member"];
            childOfChild = child.Children.Find("CN=Guests");
            PropertyValueCollection memeberAttrGuests = childOfChild.Properties["member"];
            child = domainEntry.Children.Find("CN=users");
            DirectoryEntry userChild = child.Children.Find($"CN={adAdapter.DomainAdministratorName}");
            string dnOfAdmin = userChild.Properties["distinguishedName"].Value.ToString();
            userChild = child.Children.Find("CN=Domain Admins");
            string dnOfDomainAdmin = userChild.Properties["distinguishedName"].Value.ToString();
            userChild = child.Children.Find("CN=Enterprise Admins");
            string dnOfEnterpriseAdmin = userChild.Properties["distinguishedName"].Value.ToString();

            // MS-ADTS-Schema_R761 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (memeberAttr.Contains(dnOfAdmin)
                && memeberAttr.Contains(dnOfDomainAdmin)
                && memeberAttr.Contains(dnOfEnterpriseAdmin)),
                761,
                @"The member attribute of Administrators Group Object must be Administrator, Domain Administrators, 
                Enterprise Administrators");

            userChild = child.Children.Find("CN=Guest");
            string dnOfGuest = userChild.Properties["distinguishedName"].Value.ToString();
            userChild = child.Children.Find("CN=Domain Guests");
            string dnOfDomainGuests = userChild.Properties["distinguishedName"].Value.ToString();
            // MS-ADTS-Schema_R762 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (memeberAttrGuests.Contains(dnOfGuest)
                && memeberAttrGuests.Contains(dnOfDomainGuests)),
                762,
                "The member attribute of Guests Group Object must be Guest, Domain Guests.");
        }

        #endregion

        #region LDSWellKnowObjects Implementation.

        /// <summary>
        /// This method validates the requirements under 
        /// LDSWellKnowObjects Scenario.
        /// </summary>
        public void ValidateLDSWellKnownObjects()
        {
            //Configuration naming context 
            DirectoryEntry configEntry;
            if (!adAdapter.GetLdsObjectByDN("CN=Configuration," + adAdapter.LDSRootObjectName, out configEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    "CN=Configuration,"
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            Object wKnownConfigObjects = configEntry.Properties["wellKnownObjects"].Value;
            DirectoryEntry child;
            //Variables for storing the values of systemflags.
            string systemFlag;
            int systemFlagVal;
            int isrecycle;
            //Cresting variable for the Property value of objectClasses,isCriticalSystemObjects
            PropertyValueCollection objectClasses, isCriticalSystemObjects;
            List<string> wellKnownObjects = new List<string>();

            foreach (DNWithBinary wkObjects in (IEnumerable)wKnownConfigObjects)
            {
                byte[] bytes = (byte[])wkObjects.BinaryValue;
                string byteToHex = BitConverter.ToString(bytes);
                string dnOfWellKnownObj = wkObjects.DNString;
                byteToHex = byteToHex.Replace("-", "");
                wellKnownObjects.Add(byteToHex.ToUpper());
            }

            // MS-ADTS-Schema_R692 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("18E2EA80684F11D2B9AA00C04F79F805"),
                692,
                @"The GUID value of Deleted Objects symbolic name of The GUID value of Well-Known GUID  
                is 18E2EA80684F11D2B9AA00C04F79F805.");

            // MS-ADTS-Schema_R694 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("22B70C67D56E4EFB91E9300FCA3DC1AA"),
                694,
                @"The GUID value of ForeignSecurityPrincipal container symbolic name of The GUID value of Well-Known 
                GUID is 22B70C67D56E4EFB91E9300FCA3DC1AA.");

            // MS-ADTS-Schema_R696 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("AB8153B7768811D1ADED00C04FD8D5CD"),
                696,
                @"The GUID value of LostAndFound container symbolic name of The GUID value of Well-Known GUID 
                is AB8153B7768811D1ADED00C04FD8D5CD.");

            // MS-ADTS-Schema_R698 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("6227F0AF1FC2410D8E3BB10615BB5B0F"),
                698,
                @"The GUID value of NTDS Quota Container symbolic name of The GUID value of Well-Known GUID is
                6227F0AF1FC2410D8E3BB10615BB5B0F.");

            // MS-ADTS-Schema_R701 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                wellKnownObjects.Contains("A9D1CA15768811D1ADED00C04FD8D5CD"),
                701,
                @"The GUID value of User Container symbolic name of The GUID value of Well-Known GUID 
                is A9D1CA15768811D1ADED00C04FD8D5CD.");

            child = configEntry.Children.Find("CN=LostAndFoundConfig");
            objectClasses = child.Properties["objectClass"];

            // MS-ADTS-Schema_R703 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("lostAndFound"),
                703,
                "The objectClass attribute of the Lost and Found Container Well-Known Objects must be lostAndFound");

            if (!Utilities.IsObjectExist(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, adAdapter.PDCNetbiosName,
                    adAdapter.ADDSPortNum, adAdapter.DomainAdministratorName, adAdapter.DomainUserPassword))
            {
                //To create the Application NC in the Active Directory.
                AdLdapClient.Instance().ConnectAndBind(adAdapter.PDCNetbiosName,
                    adAdapter.PDCIPAddr, Convert.ToInt32(adAdapter.ADDSPortNum), adAdapter.DomainAdministratorName,
                    adAdapter.DomainUserPassword, adAdapter.PrimaryDomainDnsName,
                    AuthType.Basic | AuthType.Kerberos);
                List<DirectoryAttribute> attrs = new List<DirectoryAttribute>();
                attrs.Add(new DirectoryAttribute("instancetype:5"));
                attrs.Add(new DirectoryAttribute("objectclass:domainDNS"));
                AdLdapClient.Instance().AddObject(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, attrs, null);
                AdLdapClient.Instance().Unbind();
            }

            DirectoryEntry applicationEntry, applicationChild;
            if (!adAdapter.GetObjectByDN(adAdapter.PartitionPath + "," + adAdapter.rootDomainDN, out applicationEntry))
            {
                DataSchemaSite.Assume.IsTrue(
                    false,
                    adAdapter.PartitionPath
                    + ","
                    + adAdapter.LDSRootObjectName
                    + " Object is not found in server");
            }
            applicationChild = applicationEntry.Children.Find("CN=LostAndFound");
            systemFlag = applicationChild.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE");
            // MS-ADTS-Schema_R705 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                705,
                @"The systemFlags attribute of the Lost and Found Container Well-Known Objects on application NCs must 
                be  either of FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            ResultPropertyValueCollection objectClass = null, isDeleted = null, systemFlags = null,
                isCriticalSystemObject = null, isre = null;
            bool isDel = true;
            bool isRecyclenull = true;
            bool isCriticalSystem = true;
            DirectorySearcher ds = new DirectorySearcher(configEntry, "(isDeleted=TRUE)");
            ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
            ds.Tombstone = true;
            using (SearchResultCollection src = ds.FindAll())
            {
                foreach (System.DirectoryServices.SearchResult sr in src)
                {
                    objectClass = sr.Properties["objectClass"];
                    isDeleted = sr.Properties["isDeleted"];
                    if (isDeleted != null)
                    {
                        if (isDeleted.Count > 0)
                        {
                            if (!(bool)isDeleted[0])
                            {
                                isDel = false;
                            }
                        }
                    }
                    //IsCriticalSystemObjects of Deleted Objects
                    isCriticalSystemObject = sr.Properties["isCriticalSystemObject"];
                    if (isCriticalSystemObject != null)
                    {
                        if (isCriticalSystemObject.Count > 0)
                        {
                            if (!(bool)isCriticalSystemObject[0])
                            {
                                isCriticalSystem = false;
                            }
                        }
                    }
                    isre = sr.Properties["isRecycled"];
                    isrecycle = isre.Count;
                    if (0 != isrecycle)
                    {
                        isRecyclenull = false;
                    }
                    systemFlags = sr.Properties["systemFlags"];
                    systemFlag = systemFlags[0].ToString();
                }
            }

            // MS-ADTS-Schema_R709 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClass.Contains("container"),
                709,
                "The objectClass attribute of the Deleted Objects Container Well-Known Objects must be container.");

            // MS-ADTS-Schema_R710 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isDel,
                710,
                "The isDeleted attribute of the Deleted Objects Container Well-Known Objects must be true.");

            // MS-ADTS-Schema_R711 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                711,
                @"The systemFlags attribute of the Deleted Objects  Well-Known Objects container must be
                FLAG_DISALLOW_DELETE | FLAG_DOMAIN_DISALLOW_RENAME | FLAG_DOMAIN_DISALLOW_MOVE.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R4536
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isRecyclenull,
                4536,
                @"[the replication metadata for the isDeleted attribute must show that the time at which the isDeleted 
                attribute was set to true is 9999-12-29] Furthermore, the isRecycled attribute must have no values.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R102992
            DataSchemaSite.CaptureRequirementIfIsTrue(
                isCriticalSystem,
                102992,
                @"The isCriticalSystemObject attribute of the Deleted Objects Container Well-Known Objects is TRUE.");

            isCriticalSystem = false;
            child = configEntry.Children.Find("CN=NTDS Quotas");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of NTDS Quotas
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            applicationChild = applicationEntry.Children.Find("CN=NTDS Quotas");
            PropertyValueCollection objectClassesOfApplicationNC = applicationChild.Properties["objectClass"];
            //MS-ADTS-Schema_R713 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("msDS-QuotaContainer")
                && objectClassesOfApplicationNC.Contains("msDS-QuotaContainer"),
                713,
                "The objectClass attribute of the NTDS Quotas Container Well-Known Objects must be msDS-QuotaContainer.");

            systemFlag = child.Properties["systemFlags"].Value.ToString();
            systemFlagVal = ParseSystemFlagsValue("FLAG_DISALLOW_DELETE");

            // MS-ADTS-Schema_R714 
            DataSchemaSite.CaptureRequirementIfAreEqual<string>(
                systemFlagVal.ToString(),
                systemFlag,
                714,
                "The systemFlags attribute of the NTDS Quotas Container Well-Known Objects must be FLAG_DISALLOW_DELETE");

            //Verify MS-AD_Schema requirement:MS-ADTS_R102995
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                102995,
                @"The isCriticalSystemObject attribute of the NTDS Quotas Container Well-Known Objects is TRUE.");

            isCriticalSystem = false;
            child = configEntry.Children.Find("CN=ForeignSecurityPrincipals");
            objectClasses = child.Properties["objectClass"];
            //IsCriticalSystemObjects of ForeignSecurityPrincipals  
            isCriticalSystemObjects = child.Properties["isCriticalSystemObject"];

            // MS-ADTS-Schema_R737 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                child != null,
                738,
                @"For the Foreign Security Principals Container Well-Known Objects the Parent must be config NC root 
                on AD/LDS.");

            // MS-ADTS-Schema_R739 
            DataSchemaSite.CaptureRequirementIfIsTrue(
                objectClasses.Contains("container"),
                739,
                @"The objectClass attribute of the Foreign Security Principals Container Well-Known Objects 
                must be container.");

            //Verify MS-AD_Schema requirement:MS-ADTS_R103026
            DataSchemaSite.CaptureRequirementIfIsTrue(
                (bool)isCriticalSystemObjects.Value,
                103026,
                @"The isCriticalSystemObject attribute of the Foreign Security Principals Container Well-Known Objects 
                is TRUE.");
        }

        #endregion
    }
}