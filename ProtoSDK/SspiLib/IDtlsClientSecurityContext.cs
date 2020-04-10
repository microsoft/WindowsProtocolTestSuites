namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib
{
    public interface IDtlsClientSecurityContext
    {
        bool HasMoreFragments { get; }

        SecurityPackageContextStreamSizes StreamSizes { get; }

        void Initialize(byte[] serverToken);
    }
}
