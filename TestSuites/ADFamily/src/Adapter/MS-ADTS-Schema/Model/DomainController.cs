// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// A class representing a domain controller.
    /// </summary>
    public partial class ModelDomainController
    {

        #region State

        /// <summary>
        /// The GUID of the server.
        /// </summary>
        public Guid serverGuid;

        //public PrefixTable

        /// <summary>
        /// The domain naming context replica.
        /// </summary>
        public Replica domainReplica;

        /// <summary>
        /// The application naming context replica.
        /// </summary>
        public Replica applicationReplica;

        /// <summary>
        /// The configuration naming context replica.
        /// </summary>
        public Replica configReplica;

        /// <summary>
        /// The schema naming context replica.
        /// </summary>
        public Replica schemaReplica;

        /// <summary>
        /// A map of attribute display names to objects
        /// </summary>
        public Dictionary<string, ModelObject> attributeMap = new Dictionary<string, ModelObject>();

        /// <summary>
        /// A map from attribute ids to their according display name. Used to resolve DNs which
        /// use ID notation '1.2.4=value' instead of name notation.
        /// </summary>
        public Dictionary<string, string> attributeIdToDisplayNameMap = new Dictionary<string, string>();

        /// <summary>
        /// A map of class display names to class objects 
        /// </summary>
        public Dictionary<string, ModelObject> classMap = new Dictionary<string, ModelObject>();

        /// <summary>
        /// A map of class governs ids to display names of classes.
        /// </summary>
        public Dictionary<string, string> classGovernsIdToDisplayNameMap = new Dictionary<string, string>();

        //partialDomainNCs, AppNCs, PdcChangeLog, NT4ReplicationState, LDAPConnections, ReplicationQueue,
        //KCCFailedConnections, KCCFailedLinks, RPCClientContexts, RPCOutgoingContexts

        /// <summary>
        /// A partial domain NC replica for each domain NC in the forest
        /// </summary>
        public Replica partialDomainNCs;
        /// <summary>
        /// Replicas of any subset of the application NCs in the forest.
        /// </summary>
        public Replica AppNCs;
        /// <summary>
        /// Replication state for NT4
        /// </summary>
        public NT4ReplicationState nt4ReplicationState;
        /// <summary>
        /// Ldap connections for the DC
        /// </summary>
        public LDAPConnections ldapConnections;
        /// <summary>
        /// Queue for storing the pending replication operations.
        /// </summary>
        public ReplicationQueue replicationQueue;
        /// <summary>
        /// Entry for failed connections on DC 
        /// </summary>
        public KCCFailedConnections kccFailedConnections;
        /// <summary>
        /// Entry for for failed links.
        /// </summary>
        public KCCFailedLinks kccFailedLinks;
        /// <summary>
        /// RPC context for an incoming RPC session to the DC
        /// </summary>
        public RPCClientContexts rpcClientContexts;
        /// <summary>
        /// RPC context for an outgoing RPC session from the DC.
        /// </summary>
        public RPCOutgoingContexts rpcOutgoingContexts;
        #endregion

        #region Construction

        /// <summary>
        /// Constructs an empty AD/DS DC node with replicas initialized with core definitions.
        /// </summary>
        /// <param name="domainDN">The domain DN.</param>
        /// <param name="applicationDN">The application DN. If null, no application replica will be created.</param>
        public ModelDomainController(string domainDN, string applicationDN)
        {
            domainReplica = new Replica(this, ReplicaKind.Domain, domainDN, true);
            //CHECKME: is that always complete replica?
            configReplica = new Replica(
                this, 
                ReplicaKind.Configuration, 
                NameHelper.MakeDN("cn=Configuration", domainDN), 
                false);
            //CHECKME: is that always complete replica?                        
            schemaReplica = new Replica(
                this, 
                ReplicaKind.Schema, 
                NameHelper.MakeDN("cn=Schema", configReplica.dn), 
                false);

            if (applicationDN != null)
            {
                applicationReplica = new Replica(this, ReplicaKind.Application, applicationDN, true);
            }
        }
        #endregion


        #region Printing

        /// <summary>
        /// Print the indented text writer.
        /// </summary>
        /// <param name="writer">The writer of indented text.</param>
        public void Print(IndentedTextWriter writer)
        {
            if (domainReplica != null)
            {
                domainReplica.Print(writer);
            }

            if (applicationReplica != null)
            {
                applicationReplica.Print(writer);
            }

            if (configReplica != null)
            {
                configReplica.Print(writer);
            }

            if (schemaReplica != null)
            {
                schemaReplica.Print(writer);
            }
        }

        #endregion

        #region Basic Methods

        /// <summary>
        /// Try get an attribute object by its display name or by its object id.
        /// </summary>
        /// <param name="name">The name of attribute.</param>
        /// <param name="obj">The object of model.</param>
        /// <returns>Returns true if get attribute.</returns>
        public bool TryGetAttribute(string name, out ModelObject obj)
        {
            string temp;
            name = name.ToLower();

            if (name.Length > 0 && Char.IsDigit(name[0]) && attributeIdToDisplayNameMap.TryGetValue(name, out temp))
            {
                name = temp;
            }
            if (attributeMap.TryGetValue(name, out obj))
            {
                return true;
            }
            else
            {
                obj = null;
                return false;
            }
        }

        /// <summary>
        /// Get an attribute object by its display name or by its object id.
        /// </summary>
        /// <param name="name">The name of attribute.</param>
        /// <returns>Returns attribute.</returns>
        public ModelObject GetAttribute(string name)
        {
            ModelObject obj;
            name = name.ToLower();
            Assert.IsTrue(TryGetAttribute(name, out obj), "attribute '{0}' not defined in schema", name);

            return obj;
        }

        /// <summary>
        /// Get a class object by its display name or by its governs-id 
        /// </summary>
        /// <param name="name">The name of attribute.</param>
        /// <param name="obj">The object of model.</param>
        /// <returns>Returns true if get class.</returns>
        public bool TryGetClass(string name, out ModelObject obj)
        {
            string temp;
            if (name.Length > 0 && Char.IsDigit(name[0]) && classGovernsIdToDisplayNameMap.TryGetValue(name, out temp))
            {
                name = temp;
            }
            if (classMap.TryGetValue(name.ToLower(), out obj))
            {
                return true;
            }
            else
            {
                obj = null;

                return false;
            }
        }

        /// <summary>
        /// Get a class object by its display name or by its governs-id 
        /// </summary>
        /// <param name="name">The name of class.</param>
        /// <returns>Returns true if get class.</returns>
        public ModelObject GetClass(string name)
        {
            ModelObject obj ;
            Assert.IsTrue(TryGetClass(name, out obj), "class '{0}' not defined in schema", name);

            return obj;
        }



        /// <summary>
        /// Get the syntax of an attribute by its display name or id.
        /// </summary>
        /// <param name="name">The name of attribute.</param>
        /// <returns>Returns attributeContext.</returns>
        public AttributeContext GetAttributeContext(string name)
        {
            AttributeContext context;
            Assert.IsTrue(TryGetAttributeContext(name, out context), "attribute context not available: " + name);

            return context;
        }


        /// <summary>
        /// Try get the syntax of an attribute by its display name or id.
        /// </summary>
        /// <param name="name">The name of attribute display.</param>
        /// <param name="context">The context of attribute.</param>
        /// <returns>Returns true if get attribute context.</returns>
        public bool TryGetAttributeContext(string name, out AttributeContext context)
        {
            context = new AttributeContext();
            ModelObject obj;
            if (!TryGetAttribute(name, out obj))
            {
                return false;
            }
            string attributeSyntax = (string)obj[StandardNames.attributeSyntax];
            int oMSyntax = (int)obj[StandardNames.oMSyntax];
            string oMObjectClass = (string)obj[StandardNames.oMObjectClass];
            Value isSingleValue = obj[StandardNames.isSingleValued];
            Type symbolEnumType = IntegerSymbols.GetSymbolEnumType(name);

            context = new AttributeContext(
                this, 
                name, 
                Syntax.Lookup(attributeSyntax, oMSyntax, oMObjectClass),
                isSingleValue == null || (bool)isSingleValue.UnderlyingValues[0], 
                symbolEnumType);
            return true;
        }
        
        /// <summary>
        /// Get the display name of an attribute by its attribute Id.
        /// </summary>
        /// <param name="attrId">The id of attribute.</param>
        /// <returns>Returns attribute display name.</returns>
        public string GetAttributeDisplayName(string attrId)
        {
            string result;

            if (attrId.Length > 0 && Char.IsDigit(attrId[0]))
            {
                Assert.IsTrue(
                    attributeIdToDisplayNameMap.TryGetValue(attrId, out result),
                    "attribute id '{0}' must map to attribute display name", 
                    attrId);
            }
            else
            {
                result = attrId;
            }
            return result;
        }

        /// <summary>
        /// Get the RDN attribute display name of an object. 
        /// </summary>
        /// <param name="obj">Model object.</param>
        /// <returns>Returns RDN attribute name.</returns>
        public string GetRDNAttributeName(ModelObject obj)
        {
            ModelObject classStruct = GetClass(obj.GetStructuralClassId());
            if (classStruct == null)
            {
                return null;
            }
                Value rdn = classStruct[StandardNames.rDNAttID];
            
            while (rdn == null && (string)classStruct[StandardNames.governsId] != StandardNames.topGovernsId)
            {
                classStruct = GetClass((string)classStruct[StandardNames.subClassOf]);
                rdn = classStruct[StandardNames.rDNAttID];
            }
            Assert.IsTrue(rdn != null, "rDNAttId not defined in class or its superclasses");
            if (rdn != null)
            {
                return GetAttributeDisplayName((string)rdn);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Add a child to its parent.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="child">Child object.</param>
        public void AddChild(ModelObject parent, ModelObject child)
        {
            string rdnName = GetRDNAttributeName(child);
            if (rdnName == null)
            {
                return;
            }
                string rdn = (string)child[rdnName];
            if (rdn != null)
            {
                child.parent = parent;
                parent.childs[rdn] = child;
            }
        }

        /// <summary>
        /// Finds the object with given DN, if present.
        /// </summary>
        /// <param name="distinguishedName">The name of distringuised.</param>
        /// <returns>Returns model object.</returns>
        public ModelObject TryFindObject(string distinguishedName)
        {
            if (schemaReplica != null && distinguishedName == schemaReplica.dn)
            {
                return schemaReplica.root;
            }
            else if (configReplica != null && distinguishedName == configReplica.dn)
            {
                return configReplica.root;
            }
            else if (applicationReplica != null && distinguishedName == applicationReplica.dn)
            {
                return applicationReplica.root;
            }
            else if (domainReplica != null && distinguishedName == domainReplica.dn)
            {
                return domainReplica.root;
            }
            else
            {
                string parentName = NameHelper.GetDNParent(distinguishedName);
                if (parentName == null)
                {
                    return null;
                }
                ModelObject parentObj = TryFindObject(parentName);
                if (parentObj != null)
                {
                    string rdn = NameHelper.GetDNRDN(distinguishedName);
                    ModelObject result;
                    parentObj.childs.TryGetValue(rdn, out result);

                    return result;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Enumerates the objects in the trees of all replicas in the DC.
        /// </summary>
        /// <returns>Returns model object.</returns>
        public IEnumerable<ModelObject> GetAllObjects()
        {
            if (domainReplica != null)
            {
                foreach (ModelObject obj in GetChildsAndObject(domainReplica.root))
                {
                    yield return obj;
                }
            }
            if (applicationReplica != null)
            {
                foreach (ModelObject obj in GetChildsAndObject(applicationReplica.root))
                {
                    yield return obj;
                }
            }
            if (configReplica != null)
            {
                foreach (ModelObject obj in GetChildsAndObject(configReplica.root))
                {
                    yield return obj;
                }
            }
            if (schemaReplica != null)
                foreach (ModelObject obj in GetChildsAndObject(schemaReplica.root))
                {
                    yield return obj;
                }
        }

        /// <summary>
        /// Enumerates the given object and all its childs. The childs will come first.
        /// </summary>
        /// <param name="obj">The object of model.</param>
        /// <returns>Returns childs object.</returns>
        public IEnumerable<ModelObject> GetChildsAndObject(ModelObject obj)
        {
            foreach (ModelObject obj1 in obj.childs.Values)
            {
                foreach (ModelObject obj2 in GetChildsAndObject(obj1))
                {
                    yield return obj2;
                }
            }
            yield return obj;
        }
                
        #endregion
              
    }
    /// <summary>
    /// Nt4ReplicationState is an abstract type for the replication state for Windows NT 4.0 BDCs.
    /// </summary>
    public struct NT4ReplicationState
    {
        /// <summary>
        /// An update sequence number for updates that occur in AD/DS that are relevant to the NT4 replication protocol.
        /// </summary>
        public int SamNT4ReplicationUSN;

        /// <summary>
        /// An update sequence number for updates that occur in AD/DS that are relevant to the NT4 replication protocol.
        /// </summary>
        public int BuiltinNT4ReplicationUSN;
    }

    /// <summary>
    /// LDAPConnections is an abstract type for the LDAP connections associated with a DC.
    /// </summary>
    public struct LDAPConnections
    {
        /// <summary>
        /// The IPv4 address of the client machine that established the connection.
        /// </summary>
        public uint ipAddress;
        /// <summary>
        /// The number of LDAP notifications enabled on the connection.
        /// </summary>
        public int notificationCount;
        /// <summary>
        /// The time, in seconds, that the connection has been open.
        /// </summary>
        public int secTimeConnected;
        /// <summary>
        /// The LDAP_CONN_PROPERTIES bit flags that identify properties of the connection.
        /// </summary>
        public uint flags;
        /// <summary>
        /// The total number of LDAP requests processed on the connection.
        /// </summary>
        public int totalRequests;
        /// <summary>
        /// The name of the security principal that opened the connection.
        /// </summary>
        /// Unicode string.
        public string userName;   

    }

    /// <summary>
    /// ReplicationQueue is an abstract type for queued pending replication operations.
    /// </summary>
    public struct ReplicationQueue
    {
        /// <summary>
        /// A unique identifier associated with the operation.
        /// </summary>
        public ulong SerialNumber;
        /// <summary>
        /// The priority of the operation.
        /// </summary>
        public ulong Priorty;
        /// <summary>
        /// The type of operation
        /// </summary>
        public DsReplOpType OperationType;
        /// <summary>
        /// The NC root of the NC replica associated with the operation.
        /// </summary>
        /// Unicode string.
        public string NameingContext; 
        /// <summary>
        /// The DN of the nTDSDSA object of the DC associated with the operation.
        /// </summary>
        /// Unicode string.
        public string DsaDN;         
        /// <summary>
        /// The network address of the DC associated with the operation.
        /// </summary>
        /// Unicode string.
        public string DsaAddress;     
        /// <summary>
        /// The objectGUID of the NC root of the NC replica associated with the operation.
        /// </summary>
        public Guid UUIDNC;
        /// <summary>
        /// The DSA GUID of the DC associated with the operation.
        /// </summary>
        public Guid UUIDDsa;

    }

    /// <summary>
    /// KCCFailedConnections is an abstract type consisting of a sequence of tuples, one tuple for each DC for which
    /// the connection attempt failed.
    /// </summary>
    public struct KCCFailedConnections
    {
        /// <summary>
        /// The DN of the nTDSDSA object that corresponds to the DC.
        /// </summary>
        /// Unicode string.
        public string DsaDN;  
        /// <summary>
        /// The DSA GUID of the DC.
        /// </summary>
        public Guid UUIDDsa;
        /// <summary>
        /// The total number of failures the KCC encountered while contacting the DC.
        /// </summary>
        public int FailureCount;
        /// <summary>
        /// Windows error code that indicates the reason for the last failure.
        /// </summary>
        public uint LastResult;

    }

    /// <summary>
    /// KCCFailedLinks is an abstract type that consists of a sequence of tuples, one tuple for each neighboring DC for 
    /// which a connection attempt failed.
    /// </summary>
    public struct KCCFailedLinks
    {
        /// <summary>
        /// The DN of the nTDSDSA object that corresponds to the DC.
        /// </summary>
        public string DsaDN;   
        /// <summary>
        /// The DSA GUID of the DC.
        /// </summary>
        public Guid UUIDDsa;
        /// <summary>
        /// The total number of failures the KCC encountered while contacting the DC.
        /// </summary>
        public int FailureCount;
        /// <summary>
        /// Windows error code that indicates the reason for the last failure.
        /// </summary>
        public uint LastResult;

    }

    /// <summary>
    /// RPCClientContexts is an abstract type that is a sequence of tuples, one tuple per RPC context for an incoming 
    /// RPC session to the DC.
    /// </summary>
    public struct RPCClientContexts
    {
        /// <summary>
        /// A unique identifier for the context.
        /// </summary>
        /// ULONGLONG.
        public ulong BindingContext;   
        /// <summary>
        /// The number of references to the context.
        /// </summary>
        public int RefCount;
        /// <summary>
        /// A Boolean value that is true if IDL_DRSUnbind has not yet been called on the RPC context represented by 
        /// this tuple, and false otherwise.
        /// </summary>
        public Boolean IsBound;
        /// <summary>
        /// The value that was passed in as the puuidClientDsa argument of IDL_DRSBind while establishing the context.
        /// </summary>
        public Guid UUIDClient;
        /// <summary>
        /// The IPv4 address of the client associated with the context.
        /// </summary>
        public uint IPAddress;
        /// <summary>
        /// The process ID passed in by the client as the pextClient argument of IDL_DRSBind while establishing the
        /// context.
        /// </summary>
        public int PID;
    }

    /// <summary>
    /// RPCOutgoingContexts is an abstract type that is a sequence of tuples, one tuple per RPC context for an outgoing 
    /// RPC session from the DC.
    /// </summary>
    public struct RPCOutgoingContexts
    {
        /// <summary>
        /// The host name of the server.
        /// </summary>
        /// Unicode string.
        public string ServerName;  
        /// <summary>
        /// A Boolean value that is true if IDL_DRSUnbind has not yet been called on the RPC context represented by this 
        /// tuple, and false otherwise.
        /// </summary>
        public Boolean IsBound;
        /// <summary>
        /// A Boolean value that is true if the context handle was retrieved from the cache, and false otherwise.
        /// </summary>
        public Boolean HandleFromCache;
        /// <summary>
        /// A Boolean value that is true if the context handle is still in the cache, and false otherwise.
        /// </summary>
        public Boolean HandleInCache;
        /// <summary>
        /// The thread ID of the thread that is using the context.
        /// </summary>
        public int ThreadId;
        /// <summary>
        /// If the context is set to be canceled, then this field contains the time-out, in minutes.
        /// </summary>
        public int BindingTimeOut;
        /// <summary>
        /// The time when the context was created.
        /// </summary>
        /// DSTIME.
        public ulong CreateTime; 
        /// <summary>
        /// The type of RPC call that the DC is waiting on.
        /// </summary>
        public int CallType;
    }

    /// <summary>
    /// enumaration to specify the type of replication operation 
    /// </summary>
    public enum DsReplOpType
    {
        /// <summary>
        /// Sync
        /// </summary>
        Sync,

        /// <summary>
        /// Add
        /// </summary>
        Add,

        /// <summary>
        /// Delete
        /// </summary>
        Delete,

        /// <summary>
        /// Modify
        /// </summary>
        Modify,

        /// <summary>
        /// UpdateReps
        /// </summary>
        UpdateReps
    }
}