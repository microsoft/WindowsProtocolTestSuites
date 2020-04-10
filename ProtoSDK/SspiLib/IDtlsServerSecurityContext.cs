namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public interface IDtlsServerSecurityContext
    {
        bool HasMoreFragments { get; }

        SecurityPackageContextStreamSizes StreamSizes { get; }

        void Accept(byte[] clientToken);
    }
}
