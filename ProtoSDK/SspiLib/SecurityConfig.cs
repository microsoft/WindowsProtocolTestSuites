namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    /// <summary>
    /// Base class of all kinds of security configuration.
    /// </summary>
    public class SecurityConfig
    {
        /// <summary>
        /// Security package type.
        /// </summary>
        private SecurityPackageType securityType;

        /// <summary>
        /// Security package type.
        /// </summary>
        public SecurityPackageType SecurityType
        {
            get
            {
                return this.securityType;
            }
            set
            {
                this.securityType = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="securityPackageType">Security package type.</param>
        protected SecurityConfig(SecurityPackageType securityPackageType)
        {
            this.securityType = securityPackageType;
        }
    }
}
