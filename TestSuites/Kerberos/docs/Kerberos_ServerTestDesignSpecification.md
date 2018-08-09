
# Kerberos Test Suite Design Specification

## Table of Contents
* [Test Environment](#_Toc430273907)
    * [KILE PAC Environment](#_Toc430273908)
    * [KKDCP Environment](#_Toc430273909)
    * [Security Context Parameters](#_Toc430273910)
		* [Client – [MS-KILE] section 3.2.1](#_Toc430273911)
		* [KDC – [MS-KILE] section 3.3.1](#_Toc430273912)
		* [Application Server – [MS-KILE] section 3.4.1](#_Toc430273913)
		* [KKDCP – [MS-KKDCP] section 3.1.1](#_Toc430273914)
    * [Assumptions](#_Toc430273915)
* [Test Scenarios Design](#_Toc430273916)
    * [Single Domain: Interactive Logon Test](#_Toc430273917)
		* [Basic](#_Toc430273918)
		* [FAST](#_Toc430273919)
		* [Protected Users](#_Toc430273920)
    * [Single Domain: Network Logon Test](#_Toc430273921)
		* [Basic](#_Toc430273922)
		* [Claims](#_Toc430273923)
		* [Compound Identity](#_Toc430273924)
		* [Network Logon with Authentication Policy Silos](#_Toc430273925)
    * [Multiple Domains: Network Logon Test](#_Toc430273926)
		* [Basic](#_Toc430273927)
		* [FAST](#_Toc430273928)
		* [Compound Identity](#_Toc430273929)
    * [KRB_ERROR Test](#_Toc430273930)
		* [KRB_ERROR for AS Request Test](#_Toc430273931)
		* [KRB_ERROR for TGS Request Test](#_Toc430273932)
		* [KRB_ERROR for AP Request Test](#_Toc430273933)
    * [KKDCP Test](#_Toc430273934)
		* [Basic](#_Toc430273935)
		* [Negative](#_Toc430273936)
		* [Kpassword](#_Toc430273937)
    * [RC4 Test](#_Toc430273938)
		* [Single Domain: Interactive Logon Test with FAST](#_Toc430273939)
		* [Single Domain: Network Logon Test with Compound Identity](#_Toc430273940)
		* [Multiple Domains: Network Logon Test with Compound Identity](#_Toc430273941)
* [Test Cases Design](#_Toc430273942)
    * [Single Domain: Interactive Logon Test](#_Toc430273943)
		* [Basic](#_Toc430273944)
    * [Section 2.1   Transport](#_Toc430273945)
		* [FAST](#_Toc430273946)
		* [Protected Users](#_Toc430273947)
    * [Single Domain: Network Logon Test](#_Toc430273948)
		* [Basic](#_Toc430273949)
		* [Claims](#_Toc430273950)
		* [Compound Identity](#_Toc430273951)
		* [Authentication Silo](#_Toc430273952)
    * [Multiple Domains: Network Logon Test](#_Toc430273953)
		* [Basic](#_Toc430273954)
		* [FAST](#_Toc430273955)
		* [Compound Identity](#_Toc430273956)
    * [KRB_ERROR Test](#_Toc430273957)
		* [KRB_ERROR for AS_REQ Test](#_Toc430273958)
		* [KRB_ERROR for TGS_REQ Test](#_Toc430273959)
		* [KRB_ERROR for AP_REQ Test](#_Toc430273960)
    * [KKDCP Test](#_Toc430274036)
		* [Negative](#_Toc430274037)
		* [Kpassword](#_Toc430274038)
    * [Change Network Topology](#_Toc430274039)
		* [RODC](#_Toc430274040)
		* [Proxy](#_Toc430274041)
		* [Pre Win8 KDC](#_Toc430274042)
		* [AZOD_Synthetic_Test](#_Toc430274043)

## <a name="_Toc430273907"/>Test Environment

### <a name="_Toc430273908"/>KILE PAC Environment
1. DNS Server
2. Active Directory
3. Use IPv4 by default
4. All full DC, need RODC for P2 cases

### <a name="_Toc430273909"/>KKDCP Environment
1. Web Server (IIS)
2. KDC Proxy Server service (KPSSVC) 

### <a name="_Toc430273910"/>Security Context Parameters

#### <a name="_Toc430273911"/>Client – [MS-KILE] section 3.2.1

#####GSS API Parameters
ChannelBinding
Confidentiality
DatagramStyle
DCE Style
Delegate
ExtendedError 
Identify
Integrity 
MessageBlockSize
MutualAuthentication
ReplayDetect 
SequenceDetect 
UseSessionKey 

#### <a name="_Toc430273912"/>KDC – [MS-KILE] section 3.3.1

#####Group Policy
Minimum lifetime
MaxClockSkew
MaxServiceTicketAge
MaxTicketAge
AuthenticationOptions

#####Registry Key
ClaimsCompIdFASTSupport

#####AD context
NetbiosServerName
NetbiosDomainName
DomainSid
Secret keys
KerbSupportedEncryptionTypes
AuthorizationDataNotRequired
AssignedPolicy
AssignedSilo
DelegationNotAllowed
Disabled
Expired
GroupMembership
Locked
LogonHours
PasswordMustChange
Pre-AuthenticationNotRequired
TrustedForDelegation
UseDESOnly

#### <a name="_Toc430273913"/>Application Server – [MS-KILE] section 3.4.1

#####GSS API Parameters
ChannelBinding
Confidentiality
DatagramStyle
DCE Style
Delegate
ExtendedError 
Identify
Integrity 
MessageBlockSize
MutualAuthentication
ReplayDetect 
SequenceDetect 
UseSessionKey
ApplicationRequiresCBT
ImpersonationAccessToken (public)

#####AD context
msDS-SupportedEncryptionTypes

#### <a name="_Toc430273914"/>KKDCP – [MS-KKDCP] section 3.1.1

#####GSS API Parameters
KKDCPServerURL
KerberosMessage
Error
TargetDomain

### <a name="_Toc430273915"/>Assumptions
One machine has only one KDC installed. (RFC4120, port 88)
Set all krbtgt account with the same password in the same domain. ([MS-KILE] section 3.3.3) + ([RFC4120] section 6.2)
Use timeout mechanism to fail AS_REQ and TGS_REQ. ([MS-KILE] section 3.2.6)

## <a name="_Toc430273916"/>Test Scenarios Design

| &#32;| &#32;| &#32;| &#32; |
| -------------| -------------| -------------| ------------- |
| Scenarios| Protocol| Test Cases| Description| 
| Interactive Logon Test – Basic| RFC4120\MS-KILE|  | A user log on to a client| 
| Interactive Logon Test – FAST| RFC4120\RFC6113|  | A user log on to a client using FAST authentication| 
| Network Logon Test – File Server Basic| RFC4120\MS-KILE\MS-PAC|  | A user access a file server and invoke Kerberos-based authentication| 
| Network Logon Test – File Server Compound Identity| RFC4120\RFC6113\MS-KILE\MS-PAC|  | A user access a file server and invoke Compound Identity authentication| 
| Network Logon Test – File Server Claims| RFC4120\RFC6113\MS-KILE\MS-PAC|  | A user access a file server and requires claims authorization data| 
| Network Logon Test – Web Server Basic| RFC4120\MS-KILE\MS-PAC|  | A user access a web server and invoke Kerberos-based authentication| 
| Cross Forest Network Logon Test – File Server Basic| RFC4120\MS-KILE\MS-PAC|  | A user access a file server registered in another domain and invoke Kerberos-based authentication| 
| Cross Forest Network Logon Test – File Server Compound Identity with claims| RFC4120\RFC6113\MS-KILE\MS-PAC|  | A user access a file server registered in another domain and invoke compound identity authentication and requires claims authorization data| 
| MIT KDC interests – Cross Forest Network Logon Test – File Server Compound Identity with claims| RFC4120\RFC6113\MS-KILE\MS-PAC|  | A user access a cross realm file server and invoke Kerberos-based authentication, SPN translation, authorization data understanding| 
| KRB_ERROR Test| RFC4120\RFC6113|  | Test different types of KRB_ERROR| 
| KKDCP Test| RFC4120\MS-KKDCP\RFC3244|  | A user uses KKDCP server to relay the Kerberos V5 and Kerberos change password messages between client and KDC| 
| RC4 Encryption Test| RFC4120\RFC4757\RFC6113\MS-KILE\MS-PAC|  | Test FAST and Compound Identity cases when using RC4-HMAC encryption| 

### <a name="_Toc430273917"/>Single Domain: Interactive Logon Test

#### <a name="_Toc430273918"/>Basic

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined local host using a basic KDC.| 
|  **Call Sequence**| AS Request for user (no pre-authentication)| 
| | KRB Error (PA-ENC-TIMESTAMP)| 
| | AS Request for user| 
| | AS Response| 
| | TGS Request| 
| | TGS Response| 

#### <a name="_Toc430273919"/>FAST

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on the domain-joined local host using a FAST supported KDC| 
|  **Call Sequence**| AS Request for device (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request for device| 
| | AS Response| 
| | FAST AS Request for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request for user | 
| | FAST AS Response| 
| | FAST TGS Request| 
| | FAST TGS Response| 

#### <a name="_Toc430273920"/>Protected Users

#####Interactive Logon

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test protected users interactive log on| 
|  **Call Sequence**| AS Request for user (no pre-authentication)| 
| | KRB Error (PA-ENC-TIMESTAMP)| 
| | AS Request for user| 
| | AS Response| 
| | TGS Request| 
| | TGS Response| 

#####Down-level Protection

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test Client and KDC behavior with down-level version of Windows.| 
|  **Call Sequence**| AS Request for user (no pre-authentication)| 
| | KRB Error (PA-ENC-TIMESTAMP)| 
| | AS Request for user| 
| | AS Response| 
| | TGS Request| 
| | TGS Response| 

### <a name="_Toc430273921"/>Single Domain: Network Logon Test

#### <a name="_Toc430273922"/>Basic

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined application server using a basic KDC| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request for user (no pre-authentication)| 
| | KRB Error (PA-ENC-TIMESTAMP)| 
| | AS Request for user| 
| | AS Response| 
| | TGS Request| 
| | TGS Response| 
| | AP Session setup request| 
| | AP Session setup response| 

#### <a name="_Toc430273923"/>Claims

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined CBAC-aware application server using a basic KDC| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request for device (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request for device| 
| | AS Response (device claims, PA-SUPPORTED-ENCTYPES)| 
| | FAST AS Request for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request for user| 
| | FAST AS Response (user claims, PA-SUPPORTED-ENCTYPES)| 
| | Compound Identity TGS Request| 
| | Compound Identity TGS Response (user and device claims)| 
| | AP Session setup request| 
| | AP Session setup response| 

#### <a name="_Toc430273924"/>Compound Identity

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined CBAC-aware application server using a Compound Identity supported KDC| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request for device (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request for device| 
| | AS Response (device claims, PA-SUPPORTED-ENCTYPES)| 
| | FAST AS Request for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request for user| 
| | FAST AS Response (user claims, PA-SUPPORTED-ENCTYPES)| 
| | Compound Identity TGS Request| 
| | Compound Identity TGS Response (user and device claims)| 
| | AP Session setup request| 
| | AP Session setup response| 

#### <a name="_Toc430273925"/>Network Logon with Authentication Policy Silos

#####AllowedToAuthenticatedTo

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Domain administrators will be able to restrict which devices a user can use their passwords and smartcards to successfully authenticate by their Authentication Policy Silo.| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request for user (no pre-authentication)| 
| | KRB Error (PA-ENC-TIMESTAMP)| 
| | AS Request for user| 
| | AS Response| 
| | TGS Request| 
| | TGS Response| 
| | AP Session setup request| 
| | AP Session setup response| 

#####AllowedToAuthenticatedFrom

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Service administrators will be able to restrict user authentication using compound authentication to their service by configuring the Authentication Policy on the domain controller| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request for device (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request for device| 
| | AS Response (device claims, PA-SUPPORTED-ENCTYPES)| 
| | FAST AS Request for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request for user| 
| | FAST AS Response (user claims, PA-SUPPORTED-ENCTYPES)| 
| | Compound Identity TGS Request| 
| | Compound Identity TGS Response (user and device claims)| 
| | AP Session setup request| 
| | AP Session setup response| 

### <a name="_Toc430273926"/>Multiple Domains: Network Logon Test

#### <a name="_Toc430273927"/>Basic

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined application server using basic KDCs in all domains (local host in domain A, user principal in domain B and application server in domain C)| 
| | Pre-requisite: | 
| | local host must join domain A| 
| | must use email type user principal name| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response| 
| | AS Request to domain A for user (no pre-authentication)| 
| | KRB Error (wrong realm)| 
| | AS Request to domain B for user (no pre-authentication)| 
| | KRB Error (PA-ENC-TIMESTAMP)| 
| | AS Request do domain B for user| 
| | AS Response| 
| | TGS Request to domain B for service| 
| | TGS Response (referral TGT)| 
| | Referral TGS Request to domain C for service| 
| | Referral TGS Response| 
| | AP Session setup request| 
| | AP Session setup response| 

#### <a name="_Toc430273928"/>FAST

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined application server using FAST supported KDCs in all domains (local host in domain A, user principal in domain B and application server in domain C)| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response| 
| | AS Request to domain A for local host (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request to domain A for local host| 
| | AS Response| 
| | FAST AS Request to domain A for user (no pre-authentication)| 
| | KRB Error (wrong realm)| 
| | FAST AS Request to domain B for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request to domain B for user| 
| | FAST AS Response| 
| | FAST TGS Request to domain B for service| 
| | FAST TGS Response to domain B for service (referral TGT)| 
| | FAST Referral TGS Request to domain C for service| 
| | FAST Referral TGS Response to domain C for service| 
| | AP Session setup request| 
| | AP Session setup response| 

#### <a name="_Toc430273929"/>Compound Identity

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| A user logs on a domain-joined CBAC-Aware application server using Compound Identity supported KDCs (local host in domain A, user principal in domain B and application server in domain C)| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request to domain A for local host (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request to domain A for local host| 
| | AS Response (device claims)| 
| | FAST AS Request to domain A for user (no pre-authentication)| 
| | KRB Error (wrong realm)| 
| | FAST AS Request to domain B for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request to domain B for user| 
| | FAST AS Response (user claims)| 
| | Compound Identity TGS Request to domain B for service| 
| | Compound Identity TGS Response (referral TGT)| 
| | Referral Compound Identity TGS Request to domain C for service| 
| | Referral Compound Identity TGS Response to domain C for service (user and device claims)| 
| | AP Session setup request| 
| | AP Session setup response| 

### <a name="_Toc430273930"/>KRB_ERROR Test

#### <a name="_Toc430273931"/>KRB_ERROR for AS Request Test

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test different types of KRB_ERROR during AS Exchange| 
|  **Call Sequence**| AS Request| 
| | KRB Error| 

#### <a name="_Toc430273932"/>KRB_ERROR for TGS Request Test

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test different types of KRB_ERROR during TGS Exchange| 
|  **Call Sequence**| AS Request| 
| | AS Response| 
| | TGS Request| 
| | KRB Error| 

#### <a name="_Toc430273933"/>KRB_ERROR for AP Request Test

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test different types of KRB_ERROR during TGS Exchange| 
|  **Call Sequence**| AS Request| 
| | AS Response| 
| | TGS Request| 
| | TGS Response| 
| | AP Request| 
| | KRB Error| 

### <a name="_Toc430273934"/>KKDCP Test

#### <a name="_Toc430273935"/>Basic

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test existing cases using KKDCP Server | 
| | Pre-requisite: | 
| |   1. Set up KKDCP server | 
| |   2. Set UseProxy=true in ptfconfig file| 
|  **Call Sequence**| SendProxyRequest| 
| | GetProxyResponse| 

#### <a name="_Toc430273936"/>Negative

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test different types of KKDCP Error | 
|  **Call Sequence**| AS Proxy Request| 
| | KKDCP Error| 

#### <a name="_Toc430273937"/>Kpassword

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test Kpassword when the password change succeeds/fails| 
| | Pre-requisite: | 
| |   1. Set up KKDCP server | 
| |   2. Set UseProxy=true in ptfconfig file| 
| |   3. Set password group policy MinimumPasswordAge=0 | 
| |   4. Set password group policy PasswordHistorySize=0 | 
|  **Call Sequence**| Kpassword Request| 
| | Kpassword Response| 

### <a name="_Toc430273938"/>RC4 Test

#### <a name="_Toc430273939"/>Single Domain: Interactive Logon Test with FAST

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test existing single domain FAST interactive logon cases using RC4-HMAC encryption.| 
|  **Call Sequence**| AS Request for device (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request for device RC4 encryption| 
| | AS Response RC4 encryption| 
| | FAST AS Request for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request for user RC4 encryption| 
| | FAST AS Response RC4 encryption| 
| | FAST TGS Request RC4 encryption| 
| | FAST TGS Response RC4 encryption| 

#### <a name="_Toc430273940"/>Single Domain: Network Logon Test with Compound Identity

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test existing single domain compound identity network logon cases using RC4-HMAC encryption.| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request for device (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request for device RC4 encryption| 
| | AS Response (device claims, PA-SUPPORTED-ENCTYPES) RC4 encryption| 
| | FAST AS Request for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request for user RC4 encryption| 
| | FAST AS Response (user claims, PA-SUPPORTED-ENCTYPES) RC4 encryption| 
| | Compound Identity TGS Request RC4 encryption| 
| | Compound Identity TGS Response (user and device claims) RC4 encryption| 
| | AP Session setup request RC4 encryption| 
| | AP Session setup response RC4 encryption| 

#### <a name="_Toc430273941"/>Multiple Domains: Network Logon Test with Compound Identity

| &#32;| &#32; |
| -------------| ------------- |
|  **Description**| Test existing multiple domain compound identity network logon cases using RC4-HMAC encryption.| 
|  **Call Sequence**| AP Negotiate request| 
| | AP Negotiate response (use Kerberos)| 
| | AS Request to domain A for local host (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | AS Request to domain A for local host RC4 encryption| 
| | AS Response (device claims) RC4 encryption| 
| | FAST AS Request to domain A for user (no pre-authentication)| 
| | KRB Error (wrong realm)| 
| | FAST AS Request to domain B for user (no pre-authentication)| 
| | KRB Error (PA-FX-FAST)| 
| | FAST AS Request to domain B for user RC4 encryption| 
| | FAST AS Response (user claims) RC4 encryption| 
| | Compound Identity TGS Request to domain B for service RC4 encryption| 
| | Compound Identity TGS Response (referral TGT) RC4 encryption| 
| | Referral Compound Identity TGS Request to domain C for service RC4 encryption| 
| | Referral Compound Identity TGS Response to domain C for service (user and device claims) RC4 encryption| 
| | AP Session setup request RC4 encryption| 
| | AP Session setup response RC4 encryption | 

## <a name="_Toc430273942"/>Test Cases Design

### <a name="_Toc430273943"/>Single Domain: Interactive Logon Test

#### <a name="_Toc430273944"/>Basic

#####Normal

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to go through the whole process with default settings.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | cname:  &#60; username &#62; | 
| | realm: contoso.com| 
| | sname: krbtgt/contoso.com| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | crealm: contoso.com| 
| | realm: contoso.com| 
| | sname: krbtgt/contoso.com| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(claims, forward to full DC)| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | cname:  &#60; username &#62; | 
| | realm: contoso.com| 
| | sname: krbtgt/contoso.com | 
| | KDC return AS_REP| 
| | crealm: contoso.com| 
| | ticket: user’s TGT (isCommon1)| 
| | authorization-data: KERB_VALIDATION_INFO, PAC_CLIENT_INFO, PAC_SIGNATURE_DATA, UPN_DNS_INFO, PAC_CLIENT_CLAIMS_INFO| 
| | key: session key| 
| | flags: INITIAL, RENEWABLE, PRE-AUTHENT, FORWARDABLE, CANONICALIZE| 
| | srealm: contoso.com| 
| | sname: krbtgt/contoso.com| 
| | enc-padata: PA-SUPPORTED-ENCTYPES| 
| | Client send TGS request| 
| | padata: PA-TGS-REQ, PA-PAC-OPTIONS(branch aware)| 
| | ap-options: use-session-key, mutual authentication| 
| | ticket: user’s TGT| 
| | authenticator:| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data(authenticator): PAC| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | cname:  &#60; username &#62; | 
| | realm: contoso.com| 
| | sname: host/ &#60; clientcomputername &#62; .contoso.com| 
| | enc-authorization-data: KERB-LOCAL| 
| | KDC returns TGS response| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | ticket: service ticket (isCommon2)| 
| | realm: contoso.com| 
| | sname: host/ &#60; clientcomputername &#62; .contoso.com| 
| | flags: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data:KERB-LOCAL ,KERB_VALIDATION_INFO, PAC_CLIENT_INFO, PAC_SIGNATURE_DATA, UPN_DNS_INFO, PAC_CLIENT_CLAIMS_INFO| 
| | key: session key| 
| | flags: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | srealm: contoso.com| 
| | sname: host/ &#60; clientcomputername &#62; .contoso.com| 
| | enc-padata: PA-SUPPORTED-ENCTYPES| 
|  **Requirements covered**| Section| 
|  **Cleanup**|  | 

#####IPv6 Addresses

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| IPv6_Addresses| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the KDC support IPv6.| 
|  **Prerequisites**| Change the client and KDC’s addresses to IPv6 type;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: user TGT (isKile1)| 
|  **Requirements covered**| isKile:| 
| | KILE SHOULD support IPv6 addresses. ([MS-KILE] section 3.1.5.6);| 
|  **Cleanup**| Change the IP versions back.| 

#####Directional Addresses

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Directional_Addresses| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the KDC support directional addresses.| 
|  **Prerequisites**| Change the client and KDC’s addresses to directional address type;| 
|  **Test Execution Steps**| Client sends AS_REQ for user TGT| 
| | KDC return AS_REP timeout (isKile1)| 
|  **Requirements covered**| isKile:| 
| | KILE MUST NOT support directional addresses. If the directional addresses are present, they MUST be ignored.([MS-KILE] section 3.1.5.6);| 
|  **Cleanup**| Change the IP versions back.| 

#####UDP/IP transport

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| UDP_IP_transport| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC support UDP/IP transport.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client locates a KDC using DNS SRV records.| 
| | KDC returns its own IP address (isKile1)| 
| | Client sends AS_REQ for user over UDP port 88| 
| | KDC returns KRB_ERROR over UDP port 88 (isCommon1)| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ for user over UDP port 88| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC return AS response over UDP port 88 (isCommon1)| 
|  **Requirements covered**| isKile:| 
| | KILE MUST have a working DNS infrastructure ([MS-KILE] Section 2.1); | 
| | Kerberos client implementations MUST provide a means for the client to determine the location of the Kerberos KDCs. (_DNS is one of them._) ([RFC4120] section 7.2.3)| 
| | isCommon:| 
| | Kerberos servers (KDCs) supporting IP transports MUST accept UDP requests and SHOULD listen for them on port 88 (decimal) unless specifically configured to listen on an alternative UDP port. ([RFC4120] section 7.2.1); | 
| | KILE SHOULD use UDP by default ([MS-KILE] section 2.1);| 
|  **Cleanup**|  | 

#####TCP/IP transport

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| TCP_IP_transport| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC support TCP/IP transport.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client locates a KDC using DNS SRV records.| 
| | KDC returns its own IP address (isKile1)| 
| | Client sends AS_REQ for user over UDP port 88| 
| | KDC returns KRB_ERROR over TCP port 88 (isCommon1)| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ for user over UDP port 88| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC return AS response over TCP port 88 (isCommon1)| 
|  **Requirements covered**| isKile:| 
| | KILE MUST have a working DNS infrastructure ([MS-KILE] Section 2.1); | 
| | Kerberos client implementations MUST provide a means for the client to determine the location of the Kerberos KDCs. (_DNS is one of them._) ([RFC4120] section 7.2.3)| 
| | isCommon:| 
| | Kerberos servers (KDCs) supporting IP transports MUST accept TCP requests and SHOULD listen for them on port 88 (decimal) unless specifically configured to listen on an alternative TCP port. ([RFC4120] section 7.2.1);| 
|  **Cleanup**|  | 

#####UDP to TCP

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| UDP_to_TCP| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC can deal with too big response sent from client.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for user over UDP port 88| 
| | KDC returns KRB_ERROR over UDP port 88| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ for user over UDP port 88| 
| | Make the PDU of the AS-REQ larger than 1465 bytes. (isKile1)| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns KRB_ERROR over UDP port 88| 
| | error-code: KDC_ERR_RESPONSE_TOO_BIG (isCommon1)| 
| | Client sends AS_REQ for user over TCP port 88| 
| | Make the PDU of the AS-REQ larger than 1465 bytes. (isKile1)| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC return AS response over TCP port 88 (isCommon1)| 
|  **Requirements covered**| Section 2.1   Transport| 
| | isKile:| 
| | If the message size exceeds a specific configurable value (message size threshold), TCP SHOULD be used. The threshold applies to AS and TGS messages. The do not apply to AP messages because the transport is controlled by the application protocol. ([MS-KILE] section 2.1); | 
| | The default value for the message size threshold for Windows Server 8 is 1465 bytes. ([MS-KILE] section 6  &#60; 2 &#62; );| 
| | isCommon:| 
| | If the response cannot be handled using UDP (for example, because it is too large), the KDC MUST return KRB_ERR_RESPONSE_TOO_BIG, forcing the client to retry the request using the TCP transport. ([RFC4120] section 7.2.1); | 
| | KILE MUST support the KRB_ERR_RESPONSE_TOO_BIG error message. ([MS-KILE] section 3.1.5.5); | 
|  **Cleanup**|  | 

#####Unknown Pre-authentication type

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Unknown_Pre-authentication_Type| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the unknown pre-authentication types can be ignored by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data,  &#60; unknown pa-data &#62; | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using the client’s password),  &#60; unknown pa-data &#62; | 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence) (isKile1)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (check existence) (isKile1)| 
|  **Requirements covered**| isKile:| 
| | Unknown pre-authentication types MUST be ignored by KDCs. ([MS-KILE] section 3.1.5.1);| 
|  **Cleanup**|  | 

#####Using Password with AES256

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Using_Password_with_AES256| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports AES encryption type.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x1F for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including AES256 &#62; | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: | 
| | padata[0]: PA-ENC-TIMESTAMP (isKile1)| 
| | padata[1]: PA-ETYPE-INFO2 (including AES)| 
| | etype: AES256 (isKile2)| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using AES256)| 
| | KDC return AS_REP| 
| | ticket: user TGT (check existence) (isKile3)| 
| | etype: AES256 (isKile2)| 
| | kvno: key version number (isCommon1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES = 0x1F (isKile3)| 
|  **Requirements covered**| isCommon:| 
| | kvno: This field contains the version number of the key under which data is encrypted. It is only present in messages encrypted under long lasting keys, such as principals’ secret keys. ([RFC4120] section 5.2.9)| 
| | isKile:| 
| | When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.| 
| | ([MS-KILE] section 3.1.5.2); | 
| | KILE SHOULD support the Advanced Encryption Standard (AES) encryption types:| 
| | AES256-CTS-HMAC-SHA1-96 [18]| 
| | AES128-CTS-HMAC-SHA1-96 [17]| 
| | ([MS-KILE] section 3.1.5.2);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported.| 
| | If domainControllerFunctionality returns a value  &#62; =3: the KDC SHOULD, in the encrypted pre-auth data part of the AS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F.([MS-KILE] section 3.3.5.3)| 
|  **Cleanup**|  | 

#####Using Password with AES128

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Using_Password_with_AES128| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports AES encryption type.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0xF for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including AES128 &#62; | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: | 
| | padata[0]: PA-ENC-TIMESTAMP (isKile1)| 
| | padata[1]: PA-ETYPE-INFO2 (including AES128)| 
| | etype: AES128 (isKile2)| 
| | salt: CONTOSO.COMtest01 or CONTOSO.COMhostendpoint01.contoso.com (isKile3)| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using AES128)| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence) (isKile4)| 
| | etype: AES128 (isKile2)| 
| | kvno: key version number (isCommon1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES=0xF (isKile4)| 
|  **Requirements covered**| isCommon:| 
| | kvno: This field contains the version number of the key under which data is encrypted. It is only present in messages encrypted under long lasting keys, such as principals’ secret keys. ([RFC4120] section 5.2.9)| 
| | isKile:| 
| | When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.| 
| | ([MS-KILE] section 3.1.5.2); | 
| | KILE SHOULD support the Advanced Encryption Standard (AES) encryption types:| 
| | AES256-CTS-HMAC-SHA1-96 [18]| 
| | AES128-CTS-HMAC-SHA1-96 [17]| 
| | ([MS-KILE] section 3.1.5.2);| 
| | When KILE creates an AES128 key, the password MUST be converted from a Unicode (UTF16) string to a UTF8 string ([MS-KILE] section 3.1.1.2);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3)| 
|  **Cleanup**|  | 

#####Using Password with RC4

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Using_Password_with_RC4| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports RC4 encryption type.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x7 for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including RC4 &#62; | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: | 
| | padata[0]: PA-ENC-TIMESTAMP (isKile1)| 
| | padata[1]: PA-ETYPE-INFO (including RC4) (isKile2)| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using RC4)| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | etype: RC4 (isKile2)| 
| | kvno: key version number (isCommon1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES=0x7 (isKile3)| 
|  **Requirements covered**| isCommon:| 
| | kvno: This field contains the version number of the key under which data is encrypted. It is only present in messages encrypted under long lasting keys, such as principals’ secret keys. ([RFC4120] section 5.2.9)| 
| | isKile:| 
| | When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.| 
| | ([MS-KILE] section 3.1.5.2); | 
| | KILE MAY support the other following encryption types, which are listed in order of relative strength:| 
| | RC4-HMAC [23]| 
| | RC4-HMAC-EXP [24]| 
| | DES-CBC-MD5 [3]| 
| | DES-CBC-CRC [1]| 
| | ([MS-KILE] section 3.1.5.2);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3)| 
|  **Cleanup**|  | 

#####Using Password with DES

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Using_Password_with_DES| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC supports client initiated password-based authentication and whether the KDC supports DES encryption type.| 
|  **Prerequisites**| Set UseDESOnly flag to the krbtgt account;| 
| | Or set attribute msDS-SupportedEncryptionTypes to 0x3 for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including DES &#62; | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: | 
| | padata[0]: PA-ENC-TIMESTAMP (isKile1)| 
| | padata[1]: PA-ETYPE-INFO (including DES) (isKile2)| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using DES)| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | etype: RC4 (isKile2)| 
| | kvno: key version number (isCommon1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES = 0x3 (isKile3)| 
|  **Requirements covered**| isKile:| 
| | When clients perform a password-based initial authentication, they MUST supply the PA-ENC-TIMESTAMP pre-authentication type when they construct the initial AS request.| 
| | ([MS-KILE] section 3.1.5.2); | 
| | KILE MAY support the other following encryption types, which are listed in order of relative strength:| 
| | RC4-HMAC [23]| 
| | RC4-HMAC-EXP [24]| 
| | DES-CBC-MD5 [3]| 
| | DES-CBC-CRC [1]| 
| | ([MS-KILE] section 3.1.5.2);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported.| 
| | If the UseDESOnly flag is set: the KDC SHOULD, in the encrypted pre-auth data part of the AS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x3. ([MS-KILE] section 3.3.5.3)| 
|  **Cleanup**| Unset UseDESOnly flag to the krbtgt account;| 

#####Checksum
**How to trigger other checksum?**

#####INITIAL and PRE-AUTHENT Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| INITIAL_and_PRE-AUTHENT_Flag| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the INITIAL and PRE-AUTHENT ticket flags are supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using the client’s password)| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | flags: INITIAL (isCommon1) and PRE-AUTHENT (isCommon2)| 
|  **Requirements covered**| isCommon:| 
| | The INITIAL flag indicates that a ticket was issued using the AS protocol, rather than issued based on a TGT. Application servers that want to require the demonstrated knowledge of a client’s secret key (e.g., a password-changing program) can insist that this flag be set in any tickets they accept, and can thus be assured that the client’s key was recently presented to the authentication server. ([RFC4120] section 2.1);| 
| | The PRE-AUTHENT and HW-AUTHENT flags provide additional information about the initial authentication, regardless of whether the current ticket was issued directly (in which case INITIAL will also be set) or issued on the basis of a TGT (in which case the INITIAL flag is clear, but the PRE-AUTHENT and HW-AUTHENT flags are carried forward from the TGT). ([RFC4120] section 2.1);| 
| | The INITIAL and PRE-AUTHENT flags: By default, KDCs require pre-authentication when they issue tickets. Clients SHOULD pre-authenticate. KDCs MUST enforce pre-authentication. Therefore, unless the account has been explicitly set to not require Kerberos pre-authentication, the ticket will have the PRE-AUTHENT flag set. ([MS-KILE] section 3.1.5.4);| 
|  **Cleanup**|  | 

#####HW-AUTHENT Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| HW-AUTHENT_Flag| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the HW-AUTHENT ticket flag is supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data| 
| | kdc-options: OPT-HARDWARE-AUTH option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using the client’s password)| 
| | kdc-options: OPT-HARDWARE-AUTH option| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | flags: INITIAL (isCommon1) and HW-AUTHENT set (isCommon2) not set (isKile1)| 
|  **Requirements covered**| isCommon:| 
| | The INITIAL flag indicates that a ticket was issued using the AS protocol, rather than issued based on a TGT. Application servers that want to require the demonstrated knowledge of a client’s secret key (e.g., a password-changing program) can insist that this flag be set in any tickets they accept, and can thus be assured that the client’s key was recently presented to the authentication server. ([RFC4120] section 2.1);| 
| | The PRE-AUTHENT and HW-AUTHENT flags provide additional information about the initial authentication, regardless of whether the current ticket was issued directly (in which case INITIAL will also be set) or issued on the basis of a TGT (in which case the INITIAL flag is clear, but the PRE-AUTHENT and HW-AUTHENT flags are carried forward from the TGT). ([RFC4120] section 2.1);| 
| | isKile:| 
| | The HW-AUTHENT flag: This flag was originally intended to indicate that hardware-supported authentication was used during pre-authentication. This flag is no longer recommended in the Kerberos V5 protocol. KDCs MUST NOT issue a ticket with this flag set. KDCs SHOULD NOT preserve this flag if it is set by another KDC. ([MS-KILE] section 3.1.5.4);| 
|  **Cleanup**|  | 

#####RENEWABLE Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| RENEWABLE_Flag| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the RENEWABLE ticket flag is supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data| 
| | kdc-options: RENEWABLE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted using the client’s password)| 
| | kdc-options: RENEWABLE option| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | flags: RENEWABLE (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The RENEWABLE flag is reset by default, but a client MAY request it be set by setting the RENEWABLE option in the KRB_AS_REQ message. ([RFC4120] section 2.3);| 
| | The RENEWABLE flag: Renewable tickets SHOULD be supported in KILE. ([MS-KILE] section 3.1.5.4);| 
|  **Cleanup**|  | 

#####MAY-POSTDATE/POSTDATED Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| MAYPOSTDATE_POSTDATED_Flag| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the postdated tickets can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | kdc-options: ALLOW-POSTDATE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | kdc-options: ALLOW-POSTDATE option| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | flags: MAY-POSTDATE set (isCommon1) not set (isKile1)| 
| | Client send TGS_REQ| 
| | kdc-options: POSTDATED option| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (check existence)| 
| | flags: POSTDATED, INVALID set (isCommon2) not set (isKile1)| 
|  **Requirements covered**| isCommon:| 
| | It is reset by default; a client MAY request it by setting the ALLOW-POSTDATE option in the KRB_AS_REQ message. ([RFC4120] section 2.4);| 
| | isKile:| 
| | The POSTDATED/MAY-POSTDATE flag: Postdated tickets SHOULD NOT be supported in KILE. ([MS-KILE] section 3.1.5.4)| 
|  **Cleanup**|  | 

#####PROXIABLE/PROXY Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PROXIABLE_PROXY_Flag| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the proxy tickets can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | kdc-options: PROXIABLE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | kdc-options: PROXIABLE option| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | flags: PROXIABLE set (isCommon1) not set (isKile1)| 
| | Client send TGS_REQ| 
| | kdc-options: PROXY option| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (check existence)| 
| | flags: PROXY set (isCommon2) not set (isKile1)| 
|  **Requirements covered**| isCommon:| 
| | By default, the client will request that it be set when requesting a TGT, and that it be reset when requesting any other ticket. ([RFC4120] section 2.5);| 
| | isKile:| 
| | The PROXY/PROXIABLE flag: Proxiable tickets SHOULD NOT be supported in KILE. ([MS-KILE] section 3.1.5.4)| 
|  **Cleanup**|  | 

#####FORWARDABLE/FORWARDED Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FORWARDABLE_FORWARDED_Flag| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the forwarded tickets can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | kdc-options: FORWARDABLE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | kdc-options: FORWARDABLE option| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | flags: FORWARDABLE set (isCommon1)| 
| | Client send TGS_REQ| 
| | kdc-options: FORWARDED option| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (check existence)| 
| | flags: FORWARDED set (isCommon2)| 
|  **Requirements covered**| isCommon:| 
| | This flag is reset by default, but users MAY request that it be set by setting the FORWARDABLE option in the AS request when they request their initial TGT. ([RFC4120] section 2.6);| 
| | The FORWARDED flag is set by the TGS when a client presents a ticket with the FORWARDABLE flag set and requests a forwarded ticket by specifying the FORWARDED KDC option and supplying a set of addresses for the new ticket. ([RFC4120] section 2.6);| 
| | The FORWARDABLE/FORWARDED flag: Forwarded tickets SHOULD be supported in KILE. ([MS-KILE] section 3.1.5.4)| 
|  **Cleanup**|  | 

#####OK-AS-DELEGATE Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| OK-AS-DELEGATE_FLAG| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the OK-AS-DELEGATE ticket flag can be supported by the KDC.| 
|  **Prerequisites**| Set local host account as trusted for delegation;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | flags: OK-AS-DELEGATE (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The copy of the ticket flags in the encrypted part of the KDC reply may have the OK-AS-DELEGATE flag set to indicate to the client that the server specified in the ticket has been determined by the policy of the realm to be a suitable recipient of the delegation. ([RFC4120] section 2.8);| 
| | The OK-AS-DELEGATE flag: The KDC MUST set the OK-AS-DELEGATE flag if the service account is trusted for delegation. ([MS-KILE] section 3.1.5.4)| 
|  **Cleanup**| Unset local host account as trusted for delegation;| 

#####Case Sensitive

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Case_Sensitive| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC is case sensitive or not during the name comparisons, whether users or domains.| 
|  **Prerequisites**| Create a user with user name all lower case.| 
| | Domain name in AD all lower case.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | cname: all upper case| 
| | realm: all upper case| 
| | sname: all upper case| 
| | KDC returns KRB_ERROR| 
| | crealm: all lower case (isKile1)| 
| | realm: all lower case| 
| | sname: all lower case| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | crealm: all lower case (isKile1)| 
| | sname: all lower case| 
| | srealm: all lower case| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
|  **Requirements covered**| isKile:| 
| | Name comparisons, whether for users or domains, MUST NOT be case sensitive in KILE. KILE MUST use UTF-8 encoding of these names. Normalization MUST NOT be performed and surrogates MUST NOT be supported. To match names, the GetWindowsSortKey algorithm with the following flags NORM_IGNORECASE, NORM_IGNOREKANATYPE, NORM_IGNORENONSPACE, and NORM_IGNOREWIDTH SHOULD be used then the CompareSortKey algorithm SHOULD be used to compare the names.Note that this applies only to names; passwords (and the transformation of a password to a key) are governed by the actual key generation specification. ([MS-KILE] section 3.1.5.7)| 
|  **Cleanup**|  | 

#####Key Usage Numbers
**How to test key usage numbers?**

#####Request PAC in TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_PAC_TGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the PA-PAC-REQUEST can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST (true)| 
| | KDC return AS_REP| 
| | ticket: user’s TGT| 
| | authorization-data: PAC (isKile1), initial set of group information (isKile2)| 
| | Client sends TGS_REQ| 
| | KDC return TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC (isKile3)| 
|  **Requirements covered**| isKile:| 
| | They SHOULD request, via the PA-PAC-REQUEST pre-authentication type, that a privilege attribute certificate (PAC) be included in issued tickets. ([MS-KILE] section 3.1.5.1);| 
| | The PAC MUST be generated by the KDC under one of the following conditions:| 
| | During an Authentication Service (AS) request that has been validated with pre-authentication.| 
| | The KDC MUST collect the user’s initial set of group information and add it to the PAC in the TGT.([MS-KILE] section 3.1.5.11);| 
| | The KILE KDC MUST copy the populated fields from the PAC in the TGT to the newly created PAC and, after processing all fields it supports, the KILE KDC MUST generate a new Server Signature and KDC Signature which replace the existing signature fields in the PAC. ([MS-KILE] section 3.3.5.4);| 
|  **Cleanup**|  | 

#####Request no PAC in TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_No_PAC_TGT| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC can issue a TGT without a PAC explicitly requested by the client.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (false)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(false), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (no PAC)| 
| | authorization-data: no PAC generated (isKile1)| 
|  **Requirements covered**| isKile:| 
| | By default, the KDC MUST generate a PAC. However, a client MAY explicitly request that a PAC be excluded through the use of a KERB-PA-PAC-REQUEST PA-DATA type. ([MS-KILE] section 3.1.5.11);| 
|  **Cleanup**|  | 

#####PAC generation when no PAC in TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PAC_generation_when_noPAC_in_TGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC can issue a service ticket when receiving a TGT with no PAC.| 
|  **Prerequisites**| Principal attribute AuthorizationDataNotRequired set to False.| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (false)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(false), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (no PAC)| 
| | authorization-data: no PAC generated| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If a TGS request includes a TGT without a PAC, the KDC SHOULD add a PAC before issuing the service ticket. This occurs when the TGT was issued by a pure realm that is trusted by the domain. The PAC MUST be inserted when there is a mapping to a domain user. ([MS-KILE] 3.3.5.4.2)| 
|  **Cleanup**|  | 

#####PAC generation when client has local groups

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PAC_generation_when_client_has_local_groups| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC can generate a PAC in a service ticket when client has domain local groups.| 
|  **Prerequisites**| Principal attribute AuthorizationDataNotRequired set to False.| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(true), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (PAC)| 
| | authorization-data: PAC generated, initial set of group information| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC updated comparing with the PAC received in user’s TGT, add domain local groups information in PAC (isKile1)| 
|  **Requirements covered**| isKile:| 
| | The PAC MUST be generated by the KDC under one of the following conditions:| 
| | During a TGS request when the client has domain local groups.| 
| | The PAC MUST be subsequently updated when the client requests a service ticket to contain additional domain local groups that are specific to the server’s domain. ([MS-KILE] 3.1.5.11)| 
|  **Cleanup**|  | 

#####Locate a DS_BEHAVIOR_WIN8 DC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Locate a DS_BEHAVIOR_WIN8 DC| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if a Win8 KDC can be successfully reached.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends DsrGetDcNameEx2 (| 
| | AccountName = client account name,| 
| | AllowableAccountControlBits = A, B, C, D, E, F,| 
| | DomainName = client domain name,| 
| | Flags = G, H, U,| 
| | All other fields = NULL,| 
| | )| 
| | KDC returns IP address (isKile1)| 
| | in DomainControllerInfo.DomainControllerAddress| 
|  **Requirements covered**| isKile:| 
| | When a DS_BEHAVIOR_WIN8 domain controller is required, DsrGetDcNameEx2 is called... ([MS-KILE] section 3.1.5.13)| 
|  **Cleanup**|  | 

#####DelegationNotAllowed

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| DelegationNotAllowed| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**| Set the user account as DelegationNotAllowed;| 
|  **Test Execution Steps**| Client sends AS request| 
| | kdc-options: PROXIABLE and FORWARDABLE options| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | kdc-options: PROXIABLE or FORWARDABLE options| 
| | KDC returns AS_REP| 
| | flags: PROXIABLE and FORWARDABLE not set (isKile1) set (isCommon1)| 
|  **Requirements covered**| isKile:| 
| | If DelegationNotAllowed is set to TRUE on the principal, the KILE KDC MUST NOT issue a PROXIALBE or FORWARDABLE ticket flags. ([MS-KILE] section 3.3.5.1)| 
|  **Cleanup**|  | 

#####TrustedForDelegation

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| TrustedForDelegation| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**| Set the user account as TrustedForDelegation;| 
|  **Test Execution Steps**| Client sends AS request| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | flags: OK-AS-DELEGATE (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If TrustedForDelegation is set to TRUE on the principal, the KILE KDC MUST set the OK-AS-DELEGATE ticket flag. ([MS-KILE] section 3.3.5.1)| 
|  **Cleanup**|  | 

#####WithoutUPN

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| WithoutUPN| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**| Set the user account as no UPN;| 
|  **Test Execution Steps**| Client sends AS request| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user TGT| 
| | authorization-data: UPN_DNS_INFO with  &#42;  &#42;  &#42; @ &#42;  &#42;  &#42;  constructed (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the user account object does not have the userPrincipalName attribute set, the KDC SHOULD send a UPN_DNS_INFO structure containing a user principal name (UPN), constructed by concatenating the user name, the "@" symbol, and the DNS name of the domain. ([MS-KILE] section 3.3.5.2)| 
|  **Cleanup**|  | 

#####Pre-AuthenticationNotRequired

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Pre-AuthenticationNotRequired| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Set Pre-AuthenticationNotRequired to TRUE for the user account;| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: nothing| 
| | cname:  &#60; username &#62;  | 
| | KDC return AS_REP| 
| | ticket: user’s TGT (verify existence) (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If Pre-AuthenticationNotRequired is set to TRUE on the principal, the KDC MUST issue a TGT without validating pre-authentication data provided. ([MS-KILE] section 3.3.5.3)| 

#####KERB_VALIDATION_INFO

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KERB_VALIDATION_INFO| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC supports a KERB_VALIDATION_INFO PAC data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(true), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (PAC)| 
| | authorization-data: KERB_VALIDATION_INFO (isKile1)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: KERB_VALIDATION_INFO (isKile2)| 
|  **Requirements covered**| isKile:| 
| | For KILE implementations that use an Active Directory for the account database, KDCs SHOULD retrieve the following attributes from local directory service instance with the same processing rules as defined in …([MS-KILE] 3.3.5.6.3.1);| 
| | The KDC populates the returned KERB_VALIDATION_INFO structure fields as follows: …([MS-KILE] 3.3.5.6.3.1);| 
| | 1. Convert the local machine time into an offset from the beginning of the week (as defined in [MS-SAMR] section 2.2.7.5). This conversion must use the same granularity as the UnitsPerWeek field of the Buffer.SAMPR_USER_ALL_INFORMATION.LogonHours field ([MS-SAMR] section 2.2.7.1) or the Buffer.SAMPR_USER_ALL_INFORMATION.AccountExpires field ([MS-SAMR] section 2.2.7.1) of the SamrQueryInformationUser2 ([MS-SAMR] section 3.1.5.5.5) response message.| 
| | 2. Starting at the offset determined in step 1, examine the remaining entries in the Buffer.SAMPR_USER_ALL_INFORMATION.LogonHours. If the value at the initial offset is disabled for authentication, the KDC MUST return Kerb Error KDC_ERROR_CLIENT_REVOKED with status code STATUS_INVALID_LOGON_HOURS. If none of the remaining entries are disabled, use the time stamp value 0x7FFFFFFFFFFFFFFF. Otherwise, compute a time stamp by adding the offset of the next disabled authentication unit to the current time.| 
| | 3. Set the LogoffTime field to the lesser of the value determined in step 2 and the value of the Buffer.SAMPR_USER_ALL_INFORMATION.AccountExpires field of the SamrQueryInformationUser2 ([MS-SAMR] section 3.1.5.5.5) response message.| 
|  **Cleanup**|  | 

#####PAC_CLIENT_INFO

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PAC_CLIENT_INFO| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC supports a PAC_CLIENT_INFO PAC data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(true), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (PAC)| 
| | authorization-data: PAC_CLIENT_INFO (isKile1)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC_CLIENT_INFO (isKile1)| 
|  **Requirements covered**| isKile:| 
| | The KDC populates the returned PAC_CLIENT_INFO structure fields as follows: …([MS-KILE] 3.3.5.3.2.2);| 
|  **Cleanup**|  | 

#####Server Signature

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Server Signature| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports a PAC_SIGNATURE_DATA PAC data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(true), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (PAC)| 
| | authorization-data: PAC_SIGNATURE_DATA (isKile1)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC_SIGNATURE_DATA (isKile2)| 
|  **Requirements covered**| isKile:| 
| | The KDC creates a keyed has of the entire PAC message with the Signature fields of both PAC_SIGNATURE_DATA structures set to zero using the server account key with the strongest cryptography that the domain supports and populates the returned PAC_SIGNATURE_DATA structure fields as follows: …([MS-KILE] 3.3.5.3.2.3);| 
| | The KILE KDC MUST copy the populated fields from the PAC in the TGT to the newly created PAC and, after processing all fields it supports, the KILE KDC MUST generate a new Server Signature and KDC Signature which replace the existing signature fields in the PAC. ([MS-KILE] section 3.3.5.4);| 
|  **Cleanup**|  | 

#####KDC Signature

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KDC Signature| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports a PAC_SIGNATURE_DATA PAC data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(true), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (PAC)| 
| | authorization-data: PAC_SIGNATURE_DATA (isKile1)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC_SIGNATURE_DATA (isKile2)| 
|  **Requirements covered**| isKile:| 
| | The KDC creates a keyed has of the Server Signature field using the strongest "krbtgt" account key and populates the returned PAC_SIGNATURE_DATA structure fields as follows: …([MS-KILE] 3.3.5.3.2.4);| 
| | The KILE KDC MUST copy the populated fields from the PAC in the TGT to the newly created PAC and, after processing all fields it supports, the KILE KDC MUST generate a new Server Signature and KDC Signature which replace the existing signature fields in the PAC. ([MS-KILE] section 3.3.5.4);| 
|  **Cleanup**|  | 

#####UPN_DNS_INFO

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| UPN_DNS_INFO| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports a UPN_DNS_INFO PAC data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (true)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(true), PA-ENC-TIMESTAMP| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (PAC)| 
| | authorization-data: UPN_DNS_INFO (isKile1)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: UPN_DNS_INFO (isKile1)| 
|  **Requirements covered**| isKile:| 
| | The KDC populates the returned UPN_DNS_INFO structure field as follows: …([MS-KILE] 3.3.5.3.2.5);| 
| | The KDC inserts the DNS and UPN information after the UPN_DNS_INFO structure following the header and starting with the corresponding offset in a consecutive buffer. The UPN and FQDN are encoded using a two-byte UTF16 scheme, in little-endian order.  ([MS-KILE] 3.3.5.3.2.5);| 
|  **Cleanup**|  | 

#####Account Disabled

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Account_Disabled| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC support checking the account policy for every ticket request.| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set POLICY_KERBEROS_VALIDATE_CLIENT bit in the AuthenticationOptions.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Disable user account **(can we do this before getting the TGT?)**| 
| | Client sends TGS_REP| 
| | KDC returns KDC_ERR_CLIENT_REVOKED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client’s domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing.| 
| | If Disable is TRUE, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.| 
| | ([MS-KILE] section3.3.5.4.1);| 
|  **Cleanup**|  | 

#####Account Expired

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Account_Expired| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC support checking the account policy for every ticket request.| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set POLICY_KERBEROS_VALIDATE_CLIENT bit in the AuthenticationOptions.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Expire user account| 
| | Client sends TGS_REP| 
| | KDC returns KDC_ERR_CLIENT_REVOKED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client’s domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing.| 
| | If Expire is TRUE, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.| 
| | ([MS-KILE] section3.3.5.4.1);| 
|  **Cleanup**|  | 

#####Account Locked

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Account_Locked| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC support checking the account policy for every ticket request.| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set POLICY_KERBEROS_VALIDATE_CLIENT bit in the AuthenticationOptions.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Lock user account| 
| | Client sends TGS_REP| 
| | KDC returns KDC_ERR_CLIENT_REVOKED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client’s domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing.| 
| | If Locked is TRUE, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.| 
| | ([MS-KILE] section3.3.5.4.1);| 
|  **Cleanup**|  | 

#####Account Out of Logon Hours

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Account_Out_of_Logon_Hours| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC support checking the account policy for every ticket request.| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set POLICY_KERBEROS_VALIDATE_CLIENT bit in the AuthenticationOptions.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Set the LogonHours for user account not include the current time.| 
| | Client sends TGS_REP| 
| | KDC returns KDC_ERR_CLIENT_REVOKED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client’s domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing.| 
| | If current time is not within the LogonHours, then the KDC MUST return KDC_ERR_CLIENT_REVOKED.| 
| | ([MS-KILE] section3.3.5.4.1);| 
|  **Cleanup**|  | 

#####Account PasswordMustChange - 1

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Account_PasswordMustChange_1| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC support checking the account policy for every ticket request.| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set POLICY_KERBEROS_VALIDATE_CLIENT bit in the AuthenticationOptions.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Set PasswordMustChange time a time in the past| 
| | Client sends TGS_REP| 
| | KDC returns KDC_ERR_KEY_EXPIRED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client’s domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing.| 
| | If the PasswordMustChange is in the past, then the KDC MUST return KDC_ERR_KEY_EXPIRED.| 
| | ([MS-KILE] section3.3.5.4.1);| 
|  **Cleanup**|  | 

#####Account PasswordMustChange - 2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Account_PasswordMustChange_2| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC support checking the account policy for every ticket request.| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set POLICY_KERBEROS_VALIDATE_CLIENT bit in the AuthenticationOptions.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | Set PasswordMustChange time to 0| 
| | Client sends TGS_REP| 
| | KDC returns KDC_ERR_KEY_EXPIRED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the POLICY_KERBEROS_VALIDATE_CLIENT bit is set in the AuthenticationOptions setting on the KDC then KILE will enforce revocation on the KDCs. When this property is set on the KDC for the client’s domain, and the TGT is older than an implementation specific time, the KDC MUST verify that the account is still in good standing.| 
| | If the PasswordMustChange is zero, then the KDC MUST return KDC_ERR_KEY_EXPIRED.| 
| | ([MS-KILE] section3.3.5.4.1);| 
|  **Cleanup**|  | 

#####Mapping Users with Principals

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Mapping_Users_with_Principals| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set AuthorizationDataNotRequired to FALSE for the application server (local host)’s account;| 
| |  **Map the  &#60; username** **2**  **&#62;  user account with a principal**  **host/ &#60; hostname &#62;**  **;**| 
| |  **(Need validate correction)**| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST (false)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST(false), PA-ENC-TIMESTAMP| 
| | cname:  &#60; username1 &#62; | 
| | KDC return AS_REP| 
| | ticket: user’s TGT (no PAC)| 
| | cname:  &#60; username1 &#62; | 
| | authorization-data: no PAC generated| 
| | Client sends TGS_REQ| 
| | cname:  &#60; username1 &#62; | 
| | KDC returns TGS_REP| 
| | ticket: service ticket (include PAC)| 
| | cname:  &#60; username2 &#62;  (isKile1)| 
| | authorization-data: PAC generated according to the mapped user account.| 
|  **Requirements covered**| isKile:| 
| | If the KDC is configured locally to map principals in the realm to accounts based on name. In this case, the KDC MUST search the mapping for a principal with the same name. ([MS-KILE] section 3.3.5.4.2);| 
|  **Cleanup**|  | 

#####Domain Local Group Membership with Resource_SID_Compression

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Domain_Local_Group_Membership_with_Resource_SID_Compression| 
|  **Priority**| P0 | 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Create a  &#60; testgroup &#62;  group in the “contoso.com” domain;| 
| | Add the  &#60; username &#62;  to  &#60; testgroup &#62; ;| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | KDC return AS response| 
| | ticket: user’s TGT (no PAC)| 
| | authorization-data: PAC generated (no group membership information)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (include PAC)| 
| | authorization-data: PAC generated (group membership information) (isKile1) (isKile2)| 
|  **Requirements covered**| isKile:| 
| | Groups can be created so that they are only visible to servers in the same domain. For every service ticket that is issued during a TGS request, except for cross-realm TGTs, the KDC MUST populate the PAC with domain local group membership for the user. ([MS-KILE] section 3.3.5.4.3);| 
| | If the Resource SID compression disabled bit is NOT set in the Application Server’s service account’s KerbSupportedEncryptionTypes: … ([MS-KILE] section 3.3.5.4.3);| 
|  **Cleanup**|  | 

#####Domain Local Group Membership with Disable Resource SID compression

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Domain_Local_Group_Membership_with_Disable_Resource_SID_Compression| 
|  **Priority**| P0 | 
|  **Description** |  | 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Create a  &#60; testgroup &#62;  group in the “contoso.com” domain;| 
| | Add the  &#60; username &#62;  to  &#60; testgroup &#62; ;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x8001C for the host/ &#60; clientcomputername &#62; .contoso.com object in AD;| 
| | (Set Resource SID compression disabled bit on AP’s service account.)| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user’s TGT (no PAC)| 
| | authorization-data: PAC generated (no group membership information)| 
| | Client send TGS request| 
| | KDC returns TGS response| 
| | ticket: PAC, According to Resource SID compression disabled bit set (isKile1)| 
| | authorization-data: SidCount, ExtraSids, Attributes (isKile2)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES =0x8001C | 
|  **Requirements covered**| isKile:| 
| | Groups can be created so that they are only visible to servers in the same domain. For every service ticket that is issued during a TGS request, except for cross-realm TGTs, the KDC MUST populate the PAC with domain local group membership for the user. ([MS-KILE] section 3.3.5.4.3);| 
| | If the Resource SID compression disabled bit is NOT set in the Application Server’s service account’s KerbSupportedEncryptionTypes: … | 
| | Otherwise: … ([MS-KILE] section 3.3.5.4.3);| 
|  **Cleanup**|  | 

#####User Preauthentication fail with DES as crypto

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| UserAccount_DES_PreAuthentication_Fail| 
|  **Priority**| P0| 
|  **Description** | By adding domain controller protection, weak authentication data is no longer usable. Test interactive logon behavior with Kerberos initial authentication with weak crypto| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Domain user testsilo04| 
| | Not a member of Protected Users group| 
| | usedesonly= false| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: AES256| 
| | padata: PA-ENC-TIMESTAMP | 
| | client sends AS_REQ | 
| | padata: PA-ENC-TIMESTAMP | 
| | pre-authentication used DES | 
| | KDC returns KDC_ERR_ETYPE_NOTSUPP| 
|  **Requirements covered**| 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

#####Computer Preauthentication fail with DES as crypto

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| ComputerAccount_DES_PreAuthentication_Fail| 
|  **Priority**| P0| 
|  **Description** | By adding domain controller protection, weak authentication data is no longer usable. Test interactive logon behavior with Kerberos initial authentication with weak crypto| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Domain user testsilo04| 
| | Not a member of Protected Users group| 
| | usedesonly= false| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: AES256| 
| | padata: PA-ENC-TIMESTAMP | 
| | client sends AS_REQ | 
| | padata: PA-ENC-TIMESTAMP | 
| | pre-authentication used DES | 
| | KDC returns KDC_ERR_ETYPE_NOTSUPP| 
|  **Requirements covered**| 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

#### <a name="_Toc430273946"/>FAST

#####Normal

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to go through the whole process with default settings.| 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set registry key ClaimsCompIdFASTSupport to 2 on the KDC (don’t use group policy).| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x1001C for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x1001C for the host/ &#60; clientcomputername &#62; .contoso.com object in AD;| 
| | Purge ticket cache on client computer;| 
|  **Test Execution Steps**| Client sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | enc-padata: PA-SUPPORTED-ENCTYPES(FAST)| 
| | Client sends FAST AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends FAST AS_REQ| 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT| 
| | enc-padata: PA-SUPPORTED-ENCTYPES(FAST)| 
| | Client send FAST TGS request| 
| | KDC returns FAST TGS response| 
| | ticket: service ticket| 
| | enc-padata: PA-SUPPORTED-ENCTYPES(FAST)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####PA-FX-FAST Advertise

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PA-FX-FAST_Advertise| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to test if the KDC supports advertising PA-FX-FAST in the KDC_ERR_PREAUTH_REQUIRED message.| 
|  **Prerequisites**| Set user’s realm support FAST.| 
|  **Test Execution Steps**| Client sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-FX-FAST (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | As with all pre-authentication types, the KDC SHOULD advertise PA-FX-FAST in a PREAUTH_REQUIRED error. KDCs MUST send the advertisement of PA-FX-FAST with an empty pa-value. ([RFC6113] section 5.4.2);| 
|  **Cleanup**| Unset user’s realm support FAST.| 

#####Request Computer TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_Computer_TGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to test if the client could request a computer TGT from the user principal’s domain.| 
|  **Prerequisites**| Set user’s realm support FAST.| 
|  **Test Execution Steps**| Client sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ| 
| | cname:  &#60; clientcomputername &#62; | 
| | realm:  &#60; user’s realm &#62; | 
| | KDC return AS_REP| 
| | ticket: computer’s TGT (isKile1)| 
| | srealm:  &#60; user’s realm &#62; | 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x10000 (isKile2)| 
|  **Requirements covered**| isKile:| 
| | If the client does not have a TGT for the realm and is creating a:| 
| | AS-REQ: the client SHOULD obtain a TGT for the computer principal from the user principal’s domain. ([MS-KILE] section 3.2.5.3);| 
| | In addition to the RFC behavior, the Kerberos client SHOULD use the PA-SUPPORTED-ENCTYPES from the TGT obtained from a realm to determine if a realm supports FAST. ([MS-KILE] section 3.2.5.3);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3);| 
|  **Cleanup**| Unset user’s realm support FAST.| 

#####Request User TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_User_TGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to test if the client could request a user TGT from the user principal’s domain using FAST.| 
|  **Prerequisites**| Set user’s realm support FAST.| 
|  **Test Execution Steps**| Client sends FAST AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends FAST AS_REQ| 
| | cname:  &#60; username &#62; | 
| | realm:  &#60; user’s realm &#62; | 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT (isKile1)| 
| | srealm:  &#60; user’s realm &#62; | 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x10000 (isKile2)| 
| | Client sends FAST TGS_REQ (isKile1)| 
| | KDC return FAST TGS_REP| 
|  **Requirements covered**| isKile:| 
| | In addition to the RFC behavior, the Kerberos client SHOULD use the PA-SUPPORTED-ENCTYPES from the TGT obtained from a realm to determine if a realm supports FAST. ([MS-KILE] section 3.2.5.3);| 
| | If the principal is not the computer account and the client is running on a domain-joined computer, the Kerberos client SHOULD use FAST when the principal’s Realm supports FAST. ([MS-KILE] section 3.2.5.4);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3);| 
|  **Cleanup**| Unset user’s realm support FAST.| 

#####ClaimsCompIdFASTSupport = 0

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| ClaimsCompIdFASTSupport = 0| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the client could support FAST or not.| 
|  **Prerequisites**| Set KDC’s ClaimsCompIdFASTSupport = 0;| 
|  **Test Execution Steps**| Client sends FAST AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: no PA-FX-FAST (isKile1)| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | pa-data: no PA-FX-FAST (isKile1)| 
| | ticket: user TGT| 
| | authorization-data: no PAC-CLIENT_CLAIMS_INFO (isKile2)| 
|  **Requirements covered**| isKile:| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 0: The KDC SHOULD respond as if it does not process FAST.| 
| | ([MS-KILE] section 3.3.5.1);| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 0: The KDC SHOULD NOT insert into the returned PAC a PAC_CLIENT_CLAIMS_INFO structure.| 
| | ([MS-KILE] section 3.3.6.3.2.6);| 
|  **Cleanup**| Unset KDC’s ClaimsCompIdFASTSupport.| 

#####ClaimsCompIdFASTSupport = 1

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| ClaimsCompIdFASTSupport = 1| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the client could support FAST or not.| 
|  **Prerequisites**| Set KDC’s ClaimsCompIdFASTSupport = 1;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: no PA-FX-FAST (isClaims1)| 
| | Client sends FAST AS_REQ| 
| | pa-data: PA-PAC-OPTIONS (claims)| 
| | KDC return FAST AS_REP (isClaims2)| 
| | ticket: user TGT| 
| | authorization-data: PAC-CLIENT_CLAIMS_INFO (isClaims4)| 
| | Client sends AS_REQ| 
| | pa-data: PA-PAC-OPTIONS (claims)| 
| | KDC return AS_REP (isClaims3)| 
| | ticket: user TGT| 
| | authorization-data: PAC-CLIENT_CLAIMS_INFO (isClaims4)| 
|  **Requirements covered**| isClaims:| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 1: … and a KDC_ERR_PREAUTH_REQUIRED is returned in the KRB_ERROR: The KDC SHOULD NOT return PA-FX-FAST in the KRB_ERROR.| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 1: and an armored AS-REQ is received: The KDC SHOULD process per FAST.| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 1: an unarmored AS-REQ is received: The KDC SHOULD continue without FAST.| 
| | ([MS-KILE] section 3.3.5.1);| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 1: If a PA-PAC-OPTIONS [167] PA-DATA type with the Claims bit set is in the AS REQ, the KDC SHOULD behave as noted in the next step, "2 or 3". Otherwise, the KDC SHOULD NOT provide a PAC_CLIENT_CLAIMS_INFO structure. | 
| | ([MS-KILE] section 3.3.6.3.2.6);| 
|  **Cleanup**| Unset KDC’s ClaimsCompIdFASTSupport.| 

#####ClaimsCompIdFASTSupport = 2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| ClaimsCompIdFASTSupport = 2| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the client could support FAST or not.| 
|  **Prerequisites**| Set KDC’s ClaimsCompIdFASTSupport = 2;| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-FX-FAST (???)| 
| | Client sends FAST AS_REQ| 
| | KDC return FAST AS_REP (isClaims1)| 
| | ticket: user TGT| 
| | authorization-data: PAC-CLIENT_CLAIMS_INFO (isClaims3)| 
| | Client sends AS_REQ| 
| | KDC return AS_REP (isClaims2)| 
| | ticket: user TGT| 
| | authorization-data: PAC-CLIENT_CLAIMS_INFO (isClaims3)| 
|  **Requirements covered**| isClaims:| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 2: and an armored AS-REQ is received: The KDC SHOULD process per FAST.| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 2: an unarmored AS-REQ is received: The KDC SHOULD continue without FAST.| 
| | ([MS-KILE] section 3.3.5.1);| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 2: the KDC SHOULD …| 
| | ([MS-KILE] section 3.3.6.3.2.6);| 
|  **Cleanup**| Unset KDC’s ClaimsCompIdFASTSupport.| 

#####ClaimsCompIdFASTSupport = 3

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| ClaimsCompIdFASTSupport = 3| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the client could support FAST or not.| 
|  **Prerequisites**| Set KDC’s ClaimsCompIdFASTSupport = 3;| 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ| 
| | KDC return AS_REP (isClaims2)| 
| | ticket: computer’s TGT| 
| | authorization-data: PAC-CLIENT_CLAIMS_INFO (isClaims3)| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-FX-FAST (isClaims1)| 
| | Client sends FAST AS_REQ| 
| | KDC return FAST AS_REP (isClaims1)| 
| | ticket: user’s TGT| 
| | authorization-data: PAC-CLIENT_CLAIMS_INFO (isClaims3)| 
|  **Requirements covered**| isClaims:| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 3: and an armored AS-REQ is received: The KDC SHOULD process per FAST.| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 3: and an AS-REQ is received: If the principal is a computer account, then the KDC SHOULD continue without FAST. Otherwise, the KDC SHOULD return KDC_ERR_PREAUTH_REQUIRED and return PA-FX-FAST.| 
| | ([MS-KILE] section 3.3.5.1);| 
| | If ClaimsCompIdFASTSupport is set to: | 
| | 2: the KDC SHOULD …| 
| | ([MS-KILE] section 3.3.6.3.2.6);| 
|  **Cleanup**| Unset KDC’s ClaimsCompIdFASTSupport.| 

#####Fresh Armor Key

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Fresh_Armor_Key| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the client will provide a fresh armor key for each conversation.| 
|  **Prerequisites**| Set KDC’s ClaimsCompIdFASTSupport = 3;| 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-key: record this value.| 
| | KDC returns FAST AS_REP| 
| | Client sends FAST AS_REQ| 
| | Armor-key: different from last armor key (isCommon1)| 
| | KDC returns FAST AS_REP| 
|  **Requirements covered**| isCommon:| 
| | Any FAST armor scheme MUST provide a fresh armor key for each conversation. ([RFC6113] section 5.4);| 
|  **Cleanup**|  | 

#####FAST Armor Field Success

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FAST_Armor_Field_Success| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports only one armor type and AP-REQ as armor value for each conversation.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-type: FX_FAST_ARMOR_AP_REQUEST (1) (isCommon1)| 
| | Armor-value: AP-REQ (isCommon1)| 
| | KDC returns FAST AS_REP| 
| | ticket: user TGT (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | Only one armor type is defined in this document. ([RFC6113] section 5.4.1);| 
|  **Cleanup**|  | 

#####FAST Armor Type Failure

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FAST_Armor_Type_Failure| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports only one armor type for each conversation.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-type: unknown type (xxx) (isCommon1)| 
| | Armor-value: AP-REQ| 
| | KDC returns KDC_ERR_PREAUTH_FAILED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | Only one armor type is defined in this document…If a FAST KDC receives an unknown armor type it MUST respond with KDC_ERR_PREAUTH_FAILED. ([RFC6113] section 5.4.1);| 
|  **Cleanup**|  | 

#####FAST Armor Value Failure

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FAST_Armor_Value_Failure| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports the AP_REQ armor value.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-type: FX_FAST_ARMOR_AP_REQUEST (1) (isCommon1)| 
| | Armor-value: not AP-REQ (isCommon1)| 
| | KDC returns KDC_ERR_PREAUTH_FAILED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | …the armor-value contains an ASN.1 DER encoded AP-REQ. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####No Subkey

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| No_Subkey| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports the AP_REQ armor value.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-type: FX_FAST_ARMOR_AP_REQUEST (1)| 
| | Armor-value: AP-REQ without a subkey field (isCommon1)| 
| | KDC returns KDC_ERR_PREAUTH_FAILED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The subkey field in the AP-REQ MUST be present. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####Armor TGT sname

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Armor_TGT_sname| 
|  **Priority**| P0 | 
|  **Description** |  | 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-type: FX_FAST_ARMOR_AP_REQUEST (1)| 
| | Armor-value: AP-REQ:| 
| | Armor Ticket:| 
| | sname: target realm (isCommon1)| 
| | KDC returns FAST AS_REP| 
|  **Requirements covered**| isCommon:| 
| | The server name field of the armor ticket MUST identify the TGS of the target realm. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####AD-FX-FAST-ARMOR in Authenticator

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| AD-FX-FAST-ARMOR_in_Authenticator| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC can reject an AD-FX-FAST-ARMOR in authenticator.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | Client sends FAST TGS_REQ| 
| | pa-data: PA-TGS-REQ (1)| 
| | authenticator: | 
| | authorization-data: AD-FX-FAST-ARMOR (71) (isCommon1)| 
| | KDC returns Failure| 
|  **Requirements covered**| isCommon:| 
| | The TGS MUST reject a request if there is an AD-fx-fast-armor (71) element in the authenticator of the pa-tgs-req padata. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####AD-FX-FAST-ARMOR in ticket

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| AD-FX-FAST-ARMOR_in_ticket| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC can reject an AD-FX-FAST-ARMOR in ticket.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | Client sends FAST TGS_REQ| 
| | pa-data: PA-TGS-REQ (1)| 
| | ticket:| 
| | authorization-data: AD-FX-FAST-ARMOR (71) (isCommon1)| 
| | KDC returns Failure| 
|  **Requirements covered**| isCommon:| 
| | The TGS MUST reject a request if … or if the ticket in the authenticator of a pa-tgs-req contains the AD-fx-fast-armor authorization data element. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####AD-FX-FAST-ARMOR in enc-authorization-data

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| AD-FX-FAST-ARMOR_in_enc-authorization-data| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC can support an AD-FX-FAST-ARMOR in enc-authorization-data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | Client sends FAST TGS_REQ| 
| | pa-data: PA-TGS-REQ (1)| 
| | req-body: | 
| | enc-authorization-data: AD-FX-FAST-ARMOR (71) (isCommon1)| 
| | KDC returns FAST TGS_REP| 
| | ticket: service ticket (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | In order for the host process to ensure that the host ticket is not accidentally or intentionally misused, it MUST include a critical authorization data element of the type AD-fx-fast-armor when providing the authenticator or in the enc-authorization-data field of the TGS request used to obtain the TGT. The corresponding ad-data field of the AD-fx-fast-armor element is empty. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####AD-FX-FAST-USED in authenticator

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| AD-FX-FAST-USED_in_authenticator| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC can support an AD-FX-FAST-USED in authenticator.| 
|  **Prerequisites** |  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | Client sends TGS_REQ| 
| | pa-data: do not include PA-FX-FAST| 
| | pa-data: PA-TGS-REQ (1)| 
| | authenticator: AD-FX-FAST-USED (71) (isCommon1)| 
| | KDC returns KRB_APP_ERR_MODIFIED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | In a TGS request, a client MAY include the AD-fx-fast-used authdata either in the pa-tgs-req authenticator or in the authorization data in the pa-tgs-req ticket. If the KDC receives this authorization data but does not find a FAST padata, then it MUST return KRB_APP_ERR_MODIFIED. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####AD-FX-FAST-USED in ticket

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| AD-FX-FAST-USED_in_ticket| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC can support an AD-FX-FAST-USED in ticket authorization data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | Client sends TGS_REQ| 
| | pa-data: do not include PA-FX-FAST| 
| | pa-data: PA-TGS-REQ (1)| 
| | ticket:| 
| | authorization-data: AD-FX-FAST-USED (71) (isCommon1)| 
| | KDC returns KRB_APP_ERR_MODIFIED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | In a TGS request, a client MAY include the AD-fx-fast-used authdata either in the pa-tgs-req authenticator or in the authorization data in the pa-tgs-req ticket. If the KDC receives this authorization data but does not find a FAST padata, then it MUST return KRB_APP_ERR_MODIFIED. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####Armor TGS request with non-TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Armor TGS request with non-TGT| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the KDC can support FAST armoring TGS request with a non TGT ticket.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | Client sends FAST TGS_REQ| 
| | Armor ticket: non TGT (isCommon1)| 
| | KDC returns FAST TGS_REP| 
| | ticket: service ticket (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | Clients MAY present a non-TGT in the PA-TGS-REQ authenticator and omit the armor field, in which case the armor key is the same that would be computed if the authenticator were used in an FX_FAST_ARMOR_AP_REQUEST armor. This is the only case where a ticket other than a TGT can be used to establish an armor key; even though the armor key is computed the same as an FX_FAST_ARMOR_AP_REQUEST, a non-TGT cannot be used as an armor ticket in FX_FAST_ARMOR_AP_REQUEST. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####Armor TGS request with TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_Service_Ticket| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports a FAST TGS_REQ.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | ticket: user’s TGT (isCommon1)| 
| | Client sends FAST TGS_REQ| 
| | Armor ticket: user’s TGT (isCommon1)| 
| | KDC returns FAST TGS_REP| 
| | ticket: service ticket (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | TGS armor types MUST authenticate the client to the KDC, typically by binding the TGT sub-session key to the armor key. ([RFC6113] section 5.4.1);| 
| | If the ticket presented in the PA-TGS-REQ authenticator is a TGT, then the client SHOULD NOT include the armor field in the Krbfastreq and a subkey MUST be included in the PA-TGS-REQ authenticator. In this case, the armor key is the same armor key that would be computed if the TGS-REQ authenticator was used in an FX_FAST_ARMOR_AP_REQUEST armor. ([RFC6113] section 5.4.2)| 
|  **Cleanup**|  | 

#####Req-checksum

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Req-checksum_for_AS-REQ| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify the Req-checksum field.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Req-checksum: over KDC-REQ-BODY (isKile1)| 
| | KDC returns FAST AS_REP| 
| | Client sends FAST TGS_REQ| 
| | Req-checksum: over AP-REQ in the PA-TGS-REQ padata (isKile2)| 
| | KDC returns FAST TGS_REP| 
|  **Requirements covered**| isCommon:| 
| | For an AS-REQ, it is performed over the type KDC-REQ-BODY for the req-body field of the KDC-REQ structure of the containing message; ([RFC6113] section 5.4.2);| 
| | For a TGS-REQ, it is performed over the type AP-REQ in the PA-TGS-REQ padata of the TGS request. ([RFC6113] section 5.4.2).| 
|  **Cleanup**|  | 

#####FAST Fallback

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FASTFallback| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the client supports fast fallback.| 
|  **Prerequisites**| Set KDC support FAST.| 
|  **Test Execution Steps**| Client sends AS request | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-FX-FAST (isCommon1)| 
| | Client sends AS request| 
| | KDC return AS response (isCommon1)| 
| | Client send TGS request| 
| | KDC returns TGS response| 
|  **Requirements covered**| isCommon:| 
| | Clients MAY also support policies that fall back to other mechanisms or that do not use pre-authentication when FAST is unavailable. ([RFC6113] section 5.4.3).| 
|  **Cleanup**|  | 

#####Pa-data in FAST

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Pa-data_in_FAST| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify pa-data fields in FAST.| 
|  **Prerequisites**| Set KDC support FAST.| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-FX-FAST (isCommon1)| 
| | Client sends FAST AS_REQ| 
| | pa-data: PA-FX-FAST (isCommon2)| 
| | pa-data (in FAST): all the outer PA-DATA when FAST not used | 
| | KDC return FAST AS_REP| 
| | pa-data: PA-FX-FAST| 
| | pa-data (in FAST): all the outer PA-DATA when FAST not used (isCommon1)| 
| | Client send FAST TGS_REQ| 
| | pa-data: PA-FX-FAST (isCommon3)| 
| | pa-data (in FAST): all the outer PA-DATA when FAST not used | 
| | padata: PA-TGS-REQ| 
| | KDC returns FAST TGS_REP| 
|  **Requirements covered**| isCommon:| 
| | Unless otherwise specified, the KDC MUST include any padata that are otherwise in the outer KDC-REP or KDC-ERROR structure into this field. The padata field in the KDC reply structure outside of the PA-FX-FAST-REPLY structure typically includes only the PA-FX-FAST-REPLY padata. ([RFC6113] section 5.4.3).| 
| | The outer AS request typically only includes one pa-data item: PA-FX-FAST. The client MAY include additional pa-data, but the KDC MUST ignore the outer request body and any padata besides PA-FX-FAST if and only if PA-FX-FAST is processed. In the case of the TGS request, the outer request should include PA-FX-FAST and PA-TGS-REQ.| 
| | When an AS generates a response, all padata besides PA-FX-FAST should be included in PA-FX-FAST. The client MUST ignore other padata outside of PA-FX-FAST. ([RFC6113] section 5.4.5).| 
| | In the case of the TGS request, the outer request should include PA-FX-FAST and PA-TGS-REQ. ([RFC6113] section 5.4.5).| 
|  **Cleanup**|  | 

#####Strengthen Key

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Strengthen_Key| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC support strengthen key.| 
|  **Prerequisites**|  **Set KDC support strengthen key.**| 
|  **Test Execution Steps**| Client sends AS request | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | no strengthen key (isCommon2)| 
| | Client sends AS request| 
| | KDC return AS response | 
| | Strengthen key: used to generate the new reply key to decrypt reply encrypted part. (isCommon1)| 
| | Client send TGS request| 
| | KDC returns TGS response| 
| | Strengthen key: used to generate the new reply key to decrypt reply encrypted part. (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The strengthen-key field provides a mechanism for the KDC to strengthen the reply key. If set, the strengthen-key value MUST be randomly generated to have the same etype as that of the reply keybefore being strengthened, and then the reply key is strengthened after all padata items are processed. ([RFC6113] section 5.4.3).| 
| | The strengthen-key field MAY be set in an AS reply; it MUST be set in a TGS reply; it must be absent in an error reply. ([RFC6113] section 5.4.3).| 
|  **Cleanup**|  **Unset KDC support strengthen key.**| 

#####KrbFastFinished

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KrbFastFinished| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC support KrbFastFinished structure.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | no KrbFastFinished (isCommon1)| 
| | Client sends AS request| 
| | KDC return AS response | 
| | Ticket: user TGT| 
| | KrbFastFinished (isCommon1)| 
| | Client send TGS request| 
| | KDC returns TGS response| 
| | Ticket: service ticket| 
| | KrbFastFinished (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The finished field contains a KrbFastFinished structure. It is filled by the KDC in the final message in the conversation. This field is present in an AS-REP or a TGS-REP when a ticket is returned, and it is not present in an error reply. ([RFC6113] section 5.4.3).| 
| | The strengthen-key field MAY be set in an AS reply; it MUST be set in a TGS reply; it must be absent in an error reply. ([RFC6113] section 5.4.3).| 
|  **Cleanup**|  | 

#####FAST KRB-ERROR

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FAST_KRB-ERROR| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC support FAST KRB-ERROR.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends FAST AS_REQ (expect some error)| 
| | KDC returns KRB_ERROR | 
| | error-code:| 
| | e-data: PA-FX-FAST:| 
| | KrbFastResponse:| 
| | pa-data: PA-ETYPE-INFO2 (isCommon1)| 
| | pa-data: PA-FX-ERROR (isCommon2)| 
| | edata: absent (isCommon3)| 
|  **Requirements covered**| isCommon:| 
| | The KDC MUST include all the padata elements such as PA-ETYPE-INFO2 and padata elements that indicate acceptable pre-authentication mechanisms in the KrbFastResponse structure. ([RFC6113] section 5.4.3).| 
| | The KDC MUST also include a PA-FX-ERROR padata item in the KRBFastResponse structure. ([RFC6113] section 5.4.3).| 
| | The e-data field MUST be absent in the PA_FX-ERROR padata. All other fields should be the same as the outer KRB-ERROR. ([RFC6113] section 5.4.3).| 
|  **Cleanup**|  | 

#####FAST-options hide-client-names

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| FAST-options_hide-client-names| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC support FAST options.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends FAST AS_REQ| 
| | Fast-options: hide-client-names (isCommon1)| 
| | Cname: can be anything you want (isCommon1)| 
| | KDC return FAST AS_REP | 
| | Ticket: user TGT| 
| | cname:  &#60; username &#62;  (isCommon1)| 
| | Client send TGS request| 
| | Fast-options: hide-client-names (isCommon1)| 
| | Cname: can be anything you want (isCommon1)| 
| | KDC returns TGS response| 
| | Ticket: service ticket| 
| | Cname:  &#60; username &#62;  (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The Kerberos response defined in [RFC4120] contains the client identity in clear text. This makes traffic analysis straightforward. The hide-client-names option is designed to complicate traffic analysis. If the hide-client-names option is set, the KDC implementing PA-FX-FAST MUST identify the client as the anonymous principal in the KDC reply and the error response. Hence, this option is set by the client if it wishes to conceal the client identity in the KDC response.  A conforming KDC ignores the client principal name in the outer KDC-REQ-BODY field, and identifies the client using the cname and crealm fields in the req-body field of the KrbFastReq structure. ([RFC6113] section 5.4.2).| 
|  **Cleanup**|  | 

#####PA-FX-COOKIE
When FAST padata is included, the PA-FX-COOKIE padata as defined in section 5.2 MUST be included in the padata sequence in the KrbFastResponse sequence if the KDC expects at least one more message from the client in order to complete the authentication. ([MS-KILE] section 5.4.3)

#### <a name="_Toc430273947"/>Protected Users

#####Interactive logon fail with DES crypto

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_DES_PreAuthentication_Fail| 
|  **Priority**| P0| 
|  **Description** | By adding domain controller protection, weak authentication data is no longer usable. Test interactive logon behavior with Kerberos initial authentication with weak crypto| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Domain user testsilo01| 
| | a member of Protected Users group| 
| | use des only= false| 
| | msds-supportedEncryptiontypes: =3 (DES)| 
| | DC: network security: Configure encryption types allowed for Kerberos: Check DES, RC4 only| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including DES  &#62; | 
| | pre-authentication used DES or RC4| 
| | KDC returns KDC_ERR_ETYPE_NOTSUPP| 
|  **Requirements covered**| 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

#####Interactive logon fail with RC4 crypto

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_RC4_PreAuthentication_Fail| 
|  **Priority**| P0| 
|  **Description** | By adding domain controller protection, weak authentication data is no longer usable. Test interactive logon behavior with Kerberos initial authentication with weak crypto| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Domain user testsilo03| 
| | a member of Protected Users group| 
| | use des only= false| 
| | msds-supportedEncryptiontypes: =4(RC4)| 
| | DC: network security: Configure encryption types allowed for Kerberos: Check DES, RC4 only| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including RC4 &#62; | 
| | pre-authentication used DES or RC4| 
| | KDC returns KDC_ERR_ETYPE_NOTSUPP| 
|  **Requirements covered**| 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

#####Interactive logon succeed with user A2AF

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_Interactive_Logon_Succeed| 
|  **Priority**| P0| 
|  **Description** | Expect Testsilo02 logon to the Endpoint01 successfully. User’s A2AF is defined in UserDepartmentRestrictPolicy| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Krbtgt account has no UseDesOnly flag set| 
| | Domain user testsilo02| 
| | a member of Protected Users group| 
| | use des only= false| 
| | A2AF： User.Department==HR| 
| | A2A2=NULL| 
| | Department: HR| 
| | Company: Contoso| 
| | msds-supportedEncryptiontypes: =24, AES(If pre-authentication used AES)| 
| | Domain Computer Endpoint01| 
| | Company=Contoso| 
| | Encbacandarmor=1| 
| | Department=HR| 
| | A2AF, A2A2 are null| 
| | Testsilo02 Interactive logon to Endpoint01| 
| | Policy: | 
| | User: A2AF=user.department==HR| 
| | User TGT lifetime: 60 minutes| 
| | Computer: A2A2=NULL| 
| | Service: A2AF=NULL, A2A2=NULL| 
|  **Test Execution Steps**| Client computer sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client computer sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | enc-padata: PA-SUPPORTED-ENCTYPES(FAST)| 
| | Client user sends FAST AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client user sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | ticket: user’s TGT| 
| | enc-padata: | 
| | padata-type PA-SUPPORTED-ENCTYPES(FAST)| 
| | padata-value set to 0x1F| 
| | check TGT is non-renewable (PG document)| 
| | Check TGT account cannot be delegated (PG document)| 
| | check PROXIABLE or FORWARDABLE ticket flags are not set (isKILE 1)| 
| | check MaxRenewAge and MaxTicketAge ==TGTLifeTime| 
| | Client send FAST TGS request| 
| | KDC returns FAST TGS response| 
| | ticket: service ticket| 
| | padata-type PA-SUPPORTED-ENCTYPES(FAST)| 
| | padata-value set to 0x1F| 
|  **Requirements covered**| isKILE 1| 
| | 3.3.5.1   Request Flag Ticket-issuing Behavior| 
| | KILE KDCs use the following account variables to enforce TicketFlags:| 
| |  If DelegationNotAllowed is set to TRUE on the principal, (or if domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25) and the principal is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4)), the KILE KDC MUST NOT set the PROXIABLE or FORWARDABLE ticket flags ([RFC4120] sections 2.5 and 2.6).| 
| | isKILE 2| 
| | 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
| | 3.3.5.6 as exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 3: the KDC SHOULD, in the encrypted pre-auth data part ([[Referrals-11]](http://go.microsoft.com/fwlink/?LinkId=139781), Appendix A) of the AS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F.| 
| | isKIle 3| 
| | MaxRenewAge for the TGT is 4 hours unless specified by policy.| 
| | MaxTicketAge for the TGT is 4 hours unless specified by policy.| 
| | If domainControllerFunctionality returns a value  &#62; = 6, the KDC MUST determine whether an Authentication Policy is applied to the account. If Enforced is TRUE, then: &#60; 51 &#62; | 
| | If TGTLifetime is not 0: MaxRenewAge for the TGT is TGTLifetime.| 
| | If TGTLifetime is not 0: MaxTicketAge for the TGT is TGTLifetime.| 
|  **Cleanup**|  | 

#####Interactive logon Failed with user A2AF

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_Interactive_Logon_User_A2A2_Fail| 
|  **Priority**| P0| 
|  **Description** | Expect testsilo02 logon to ap01 failed with error TGS_REP: KDC_ERR_POLICY, because user’s A2AF access check failed.| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Krbtgt account has no UseDesOnly flag set| 
| | Domain user testsilo02| 
| | a member of Protected Users group| 
| | use des only= false| 
| | A2AF： User.Department==HR| 
| | A2A2=NULL| 
| | Department: HR| 
| | Company: Contoso| 
| | msds-supportedEncryptiontypes: =24, AES(If pre-authentication used AES)| 
| | Domain Computer AP01| 
| | Company=Contoso| 
| | Encbacandarmor=1| 
| | Department=Financial| 
| | Testsilo02 Interactive logon to AP01| 
| | Policy: | 
| | User: A2AF=user.department==HR| 
| | User TGT lifetime: 60 minutes| 
| | Computer: A2A2=NULL| 
| | Service: A2AF=NULL, A2A2=NULL| 
|  **Test Execution Steps**| Client sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | enc-padata: PA-SUPPORTED-ENCTYPES(FAST)| 
| | Client sends FAST AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends FAST AS_REQ| 
| | KDC return FAST KDC_ERR_POLICY| 
|  **Requirements covered**| isKILE 1| 
| | 3.3.5.1   Request Flag Ticket-issuing Behavior| 
| | KILE KDCs use the following account variables to enforce TicketFlags:| 
| |  If DelegationNotAllowed is set to TRUE on the principal, (or if domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25) and the principal is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4)), the KILE KDC MUST NOT set the PROXIABLE or FORWARDABLE ticket flags ([RFC4120] sections 2.5 and 2.6).| 
| | isKILE 2| 
| | 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
| | 3.3.5.6 as exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 3: the KDC SHOULD, in the encrypted pre-auth data part ([[Referrals-11]](http://go.microsoft.com/fwlink/?LinkId=139781), Appendix A) of the AS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F.| 
| | isKIle 3| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST determine whether an Authentication Policy is applied to the server or service; if Enforced is TRUE then: &#60; 61 &#62; | 
| | If AllowedToAuthenticateTo is not NULL, the PAC of the user and the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateTo. If the access check fails, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

#####Interactive logon succeed with Computer A2A2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_Interactive_Logon_Computer_A2A2_Succeed| 
|  **Priority**| P0| 
|  **Description** | Expect testsilo02 logon to endpiont01 succeed. Access check against A2A2 passed| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Krbtgt account has no UseDesOnly flag set| 
| | Domain user testsilo02| 
| | a member of Protected Users group| 
| | use des only= false| 
| | A2AF=NULL| 
| | A2A2=NULL| 
| | Department: HR| 
| | Company: Contosos| 
| | msds-supportedEncryptiontypes: =24, AES(If pre-authentication used AES)| 
| | Interactive logon to endpoint01 (in silo and policy applied)| 
| | Endpoint01:| 
| | Encbacandarmor=1| 
| | Silo: | 
| | testsilo01, testsilo02, Endpoint01| 
| | Policy: | 
| | User: A2AF=NULL, A2A2=NULL| 
| | User TGT lifetime: 60 minutes| 
| | Computer: A2A2: User.Department==HR| 
| | Service: A2AF=NULL, A2A2=NULL| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including AES &#62;  isKILE 2| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: | 
| | padata: PA-ENC-TIMESTAMP | 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP | 
| | KDC returns AS_REP(isKILE2, isKILE 3)| 
| | check TGT is non-renewable (PG document)| 
| | Check TGT account cannot be delegated (PG document)| 
| | check PROXIABLE or FORWARDABLE ticket flags are not set (isKILE 1)| 
| | check MaxRenewAge and MaxTicketAge ==TGTLifeTime| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP (isKILE4)| 
|  **Requirements covered**| isKILE 1| 
| | 3.3.5.1   Request Flag Ticket-issuing Behavior| 
| | KILE KDCs use the following account variables to enforce TicketFlags:| 
| |  If DelegationNotAllowed is set to TRUE on the principal, (or if domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25) and the principal is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4)), the KILE KDC MUST NOT set the PROXIABLE or FORWARDABLE ticket flags ([RFC4120] sections 2.5 and 2.6).| 
| | isKILE 2| 
| | 3.3.5.6   AS Exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |  If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
| | 3.3.5.6 as exchange| 
| | If domainControllerFunctionality returns a value  &#62; = 3: the KDC SHOULD, in the encrypted pre-auth data part ([[Referrals-11]](http://go.microsoft.com/fwlink/?LinkId=139781), Appendix A) of the AS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x1F.| 
| | isKIle 3| 
| | MaxRenewAge for the TGT is 4 hours unless specified by policy.| 
| | MaxTicketAge for the TGT is 4 hours unless specified by policy.| 
| | If domainControllerFunctionality returns a value  &#62; = 6, the KDC MUST determine whether an Authentication Policy is applied to the account. If Enforced is TRUE, then:[ &#60; 51 &#62; ](#z159)| 
| | If TGTLifetime is not 0: MaxRenewAge for the TGT is TGTLifetime.| 
| | If TGTLifetime is not 0: MaxTicketAge for the TGT is TGTLifetime.| 
| | isKILE 4| 
| | If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST determine whether an Authentication Policy is applied to the server or service; if Enforced is TRUE then:[ &#60; 61 &#62; ](#z179)| 
| | If AllowedToAuthenticateTo is not NULL, the PAC of the user and the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateTo. If the access check fails, the KDC MUST return KDC_ERR_POLICY.| 
| | If there are no claims in the PAC and the PA-PAC-OPTIONS [167] PA-DATA type does not have the Claims bit set, then the KDC SHOULD NOT call the TransformClaimsOnTrustTraversal procedure ([MS-ADTS] section 3.1.1.11.2.11). Otherwise the KDC SHOULD call this procedure. &#60; 62 &#62; | 
| | When KERB-LOCAL data is present, the KDC SHOULD copy the authorization data field ([[RFC4120]](http://go.microsoft.com/fwlink/?LinkId=90458) section 5.2.6) with ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure as an AD-IF-RELEVANT to the end of authorization data in the service ticket. &#60; 63 &#62; | 
| | The KILE KDC MUST copy the populated fields from the PAC in the TGT to the newly created PAC and, after processing all fields it supports, the KILE KDC MUST generate a new Server Signature and KDC Signature which replace the existing signature fields in the PAC. The KDC MUST ensure that the PAC structure specified in [MS-PAC] does not end with a zero-length buffer.| 
|  **Cleanup**|  | 

#####Interactive logon failed with computer A2A2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_Interactive_Logon_Computer_A2A2_Fail| 
|  **Priority**| P0| 
|  **Description** | Testsilo02 interactive logon to endpont01 and failed with KDC_ERR_POLICY because of A2AF access check failed.| 
|  **Prerequisites**| DC DFL &#62; =6| 
| | Krbtgt account has no UseDesOnly flag set| 
| | Domain user testsilo03| 
| | a member of Protected Users group| 
| | use des only= false| 
| | A2AF= NULL| 
| | A2A2=NULL| 
| | msds-supportedEncryptiontypes: =24, AES(If pre-authentication used AES)| 
| | Interactive logon to endpoint01 (not in silo and policy applied)| 
| | Endpoint01:| 
| | Encbacandarmor=1| 
| | Policy: | 
| | User:NULL, A2A2=Null| 
| | User TGT lifetime: 60 minutes| 
| | Computer:: A2A2 User.department==HR| 
| | Service: A2AF=NULL, A2A2=NULL| 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | etype:  &#60; desired encryption algorithm to be used by the client including AES &#62;  isKILE 2| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: | 
| | padata: PA-ENC-TIMESTAMP | 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP | 
| | KDC returns AS_REP(isKILE2, isKILE 3)| 
| | check TGT is non-renewable (PG document)| 
| | Check TGT account cannot be delegated (PG document)| 
| | check PROXIABLE or FORWARDABLE ticket flags are not set (isKILE 1)| 
| | check MaxRenewAge and MaxTicketAge ==TGTLifeTime| 
| | Client sends TGS_REQ| 
| | KDC returns KRB_ERR_POLICY isKILE 1| 
|  **Requirements covered**| isKILE 1| 
| |    3.3.5.6   AS Exchange| 
| |    If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST check whether the account is a member of PROTECTED_USERS ([MS-DTYP] section 2.4.2.4). If it is a member of PROTECTED_USERS, then: &#60; 50 &#62; | 
| |     If pre-authentication used DES or RC4, the KDC MUST return KDC_ERR_POLICY.| 
| |    3.3.5.6   AS Exchange| 
| |    If AllowedToAuthenticateFrom is not NULL, the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateFrom. If the access check fails, the KDC MUST return KDC_ERR_POLICY. | 
|  **Cleanup**|  | 

### <a name="_Toc430273948"/>Single Domain: Network Logon Test

#### <a name="_Toc430273949"/>Basic

#####Normal

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to go through the whole process with default settings.| 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Join file server to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set MutualAuthentication=TRUE for client’s security context;| 
| | Set UseSessionKey=TRUE for client’s security context;| 
| | Set ChannelBinding=FALSE for client’s security context;| 
| | Set ApplicationRequiresCBT=FALSE for server’s security context;| 
| | Set registry key ClaimsCompIdFASTSupport to 0 on the KDC (don’t use group policy).| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x1F for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x1F for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
| | Purge ticket cache on client computer;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS _REQ (no pre-authentication)| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(forward to full DC)| 
| | kdc-options: INITIAL, PRE-AUTHENT, RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | flags: FORWARDABLE, RENEWABLE, PRE-AUTHENT| 
| | authorization-data: PAC, KERB_VALIDATION_INFO, PAC_CLIENT_INFO, PAC_SIGNATURE_DATA, UPN_DNS_INFO, PAC_CLIENT_CLAIMS_INFO(should have no claims info, because ClaimsCompIdFASTSupport=0)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (165) = 0x1F| 
| | Client send TGS_REQ| 
| | sname: cifs/ap01.contoso.com| 
| | enc-authorization-data: KERB-LOCAL| 
| | KDC returns TGS response| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: cifs/ap01.contoso.com| 
| | flags: FORWARDABLE, RENEWABLE| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data: KERB-LOCAL| 
| | key: session key| 
| | flags: FORWARDABLE, RENEWABLE| 
| | srealm: contoso.com| 
| | sname: cifs/ap01.contoso.com| 
| | enc-padata: PA-SUPPORTED-ENCTYPES| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: cifs/ap01.contoso.com| 
| | flags:| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data: PAC| 
| | authenticator: PAC info| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data(authenticator): PAC, KERB-LOCAL, KERB-AUTH-DATA-TOKEN-RESTRICTIONS| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 
|  **Cleanup**|  | 

#####Export Key from Security Context

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Export_Key_from_Security_Context| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the key can be exported as an attribute of the completed security context in the SSPI API.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS _REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS response| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
| | Export key as an attribute of the complete security context in the SSPI API. (isKile1)| 
|  **Requirements covered**| isKile:| 
| | Using KILE, application clients (for example, CIFS/SMB clients) MAY use the negotiated key directly. When an application client uses the session key, the application protocol MUST document the explicit use of the key in its protocol specification. The key MAY be exported as an attribute of the completed security context in the SSPI API. ([MS-KILE] section 3.1.1.2)| 
|  **Cleanup**|  | 
|  **Cleanup**|  | 

#####Server use AES when KerbSupportedEncryptionTypes set

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Server_use_AES_when_KerbSupportedEncryptionTypes_set| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports AES encryption for servers or services.| 
|  **Prerequisites**| Set MutualAuthentication GSS API param on client and server;| 
| | Set attribute msDS-SupportedEncryptionTypes = 0x1F for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x1F (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | authenticator:| 
| | subkey: record “subkey1”| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | subkey: new “subkey2” != “subkey1” (isKile2)| 
| | use subkey2 as sessionkey| 
|  **Requirements covered**| isKile:| 
| | If the server or service has a KerbSupportedEncryptionTypes populated with supported encryption types, then the KDC SHOULD return in the encrypted part of TGS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the server or service. ([MS-KILE] section 3.3.5.4)| 
| | The subkey in the EncAPRepPart of the KRB_AP_REP message SHOULD be used as the session key when MutualAuthentication is requested…; however when AES is used, the subkeys are different and the subkey in the KRB_AP_REP SHOULD be used. ([MS-KILE] section 3.1.1.2);| 
|  **Cleanup**|  | 

#####Server use RC4 when KerbSupportedEncryptionTypes set

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Server_use_RC4_when_KerbSupportedEncryptionTypes_set| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports RC4 encryption for servers or services.| 
|  **Prerequisites**| Set MutualAuthentication GSS API param on client and server;| 
| | Set attribute msDS-SupportedEncryptionTypes = 0x7 for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x7 (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | authenticator:| 
| | subkey: record “subkey1”| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | subkey: new “subkey2” = “subkey1” (isKile2)| 
| | use either subkey1 or subkey2 as sessionkey| 
|  **Requirements covered**| isKile:| 
| | If the server or service has a KerbSupportedEncryptionTypes populated with supported encryption types, then the KDC SHOULD return in the encrypted part of TGS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the server or service. ([MS-KILE] section 3.3.5.4)| 
| | The subkey in the EncAPRepPart of the KRB_AP_REP message SHOULD be used as the session key when MutualAuthentication is requested…; With DES and RC4, the subkey in the KRB_AP_REQ message can be used as the session key, as it is the same as the subkey in the KRB_AP_REP message. ([MS-KILE] section 3.1.1.2);| 
|  **Cleanup**|  | 

#####Server use DES when KerbSupportedEncryptionTypes set

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Server_use_DES_when_KerbSupportedEncryptionTypes_set| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports DES encryption for servers or services.| 
|  **Prerequisites**| Set MutualAuthentication GSS API param on client and server;| 
| | Set attribute msDS-SupportedEncryptionTypes = 0x3 for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x3 (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | authenticator:| 
| | subkey: record “subkey1”| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | subkey: new “subkey2” = “subkey1” (isKile2)| 
| | use either subkey1 or subkey2 as sessionkey| 
|  **Requirements covered**| isKile:| 
| | If the server or service has a KerbSupportedEncryptionTypes populated with supported encryption types, then the KDC SHOULD return in the encrypted part of TGS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the server or service. ([MS-KILE] section 3.3.5.4)| 
| | The subkey in the EncAPRepPart of the KRB_AP_REP message SHOULD be used as the session key when MutualAuthentication is requested…; With DES and RC4, the subkey in the KRB_AP_REQ message can be used as the session key, as it is the same as the subkey in the KRB_AP_REP message. ([MS-KILE] section 3.1.1.2);| 
|  **Cleanup**|  | 

#####Server use DES when KerbSupportedEncryptionTypes not set

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Server_use_DES_when_KerbSupportedEncryptionTypes_not_set| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports DES encryption for servers or services.| 
|  **Prerequisites**| Set MutualAuthentication GSS API param on client and server;| 
| | Set UseDESOnly = true to the server or service account;| 
| | Do not set attribute msDS-SupportedEncryptionTypes = 0x3 for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x3 (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If UseDESOnly is set: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x3. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#####Channel Binding Success

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Channel_Binding_Success| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the AP supports AD-AUTH-DATA-AP-OPTIONS authorization data.| 
|  **Prerequisites**| Set ChannelBinding to TRUE on client;| 
| | Set ApplicationRequiresCBT to TRUE on server.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authenticator:| 
| | authorization-data: add AD-AUTH-DATA-AP-OPTIONS with value of KERB_AP_OPTIONS_CBT (0x4000) (isKile1)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | Authentication success, not return GSS_S_BAD_BINDINGS (isKile2)| 
|  **Requirements covered**| isKile:| 
| | If ChannelBinding is set to TRUE, the client SHOULD send AD-AUTH-DATA-AP-OPTIONS data in an AD-IF-RELEVANT element. The Authorization Data Type AD-AUTH-DATA-AP-OPTIONS has an ad-type of 143 and ad-data of KERB_AP_OPTIONS_CBT (0x4000). The presence of this element indicates that the client expects the applications running on it to include channel binding information in AP requests whenever Kerberos authentication takes place over an “outer channel” such as TLS. Channel binding is provided using the ChannelBinding parameter specified in section 3.2.1. ([MS-KILE] section 3.2.5.7);| 
| | If ApplicationRequiresCBT parameter is set to TRUE, the server, if so configured, MAY return GSS_S_BAD_BINDINGS whenever the AP request message contains all-zero channel binding value and does not contain the AD-IF-RELEVANT element KERB_AP_OPTIONS_CBT. ([MS-KILE] section 3.4.5)| 
|  **Cleanup**|  | 

#####Channel Binding Failure

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| NetworkLogon_Basic_ChannelBinding| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the AP supports AD-AUTH-DATA-AP-OPTIONS authorization data.| 
|  **Prerequisites**| Set ChannelBinding to FALSE on client;| 
| | Set ApplicationRequiresCBT to TRUE on server.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | all-zero channel binding value| 
| | authenticator:| 
| | authorization-data: no AD-AUTH-DATA-AP-OPTIONS| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | return GSS_S_BAD_BINDINGS (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If ApplicationRequiresCBT parameter is set to TRUE, the server, if so configured, MAY return GSS_S_BAD_BINDINGS whenever the AP request message contains all-zero channel binding value and does not contain the AD-IF-RELEVANT element KERB_AP_OPTIONS_CBT. ([MS-KILE] section 3.4.5)| 
|  **Cleanup**|  | 

#####KERB_AUTH_DATA_TOKEN_RESTRICTIONS different MachineID

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KERB_AUTH_DATA_TOKEN_RESTRICTIONS_different_MachineID| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the AP supports KERB-AD-RESTRICTION-ENTRY authorization data.| 
|  **Prerequisites**| Don’t place AP on the client.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ticket:| 
| | sname: not krbtgt/ &#60; realm &#62; | 
| | authenticator:| 
| | authorization-data: add KERB_AUTH_DATA_TOKEN_RESTRICTIONS (141) (isKile1)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | Ignore the authorization data (isKile2)| 
|  **Requirements covered**| isKile:| 
| | When the server name is not Krbtgt, the client SHOULD send an AP request as an authorization data field, initialized as follows:| 
| | …| 
| | KERB_AUTH_DATA_TOKEN_RESTRICTIONS (141), containing the KERB-AD-RESTRICTION-ENTRY structure. ([MS-KILE] section 3.2.5.7);| 
| | The server MUST check if KERB-AD-RESTRICTION-ENTRY.Restriction.MachineID is equal to Machine ID:| 
| | If equal...| 
| | Otherwise, the server MUST ignore the KERB_AUTH_DATA_TOKEN_RESTRICTIONS [141] Authorization Data Type, the KERB-AD-RESTRICTION-ENTRY structure, the KERB-LOCAL (142), and the containing KERB-LOCAL structure. ([MS-KILE] section 3.4.5.3)| 
|  **Cleanup**|  | 

#####KERB_AUTH_DATA_TOKEN_RESTRICTIONS same MachineID

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KERB_AUTH_DATA_TOKEN_RESTRICTIONS_same_MachineID| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the AP supports KERB-AD-RESTRICTION-ENTRY authorization data.| 
|  **Prerequisites**| Place AP on the client.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ticket:| 
| | sname: not krbtgt/ &#60; realm &#62; | 
| | authenticator:| 
| | authorization-data: add KERB_AUTH_DATA_TOKEN_RESTRICTIONS (141) (isKile1)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | Process authentication as a local one (isKile2)| 
|  **Requirements covered**| isKile:| 
| | When the server name is not Krbtgt, the client SHOULD send an AP request as an authorization data field, initialized as follows:| 
| | …| 
| | KERB_AUTH_DATA_TOKEN_RESTRICTIONS (141), containing the KERB-AD-RESTRICTION-ENTRY structure. ([MS-KILE] section 3.2.5.7);| 
| | The server MUST check if KERB-AD-RESTRICTION-ENTRY.Restriction.MachineID is equal to Machine ID:| 
| | If equal, the server SHOULD process the authentication as a local one, because the client and server are on the same machine, and MAY use the KERB-LOCAL AuthorizationData for any local implementation purposes. ([MS-KILE] section 3.4.5.3)| 
|  **Cleanup**|  | 

#####KERB-LOCAL different MachineID

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KERB-LOCAL_different_MachineID| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC and AP supports KERB-LOCAL authorization data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | Client sends TGS_REQ| 
| | enc-authorization-data: KERB-LOCAL (142) (isKile1)| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authorization-data: KERB-LOCAL (142) (isKile2) (isKile3)| 
| | AP sends AP session setup response (AP_REP wrapped in GSS formatting)| 
| | Ignore the authorization data (isKile4)| 
|  **Requirements covered**| isKile:| 
| | When the server name is not krbtgt, the client SHOULD send an authorization data field with ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure in an AD-IF-RELEVANT element in the enc-authorization-data field. ([MS-KILE] section 3.2.5.6);| 
| | When the server name is not Krbtgt, the client SHOULD send an AP request as an authorization data field, initialized as follows:| 
| | Ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure.| 
| | ([MS-KILE] section 3.2.5.7);| 
| | When KERB-LOCAL data is present, the KDC SHOULD copy the authorization data field with ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure as an AD-IF-RELEVANT to the end of authorization data in the service ticket. ([MS-KILE] section 3.3.5.4);| 
| | The server MUST check if KERB-AD-RESTRICTION-ENTRY.Restriction.MachineID is equal to Machine ID:| 
| | If equal...| 
| | Otherwise, the server MUST ignore the KERB_AUTH_DATA_TOKEN_RESTRICTIONS [141] Authorization Data Type, the KERB-AD-RESTRICTION-ENTRY structure, the KERB-LOCAL (142), and the containing KERB-LOCAL structure. ([MS-KILE] section 3.4.5.3)| 
|  **Cleanup**|  | 

#####KERB-LOCAL same MachineID

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KERB-LOCAL_same_MachineID| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC and AP supports KERB-LOCAL authorization data.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | Client sends TGS_REQ| 
| | enc-authorization-data: KERB-LOCAL (142) (isKile1)| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authorization-data: KERB-LOCAL (142) (isKile2) (isKile3)| 
| | AP sends AP session setup response (AP_REP wrapped in GSS formatting)| 
| | Process authentication as a local one (isKile4)| 
|  **Requirements covered**| isKile:| 
| | When the server name is not krbtgt, the client SHOULD send an authorization data field with ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure in an AD-IF-RELEVANT element in the enc-authorization-data field. ([MS-KILE] section 3.2.5.6);| 
| | When the server name is not Krbtgt, the client SHOULD send an AP request as an authorization data field, initialized as follows:| 
| | Ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure.| 
| | ([MS-KILE] section 3.2.5.7);| 
| | When KERB-LOCAL data is present, the KDC SHOULD copy the authorization data field with ad-type KERB-LOCAL (142) and ad-data containing KERB-LOCAL structure as an AD-IF-RELEVANT to the end of authorization data in the service ticket. ([MS-KILE] section 3.3.5.4);| 
| | The server MUST check if KERB-AD-RESTRICTION-ENTRY.Restriction.MachineID is equal to Machine ID:| 
| | If equal, the server SHOULD process the authentication as a local one, because the client and server are on the same machine, and MAY use the KERB-LOCAL AuthorizationData for any local implementation purposes. ([MS-KILE] section 3.4.5.3)| 
|  **Cleanup**|  | 

#####Service Principal Names

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Service_Principal_Names| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to test if the KDC supports SPNs to identify server in TGS-REQs.| 
|  **Prerequisites**| Set SPN attribute in the server computer account.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | KDC return AS response| 
| | Client sends TGS_REQ| 
| | sname: serviceclass“/”hostname [“:”port] [“/”servicename]| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | sname: serviceclass“/”hostname [“:”port] [“/”servicename] (isKile1)| 
|  **Requirements covered**| isKile:| 
| | Kerberos V5 specifies a variety of name types for specifying the name of the server during a TGS request.| 
| | KILE SHOULD use service principal names (SPNs) to identify servers in TGS_REQs. ([MS-KILE] section 3.1.5.12)| 
|  **Cleanup**| Remove SPN attribute in the server computer account.| 

#####RestrictedKrbHost

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| RestrictedKrbHost| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the KDC and server supports the RestrictedKrbHost service class.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS request| 
| | KDC return AS response| 
| | Client sends TGS_REQ| 
| | sname:  &#60; a wrong service class &#62; / &#60; hostname &#62; | 
| | KDC returns TGS_REP| 
| | ticket: service ticket (include PAC)| 
| | sname: RestrictedKrbHost/ &#60; hostname &#62;  (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Use the service ticket with RestrictedKrbHost SPN.| 
| | AP sends AP session setup response (AP_REP wrapped in GSS formatting)| 
| | AP will decrypt the TGT with computer account’s key. And either create or use the session key – according to encryption type for “RestrictedKrbHost” SPN. (isKile2)| 
|  **Requirements covered**| isKile:| 
| | An application can supply a name of the form “RestrictedKrbHost/ &#60; hostname &#62; ” when its callers have provided the hostname but not the correct SPN for the service. Applications SHOULD NOT use “RestrictedKrbHost/ &#60; hostname &#62; ” due to the security considerations in section 5.1.2. Applications calling GSS-API directly MUST provide a target name which SHOULD be an SPN for their service applications for Kerberos authentication. ([MS-KILE] section 3.1.5.12);| 
| | When the server receives AP requests for SPNs with the serviceclass string equal to “RestrictedKrbHost”, it will decrypt the ticket with the computer account’s key and either create or use the session key for the “RestrictedKrbHost”, regardless of the account the target service is running as. ([MS-KILE] section 3.4.5).| 
|  **Cleanup**|  | 

#####UseSessionKey

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| UseSessionKey| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the KDC and server supports the UseSessionKey ap-option.| 
|  **Prerequisites**| Set UseSessionKey GSS-API parameter on client and server.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS request| 
| | KDC return AS response| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ap-options: USE-SESSION-KEY (isKile1)| 
| | AP sends AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If UseSessionKey is set to TRUE, the client SHOULD set the USE-SESSION-KEY flag to TRUE in the ap-options field of the AP-REQ. ([MS-KILE] section 3.2.5.7);| 
|  **Cleanup**|  | 

#####Service ticket without PAC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| NetworkLogon_Basic_NoPAC| 
|  **Priority**| P0| 
|  **Description** | This test case is set to verify if the KDC could issue a service ticket without PAC information in it.| 
|  **Prerequisites**| Set AuthorizationDataNotRequired to the service account.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket with no PAC information (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP sends AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If the Application Server’s service account AuthorizationDataNotRequired is set to TRUE, the KDC MUST NOT include a PAC in the service ticket. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#####KRB_SAFE
**KILE does not implement KRB_SAFE messages.**

#####KRB_PRIV with a time stamp
**KILE does not implement KRB_PRIV**  **messages**  **with a time stamp** **.**

#####KRB_PRIV with a sequence number
**KILE implement KRB_** **PRIV**  **messages**  **with a sequence number** **.**

#####Unknown AP_REQ

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Unknown_AP-REQ| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Forge a faked and an ill-formed AP_REQ| 
| | AP returns AP session setup response| 
| | AP_REP zero length (isKile1)| 
|  **Requirements covered**| isKile:| 
| | KILE will return a zero-length message whenever it receives a message that is either not well-formed or not supported. ([MS-KILE] section 3.4.5);| 
|  **Cleanup**|  | 

#####Key Cache

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Key_Cache| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket with old key| 
| | Change machine password from both AP and KDC side| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
| | Ticket can’t be decrypted by the new password, file server tries to use the old versions of passwords. (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the decryption of the ticket fails and the KILE server has older versions of the server key, the server SHOULD retry decrypting the ticket with the older keys. ([MS-KILE] section 3.4.5);| 
|  **Cleanup**| Change machine password back from both AP and KDC side.| 

#####Detect Ticket Modification

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Detect_Ticket_Modification| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Send a modified service ticket| 
| | AP returns KRB_AP_ERR_MODIFIED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the decryption routines detect a modification of the ticket, the KRB_AP_ERR_MODIFIED error message is returned. ([MS-KILE] section 3.4.5);| 
|  **Cleanup**|  | 

#####Detect Authenticator Modification

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Detect_Authenticator_Modification| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Send a modified authenticator| 
| | AP returns KRB_AP_ERR_MODIFIED (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the decryption routines detect a modification of the authenticator, the KRB_AP_ERR_MODIFIED error message is returned. ([MS-KILE] section 3.4.5);| 
|  **Cleanup**|  | 

#####Three-Leg DCE-Style Mutual Authentication

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Three-Leg_DCE-Style_Mutual_Authentication| 
|  **Priority**| P0 | 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set MutualAuthentication=TRUE for client’s security context;| 
| | Set ChannelBinding=FALSE for client’s security context;| 
| | Set ApplicationRequiresCBT=FALSE for server’s security context;| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ NOT wrapped in GSS formatting, AP_REP not encapsulated.)| 
| | Header + AP_REQ (isKile1)| 
| |  **S** **ignature and encryption message without length of application data.**| 
| | AP sends AP session setup response (AP_REP NOT wrapped in GSS formatting)| 
| | Header + AP_REP (isKile1)| 
| | Client sends SMB3 session setup request (AP_REP NOT wrapped in GSS formatting)| 
| | Header + AP_REP (isKile1)| 
| | Additional AP_REP, the same as is received from the last message. | 
| | Set GSS_C_DCE_STYLE flag to TRUE in the authenticator cksum field.| 
|  **Requirements covered**| isKile:| 
| | An application protocol using the Kerberos protocol must exchange application protocol messages with Kerberos signing or encryption applied in order to verify mutual authentication. DCE, in the authn_dce_secret authentication service mandated that mutual authentication be verified before any RPC messages were exchanged. To accommodate that requirement, the DCE Kerberos implementation issued an additional AP_REPLY message from the client to the server as part of the AP exchange subprotocol.| 
| | Kerberos V5 is not interoperable with the DCE authn_dce_secret security protocol. KILE MUST have compatible extensions for third-party extensions. KILE emulates this behavior as follows:| 
| | The AP-REQ message MUST NOT have GSS-API wrapping. It is sent as is without encapsulating it in a header.| 
| | The signature message and the encryption message MUST NOT include the length of the application data; they are no longer RFC 1964–compliant.| 
| | The client MUST generate an additional AP reply message exactly as the server would as the final message to send to the server. The client SHOULD set the GSS_C_DCE_STYLE flag to TRUE in the authenticator's checksum field. In GSS terms, the client must return success and a message to the server. It is up to the application to deliver the message to the server.| 
| | The server MUST receive the additional AP reply message and verify that the message is constructed correctly.| 
| | The GSS_Wrap() and GSS_WrapEx() methods are not supported with DCE Style authentication. ([MS-KILE] section 3.4.5.1);| 
|  **Cleanup**|  | 

#####Datagram-Style Authentication

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Datagram-Style_Mutual_Authentication| 
|  **Priority**| P0| 
|  **Description** |  **Not very sure about this case.**| 
|  **Prerequisites**| Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Set MutualAuthentication=TRUE for client’s security context;| 
| | Set UseSessionKey=TRUE for client’s security context;| 
| | Set ChannelBinding=FALSE for client’s security context;| 
| | Set ApplicationRequiresCBT=FALSE for server’s security context;| 
| | Set registry key ClaimsCompIdFASTSupport to 0 on the KDC (don’t use group policy);| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x1C for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (include PAC)| 
| | Client sends AP session setup request (AP_REQ not wrapped in GSS formatting, AP_REP not encapsulated.)| 
| | Header + AP_REQ (no authenticator)| 
| |  **S** **ignature and encryption message without length of application data.**| 
| | AP sends AP session setup response (Demand for authenticator)| 
| | Header + AP_REP| 
| | Client sends AP session setup request (AP_REQ not wrapped in GSS formatting, AP_REP not encapsulated.)| 
| | Header + AP_REQ (with authenticator)| 
| |  **S** **ignature and encryption message without length of application data.**| 
| | AP sends AP session setup response (AP_REP not wrapped in GSS formatting)| 
| | Header + AP_REP| 
| | Client decide what session key to use for next messages. Subkey not used anymore in this case. (isKile1)| 
|  **Requirements covered**| isKile:| 
| | Datagram-style authentication is another DCE RPC-inspired variation. In summary, datagram style initializes the security context but does not transmit the authentication message. Instead, the first application data packet is signed or encrypted as decided by the higher-level application protocol and sent to the server. The server, presented with a packet for which it has no security context, sends a demand for authentication back to the client. At that point, the client sends the authentication token previously obtained from the authentication mechanism. Authentication proceeds as normal.| 
| | When authentication is complete, the server verifies or decrypts the application packet. An application protocol that uses this datagram capability MUST have the means within the application protocol to indicate the nature of the security mechanism that is used (if mechanisms other than the Kerberos V5 protocol are possible), and the nature of the protection (signature or encryption) that is applied to the application protocol message. For DCE RPC the application packet is not retransmitted. Therefore, the session key that will be used MUST be decided by the client before any communication with the server. This precludes the sub-session key option of the Kerberos V5 protocol. ([MS-KILE] section 3.4.5.2);| 
|  **Cleanup**|  | 

#####ImpersonationAccessToken
**For KILE implementations**  **that**  **use a security identifier (SID)-based authorization model, the server SHOULD**  **populate**  **the User SID and Security Group SIDs in the ImpersonationAccessToken parameter as follows:**  **…**  **([MS-KILE] section 3.4.5.3);**

#####LDAP AP Without PAC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to go through the whole process with default settings.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS _REQ (no pre-authentication)| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP| 
| | kdc-options: INITIAL, PRE-AUTHENT, RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | flags: FORWARDABLE, RENEWABLE, PRE-AUTHENT| 
| | Client send TGS_REQ| 
| | sname: ldap/ap01.contoso.com| 
| | KDC returns TGS response| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | flags: FORWARDABLE, RENEWABLE| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | key: session key| 
| | flags: FORWARDABLE, RENEWABLE| 
| | srealm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | flags:| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authenticator: | 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 
|  **Cleanup**|  | 

#####LDAP AP with PAC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to go through the whole process with default settings.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS _REQ (no pre-authentication)| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(forward to full DC)| 
| | kdc-options: INITIAL, PRE-AUTHENT, RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | flags: FORWARDABLE, RENEWABLE, PRE-AUTHENT| 
| | authorization-data: PAC, KERB_VALIDATION_INFO, PAC_CLIENT_INFO, PAC_SIGNATURE_DATA, UPN_DNS_INFO, | 
| | enc-padata: PA-SUPPORTED-ENCTYPES (165) = 0x1F| 
| | Client send TGS_REQ| 
| | sname: ldap/ap01.contoso.com| 
| | enc-authorization-data: KERB-LOCAL| 
| | KDC returns TGS response| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | flags: FORWARDABLE, RENEWABLE| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data: KERB-LOCAL| 
| | key: session key| 
| | flags: FORWARDABLE, RENEWABLE| 
| | srealm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | enc-padata: PA-SUPPORTED-ENCTYPES| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | flags:| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data: PAC| 
| | authenticator: PAC info| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data(authenticator): PAC, KERB-LOCAL, KERB-AUTH-DATA-TOKEN-RESTRICTIONS| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 
|  **Cleanup**|  | 

#####LDAP AP with PAC and Claims

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to go through the whole process with default settings.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS _REQ (no pre-authentication)| 
| | kdc-options: RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(Claim, forward to full DC)| 
| | kdc-options: INITIAL, PRE-AUTHENT, RENEWABLE, FORWARDABLE, CANONICALIZE| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | flags: FORWARDABLE, RENEWABLE, PRE-AUTHENT| 
| | authorization-data: PAC, KERB_VALIDATION_INFO, PAC_CLIENT_INFO, PAC_SIGNATURE_DATA, UPN_DNS_INFO, PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (165) = 0x1F| 
| | Client send TGS_REQ| 
| | sname: ldap/ap01.contoso.com| 
| | enc-authorization-data: KERB-LOCAL| 
| | KDC returns TGS response| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | flags: FORWARDABLE, RENEWABLE| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data: KERB-LOCAL| 
| | key: session key| 
| | flags: FORWARDABLE, RENEWABLE| 
| | srealm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | enc-padata: PA-SUPPORTED-ENCTYPES| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | ap-options: use-session-key, mutual authentication| 
| | ticket: service ticket| 
| | realm: contoso.com| 
| | sname: ldap/ap01.contoso.com| 
| | flags:| 
| | key: session key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data: PAC| 
| | authenticator: PAC info| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data(authenticator): PAC, KERB-LOCAL, KERB-AUTH-DATA-TOKEN-RESTRICTIONS| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 
|  **Cleanup**|  | 

#### <a name="_Toc430273950"/>Claims

#####Normal

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Join AP to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Add claims information to  &#60; username &#62;  account;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set MutualAuthentication=TRUE for client’s security context;| 
| | Set UseSessionKey=TRUE for client’s security context;| 
| | Set ApplicationRequiresCBT=FALSE for server’s security context;| 
| | Set registry key ClaimsCompIdFASTSupport to 1 on the KDC (don’t use group policy).| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x4001C for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x4001C for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
| | Purge ticket cache on client computer;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS _REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS(claims, forward to full DC)| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client send TGS_REQ| 
| | KDC returns TGS response| 
| | ticket: service ticket| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Request Claims for Computer

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_Claims_for_Computer| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC supports claims request.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS _REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS(claims) (isClaims1)| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(claims)| 
| | KDC return AS_REP| 
| | ticket: computer TGT| 
| | authorization-data: NOT SURE!! (isClaims1)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x40000 (isClaims2)| 
|  **Requirements covered**| isClaims:| 
| | When sending the AS REQ, add a PA-PAC-OPTIONS [167] PA-DATA type with the Claims bit set in the AS REQ to request claims authorization data. ([MS-KILE] section 3.2.5.4);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3);| 
|  **Cleanup**|  | 

#####Request Claims for User

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_Claims_for_User| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC supports claims request.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS _REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS(claims) (isClaims1)| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(claims)| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO (isClaims1)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x40000 (isClaims2)| 
|  **Requirements covered**| isClaims:| 
| | When sending the AS REQ, add a PA-PAC-OPTIONS [167] PA-DATA type with the Claims bit set in the AS REQ to request claims authorization data. ([MS-KILE] section 3.2.5.4);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3);| 
|  **Cleanup**|  | 

#####Locate a DS_BEHAVIOR_WIN8 DC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Locate a DS_BEHAVIOR_WIN8 DC| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if the client can locate a Win8 DC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS _REQ (no pre-authentication)| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS(claims)| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(claims)| 
| | KDC return AS_REP| 
| | Padata: PA-PAC-OPTIONS (claims not set) (isClaims1)| 
| | ticket: user TGT| 
| | authorization-data: | 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x40000 (isClaims1)| 
| | Client locates a Win8 DC| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS(claims)| 
| | KDC return AS_REP| 
| | Padata: PA-PAC-OPTIONS (claims) (isClaims1)| 
| | ticket: user TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO | 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x40000 (isClaims1)| 
|  **Requirements covered**| isClaims:| 
| | When receiving the AS_REP, if the Claims bit is set in PA-SUPPORTED-ENCTYPES [165], and not set in PA-PAC-OPTIONS [167], the client SHOULD locate a DS_BEHAVIOR_WIN8 DC. ([MS-KILE] section 3.2.5.4);| 
|  **Cleanup**|  | 

#####Using FAST

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Claims_Using_FAST| 
|  **Priority**| P1| 
|  **Description** |  **This test case tests claims using FAST.**  **This will result in authorization failure because no device claims information transmitted.**  **Need Compound Identity support??**| 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Join file server to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Add claims information to  &#60; username &#62;  account;| 
| | Purge ticket cache on client computer;| 
| | Set registry key ClaimsCompIdFASTSupport = 2 on the KDC (don’t use group policy).| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x5001C for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x5001C for the cifs/ &#60; servername &#62; .contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS _REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-FX-FAST| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer TGT| 
| | authorization-data: PAC_DEVICE_CLAIMS_INFO??| 
| | Client sends AS _REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | e-data: PA-FX-FAST| 
| | Client sends FAST AS_REQ| 
| | padata: PA-FX-FAST[PA-PAC-REQUEST, PA-PAC-OPTIONS(claims)]| 
| | KDC return FAST AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client send FAST TGS_REQ| 
| | KDC returns FAST TGS response| 
| | ticket: service ticket| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO, no PAC_DEVICE_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#### <a name="_Toc430273951"/>Compound Identity

#####Normal

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| CompId_Normal| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Join file server to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Add claims information to  &#60; username &#62;  account;| 
| | Add claims information to  &#60; clientcomputername &#62;  accout;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set MutualAuthentication=TRUE for client’s security context;| 
| | Set UseSessionKey=TRUE for client’s security context;| 
| | Set ApplicationRequiresCBT=FALSE for server’s security context;| 
| | Set registry key ClaimsCompIdFASTSupport to 3 on the KDC (don’t use group policy).| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/contoso.com object in AD;| 
| | Purge ticket cache on client computer;| 
| | Create Share Folder on file server and configure CBAC.| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ for computer TGT| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (compId)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends FAST AS_REQ| 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT| 
| | enc-padata: PA-SUPPORTED-ENCTYPES(compId)| 
| | Client send Compound Identity TGS request| 
| | KDC returns Compound Identity TGS response| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Request Computer TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_Computer_TGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to test if the client could request a computer TGT from the user principal’s domain.| 
|  **Prerequisites**| Set user’s realm support FAST.| 
|  **Test Execution Steps**| Client sends AS_REQ | 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ| 
| | cname:  &#60; clientcomputername &#62; | 
| | realm:  &#60; user’s realm &#62; | 
| | KDC return AS_REP| 
| | ticket: computer’s TGT (isKile1)| 
| | srealm:  &#60; user’s realm &#62; | 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x70000 (isKile2)| 
|  **Requirements covered**| isKile:| 
| | If the client does not have a TGT for the realm and is creating a:| 
| | AS-REQ: the client SHOULD obtain a TGT for the computer principal from the user principal’s domain. ([MS-KILE] section 3.2.5.3);| 
| | In addition to the RFC behavior, the Kerberos client SHOULD use the PA-SUPPORTED-ENCTYPES from the TGT obtained from a realm to determine if a realm supports FAST.| 
|  **Cleanup**| Unset user’s realm support FAST.| 

#####Request User TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Request_User_TGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to test if the client could request a user TGT from the user principal’s domain using FAST.| 
|  **Prerequisites**| Set user’s realm support FAST.| 
|  **Test Execution Steps**| Client sends AS_REQ for computer TGT| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ for computer TGT| 
| | KDC returns AS_REP| 
| | Client sends FAST AS_REQ for user TGT| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends FAST AS_REQ for user TGT| 
| | cname:  &#60; username &#62; | 
| | realm:  &#60; user’s realm &#62; | 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT (isKile1)| 
| | srealm:  &#60; user’s realm &#62; | 
| | enc-padata: PA-SUPPORTED-ENCTYPES = 0x70000 (isKile2)| 
| | Client sends FAST TGS_REQ (isKile1)| 
| | KDC return FAST TGS_REP| 
|  **Requirements covered**| isKile:| 
| | In addition to the RFC behavior, the Kerberos client SHOULD use the PA-SUPPORTED-ENCTYPES from the TGT obtained from a realm to determine if a realm supports FAST. ([MS-KILE] section 3.2.5.3);| 
| | If the principal is not the computer account and the client is running on a domain-joined computer, the Kerberos client SHOULD use FAST when the principal’s Realm supports FAST. ([MS-KILE] section 3.2.5.4);| 
| | The KDC SHOULD return in the encrypted part of the AS-REP message PA-DATA with padata-type set to PA-SUPPORTED-ENCTYPES (165), to indicate what encryption types are supported by the KDC, and whether Claims or FAST are supported. ([MS-KILE] section 3.3.5.3);| 
|  **Cleanup**| Unset user’s realm support FAST.| 

#####PAC_DEVICE_INFO

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PAC_DEVICE_INFO| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports PAC_DEVICE_INFO.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x3001C for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ for computer TGT| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ for computer TGT| 
| | KDC returns AS_REP| 
| | Client sends FAST AS_REQ for user TGT| 
| | KDC returns FAST AS_REP| 
| | Client sends Compound Identity TGS_REQ for service ticket| 
| | KDC returns Compound Identity TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC_DEVICE_INFO (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If a compound identity TGS-REQ (FAST TGS-REQ explicitly armored with the computer’s ticket-granting ticket (TGT)) is received and a Compound Identity-supported bit is set in the application server's service account’s KerbSupportedEncryptionTypes, the KDC SHOULD add to the privilege attribute certificate (PAC) a PAC_DEVICE_INFO structure and PAC_DEVICE_CLAIMS_INFO structure with the group membership and claims for the computer.| 
|  **Cleanup**|  | 

#####PAC_DEVICE_CLAIMS_INFO

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PAC_DEVICE_CLAIMS_INFO| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if the KDC supports PAC_DEVICE_CLAIMS_INFO.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/contoso.com object in AD;| 
|  **Test Execution Steps**| Client sends AS_REQ for computer TGT| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends AS_REQ for computer TGT| 
| | KDC returns AS_REP| 
| | Client sends FAST AS_REQ for user TGT| 
| | KDC returns FAST AS_REP| 
| | Client sends Compound Identity TGS_REQ for service ticket| 
| | KDC returns Compound Identity TGS_REP| 
| | ticket: service ticket| 
| | authorization-data: PAC_DEVICE_CLAIMS_INFO (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If a compound identity TGS-REQ (FAST TGS-REQ explicitly armored with the computer’s ticket-granting ticket (TGT)) is received and a Compound Identity-supported bit is set in the application server's service account’s KerbSupportedEncryptionTypes, the KDC SHOULD add to the privilege attribute certificate (PAC) a PAC_DEVICE_INFO structure and PAC_DEVICE_CLAIMS_INFO structure with the group membership and claims for the computer.| 
|  **Cleanup**|  | 

#### <a name="_Toc430273952"/>Authentication Silo

#####Network logon failed with computer A2A2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_Network_Logon_Computer_A2A2_Fail| 
|  **Priority**| P0| 
|  **Description** | Access check the protected user with computerr’s AllowToAuthenticateTo policy set, and Enforced flag are set.| 
|  **Prerequisites**| All DC DFL &#62; =6| 
| | User is member of Protected users group| 
| | User is in the silo, with no policy linked| 
| | Enforced flag is true| 
| | Computer has authentication policy configured, and  AllowedToAuthenticateTo is set| 
| | User doesn’t match the Computer’s A2A2 condition| 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KDC_ERR_POLICY| 
|  **Requirements covered**|    3.3.5.6   AS Exchange| 
| |    If domainControllerFunctionality returns a value  &#62; = 6, the KDC MUST determine whether an Authentication Policy is applied to the account (section 3.3.5.5). If Enforced is TRUE, then: &#60; 51 &#62; | 
| |     If TGTLifetime is not 0: MaxRenewAge for the TGT is TGTLifetime.| 
| |      If TGTLifetime is not 0: MaxTicketAge for the TGT is TGTLifetime.| 
| |      If AllowedToAuthenticateFrom is not NULL, the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateFrom. If the access check fails, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

#####Network logon succeed with computer A2A2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Protected_Users_Network_Logon_Computer_A2A2_Fail| 
|  **Priority**| P0| 
|  **Description** | Access check the protected user with computerr’s AllowToAuthenticateTo policy set, and Enforced flag are set.| 
|  **Prerequisites**| All DC DFL &#62; =6| 
| | User is member of Protected users group| 
| | User is in the silo, with no policy linked| 
| | Enforced flag is true| 
| | Computer has authentication policy configured, and  AllowedToAuthenticateTo is set| 
| | User match the Computer’s A2A2 condition| 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | KDC returns FAST AS_REP| 
| | ticket: user’s TGT (isKile)| 
| | Client sends FAST TGS_REQ| 
| | Armor ticket: user’s TGT (isKile)| 
| | KDC returns TGS response(isKile)| 
|  **Requirements covered**| isKile| 
| |     3.3.5.6   AS Exchange| 
| |     If domainControllerFunctionality returns a value  &#62; = 6, the KDC MUST determine whether an Authentication Policy is applied to the account (section 3.3.5.5). If Enforced is TRUE, then: &#60; 51 &#62; | 
| |      If TGTLifetime is not 0: MaxRenewAge for the TGT is TGTLifetime.| 
| |      If TGTLifetime is not 0: MaxTicketAge for the TGT is TGTLifetime.| 
| |      If AllowedToAuthenticateFrom is not NULL, the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateFrom. If the access check fails, the KDC MUST return KDC_ERR_POLICY.| 
| | isKile| 
| |     3.3.5.7   TGS Exchange| 
| |     If domainControllerFunctionality returns a value  &#62; = 6 ([MS-ADTS] section 3.1.1.3.2.25), the KDC MUST determine whether an Authentication Policy is applied to the server or service (section 3.3.5.5); if Enforced is TRUE then: &#60; 61 &#62; | 
| |      If AllowedToAuthenticateTo is not NULL, the PAC of the user and the PAC of the armor TGT MUST be used to perform an access check for the ACTRL_DS_CONTROL_ACCESS right with additional rights GUID against the AllowedToAuthenticateTo. If the access check fails, the KDC MUST return KDC_ERR_POLICY.| 
|  **Cleanup**|  | 

### <a name="_Toc430273953"/>Multiple Domains: Network Logon Test

#### <a name="_Toc430273954"/>Basic

#####Normal - User in another realm

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal_User_in_another_realm| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “kerb.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “contoso.com” domain;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set the service account as trusted for delegation;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Wrong realm| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Pre-authentication required| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client TGS_REQ| 
| | KDC TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Normal - Resource in another realm

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal_Resource_in_another_realm | 
| | Case name: CrossRealmGetReferralTGT| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “contoso.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “kerb.com” domain;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set the service account as trusted for delegation;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ| 
| | Flag: canonicalize| 
| | KDC returns TGS_REP| 
| | ticket: referral user TGT to the resource’s domain| 
| | Realm: CONTOSO.COM| 
| | sname: krbtgt/KERB.COM| 
| | EncTicketPart: | 
| | Flags: forwardable(1) ?| 
| | Key: inter-key| 
| | crealm: contoso.com| 
| | cname:  &#60; username &#62; | 
| | authorization-data:| 
| | PA-SVR-REFERRAL-INFO       20| 
| |        PA-SVR-REFERRAL-DATA ::= SEQUENCE {| 
| | referred-name  [1] PrincipalName OPTIONAL,| 
| | referred-realm  [0] Realm| 
| | }| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####CANONICALIZE Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| CANONICALIZE_Flag | 
| | CanonicalizeSpnInReferralTgt| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the CANONICALIZE ticket flag can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | Client sends TGS_REQ| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns TGS_REP | 
| | Client sends referral TGS_REQ| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket (check existence)| 
| | spn: check spn is canonicalized| 
|  **Requirements covered**| isCommon:| 
| | In order to request referrals as defined in later sections, the Kerberos client MUST explicitly request the canonicalize KDC option for the AS-REQ or TGS-REQ. This flag indicates to the KDC that the client is prepared to receive a reply that contains a principal name other than the one requested. ([Referrals-11] section 3);| 
| | Clients SHOULD set the canonicalize flag. For non-KILE realms, if RealmCanonicalize is not set for th realm, the client SHOULD NOT set the canonicalize flag. ([MS-KILE] section 3.2.5.1);| 
| | If the canonicalize flag is set, KILE KDCs SHOULD return the krbtgt/FQDN for the domain. | 
|  **Cleanup**|  | 

#####CANONICALIZE Flag – kadmin/changepw

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| CANONICALIZE_Flag – kadmin/changepw| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the CANONICALIZE ticket flag can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | Client sends TGS_REQ| 
| | kdc-options: CANONICALIZE option| 
| | sname: kadmin/changepw| 
| | KDC returns TGS_REP| 
| | flags: CANONICALIZE not set (isCommon1) | 
|  **Requirements covered**| isKile:| 
| | KILE KDCs SHOULD canonicalize principals unless:| 
| | …| 
| | The server principal is kadmin/changepw.| 
| | ([MS-KILE] section 3.3.5.1);| 
|  **Cleanup**|  | 

#####CANONICALIZE Flag – account UseDESOnly

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| CANONICALIZE_Flag – UseDESOnly| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the CANONICALIZE ticket flag can be supported by the KDC.| 
|  **Prerequisites**| Set server or service account as UseDESOnly.| 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | Client sends TGS_REQ| 
| | kdc-options: CANONICALIZE option| 
| | KDC returns TGS_REP| 
| | flags: CANONICALIZE not set (isKile1) | 
|  **Requirements covered**| isKile:| 
| | KILE KDCs SHOULD canonicalize principals unless:| 
| | …| 
| | …| 
| | The account is marked as DES only.| 
| | ([MS-KILE] section 3.3.5.1);| 
|  **Cleanup**|  | 

#####TRANSITED-POLICY-CHECKED Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| TRANSITED-POLICY-CHECKED_Flag| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the TRANSITED-POLICY-CHECKED ticket flag can be supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends referral TGS_REQ| 
| | kdc-options: DISABLE-TRANSITED-CHECK option not set| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket (check existence)| 
| | flags: TRANSITED-POLICY-CHECKED set (isCommon1) not set (isKile1)| 
|  **Requirements covered**| isCommon:| 
| | When the KDC applies such checks and accepts such cross-realm authentication, it will set the TRANSITED-POLICY-CHECKED flag in the service tickets it issues based on the cross-realm TGT. ([RFC4120] section 2.7);| 
| | isKile:| 
| | The TRANSITED-POLICY-CHECKED flag: KILE MUST NOT check for transited domains on servers or a KDC. Application servers MUST ignore the TRANSITED-POLICY-CHECKED setting. ([MS-KILE] section 3.1.5.4)| 
|  **Cleanup**|  | 

#####TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION Flag

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION Flag| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify if the TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION flag can be supported by the KDC.| 
|  **Prerequisites**| Trusted realm dc has | 
| | TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION flag set| 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | Client sends TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket (check existence)| 
| | flags: the KDC MUST return a ticket with the ok-as-delegate flag not set in TicketFlags.| 
| | (isKile1)| 
|  **Requirements covered**| isKile:| 
| | If the TRUST_ATTRIBUTE_CROSS_ORGANIZATION_NO_TGT_DELEGATION flag is set in the trustAttributes field ([MS-ADTS] section 6.1.6.7.9), the KDC MUST return a ticket with the ok-as-delegate flag not set in TicketFlags.| 
|  **Cleanup**|  | 

#####Forwardable ticket

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Forwardable_ticket| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the FORWARDABLE ticket flag can be supported by the KDC.| 
|  **Prerequisites**| Set Delegate = TRUE on client;| 
| | Set local host account as trusted for delegation;| 
|  **Test Execution Steps**| Client sends AS request| 
| | padata: no pre-authentication data| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS request| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user TGT (check existence)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES = encrypt types supported by user’s domain(isKile2)| 
| | Client sends TGS_REQ| 
| | kdc-options: FORWARDABLE option| 
| | etype: ? (isKile1)| 
| | KDC returns TGS_REP| 
| | flags: FORWARDABLE, OK-AS-DELEGATE (isCommon1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES = encrypt types supported by target domain(isKile2)| 
| | Client sends referral TGS_REQ| 
| | kdc-options: FOWARDABLE, FORWARDED option| 
| | etype: keytype field of previous TGS-REP (isKile1)| 
| | pa-data: PA-SUPPORTED-ENCTYPES = mutually supported by KDC (AS-REP) and application server (previous TGS-REP) (isKile2)| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket (check existence)| 
| | flags: FORWARDABLE, FORWARDED, OK-AS-DELEGATE (isCommon2)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES = encrypt types supported by server or service | 
|  **Requirements covered**| isCommon:| 
| | This flag is reset by default, but users MAY request that it be set by setting the FORWARDABLE option in the AS request when they request their initial TGT. ([RFC4120] section 2.6);| 
| | If Delegate is set to TRUE, the client SHOULD set the FORWARDABLE option in the TGS request. When the client receives a forwardable ticket, it puts the ticket in a KRB_CRED structure. The client SHOULD NOT forward the ticket unless the TGT is marked OK-AS-DELEGATE. ([MS-KILE] section 3.2.5.1)| 
| | The FORWARDED flag is set by the TGS when a client presents a ticket with the FORWARDABLE flag set and requests a forwarded ticket by specifying the FORWARDED KDC option and supplying a set of addresses for the new ticket. ([RFC4120] section 2.6);| 
| | The FORWARDABLE/FORWARDED flag: Forwarded tickets SHOULD be supported in KILE. ([MS-KILE] section 3.1.5.4)| 
| | isKile:| 
| | When the client requests a forwardable TGT for the application server, the client SHOULD:| 
| | Set the etype field of the TGS-REQ to the contents of the keytype field in the previous TGS-REP to specify the common encryption type. ([MS-KILE] section 3.2.5.5)| 
| | When the client requests a forwardable TGT for the application server, the client SHOULD:| 
| | Provide a PA-SUPPORTED-ENCTYPES value for padata, based on the encryption types mutually supported by the KDC and the application server for the session key with the delegated TGT. The client uses the KDC encryption types provided in the AS-REP from the KDC and the application server encryption types provided in the previous TGS-REP for the application. ([MS-KILE] section 3.2.5.5)| 
|  **Cleanup**|  | 

#####Target Realm use AES

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Realm_use_AES| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if the KDC supports AES encryption for servers or services.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x1F for the krbtgt/ &#60; targetrealm &#62;  object in AD;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | sname: krbtgt/ &#60; target realm &#62;  (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x1F (isKile1)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | Otherwise:| 
| | If the account is krbtgt, and domainControllerFunctionality returns greater than or equal to 3: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), the padata-value set to 0x1F, the Claims-supported bit if claims is supported, and the FAST-supported bit if FAST is supported. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#####Target Realm use RC4

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Realm_use_RC4| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports RC4 encryption for servers or services.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x7 for the krbtgt/ &#60; targetrealm &#62;  object in AD;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | sname: krbtgt/ &#60; target realm &#62;  (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x7 (isKile1)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | Otherwise:| 
| | If the account is krbtgt, and domainControllerFunctionality returns greater than or equal to 3: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), the padata-value set to 0x1F, the Claims-supported bit if claims is supported, and the FAST-supported bit if FAST is supported. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#####Target Realm use DES

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Realm_use_DES| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports DES encryption for servers or services.| 
|  **Prerequisites**| Set UseDESOnly = true on the krbtgt/ &#60; targetrealm &#62;  account and do not | 
| | set attribute msDS-SupportedEncryptionTypes to 0x3 for the krbtgt/ &#60; targetrealm &#62;  object in AD;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | sname: krbtgt/ &#60; target realm &#62;  (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x3 (isKile1, isKILE2)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If UseDESOnly is set: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x3. ([MS-KILE] section 3.3.5.4)| 
| | DFL &#62; =6: DES MUST NOT be used unless no other etype is supported. &#60; 67 &#62; | 
|  **Cleanup**|  | 

#####Target Realm support Claims

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Realm_support_Claims| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports AES encryption for servers or services.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x4001F for the krbtgt/ &#60; targetrealm &#62;  object in AD;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | sname: krbtgt/ &#60; target realm &#62;  (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x4001F (isKile1)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | Otherwise:| 
| | If the account is krbtgt, and domainControllerFunctionality returns greater than or equal to 3: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), the padata-value set to 0x1F, the Claims-supported bit if claims is supported, and the FAST-supported bit if FAST is supported. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#####PAC generation when no PAC in TGT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| PAC_generation_when_no_PAC_in_TGT| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports PAC generation when no PAC TGT is received.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST (false)| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST (false)| 
| | KDC return AS_REP| 
| | ticket: user TGT without PAC| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | authorization-data: PAC generated (isKile1)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | The PAC MUST be generated by the KDC under one of the following conditions:| 
| | During a TGS request when the TGT for the client in the request does not contain a PAC and the ticket to be returned is a cross-realm referral TGT. ([MS-KILE] section 3.1.5.11)| 
|  **Cleanup**|  | 

#####Other Organization SID in PAC Success

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Other_Organization_SID_in_PAC_Success| 
|  **Priority**| P0 (BVT)| 
|  **Description** | This test case is designed to verify if KDC supports Other Organization SID in PAC.| 
|  **Prerequisites**| Set the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag in the TrustAttributes field;| 
| |  **Set:**| 
| |  **Security descriptor = server AD account object;**| 
| |  **Client principal = Client user;**| 
| |  **R** **equested access = ACTRL_DS_CONTROL_ACCESS;**| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | cname:  &#60; username &#62;  | 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC, SID S-1-5-1000 (isKile2)| 
| | Client sends TGS_REQ| 
| | KDC perform an access check for the Allowed-To-Authenticate right (ACL check)| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (isKile1)| 
| | Else: KDC_ERR_POLICY| 
|  **Requirements covered**| isKile1:| 
| | If the PAC contains the SID S-1-5-1000 (Other Organization), the PAC MUST be used to perform an access check for the Allowed –To-Authenticate right against the Active Directory object of the account for which the service ticket request is being made. If the access check succeeds, the service ticket MUST be issued; otherwise, the KDC MUST return KDC_ERR_POLICY. ([MS-KILE] section 3.3.5.4)| 
| | If the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag is set in the TrustAttributes field, the OTHER_ORGANIZATION SID MUST be added to the user’s PAC. ([MS-KILE] section 3.3.5.4.5);| 
|  **Cleanup**| Unset SID information.| 

#####Other Organization SID in PAC Failure - 1

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Other_Organization_SID_in_PAC_Failure_1| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC supports Other Organization SID in PAC.| 
|  **Prerequisites**| Set SID information to the user account;| 
| | Set the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag in the TrustAttributes field.| 
| | Set:| 
| |  **Security descriptor != server AD account object;**| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | cname:  &#60; username &#62;  | 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC, SID S-1-5-1000 (isKile2)| 
| | Client sends TGS_REQ| 
| | KDC perform an access check for the Allowed-To-Authenticate right (ACL check failed)| 
| | Security descriptor != server AD account object (isKile3)| 
| | KDC returns KDC_ERR_POLICY (isKile1)| 
|  **Requirements covered**| isKile1:| 
| | If the PAC contains the SID S-1-5-1000 (Other Organization), the PAC MUST be used to perform an access check for the Allowed –To-Authenticate right against the Active Directory object of the account for which the service ticket request is being made. If the access check succeeds, the service ticket MUST be issued; otherwise, the KDC MUST return KDC_ERR_POLICY. ([MS-KILE] section 3.3.5.4);| 
| | If the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag is set in the TrustAttributes field, the OTHER_ORGANIZATION SID MUST be added to the user’s PAC. ([MS-KILE] section 3.3.5.4.5);| 
| | The KDC MUST perform an ACL check while processing the TGS request as follows:| 
| | The security descriptor MUST be that of the server AD account object,| 
| | The client principal MUST be that of the client user,| 
| | And the requested access MUST be ACTRL_DS_CONTROL_ACCESS.| 
| | If there is a failure in the check, the KDC MUST reject the authentication request with KDC_ERROR_POLICY. ([MS-KILE] section 3.3.5.4.5);| 
|  **Cleanup**|  | 

#####Other Organization SID in PAC Failure - 2

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Other_Organization_SID_in_PAC_Failure_2| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC supports Other Organization SID in PAC.| 
|  **Prerequisites**| Set SID information to the user account;| 
| | Set the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag in the TrustAttributes field.| 
| |  **Set:**| 
| |  **C** **lient principal != client user;**| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | cname:  &#60; username &#62;  | 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC, SID S-1-5-1000 (isKile2)| 
| | Client sends TGS_REQ| 
| | KDC perform an access check for the Allowed-To-Authenticate right (ACL check failed)| 
| | Client principal != client user (isKile3)| 
| | KDC returns KDC_ERR_POLICY (isKile1)| 
|  **Requirements covered**| isKile1:| 
| | If the PAC contains the SID S-1-5-1000 (Other Organization), the PAC MUST be used to perform an access check for the Allowed –To-Authenticate right against the Active Directory object of the account for which the service ticket request is being made. If the access check succeeds, the service ticket MUST be issued; otherwise, the KDC MUST return KDC_ERR_POLICY. ([MS-KILE] section 3.3.5.4);| 
| | If the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag is set in the TrustAttributes field, the OTHER_ORGANIZATION SID MUST be added to the user’s PAC. ([MS-KILE] section 3.3.5.4.5);| 
| | The KDC MUST perform an ACL check while processing the TGS request as follows:| 
| | The security descriptor MUST be that of the server AD account object,| 
| | The client principal MUST be that of the client user,| 
| | And the requested access MUST be ACTRL_DS_CONTROL_ACCESS.| 
| | If there is a failure in the check, the KDC MUST reject the authentication request with KDC_ERROR_POLICY. ([MS-KILE] section 3.3.5.4.5);| 
|  **Cleanup**|  | 

#####Other Organization SID in PAC Failure - 3

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Other_Organization_SID_in_PAC_Failure_1| 
|  **Priority**| P0| 
|  **Description** | This test case is designed to verify if KDC supports Other Organization SID in PAC.| 
|  **Prerequisites**| Set SID information to the user account;| 
| | Set the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag in the TrustAttributes field.| 
| |  **Set:**| 
| |  **requested access != ACTRL_DS_CONTROL_ACCESS;**| 
|  **Test Execution Steps**| Client sends AS_REQ (no pre-authentication)| 
| | cname:  &#60; username &#62;  | 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | ticket: user TGT| 
| | authorization-data: PAC, SID S-1-5-1000 (isKile2)| 
| | Client sends TGS_REQ| 
| | KDC perform an access check for the Allowed-To-Authenticate right (ACL check failed)| 
| | requested access != ACTRL_DS_CONTROL_ACCESS (isKile3)| 
| | KDC returns KDC_ERR_POLICY (isKile1)| 
|  **Requirements covered**| isKile1:| 
| | If the PAC contains the SID S-1-5-1000 (Other Organization), the PAC MUST be used to perform an access check for the Allowed –To-Authenticate right against the Active Directory object of the account for which the service ticket request is being made. If the access check succeeds, the service ticket MUST be issued; otherwise, the KDC MUST return KDC_ERR_POLICY. ([MS-KILE] section 3.3.5.4);| 
| | If the TRUST_ATTRIBUTE_CROSS_ORGANIZATION flag is set in the TrustAttributes field, the OTHER_ORGANIZATION SID MUST be added to the user’s PAC. ([MS-KILE] section 3.3.5.4.5);| 
| | The KDC MUST perform an ACL check while processing the TGS request as follows:| 
| | The security descriptor MUST be that of the server AD account object,| 
| | The client principal MUST be that of the client user,| 
| | And the requested access MUST be ACTRL_DS_CONTROL_ACCESS.| 
| | If there is a failure in the check, the KDC MUST reject the authentication request with KDC_ERROR_POLICY. ([MS-KILE] section 3.3.5.4.5);| 
|  **Cleanup**|  | 

#####Forwarded TGT etype

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Forwarded_TGT_Etype| 
|  **Priority**| P0| 
|  **Description** |  | 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | kdc-options: forwardable| 
| | KDC returns TGS_REP| 
| | flags: forwardable| 
| | Client send referral TGS_REQ| 
| | kdc-options: forwarded| 
| | ticket: user’s referral TGT| 
| | etype: not supported by the referred KDC| 
| |  **enc-padata: PA-SUPPORTED-ENCTYPES (??** **?** **)**| 
| | KDC returns referral TGS_REP| 
| | flags: forwarded (isKile1)| 
| | ticket:| 
| | etype: generated by the etype in referral TGT and the PA-SUPPORTED-ENCTYPES (isKile1)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (etype supported by this KDC) | 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If a TGS-REQ message requesting a FORWARDED TGT provides an etype value that is not supported by the KDC, and the client provides a PA_SUPPORTED-ENCTYPES with encryption types the KDC supports, then the KDC MAY select the strongest encryption type that is both included in the PA-SUPPORTED-ENCTYPES and supported by the KDC to generate the random session key. ([MS-KILE] section 3.3.5.4.6);| 
|  **Cleanup**|  | 

#####Cross realm –pa-srv-referral-info

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal_Resource_in_another_realm| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “contoso.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “kerb.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ with Canonicalize option| 
| | KDC returns TGS_REP| 
| | ticket: referral user TGT to the resource’s domain| 
| | (windows)padata: principal name and target realm in encrypted part: pa-srv-referral-info(type 20) | 
| | (rfc) padata: principal name and target realm in padata part: pa-server-referral (type 25, keyusage number 26)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Cross realm – Reject to referral if canonicalize not set

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Normal_Resource_in_another_realm| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “contoso.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “kerb.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ without Canonicalize option| 
| | KDC returns TGS_REP| 
| | ticket: referral user TGT to the resource’s domain| 
| | (windows)padata: principal name and target realm in encrypted part: pa-srv-referral-info(type 20) | 
| | (rfc) padata: principal name and target realm in padata part: pa-server-referral (type 25, keyusage number 26)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Cross realm without PAC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**|  | 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “contoso.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “kerb.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ with Canonicalize option, | 
| | KDC returns TGS_REP| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Cross realm with PAC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**|  | 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “contoso.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “kerb.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ with PA_PAC_REQUEST, with Canonicalize option| 
| | KDC returns TGS_REP| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket with PAC| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####Cross realm with PAC and claim

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**|  | 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Create  &#60; username &#62;  user account in “contoso.com”;| 
| | Join client computer in “contoso.com” domain;| 
| | Join File Server in “kerb.com” domain;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ with PA_PAC_REQUEST, PA_PAC_OPTIONS (claim, forward to full dc), with Canonicalize option,| 
| | KDC returns TGS_REP| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket with PAC and claim| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#### <a name="_Toc430273955"/>FAST

#####Target Realm support FAST

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Realm_support_FAST| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports AES encryption for servers or services.| 
|  **Prerequisites**| Set attribute msDS-SupportedEncryptionTypes to 0x1001F for the krbtgt/ &#60; targetrealm &#62;  object in AD;| 
| | Set domainControllerFunctionality  &#62; = 3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | sname: krbtgt/ &#60; target realm &#62;  (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x1001F (isKile1)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | Otherwise:| 
| | If the account is krbtgt, and domainControllerFunctionality returns greater than or equal to 3: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), the padata-value set to 0x1F, the Claims-supported bit if claims is supported, and the FAST-supported bit if FAST is supported. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#####Armor TGT sname

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Armor_TGT_sname| 
|  **Priority**| P0 | 
|  **Description** |  | 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ for computer account| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends AS_REQ to the user’s realm| 
| | KDC return AS_REP| 
| | ticket: computer’s referral TGT to the user’s realm| 
| | Client sends AS_REQ for user account| 
| | KDC returns KRB_ERROR| 
| | Client sends FAST AS_REQ| 
| | Armor-type: FX_FAST_ARMOR_AP_REQUEST (1)| 
| | Armor-value: AP-REQ:| 
| | Armor Ticket: the ticket obtained in step 6| 
| | sname: target realm (isCommon1)| 
| | KDC returns FAST AS_REP| 
|  **Requirements covered**| isCommon:| 
| | The server name field of the armor ticket MUST identify the TGS of the target realm. ([RFC6113] section 5.4.1.1);| 
|  **Cleanup**|  | 

#####User in another domain

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| User_in_another_domain| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “kerb.com” domain;| 
| | Join AP to the “kerb.com” domain;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set the service account as trusted for delegation.| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/kerb.com object in AD;| 
| | Set registry key ClaimsCompIdFASTSupport to 2 on all the KDCs (don’t use group policy).| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ for device TGT to user’s domain| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-FX-FAST| 
| | Client sends AS_REQ for device TGT to user’s domain| 
| | KDC return AS_REP| 
| | ticket: referral computer’s TGT (isKile1)| 
| | Client sends FAST AS_REQ for user TGT to user’s domain| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-FX-FAST[PA-ENC-TIMESTAMP, PA-ETYPE-INFO2, PA-PK-AS-REQ, PA-PK-AS-REP_OLD]| 
| | Client sends FAST AS_REQ| 
| | armor ticket: referral computer’s TGT| 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT| 
| | Client send FAST TGS_REQ| 
| | KDC returns FAST TGS_REP| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If the client does not have a TGT for the realm and is creating a:| 
| | AS-REQ: the client SHOULD obtain a TGT for the computer principal from the user’s principal’s domain. ([MS-KILE] section 3.2.5.3);| 
|  **Cleanup**|  | 

#####Resource in another domain

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Resource_in_another_domain| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Join AP to the “kerb.com” domain;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set the service account as trusted for delegation.| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/kerb.com object in AD;| 
| | Purge ticket cache on client computer;| 
| | Set registry key ClaimsCompIdFASTSupport to 2 on all the KDCs (don’t use group policy).| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ for device to user’s domain| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ for device to user’s domain| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends FAST AS_REQ for user| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends FAST AS_REQ for user| 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT| 
| | Client sends AS_REQ for device to resource’s domain| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ for device to resource’s domain| 
| | KDC return AS_REP| 
| | ticket: referral computer’s TGT| 
| | Client send FAST AS_REQ for referral user TGT to resource’s domain| 
| | KDC returns FAST AS_REP| 
| | ticket: referral user TGT to resource’s domain (isKile1)| 
| | Client send FAST TGS_REQ for service ticket to resource’s domain| 
| | Armor field: no| 
| | Ticket in PA-TGS-REQ: referral user TGT (implicit armor)| 
| | KDC returns FAST TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | File server returns SMB3 session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If the client does not have a TGT for the realm and is creating a TGS-REQ, the client SHOULD obtain a referral TGT for the user principal for the target domain. ([MS-KILE] section 3.2.5.3);| 
|  **Cleanup**|  | 

#### <a name="_Toc430273956"/>Compound Identity

#####Resource in another domain

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Resource_in_another_domain| 
|  **Priority**| P0 (BVT)| 
|  **Description** |  | 
|  **Prerequisites**| Join client computer to the “contoso.com” domain;| 
| | Create  &#60; username &#62;  user account in the “contoso.com” domain;| 
| | Join AP to the “kerb.com” domain;| 
| | Set Delegate=TRUE for client’s security context;| 
| | Set the service account as trusted for delegation.| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/contoso.com object in AD;| 
| | Set attribute msDS-SupportedEncryptionTypes to 0x7001C for the krbtgt/kerb.com object in AD;| 
| | Purge ticket cache on client computer;| 
| | Set registry key ClaimsCompIdFASTSupport to 2 on all the KDCs (don’t use group policy).| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ for device to user’s domain| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ for device to user’s domain| 
| | KDC return AS_REP| 
| | ticket: computer’s TGT| 
| | Client sends FAST AS_REQ for user| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | Client sends FAST AS_REQ for user| 
| | KDC return FAST AS_REP| 
| | ticket: user’s TGT| 
| | Client sends AS_REQ for device to resource’s domain| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ for device to resource’s domain| 
| | KDC return AS_REP| 
| | ticket: referral computer’s TGT (isKile1)| 
| | Client send FAST AS_REQ for referral user TGT to resource’s domain| 
| | KDC returns FAST AS_REP| 
| | ticket: referral user TGT to resource’s domain (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (Compound Identity bit set) (isKile2)| 
| | Client send Compound Identity TGS_REQ for service ticket to resource’s domain| 
| | Armor ticket: referral computer TGT (explicit armor)| 
| | Ticket in PA-TGS-REQ: referral user TGT| 
| | With same key version numbers| 
| | KDC returns FAST TGS_REP| 
| | ticket: service ticket| 
| | Client sends AP session setup request (AP-REQ wrapped in GSS formatting)| 
| | File server returns SMB3 session setup response (AP-REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | If the client does not have a TGT for the realm and is creating a Compound identity TGS-REQ, the client SHOULD obtain a user and computer principal TGT for the target domain with the same key version numbers. ([MS-KILE] section 3.2.5.3);| 
| | If the application server’s realm TGT's PA-SUPPORTED-ENCTYPES Compound Identity bit is set, the Kerberos client SHOULD send a compound identity TGS-REQ by using FAST with explicit armoring, using the computer’s TGT. ([MS-KILE] section 3.2.5.6)| 
|  **Cleanup**|  | 

### <a name="_Toc430273957"/>KRB_ERROR Test

#### <a name="_Toc430273958"/>KRB_ERROR for AS_REQ Test

#####KDC_ERR_C_PRINCIPAL_UNKNOWN

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_client_principal_unknown| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test KDC when it cannot find the requested principal in its database.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | cname: use a principal name that does not exist in the KDC’s database| 
| | KDC returns KDC_ERR_C_PRINCIPAL_UNKNOWN (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | If the requested client principal named in the request is unknown because it doesn’t exist in the KDC’s principal database, then an error message with a KDC_ERR_C_PRINCIPAL_UNKNOWN is returned. ([RFC4120] section 3.1.3)| 
|  **Cleanup**|  | 

#####KDC_ERR_PREAUTH_REQUIRED

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_preauthentication_required| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test KDC when pre-authentication is required.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data| 
| | KDC returns KDC_ERR_PREAUTH_REQUIRED (isCommon1)| 
| | e-data:| 
| | padata[]: check existence (possibly, PA-ETYPE-INFO/PA-ETYPE-INFO2)| 
|  **Requirements covered**| isCommon:| 
| | If pre-authentication is required, but was not present in the request, an error message with the code KDC_ERR_PREAUTH_REQUIRED is returned, and a METHOD-DATA object will be stored in the e-data field of the KRB-ERROR message to specify which pre-authentication mechanisms are acceptable. Usually this will include PA-ETYPE-INFO and/or PA-ETYPE-INFO2 elements as described below. ([RFC4120] section 3.1.3)| 
| | If the KDC does not receive the required pre-authentication message in the AS exchange, an error MUST be returned to the client. The exact error depends on what pre-authentication types were supplied. ([MS-KILE] section 3.1.5.1)| 
|  **Cleanup**|  | 

#####KDC_ERR_PREAUTH_FAILED

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_preauthentication_failed| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test KDC when pre-authentication is required and failed.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data| 
| | KDC returns KDC_ERR_PREAUTH_REQUIRED (isCommon1)| 
| | e-data:| 
| | padata[]: PA-ENC-TIMESTAMP, PA-ETYPE-INFO/PA-ETYPE-INFO2| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP (encrypted in wrong password)| 
| | KDC returns KDC_ERR_PREAUTH_FAILED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | If required to do so, the server pre-authenticates the request, and if the pre-authentication check fails, an error message with the code KDC_ERR_PREAUTH_FAILED is returned. ([RFC4120] section 3.1.3)| 
|  **Cleanup**|  | 

#####KDC_ERR_ETYPE_NOSUPP

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_encryption_type_not_supported| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test KDC when encryption type requested by the client is not supported by the KDC.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | padata: no pre-authentication data| 
| | etype: a list of encryption types the KDC does not support| 
| | KDC returns KDC_ERR_ETYPE_NOSUPP (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | If the server cannot accommodate any encryption type requested by the client, an error message with the code KDC_ERR_ETYPE_NOSUPP is returned. ([RFC4120] section 3.1.3)| 
|  **Cleanup**|  | 

#### <a name="_Toc430273959"/>KRB_ERROR for TGS_REQ Test

#####KRB_ERR_FIELD_TOOLONG

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KRB_ERR_FIELD_TOOLONG| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test KDC when Field is too long for this implementation.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | Don’t send KRB_TGS_REQ message type in TGS_REQ message, send KRB_AS_REQ type of message.| 
| | KDC returns KRB_ERR_FIELD_TOOLONG (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | Each request (KRB_KDC_REQ) and response (KRB_KDC_REP or KRB_ERROR)sent over the TCP stream is preceded by the length of the request as 4 octets in network byte order. The high bit of the length is reserved for future expansion and MUST currently be set to zero. If a KDC that does not understand how to interpret a set high bit of the length encoding receives a request with the high order bit of the length set, it MUST return a KRB-ERROR message with the error KRB_ERR_FIELD_TOOLONG and MUST close the TCP stream. ([RFC 4120] section 7.2.2);| 
|  **Cleanup**|  | 

#### <a name="_Toc430273960"/>KRB_ERROR for AP_REQ Test

#####KRB_AP_ERR_MSG_TYPE

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_Message_Type| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test the AP when it is receiving a Kerberos message that is not a KRB_AP_REQ type.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Don’t send KRB_AP_REQ message type in AP_REQ message, send in another message type (e.g. TGS_REQ type).| 
| | Application server returns KRB_AP_ERR_MSG_TYPE (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | If the message type is not KRB_AP_REQ, the server returns the KRB_AP_ERR_MSG_TYPE error. ([RFC4120] section 3.2.3)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_BADINTEGRITY-ticket

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_Bad_Integrity-ticket| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test the AP when it finds out that the ticket has been modified in the KRB_AP_REQ.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP | 
| | ticket:| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ticket: make some changes to the ticket’s encryption part.| 
| |    10. Application server returns KRB_AP_ERR_BAD_INTEGRITY (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | If the decryption routines detect a modification of the ticket (each encryption system MUST provide safeguards to detect modified cipher text), the KRB_AP_ERR_BAD_INTEGRITY error is returned (chances are good that different keys were used to encrypt and decrypt). ([RFC4120] section 3.2.3)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_BADINTEGRITY-authenticator

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_Bad_Integrity-authenticator| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test the AP when it finds out that the authenticator has been modified in the KRB_AP_REQ.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authenticator: make some changes to the athenticator’s encryption part.| 
| |    10. Application server returns KRB_AP_ERR_BAD_INTEGRITY (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The authenticator is decrypted using the session key extracted from the decrypted ticket. If decryption shows that it has been modified, the KRB_AP_ERR_BAD_INTEGRITY error is returned. ([RFC4120] section 3.2.3)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_BADMATCH

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_Bad_Match| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test the AP when it finds out that the name and realm of the client from the ticket are not the same with those from the authenticator in the KRB_AP_REQ.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ticket: change crealm and cname in the encrypted part.| 
| | authenticator: change crealm and cname in the encrypted part.| 
| | Application server returns KRB_AP_ERR_BADMATCH (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | The name and realm of the client from the ticket are compared against the same fields in the authenticator. If they don’t match, the KRB_AP_ERR_BADMATCH error is returned; normally this is caused by a client error or an attempt attack. ([RFC4120] section 3.2.3)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_SKEW

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_Clock_Skew| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test the AP when it finds out that the local (server) time is out of the allowable time skew comparing to the client time represented in by the authenticator.| 
|  **Prerequisites**| Need Client, KDC, Application Server synchronize time;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authenticator:| 
| | ctime: current time + MaxClockSkew + 1 or current time – MaxClockskew – 1| 
| | Application server returns KRB_AP_ERR_SKEW (isCommon1)| 
| | e-data:| 
| | data-type: KERB_AP_ERR_TYPE_SKEW_RECOVERY| 
| | data-value: NULL (isKile1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authenticator:| 
| | ctime: use the time in KRB-ERROR message| 
| | Application Server returns AP_REP| 
| | Session setup success. (isKile1)| 
|  **Requirements covered**| isKile:| 
| | When the client receives a KRB_AP_ERR_SKEW error with a KERB-ERROR-DATA structure in the e-data field of the KRB-ERROR message, the client SHOULD retry the AP-REQ using the time in the KRB-ERROR message to create the authenticator. ([MS-KILE] section 3.2.5.7);| 
| | When clock skew errors occur during AP exchanges, the application server SHOULD attempt a clock skew recovery by returning a KRB_AP_ERR_SKEW error containing a KERB-ERROR-DATA structure in the e-data field of the KRB-ERROR message. ([MS-KILE] section 3.4.5)| 
| | isCommon:| 
| | If the local (server) time and the client time in the authenticator differ by more than the allowable clock skew (e.g., 5 minutes), the KRB_AP_ERR_SKEW error is returned. ([RFC4120] section 3.2.3)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_REPEAT

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Error_repeated_authenticator| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to test if AP supports replay cache.| 
|  **Prerequisites**| Client local variable ReplayDetect enabled;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authenticator: record this authenticator| 
| | GSS_C_REPLAY_FLAG| 
| | Application Server returns AP_REP| 
| | Session setup success. (isCommon1)| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | authenticator: send the same authenticator as step 9| 
| | GSS_C_REPLAY_FLAG| 
| | Application Server returns KRB_AP_ERR_REPEAT (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | Unless the application server provides its own suitable means to protect against replay (for example, a challenge-response sequence initiated by the server after authentication, or use of a server-generated encryption subkey), the server MUST utilize a replay cache to remember any authenticator presented within the allowable clock skew… if a matching tuple is found, the KRB_AP_ERR_REPEAT error is returned. ([RFC4120] section 3.2.3);| 
| | KILE MUST implement a replay cache regardless of the application server replay functionality. ([MS-KILE] section 3.1.1.1);| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_MODIFIED-ticket

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KRB_AP_ERR_MODIFIED| 
|  **Priority**| P1| 
|  **Description** | This case is designed to verify if application server can detect ticket modification.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Ticket: Re-encrypt the ticket with a “WrongPassword”| 
| | Application server returns KRB_AP_ERR_MODIFIED (isCommon)| 
|  **Requirements covered**| isCommon:| 
| | Finally, the checksum is computed over the data and control information, and if it doesn't match the received checksum, a KRB_AP_ERR_MODIFIED error is generated. ([RFC4120]section 3.4.2);| 
| | If the decryption routines detect a modification of the ticket, the KRB_AP_ERR_MODIFIED error message is returned. ([MS-KILE] section 3.4.5)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_MODIFIED-authenticator

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KRB_AP_ERR_MODIFIED| 
|  **Priority**| P1| 
|  **Description** | This case is designed to verify if application server can detect authenticator modification.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | Authenticator: Encrypt authenticator with a random generated key instead of session key| 
| | Application server returns KRB_AP_ERR_MODIFIED (isCommon)| 
|  **Requirements covered**| isCommon:| 
| | Finally, the checksum is computed over the data and control information, and if it doesn't match the received checksum, a KRB_AP_ERR_MODIFIED error is generated. ([RFC4120]section 3.4.2);| 
| | If decryption shows that the authenticator has been modified, the KRB_AP_ERR_MODIFIED error message is returned. ([MS-KILE] section 3.4.5)| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_TKT_EXPIRED

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KRB_AP_ERR_TKT_EXPIRED| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test AP when ticket is expired.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ticket:| 
| | endtime: current time – MaxClockskew – 1| 
| | Application server returns KRB_AP_ERR_TKT_EXPIRED (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | if the current time is later than end time by more than the allowable clock skew, the KRB_AP_ERR_TKT_EXPIRED error is returned. ([RFC 4120] section 3.2.3);| 
|  **Cleanup**|  | 

#####KRB_AP_ERR_TKT_NYV

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| KRB_AP_ERR_TKT_NYV| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test AP when ticket is not yet valid.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AP Negotiate request to Application Server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | ticket:| 
| | starttime: current time + MaxClockskew + 1| 
| | Application server returns KRB_AP_ERR_TKT_NYV (isCommon1)| 
|  **Requirements covered**| isCommon:| 
| | If the starttime is later than the current time by more than the allowable clock skew (10 minutes), the KRB_AP_ERR_TKT_NYV error is returned. ([RFC 4120] section 3.2.3);| 
|  **Cleanup**|  | 

### <a name="_Toc430274036"/>KKDCP Test

#### <a name="_Toc430274037"/>Negative

#####Target domain not present

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Domain_not_present| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test KPS when Target Domain is not present.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| 1. Client sends AS_REQ using KKDCPClient| 
| |   TargetDomain = null| 
| | 2. KKDCPError.STATUS_NO_LOGON_SERVERS returned(isKKDCP1)| 
|  **Requirements covered**| isKKDCP:| 
| | If target-domain is not present, return ERROR_BAD_FORMAT. Validate that the KDC_PROXY_MESSAGE.kerb-message is a well-formed Kerberos message. If not, then the KKDCP server SHOULD drop the connection and stop processing. ([MS-KKDCP] section 3.2.5.1)| 
|  **Cleanup**|  | 

#####Target domain not valid

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Domain_not_valid| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to test KPS when Target Domain is not valid.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| 1. Client sends AS_REQ using KKDCPClient| 
| |   TargetDomain = InvalidDomain| 
| | 2. KKDCPError.STATUS_NO_LOGON_SERVERS returned(isKKDCP1)| 
|  **Requirements covered**| isKKDCP:| 
| | Before the KKDCP server can send a Kerberos message, it MUST discover the KDC to which the message will be sent. The KKDCP server SHOULD perform the equivalent of calling DsrGetDcNameEx2 where _DomainName_ is _TargetDomain_. ([MS-KKDCP] section 3.2.5.1)| 
|  **Cleanup**|  | 

#### <a name="_Toc430274038"/>Kpassword

#####ChangePasswordSuccess

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Change_Password_Success| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify Kpassword when the password change succeeds.| 
|  **Prerequisites**| 1. Set up KKDCP server | 
| | 2. Set UseProxy=true in ptfconfig file| 
| | 3. Set password group policy MinimumPasswordAge=0 | 
| | 4. Set password group policy PasswordHistorySize=0| 
|  **Test Execution Steps**| 1. Client sends Kpassword Request| 
| |   port:464| 
| |   sname: kadmin/changepw| 
| |   version: 0x0001| 
| | 2. KKDCP returns Kpassword Response with the Result Code as    KRB5_KPASSWD_SUCCESS| 
|  **Requirements covered**| [RFC-3244] section 2| 
|  **Cleanup**|  | 

#####ChangePasswordError

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Change_Password_Error| 
|  **Priority**| P1| 
|  **Description** | This test case is designed to verify Kpassword when the password change fails.| 
|  **Prerequisites**| 1. Set up KKDCP server | 
| | 2. Set UseProxy=true in ptfconfig file| 
| | 3. Set password group policy MinimumPasswordAge=0 | 
| | 4. Set password group policy PasswordHistorySize=0| 
|  **Test Execution Steps**| 1. Client sends Kpassword Request with new password to be “123” which doesn’t meet the complexity requirements| 
| |   port:464| 
| |   sname: kadmin/changepw| 
| |   version: 0x0001| 
| | 2. KKDCP returns Kpassword Response with the Result Code as  KRB5_KPASSWD_SOFTERROR| 
| | 3. Client sends malformed Kpassword Request with protocol version   to be 0xff80| 
| |   port:464| 
| |   sname: kadmin/changepw| 
| |   version: 0xff80| 
| | 4. KKDCP returns Kpassword Response with the Result Code as KRB5_KPASSWD_MALFORMED| 
| | 5. Client sends Kpassword Request with KRB_PRIV encrypted wrongly| 
| |   port:464| 
| |   sname: kadmin/changepw| 
| |   version: 0x0001| 
| | 6. KKDCP returns Kpassword Response with the Result Code as KRB5_KPASSWD_AUTHERROR| 
| | 7. Client sends Kpassword Request with user who has no access to change the password| 
| |   port:464| 
| |   sname: kadmin/changepw| 
| |   version: 0x0001| 
| | 8. KKDCP returns Kpassword Response with the Result Code as KRB5_KPASSWD_ACCESSDENIED| 
|  **Requirements covered**| [RFC-3244] section 2| 
|  **Cleanup**|  | 

### <a name="_Toc430274039"/>Change Network Topology

#### <a name="_Toc430274040"/>RODC

#####Key Version Number for RODC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Key_Version_Number_RODC| 
|  **Priority**| P2| 
|  **Description** | This test case is designed to test if key version numbers are supported for RODCs.| 
|  **Prerequisites**| Setup 2 RODC in one domain;| 
|  **Test Execution Steps**| Client sends AS_REQ for user TGT (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ for user TGT (RODC 1)| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user’s TGT| 
| | key: session key| 
| | kvno: kvno1 (isKile1)| 
| | Client sends AS_REQ for user TGT (RODC 2)| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP| 
| | ticket: user’s TGT| 
| | key: session key| 
| | kvno: kvno2 (isKile1)| 
|  **Requirements covered**| isKile:| 
| | KILE supports key version numbers for RODC. Each RODC will have a different key version number. This allows the domain controller to distinguish between keys that are issued to different RODCs.| 
| | The kvno consists of 32 bits. The first 16 bits, including the most significant bit, are an unsigned 16-bit number which SHOULD identify the RODC. The remaning 16 bits SHOULD be the version number of the key. ([MS-KILE] section 3.1.5.8)| 
|  **Cleanup**|  | 

#####Branch Aware and Forward to Full DC

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Branch_Aware_and_Forward_to_Full_DC| 
|  **Priority**| P2| 
|  **Description** | This test case is designed to verify if the KDC supports Branch Aware.| 
|  **Prerequisites**|  | 
|  **Test Execution Steps**| Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | Client sends AS_REQ| 
| | KDC returns AS_REP| 
| | Client sends TGS_REQ to a RODC| 
| | pa-data: PA-PAC-OPTIONS (branch aware) (isKile1)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_S_PRINCIPAL_UNKNOWN (7)| 
| | substatus: NTSTATUS_STATUS_NO_SECRETS (isKile2)| 
| | Client sends AS_REQ to a RODC| 
| | pa-data: PA-PAC-OPTIONS (forward to full DC) (isKile1)| 
| | RODC forward the AS_REQ to a Full DC| 
| | KDC (RODC) returns AS_REP| 
| | ticket: user TGT (check existence) (isKile1)| 
| | Client sends TGS_REQ to Full DC| 
| | KDC returns TGS_REP| 
| | ticket: service ticket (isKile1)| 
|  **Requirements covered**| isKile:| 
| | The Kerberos client SHOULD add a PA-PAC-OPTIONS [167] PA-DATA type with the Branch Aware bit set to the TGS REQ. If a server principal unknown with a substatus of NTSTATUS STATUS_NO_SECRETS message is returned, the client SHOULD send an AS-REQ adding a PA-PAC-OPTIONS PA-DATA type, with the Forward to Full DC bit set, to a full DC, and then send a new TGS_REQ using this TGT to the full DC. ([MS-KILE] section 3.2.5.6);| 
| | When a Key Distribution Center (KDC) which is a read-only domain controller (RODC) receives:| 
| | An AS-REQ message with a PA-PAC-OPTIONS PA-DATA type with the forward to full DC bit set, the RODC SHOULD forward the AS-REQ to a full DC.| 
| | A TGS-REQ message with a PA-PAC-OPTIONS PA-DATA type with the Branch Aware bit set, and the application server (SNAME) is not in its database, the RODC SHOULD return server principal unknown with the substatus message of NTSTATUS STATUS_NO_SECRETS. ([MS-KILE] section 3.3.5.4.7);| 
|  **Cleanup**|  | 

#### <a name="_Toc430274041"/>Proxy

#####Proxy

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Proxy| 
|  **Priority**| P2| 
|  **Description** | This test case is designed to verify if the system supports proxy;| 
|  **Prerequisites**| Setup 1 KDC in “contoso.com” but client could not reach;| 
| | Setup 1 proxy computer that client can reach;| 
| | Create a  &#60; username &#62;  user account in the “contoso.com” domain;| 
|  **Test Execution Steps**| Client sends AS_REQ for user TGT (no pre-authentication) using ProxyMessage()| 
| | KDC returns KRB_ERROR using ProxyMessage()| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ for user TGT using ProxyMessage()| 
| | padata: PA-ENC-TIMESTAMP| 
| | KDC returns AS_REP using ProxyMessage()| 
| | ticket: user’s TGT| 
| | Client sends TGS_REQ for service ticket using ProxyMessage()| 
| | KDC returns TGS_REP using ProxyMessage()| 
|  **Requirements covered**| isKile:| 
| | If the Kerberos client does not have network access to the KDC and KKDCP is supported, the Kerberos client SHOULD call ProxyMessage(). ([MS-KILE] section 3.2.5.1);| 
|  **Cleanup**|  | 

#### <a name="_Toc430274042"/>Pre Win8 KDC

######KDC does not support Claims

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| NetworkLogon_Claims_KDCNotSuppClaims| 
|  **Priority**| P2| 
|  **Description** |  | 
|  **Prerequisites**| Setup 2 KDCs in one domain, 1 Win8, 1 pre Win8, let client talk to the preWin8 first;| 
| | Set registry key ClaimsCompIdFASTSupport = 1 on the KDC (don’t use group policy).| 
|  **Test Execution Steps**| Client sends SMB3 Negotiate request to application server| 
| | Application server returns SMB3 Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS (claims)| 
| | KDC returns KRB_ERROR| 
| | error-code: KDC_ERR_PREAUTH_REQUIRED;| 
| | e-data: PA-ENC-TIMESTAMP| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS (claims)| 
| | KDC return AS_REP| 
| | padata: PA-PAC-OPTIONS (claims not set) (isKile1)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims set) (isKile1)| 
| | Client should locate a Win8 KDC| 
| | Client sends AS_REQ| 
| | padata: PA-ENC-TIMESTAMP, PA-PAC-REQUEST, PA-PAC-OPTIONS (claims)| 
| | KDC rturn AS_REP| 
| | padata: PA-PAC-OPTIONS (claims set) (isKile1)| 
| | ticket: user’s TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO(isKile1)| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims set) (isKile1)| 
|  **Requirements covered**| isKile:| 
| | The client will always include a PAC request Padata type when generating an AS-REQ message.| 
| | When sending the AS REQ, add a PA-PAC-OPTIONS [167] PADATA type with the Claims bit set in the AS REQ to request claims authorization data| 
| | When receiving the AS_REP, if the Claims bit is set in PA-SUPPORTED-ENCTYPES [165], and not set in PA-PAC-OPTIONS [167], the client SHOULD locate a DS_BEHAVIOR_WIN8 DC and go back to step 1.| 
| | ([MS-KILE] section 3.2.5.4)| 
|  **Cleanup**|  | 

######Single Realm use RC4

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Single Realm use RC4| 
|  **Priority**| P2| 
|  **Description** |  | 
|  **Prerequisites**| Set domainControllerFunctionality  &#60;  3.| 
| | Do not set ms-SupportedEncryptionTypes for the server or service’s account in AD;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to application server| 
| | Application server returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (165) = 0x7 (isKile1)| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | File server returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | (If UseDESOnly flag is not set):| 
| | If domainControllerFunctionality returns a value  &#60;  3: the KDC SHOULD, in the encrypted pa-auth data part of the AS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), and the padata-value set to 0x7. ([MS-KILE] section 3.3.5.3)| 
|  **Cleanup**|  | 

######Cross Realm use RC4

| &#32;| &#32; |
| -------------| ------------- |
|  **Test ID**| Target_Realm_use_RC4| 
|  **Priority**| P0 | 
|  **Description** | This test case is designed to verify if the KDC supports RC4 encryption for servers or services.| 
|  **Prerequisites**| Do not set attribute msDS-SupportedEncryptionTypes for the krbtgt/ &#60; targetrealm &#62;  object in AD;| 
| | Set domainControllerFunctionality  &#60;  3;| 
|  **Test Execution Steps**| Client sends AP Negotiate request to AP| 
| | AP returns AP Negotiate response (use Kerberos)| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral ticket| 
| | sname: krbtgt/ &#60; target realm &#62;  (isKile1)| 
| | enc-pa-data: PA-SUPPORTED-ENCTYPES (165)=0x7 (isKile1)| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | Client sends AP session setup request (AP_REQ wrapped in GSS formatting)| 
| | AP returns AP session setup response (AP_REP wrapped in GSS formatting)| 
|  **Requirements covered**| isKile:| 
| | Otherwise:| 
| | If the account is krbtgt, and domainControllerFunctionality returns a value  &#60;  3: the KDC SHOULD, in the encrypted pre-auth data part of the TGS-REP message, include PA-DATA with the padata-type set to PA-SUPPORTED-ENCTYPES (165), the padata-value set to 0x7. ([MS-KILE] section 3.3.5.4)| 
|  **Cleanup**|  | 

#### <a name="_Toc430274043"/>AZOD_Synthetic_Test 

#####DAC_Smb2_Possitive_AccessFile

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_AZOD_Synthetic_Test**| | 
|  **Test ID**| DAC_Smb2_Possitive_AccessFile| 
|  **Priority**| P1| 
|  **Description** | Verify the file server will allow read and write request to a file\directory when user has read and write permission| 
|  **Prerequisites**| Configure a user on DC for this test case.| 
| | Create a file share on file server, and give read/write permission to the user in this test case.| 
|  **Test Execution Steps**| The SMB2 client and the SMB2 server negotiate the authentication protocol using the SPNEGO [MS-SPNG] protocol.| 
| | Client sends KRB_AS_REQ to DC(no pre-authentication)| 
| | DC returns KRB_ERROR| 
| | Client sends KRB_AS_REQ| 
| | KDC returns KRB_AS_REP| 
| | ticket: user TGT| 
| | Client sends KRB_TGS_REQ| 
| | KDC returns TGS response| 
| | ticket: service ticket| 
| | Client sends a SMB2 SESSION_SETUP request to the file server| 
| | The file server sends an SMB2 SESSION_SETUP response message to the client. | 
| | Client completes TREE_CONNECT phase with file server.| 
| | Client sends SMB2 CREATE request, whose ShareAccess field contains FILE_SHARE_READ and FILE_SHARE_WRITE.| 
| | Expect file server to reply a SMB2 CREATE Response| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####DAC_Smb2_Negative_AccessFile

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_AZOD_Synthetic_Test**| | 
|  **Test ID**| DAC_Smb2_Negative_AccessFile| 
|  **Priority**| P1| 
|  **Description** | Verify the file server will deny read and write request to a file\directory when user doesn’t have read and write permission| 
|  **Prerequisites**| Configure a user on DC for this test case.| 
| | Create a file share on file server, but don’t give read/write permission to the user in this test case.| 
|  **Test Execution Steps**| The SMB2 client and the SMB2 server negotiate the authentication protocol using the SPNEGO [MS-SPNG] protocol.| 
| | Client sends KRB_AS_REQ to DC(no pre-authentication)| 
| | DC returns KRB_ERROR| 
| | Client sends KRB_AS_REQ| 
| | KDC returns KRB_AS_REP| 
| | ticket: user TGT| 
| | Client sends KRB_TGS_REQ| 
| | KDC returns TGS response| 
| | ticket: service ticket| 
| | Client sends a SMB2 SESSION_SETUP request to the file server| 
| | The file server sends an SMB2 SESSION_SETUP response message to the client. | 
| | Client completes TREE_CONNECT phase with file server.| 
| | Client sends SMB2 CREATE request, whose ShareAccess field contains FILE_SHARE_READ and FILE_SHARE_WRITE.| 
| | Expect file server to reply a SMB2 ERROR Response with STATUS_ACCESS_DENIED.| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####CBAC_Smb2_Possitive_AccessFile

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_AZOD_Synthetic_Test**| | 
|  **Test ID**| CBAC_Smb2_Possitive_AccessFile| 
|  **Priority**| P1| 
|  **Description** | Verify the CBAC based file server will allow read and write request to a file\directory when user has read and write permission| 
|  **Prerequisites**| Defined user claim on DC.| 
| | Defined CAR, CAP and apply it for File system | 
| | On file server, create a share folder and apply the defined CAP| 
| | On DC, create a User1, and set its property to fit the CAP | 
|  **Test Execution Steps**| The SMB2 client and the SMB2 server negotiate the authentication protocol using the SPNEGO [MS-SPNG] protocol.| 
| | Client sends KRB_AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends KRB_AS_REQ| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS(claims, forward to full DC)| 
| | KDC returns KRB_AS_REP, verify:| 
| | ticket: user TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client sends KRB_TGS_REQ| 
| | KDC returns TGS response| 
| | ticket: service ticket| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client sends a SESSION_SETUP request to the file server| 
| | The file server sends an SMB2 SESSION_SETUP response message to the client.| 
| | Client completes SMB2 TREE_CONNECT phase with file server.| 
| | Client sends SMB2 CREATE request, whose ShareAccess field contains FILE_SHARE_READ and FILE_SHARE_WRITE.| 
| | Expect file server to reply a SMB2 CREATE Response| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####CBAC_Smb2_Negative_AccessFile

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_AZOD_Synthetic_Test**| | 
|  **Test ID**| CBAC_Smb2_Negative_AccessFile| 
|  **Priority**| P1| 
|  **Description** | Verify the CBAC based file server will deny read and write request to a file\directory when user doesn’t have read and write permission| 
|  **Prerequisites**| Defined user claim on DC.| 
| | Defined CAR, CAP and apply it for File system | 
| | On file server, create a share folder and apply the defined CAP| 
| | On DC, create a User1, but set its property not fit the CAP | 
|  **Test Execution Steps**| The SMB2 client and the SMB2 server negotiate the authentication protocol using the SPNEGO [MS-SPNG] protocol.| 
| | Client sends KRB_AS_REQ (no pre-authentication)| 
| | KDC returns KRB_ERROR| 
| | Client sends KRB_AS_REQ| 
| | padata: PA-PAC-REQUEST, PA-PAC-OPTIONS(claims, forward to full DC)| 
| | KDC returns KRB_AS_REP, verify:| 
| | ticket: user TGT| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client sends KRB_TGS_REQ| 
| | KDC returns TGS response| 
| | ticket: service ticket| 
| | authorization-data: PAC_CLIENT_CLAIMS_INFO| 
| | enc-padata: PA-SUPPORTED-ENCTYPES (claims)| 
| | Client sends a SESSION_SETUP request to the file server| 
| | The file server sends an SMB2 SESSION_SETUP response message to the client.| 
| | Client completes SMB2 TREE_CONNECT phase with file server.| 
| | Client sends SMB2 CREATE request, whose ShareAccess field contains FILE_SHARE_READ and FILE_SHARE_WRITE.| 
| | Expect file server to reply a SMB2 ERROR Response with STATUS_ACCESS_DENIED.| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####CrossRealm_Smb2_Possitive_AccessFile

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_AZOD_Synthetic_Test**| | 
|  **Test ID**| CrossRealm_Smb2_Possitive_AccessFile| 
|  **Priority**| P1| 
|  **Description** | Verify the CBAC based file server will allow read and write request from a trusted domain when user’s claim fit group policy after transformed| 
|  **Prerequisites**| Create trusted relationship between local domain KDC and trusted domain KDC | 
| | Defined user claim on local KDC.| 
| | Defined user claim on trusted KDC, and create Claim transform policy| 
| | Defined CAR, CAP and apply it for File system on trusted KDC.| 
| | On file server in trusted domain, create a share folder and apply the defined CAP| 
| | On local DC, create a User1, and set its property to fit the CAP| 
|  **Test Execution Steps**| The SMB2 client and the SMB2 server negotiate the authentication protocol using the SPNEGO [MS-SPNG] protocol.| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral user TGT to the resource’s domain| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket| 
| | Client sends a SESSION_SETUP request to the file server| 
| | The file server sends an SMB2 SESSION_SETUP response message to the client.| 
| | Client completes SMB2 TREE_CONNECT phase with file server.| 
| | Client sends SMB2 CREATE request, whose ShareAccess field contains FILE_SHARE_READ and FILE_SHARE_WRITE.| 
| | Expect file server to reply a SMB2 CREATE Response| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 

#####CrossRealm_Smb2_Negative_AccessFile 

| &#32;| &#32; |
| -------------| ------------- |
|  **S9_AZOD_Synthetic_Test**| | 
|  **Test ID**| CrossRealm_Smb2_Negative_AccessFile| 
|  **Priority**| P1| 
|  **Description** | Verify the CBAC based file server will deny read and write request from a trusted domain when user’s claim doesn’t fit group policy after transformed| 
|  **Prerequisites**| Create trusted relationship between local domain KDC and trusted domain KDC | 
| | Defined user claim on local KDC.| 
| | Defined user claim on trusted KDC, and create Claim transform policy| 
| | Defined CAR, CAP and apply it for File system on trusted KDC.| 
| | On file server in trusted domain, create a share folder and apply the defined CAP| 
| | On local DC, create a User1, but set its property not fit the CAP| 
|  **Test Execution Steps**| The SMB2 client and the SMB2 server negotiate the authentication protocol using the SPNEGO [MS-SPNG] protocol.| 
| | Client sends AS_REQ| 
| | KDC returns KRB_ERROR| 
| | Client sends AS_REQ| 
| | KDC return AS_REP| 
| | ticket: user TGT| 
| | Client send TGS_REQ| 
| | KDC returns TGS_REP| 
| | ticket: referral user TGT to the resource’s domain| 
| | Client send referral TGS_REQ| 
| | KDC returns referral TGS_REP| 
| | ticket: service ticket| 
| | Client sends a SESSION_SETUP request to the file server| 
| | The file server sends an SMB2 SESSION_SETUP response message to the client.| 
| | Client completes SMB2 TREE_CONNECT phase with file server.| 
| | Client sends SMB2 CREATE request, whose ShareAccess field contains FILE_SHARE_READ and FILE_SHARE_WRITE.| 
| | Expect file server to reply a SMB2 ERROR Response with STATUS_ACCESS_DENIED.| 
|  **Requirements covered**|  | 
|  **Cleanup**|  | 