// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// The Server Abstract Data Model. Corresponding to MS-ADFSPIP 
    /// 3.1.1.1 Server State.
    /// </summary>
    /// <remarks>
    /// The server data model can be serialized to an XML file on disk.
    /// But at present, it does NOT get synced; because test cases are
    /// not run in order, syncing the data model to file and keeping it
    /// consistent through out the test is not a good idea. 
    /// The proxy side does not check data store version or application 
    /// ID, so version draw-back won't cause any problem. 
    /// </remarks>
    [DataContract]
    public sealed partial class ServerDataModel : INotifyPropertyChanged
    {
        #region Fields
        private static ServerDataModel               _model;
        private static string                        _storeDataFile;
        [DataMember] private XDocument               _metadata;
        [DataMember] private STSConfiguration        _configuration;
        [DataMember] private List<RelyingPartyTrust> _relyingPartyTrust;
        [DataMember] private List<StoreEntry>        _proxyStore;       
        [DataMember] private List<ProxyTrust>        _proxyTrustedCertificate;
        [DataMember] private ProxyRelyingPartyTrust  _proxyRelyingPartyTrust;
        #endregion

        #region Properties
        public ProxyTrust[] ProxyTrustedCertificate
        {
            get { return _proxyTrustedCertificate.ToArray(); }
        }

        public ProxyRelyingPartyTrust ProxyRelyingPartyTrust
        {
            get { return _proxyRelyingPartyTrust; }
        }

        public STSConfiguration Configuration
        {
            get { return _configuration; }
        }

        public RelyingPartyTrust[] RelyingPartyTrust
        {
            get { return _relyingPartyTrust.ToArray(); }
        }

        public StoreEntry[] ProxyStore
        {
            get { return _proxyStore.ToArray(); }
        }

        public XDocument Metadata
        {
            get { return _metadata; }
        }
        #endregion

        #region Private Methods
        private ServerDataModel()
        {
            InitializeProxyTrust();
            InitialServerCertificates();
            InitializeConfiguration();
            InitializeRelyingParties();
            InitializeDataStore();             
            InitializeFederationMetadata();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get an instance of ServerDataModel.
        /// </summary>
        /// <returns>
        /// An instance of ServerDataModel
        /// </returns>
        public static ServerDataModel InitiateServerDataModel()
        {
            // if the instance already exists
            if (_model != null) return _model;

            // or create a new instance with default settings
            _model = new ServerDataModel();

            _storeDataFile = EnvironmentConfig.DataStorePath;
            // XML-syncing is disabled at present
            // the ServerDataModel instance gets instantiated first,
            // if the value stored in the XML file is not the same
            // as the current value, it overrides the current value.

            return _model;
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Serialize the current instance to the XML file specified
        /// by storeDataFile.
        /// </summary>
        private void Save()
        {
            var serializer = new NetDataContractSerializer();
            using (var stream = File.Create(_storeDataFile)) {
                serializer.Serialize(stream, this);
            }
        }

        /// <summary>
        /// Deserialize the XML file and recover the data model instance.  
        /// </summary>
        private void Load()
        {
            ServerDataModel model;
            var serializer = new NetDataContractSerializer();
            using (var stream = File.OpenRead(_storeDataFile)) {
                model = (ServerDataModel) serializer.Deserialize(stream);
            }
            // get all fields in the data model
            var fields = GetType().GetFields(BindingFlags.Public    |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance);
            foreach (var field in fields) {
                var storedValue = field.GetValue(model);
                // if the field is stored in the XML file, load it and 
                // override the value in the current instance.
                if (storedValue != null) {
                    field.SetValue(this, storedValue);
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
      
        /// <summary>
        /// Call this method to notify a property has changes.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property.
        /// </param>
        private void NotifyPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SyncOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Save();          
        }
        #endregion
    }

}
