// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat
{
    using Microsoft.Modeling;
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// The protocol adapter interface defines all protocol methods.
    /// </summary>
    public interface ILsatAdapter : IAdapter
    {
        /// <summary>
        /// Method to do all initial settings and bind to the server.
        /// </summary>
        /// <param name="anonymousAccess">Specifies whether the requestor is anonymous or not.</param>
        /// <param name="numOfHandles">Specifies the maximum number of handles that can be opened by
        /// OpenPolicy and OpenPolcy2 methods at any instant of time.</param>
        /// <returns>Specifies the type of the server whether it is a DomainController 
        /// or a Non-DomainController.</returns>
        ProtocolServerConfig Initialize(
            AnonymousAccess anonymousAccess,
            uint numOfHandles);

        /// <summary>
        /// Method to create a policy handle with the given permissions.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null value.</param>
        /// <param name="accessMask">Contains the access to be given to the policyHandle.</param>
        /// <param name="openPolicyHandle">Output Parameter which contains Valid or Invalid or Null value.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns InvalidParameter if the parameters passed to the method are not valid.
        /// Returns AccessDenied if the caller is anonymous and the Server is a non-DomainController.</returns>
        ErrorStatus OpenPolicy(
            RootDirectory rootDirectory,
            uint accessMask,
            out Handle openPolicyHandle);

        /// <summary>
        /// Method to create a policy handle with the given permissions.
        /// </summary>
        /// <param name="rootDirectory">Contains Null value or Non-Null Value.</param>
        /// <param name="accessMask">Contains the access to be given to the policyHandle.</param>
        /// <param name="openPolicyHandle2">Output Parameter which contains Valid or Invalid or Null value.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns InvalidParameter if the parameters passed to the method are not valid.
        /// Returns AccessDenied if the caller is anonymous and the Server is a non-DomainController.</returns>
        ErrorStatus OpenPolicy2(
            RootDirectory rootDirectory,
            uint accessMask,
            out Handle openPolicyHandle2);

        /// <summary>
        /// Method to close the policy handle which is already opened by OpenPolicy or OpenPolicy2 method.
        /// </summary>
        /// <param name="handleToBeClosed">PolicyHandle which is already opened by LsarOpenPolicy or LsarOpenPolicy2 
        /// method.</param>
        /// <param name="handleAfterClose">OutPut Parameter contains Null if the method is successful 
        /// else Invalid.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns AccessDenied if the caller is not authenticated.</returns>
        ErrorStatus Close(
            int handleToBeClosed,
            out Handle handleAfterClose);

        /// <summary>
        /// Method to get the current user name and the domain name in which the user is in.
        /// </summary>
        /// <param name="authentication">Specifies whether the user is authenticated or not.</param>
        /// <param name="nameOfTheUser">It is an output parameter 
        /// where it specifies whether the userName is valid or not.</param>
        /// <param name="nameOfTheDomain">It is an output parameter 
        /// where it specifies whether the domainName is valid or not.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns AccessDenied if the caller is not authenticated.</returns>
        ErrorStatus GetUserName(
            User authentication,
            out Name nameOfTheUser,
            out Name nameOfTheDomain);

        /// <summary>
        /// Win32 Method which internally calls LsarLookupNames4 method.
        /// </summary>
        /// <param name="handle">Specifies whether the handle is valid or invalid </param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if the method is successful.
        /// Returns InvalidServerState if the server is a Non-DomainController.
        /// Returns SomeNotMapped if some of the given names are not translated into their SID form.
        /// Returns NoneMapped if none of the names are translated into their SID form.
        /// Returns AccessDenied if the handle is Invalid.</returns>
        ErrorStatus WinLookUpNames2(
            LSAHandle handle,
            Set<string> secPrincipalNames,
            out TranslatedSid translateSids,
            out MappedCount mapCount);

        /// <summary>
        /// Method to translate the given set of names into their SID form.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="optionOfLookup">Specifies the flag value whether the MSB is set or not.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalNames are translated into their SID form.
        /// Returns InvalidParameter if the method IsLookUpNameValid(secPrincipalNames) return false or lookUpLevel
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalNames into their 
        /// SID form.
        /// Returns SomeNotMapped if some of the given secPrincipalNames are not translated into their SID form.
        /// Returns NoneMapped if none of the secPrincipalNames are translated into their SID form.</returns>
        ErrorStatus LookUpNames3(
            int handle,
            Set<string> secPrincipalNames,
            LookUpOption optionOfLookup,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount);

        /// <summary>
        /// Method to translate the given set of names into their SID form.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalNames are translated into their SID form.
        /// Returns InvalidParameter if the method IsLookUpNameValid(secPrincipalNames) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalNames into their 
        /// SID form.
        /// Returns SomeNotMapped if some of the given secPrincipalNames are not translated into their SID form.
        /// Returns NoneMapped if none of the secPrincipalNames are translated into their SID form.</returns>
        ErrorStatus LookUpNames2(
            int handle,
            Set<string> secPrincipalNames,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount);

        /// <summary>
        /// Method to translate the given set of names into their SID form.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalNames">Contains the names to be translated into their SID form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateSids">An output parameter specifies whether translated sid 
        /// is Valid or Invalid.</param>
        /// <param name="mapCount">An output parameter specifies whether mapped count is Valid or Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalNames are translated into their SID form.
        /// Returns InvalidParameter if the method IsLookUpNameValid(secPrincipalNames) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalNames into their 
        /// SID form.
        /// Returns SomeNotMapped if some of the given secPrincipalNames are not translated into their SID form.
        /// Returns NoneMapped if none of the secPrincipalNames are translated into their SID form.</returns>
        ErrorStatus LookUpNames(
            int handle,
            Set<string> secPrincipalNames,
            LookUpLevel levelOfLookup,
            out TranslatedSid translateSids,
            out MappedCount mapCount);

        /// <summary>
        /// Method to translate the given set of SIDs into human readable names
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method</param>
        /// <param name="secPrincipalSids">Contains the SIDs to be translated into their Name form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateNames">An output parameter contains Valid if all the SIDs are translated
        /// into their Name form else Invalid.</param>
        /// <param name="mapCount">An output parameter contains Valid if all the SIDs are translated 
        /// into their Name form else Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalSids are translated into their Name form.
        /// Returns InvalidParameter if the method IsLookUpSidValid(secPrincipalSids) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalSids into their 
        /// Name form.
        /// Returns SomeNotMapped if some of the given secPrincipalSids are not translated into their Name form.
        /// Returns NoneMapped if none of the secPrincipalSids are translated into their Name form.</returns>
        ErrorStatus LookUpSids2(
            int handle,
            Set<string> secPrincipalSids,
            LookUpLevel levelOfLookup,
            out TranstlatedNames translateNames,
            out MappedCount mapCount);

        /// <summary>
        /// Method to translate the given set of SIDs into human readable names.
        /// </summary>
        /// <param name="handle">Policy Handle opened by LsarOpenPolicy or LsarOpenPolicy2 method.</param>
        /// <param name="secPrincipalSids">Contains the SIDs to be translated into their Name form.</param>
        /// <param name="levelOfLookup">Specifies the scope of the security principal to be searched.</param>
        /// <param name="translateNames">An output parameter contains Valid if all the SIDs are translated
        /// into their Name form else Invalid.</param>
        /// <param name="mapCount">An output parameter contains Valid if all the SIDs are translated 
        /// into their Name form else Invalid.</param>
        /// <returns>Returns Success if all of the given secPrincipalSids are translated into their Name form.
        /// Returns InvalidParameter if the method IsLookUpSidValid(secPrincipalSids) return false or lookUpLevel 
        /// is Invalid.
        /// Returns AccessDenied if the handle do not have desired access to translate secPrincipalSids into their 
        /// Name form.
        /// Returns SomeNotMapped if some of the given secPrincipalSids are not translated into their Name form.
        /// Returns NoneMapped if none of the secPrincipalSids are translated into their Name form.</returns>
        ErrorStatus LookUpSids(
            int handle,
            Set<string> secPrincipalSids,
            LookUpLevel levelOfLookup,
            out TranstlatedNames translateNames,
            out MappedCount mapCount);
    }
}
