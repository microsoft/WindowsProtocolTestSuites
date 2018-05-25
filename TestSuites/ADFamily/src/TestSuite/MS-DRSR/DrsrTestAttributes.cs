// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using System.Diagnostics;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    /// <summary>
    /// Specify the possible types of DC server.
    /// </summary>
    public enum DcServerTypes
    {
        /// <summary>
        /// Any DC.
        /// </summary>
        Any,

        /// <summary>
        /// A writable DC.
        /// </summary>
        WritableDC,

        /// <summary>
        /// A readonly DC.
        /// </summary>
        RODC,

        /// <summary>
        /// A writable DC in child domain.
        /// </summary>
        ChildDC,

        /// <summary>
        /// A writable DC in external domain.
        /// </summary>
        ExternalDC,

        /// <summary>
        /// A GC server.
        /// </summary>
        GC
    }

    /// <summary>
    /// Specify the possible AD instance types.
    /// </summary>
    public enum ADInstanceType
    {
        /// <summary>
        /// AD DS
        /// </summary>
        DS,

        /// <summary>
        /// AD DS
        /// </summary>
        LDS,

        /// <summary>
        /// AD DS or AD LDS
        /// </summary>
        Both
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class DrsrTestAttribute : Attribute
    {
        public static ITestSite Site
        {
            get;
            set;
        }

        public virtual void Do()
        {
        }
    }


    /// <summary>
    /// Used to specify the least supported function level of a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FunctionLevelAttribute : DrsrTestAttribute
    {
        DrsrDomainFunctionLevel funcLv;

        public FunctionLevelAttribute(DrsrDomainFunctionLevel functionLevel)
        {
            funcLv = functionLevel;
        }

        public DrsrDomainFunctionLevel FunctionLevel
        {
            get
            {
                return funcLv;
            }
        }


        public override void Do()
        {
            if (Site == null)
                return;

            Site.Log.Add(LogEntryKind.Comment, "Test environment Domain Function Level: " + EnvironmentConfig.DomainLevel);
            Site.Log.Add(LogEntryKind.Comment, "Test case expects Domain Function Level: " + funcLv);
            if (funcLv > EnvironmentConfig.DomainLevel)
                throw new ApplicationException("Test case expects Domain Function Level should be Equal or Less than environment Domain Function Level");
        }
    }

    /// <summary>
    /// Used to specify if a test method is BVT.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BVTAttribute : DrsrTestAttribute
    {
        public override void Do()
        {
            if (Site == null)
                return;
            Site.Log.Add(LogEntryKind.Comment, "This is a BVT level test case");
        }
    }

    /// <summary>
    /// Used to specify whether the type of the DC server.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ServerTypeAttribute : DrsrTestAttribute
    {
        DcServerTypes svrType = DcServerTypes.Any;
        public ServerTypeAttribute(DcServerTypes serverType)
        {
            svrType = serverType;
        }

        public DcServerTypes ServerType
        {
            get
            {
                return svrType;
            }
        }
    }

    /// <summary>
    /// Used to specify the required FSMO roles for server.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ServerFSMORoleAttribute : DrsrTestAttribute
    {
        FSMORoles serverRoles = FSMORoles.None;
        public ServerFSMORoleAttribute(FSMORoles serverFSMORole)
        {
            serverRoles = serverFSMORole;
        }

        public FSMORoles ServerFSMORole
        {
            get
            {
                return serverRoles;
            }
        }
    }

    /// <summary>
    /// Used to specify at least one DC partner of DC server is required for running a test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class RequireDcPartnerAttribute : DrsrTestAttribute
    {
        public RequireDcPartnerAttribute()
        {
        }
    }

    /// <summary>
    /// Used to specify the client must be a DC client.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DcClientAttribute : DrsrTestAttribute
    {
        public DcClientAttribute()
        {
        }
    }

    /// <summary>
    /// Used to specify the AD instance types that supported by a test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class SupportedADTypeAttribute : DrsrTestAttribute
    {
        ADInstanceType type;

        public SupportedADTypeAttribute(ADInstanceType adType)
        {
            type = adType;
        }

        public ADInstanceType ADType
        {
            get
            {
                return type;
            }
        }

        public override void Do()
        {

            if (Site == null)
                return;
            Site.Log.Add(LogEntryKind.Comment, "Test environment is testing AD DS: " + EnvironmentConfig.TestDS);
            Site.Log.Add(LogEntryKind.Comment, "Test case expects AD DS: " + (type == ADInstanceType.LDS ? false : true));
            DrsrTestChecker.CheckADSupport(type);
        }
    }


    /// <summary>
    /// Used to specify the test case will break environment
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BreakEnvironment : DrsrTestAttribute
    {
        public override void Do()
        {
            if (Site == null)
                return;
            if (!ADCommonServerAdapter.Instance(Site).AllowBreakEnvironment)
                throw new ApplicationException("Case is not allowed to run because PTFConfig set \"AllowBreakEnvironment\" to false");
            Site.Log.Add(LogEntryKind.Comment, "Test case will break environment, only can be recovered by reverting VM");
            EnvironmentConfig.EnvBroken = true;
        }
    }


    public class DrsrTestChecker
    {
        /// <summary>
        /// throw exception if Ad instance does not match requirement
        /// </summary>
        /// <param name="current"></param>
        public static void CheckADSupport(ADInstanceType current)
        {
            if (current == ADInstanceType.DS && !EnvironmentConfig.TestDS)
                throw new ApplicationException("This is only supported in AD DS environment");
            else if (current == ADInstanceType.LDS && EnvironmentConfig.TestDS)
                throw new ApplicationException("This is only supported in AD LDS environment");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "System.String.StartsWith(System.String)")]
        public static void Check()
        {
            StackTrace trace = new System.Diagnostics.StackTrace();
            StackFrame[] frames = trace.GetFrames();

            //find who is calling me
            System.Reflection.MethodBase method = frames[1].GetMethod();
            DrsrTestAttribute.Site = EnvironmentConfig.TestSite;
            object[] attrs = method.GetCustomAttributes(false);
            if (attrs == null)
                return;
            string level = null;
            foreach (object o in attrs)
            {
                //for out test attribute, invoke "Do" method, for MSTEST attributes, do accordingly
                Type thisType = o.GetType();
                switch (thisType.Name)
                {
                    case "DescriptionAttribute":
                        {
                            EnvironmentConfig.TestSite.Log.Add(LogEntryKind.Comment, "Test description: " + typeof(DescriptionAttribute).GetProperty("Description").GetValue(o, null).ToString());
                        }
                        break;
                    case "PriorityAttribute":
                        {
                            EnvironmentConfig.TestSite.Log.Add(LogEntryKind.Comment, "Implementation priority of this scenario is: " + typeof(PriorityAttribute).GetProperty("Priority").GetValue(o, null).ToString());
                        }
                        break;
                    case "TestCategoryAttribute":
                        {
                            TestCategoryAttribute tca = o as TestCategoryAttribute;

                            foreach (string s in tca.TestCategories)
                            {
                                if (s.StartsWith("Win"))
                                {
                                    level = s;
                                    continue;
                                }
                                // Check if SDC, RODC exist when TestCategory contain SDC, RODC
                                if (s.Equals("SDC"))
                                {
                                    if (!EnvironmentConfig.MachineStore.ContainsKey(EnvironmentConfig.Machine.WritableDC2))
                                    {
                                        EnvironmentConfig.TestSite.Assume.Fail("The test requires a Secondary writable DC in the environment. Please set the corresponding field in PTF config, or make sure the machine can be connected.");
                                    }
                                    continue;
                                }
                                if (s.Equals("RODC"))
                                {
                                    if (!EnvironmentConfig.MachineStore.ContainsKey(EnvironmentConfig.Machine.RODC))
                                    {
                                        EnvironmentConfig.TestSite.Assume.Fail("The test requires a Read-Only DC in the environment. Please set the corresponding field in PTF config, or make sure the machine can be connected.");
                                    }
                                    continue;
                                }
                            }

                        }
                        break;
                    default:
                        if (thisType.BaseType == typeof(DrsrTestAttribute))
                        {
                            try
                            {
                                thisType.GetMethod("Do").Invoke(o, null);
                            }
                            catch (Exception e)
                            {
                                DrsrTestAttribute.Site.Assert.Fail(e.InnerException.Message);
                            }
                        }
                        break;
                }
            }
            if (level == null)
                throw new Exception("Test Case not set in any domain functional level category");
            FunctionLevelAttribute fl = null;
            switch (level)
            {
                case "Win2000":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2000);
                    break;
                case "Win2003":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003);
                    break;
                case "Win2008":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008);
                    break;
                case "Win2008R2":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2008R2);
                    break;
                case "Win2012":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2012);
                    break;
                case "Win2012R2":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2012R2);
                    break;
                case "WinThreshold":
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WINTHRESHOLD);
                    break;
                case "Winv1803":
                    ADCommonServerAdapter adapter = new ADCommonServerAdapter();
                    adapter.Initialize(DrsrTestAttribute.Site);
                    ServerVersion curVersion = adapter.PDCOSVersion;
                    if (curVersion < ServerVersion.Winv1803)
                    {
                        DrsrTestAttribute.Site.Assert.Inconclusive("Test case expects PDCOSVersion {0} should be Equal or Greater than Winv1803", curVersion);
                    }
                    fl = new FunctionLevelAttribute(DrsrDomainFunctionLevel.DS_BEHAVIOR_WINTHRESHOLD);
                    break;
                default:
                    throw new Exception("Unknown domain functional level category: " + level);
            }
            fl.Do();
        }
    }

}
