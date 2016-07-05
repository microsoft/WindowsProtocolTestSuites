Following steps will guide you configure the file server quickly for an interoperability testing during event.  
Note: 

- Enable local Guest user on file server (SUT).
- For all the shares will be created below, need to grant permissions as following:
	Grant **Full Control** Permissions to admin account
	Grant Permissions **without DELETE** and **GENERIC_ALL** to nonadmin account


* There’re some default values for test settings, you could just follow them or use your own defined.


| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
|  Test Area	|  							SUT configuration									|
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
| MS-SMB2	| -Create SMB2 share for MS-SMB2 basic and MS-FSRVP test:_______________(default: SMBBasic).							|
| 		| -If SUT supports hosting Continuous Availability (CA) share:											|
| 		| 	Create a CA share:_______________(default: SMBClustered)										|
| 		| -If SUT supports encryption：															|
| 		| 	Create an encrypted share:_______________(default: SMBEncrypted) 									|
| 		| -If file system supports symbolic link: 													|
| 		| 	Create a symbolic link:_______________(default: Symboliclink) under basic share (e.g.SMBBasic) linking to basic share			|
| 		| 	Create a sub folder_______________(default: Sub) under basic share (e.g. SMBBasic)							|
| 		| 	Create a symbolic link ___________(default: Symboliclink2) under sub folder (e.g. SMBBasic\Sub) 					|
| 		| 	Link the symbolic link (default: Symboliclink2) to basic share (e.g. SMBBasic) 								|
| 		| -If SHI1005_FLAGS_FORCE_LEVELII_OPLOCK is applicable:												|
| 		| 	Create a share_______________(default: ShareForceLevel2) and set SHI1005_FLAGS_FORCE_LEVELII_OPLOCK in SHI1005_flags 			|
| 		| -If SUT supports SMB2_CREATE_APP_INSTANCE_ID:													|
|		| 	Create a share ___________(default: SameWithSMBBasic) pointing to the same local path of basic share (e.g. SMBBasic)			|
| 		| 	Create a share _________(default: DifferentFromSMBBasic) pointing to a different local path (e.g. C:\DifferentFromSMBBasic).		|
| 		| -If SUT supports ReFS file system: 														|
| 		| 	Create a share _______________(default: SMBReFSShare) on ReFS volume. 									|
| 		| -If SUT supports asymmetric share:														|
| 		| 	Create a share on optimum node_______________(default: SMBClustered)									|
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
| Auth		| -If you like to test Kerberos Authentication: 												|
| 		| 	Set the computer account password of SUT to ___________(default: Password04!)								| 	
| 		| -If you like to test Share Permission:													|
| 		| 	Create a share ___________(default: AzShare) with permission:										|
| 		| 	NTFS Permission:Allow Everyone, Share Permission: Allow Domain Admins 									|
| 		| -If you like to test Folder Permission:													|
| 		| 	Create a share ___________(default: AzFolder) with permission:										|
| 		| 	NTFS Permission:Allow Domain Admins, Share Permission: Allow Everyone									|
| 		| -If you like to test File Permission: 													|
| 		| 	Create a share ___________(default: AzFile) with permission:										|
| 		| 	NTFS Permission:Allow Domain Admins, Share Permission: Allow Everyone 									|
| 		| -If you like to test Claim-Based Access Control (CBAC): 											|
| 		| 	Create a share ___________(default: AzCBAC) with permission:										|
| 		| 	NTFS Permission:Allow Everyone, Share Permission: Allow Everyone 									|
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
| MS-DFSC	| -Create a SMB2 share for DFSC test ___________(default: FileShare) 										|
| 		| -Create DFS namespaces, two Stand-alone namespaces ___________(default: SMBDfs and Standalone) 						|
| 		| 	Assume your server name is SUT_NAME below: 												|
| 		| 		Root share for SMBDfs: \\SUT_NAME\SMBDfs 											|
| 		| 		Root share for Standalone: \\SUT_NAME\Standalone 										|
| 		| -Add one folder ___________ (default: SMBDfsLink)to 1st namespace (e.g. SMBDfs) and set link target to SMB2 share \\SUT_NAME\SMBBasic		|
| 		| -Add two folders to 2nd namespace (e.g. Standalone)												| 
| 		| 	One is DFSLink, link target is \\SUT_NAME\FileShare											| 
| 		| 	The other is Interlink, link target is \\SUT_NAME\SMBDfs\SMBDfsLink									| 
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
| MS-FSA	| -Create a SMB2 share for FSA test ___________ (default: FileShare)										| 
| 		| -Create a file in the SMB2 share (e.g.FileShare), file name___________(default: ExistingFile.txt)						| 
| 		| -Create a folder___________(default: ExistingFoler) in the SMB2 share (e.g. FileShare)							| 
| 		| -Create a mountpoint__________________(default: mountpoint) in the share_(e.g. FileShare) mounting to the volume				| 
| 		| -Create a symbolic link file ________________(default: link.txt) in the share(e.g. FileShare) linking to the file (default: ExistingFile.txt)	| 
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
| MS-RSVD	|- Set the ptfconfig property: ShareContainingSharedVHD as the path of a share which contains a VHD file.					| 
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
| MS-SQOS	| -Create a virtual hard disk file in your share (default: \\scaleoutfs\SMBClustered\sqos.vhdx)							|					
| 		| -Set the ptfconfig property: SqosVHDFullPath as the full path of the vhd file (default: \\scaleoutfs\SMBClustered\sqos.vhdx) 			| 
| 		| -Create a new policy (with MinimumIoRate: 100 and MaximumIoRate: 200 and MaximumBandwidth: 1638400)						|
| 		| -Set the ptfconfig property: SqosPolicyId as the policy id.											| 
| 		| Note: If MaximumBandwidth is not supported yet, you can ignore its configuration.								| 
| -------------	| ----------------------------------------------------------------------------------------------------------------------------------------------|
