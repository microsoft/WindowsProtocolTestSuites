// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.AccessControl;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// The ace header to be inherited
    /// </summary>
    public abstract class _AceHeader
    {
        /// <summary>
        /// An unsigned 8-bit integer that specifies a set of ACE type-specific control flags
        /// </summary>
        public ACE_FLAGS AceFlags { get; set; }

        /// <summary>
        /// An unsigned 8-bit integer that specifies the ACE types
        /// </summary>
        public ACE_TYPE AceType { get; }

        /// <summary>
        /// An unsigned 16-bit integer that specifies the size, in bytes, of the ACE
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="offset">The offset in the binary</param>
        public _AceHeader(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            if (offset < 0 || offset > binary.Length - 2)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            AceType = (ACE_TYPE)binary[offset];
            AceFlags = (ACE_FLAGS)binary[offset + 1];
        }

        /// <summary>
        /// Create the ace from thr binary input
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="offset">The offset of the binary</param>
        public static _AceHeader CreateAceFromBinary(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            if (offset < 0 || offset > binary.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            ACE_TYPE type = (ACE_TYPE)binary[offset];

            if (DtypUtility.IsObjectAce(type))
            {
                return new _ObjectAce(binary, offset);
            }
            else
            {
                return new _NonObjectAce(binary, offset);
            }
        }

        /// <summary>
        /// Create the binary from the buffer
        /// </summary>
        /// <param name="binary">The binary</param>
        /// <param name="offset">The offset in the binary</param>
        public abstract void GetBinaryForm(byte[] binary, int offset);

        /// <summary>
        /// Get the sddl string form of the ace
        /// </summary>
        /// <returns></returns>
        public abstract string GetSddlForm();
    }

    /// <summary>
    /// The non object ace inherited from the ace header
    /// </summary>
    public class _NonObjectAce : _AceHeader
    {
        private int access_mask;
        //An unsigned 16-bit integer that specifies the size, in bytes, of the ACE
        private int aceSize;
        private _SID identifier;
        private byte[] applicationData;

        #region Properties
        /// <summary>
        /// An ACCESS_MASK is a 32-bit set of flags that are used to encode the user rights to an object
        /// </summary>
        public int _AccessMask
        {
            get { return access_mask; }
            set { access_mask = value; }
        }

        /// <summary>
        /// A security identifier (SID) uniquely identifies a security principal
        /// </summary>
        public _SID _SecurityIdentifier
        {
            get { return identifier; }
            set { identifier = value; }
        }
        #endregion

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="offset">The offset in the binary</param>
        public _NonObjectAce(byte[] binary, int offset) : base(binary, offset)
        {
            //The AceSize field
            aceSize = BitConverter.ToUInt16(binary, offset + 2);

            if (offset > binary.Length - aceSize)
            {
                throw new ArgumentException(nameof(offset));
            }

            if (aceSize < 8 + DtypUtility.MinLengthOfSecurityIdentifier)
            {
                throw new ArgumentException(nameof(aceSize));
            }

            _AccessMask = BitConverter.ToInt32(binary, offset + DtypUtility.ACE_HEADER_LENGTH);
            _SecurityIdentifier = new _SID(binary, offset + DtypUtility.SHORT_FIXED_ACE_LENGTH);

            int dataLength = aceSize - (DtypUtility.SHORT_FIXED_ACE_LENGTH + _SecurityIdentifier.Size);
            if (dataLength > 0)// If there is still other application data exists
            {
                this.applicationData = new byte[dataLength];
                Array.Copy(binary, offset + DtypUtility.SHORT_FIXED_ACE_LENGTH + _SecurityIdentifier.Size, this.applicationData, 0, dataLength);
            }
        }

        /// <summary>
        /// The binary length of the non object ace
        /// </summary>
        public override int Size
        {
            get
            {
                int dataLength = this.applicationData == null ? 0 : this.applicationData.Length;
                return DtypUtility.SHORT_FIXED_ACE_LENGTH + _SecurityIdentifier.Size + dataLength;
            }
        }

        /// <summary>
        /// Create the binary from the buffer
        /// </summary>
        /// <param name="binary">The binary</param>
        /// <param name="offset">The offset in the binaryForm</param>
        public override void GetBinaryForm(byte[] binary, int offset)
        {
            int len = Size;
            binary[offset] = (byte)this.AceType;
            binary[offset + 1] = (byte)this.AceFlags;
            DtypUtility.WriteUInt16ToByteArray((ushort)len, binary, offset + 2);
            DtypUtility.WriteInt32ToByteArray(_AccessMask, binary, offset + DtypUtility.ACE_HEADER_LENGTH);

            _SecurityIdentifier.GetBinaryForm(binary, offset + DtypUtility.SHORT_FIXED_ACE_LENGTH);

            if (this.applicationData != null)
            {
                Array.Copy(this.applicationData, 0, binary, offset + DtypUtility.SHORT_FIXED_ACE_LENGTH + _SecurityIdentifier.Size, this.applicationData.Length);
            }
        }

        public override string GetSddlForm()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "({0};{1};{2};;;{3})",
                DtypUtility.ConvertAceTypeToSDDL(AceType),
                DtypUtility.ConvertAceFlagToSDDL(AceFlags),
                DtypUtility.ConvertAccessMaskToSDDL(_AccessMask),
                identifier.GetSddlForm());
        }
    }

    /// <summary>
    /// The object ace inherited from the ace header
    /// </summary>
    public class _ObjectAce : _AceHeader
    {
        private int access_mask;
        private _SID identifier;
        private byte[] applicationData;
        private _ObjectAceFlags objectFlags;
        private Guid objectType;
        private Guid inheridObjectType;

        #region Properties
        /// <summary>
        /// A 32-bit unsigned integer that specifies a set of bit flags that indicate whether the ObjectType and InheritedObjectType fields contain valid data. 
        /// </summary>
        public _ObjectAceFlags ObjectFlags
        {
            get { return this.objectFlags; }
            set { this.objectFlags = value; }
        }

        /// <summary>
        /// A GUID that identifies a property set, a property, an extended right, or a type of child object
        /// </summary>
        public Guid ObjectType
        {
            get { return this.objectType; }
            set { this.objectType = value; }
        }

        /// <summary>
        /// A GUID that identifies the type of child object that can inherit the ACE
        /// </summary>
        public Guid InheritedObjectType
        {
            get { return this.inheridObjectType; }
            set { this.inheridObjectType = value; }
        }

        /// <summary>
        /// An ACCESS_MASK that specifies the user rights allowed by this ACE
        /// </summary>
        public int _AccessMask
        {
            get { return access_mask; }
            set { access_mask = value; }
        }

        /// <summary>
        /// A security identifier (SID) uniquely identifies a security principal
        /// </summary>
        public _SID _SecurityIdentifier
        {
            get { return identifier; }
            set { identifier = value; }
        }
        #endregion

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="offset">The offset in the binary</param>
        public _ObjectAce(byte[] binary, int offset) : base(binary, offset)
        {
            int length = BitConverter.ToUInt16(binary, offset + 2);

            if (offset > binary.Length - length)
            {
                throw new ArgumentException(nameof(offset));
            }

            _AccessMask = BitConverter.ToInt32(binary, offset + DtypUtility.ACE_HEADER_LENGTH);
            ObjectFlags = (_ObjectAceFlags)BitConverter.ToInt32(binary, offset + DtypUtility.SHORT_FIXED_ACE_LENGTH);

            int pointer = DtypUtility.ACE_HEADER_LENGTH + DtypUtility.SHORT_FIXED_ACE_LENGTH;
            if (ObjectFlags.HasFlag(_ObjectAceFlags.ObjectAceTypePresent))
            {
                ObjectType = DtypUtility.ReadGuid(binary, offset + pointer);
                pointer += 16;
            }
            if (ObjectFlags.HasFlag(_ObjectAceFlags.InheritedObjectAceTypePresent))
            {
                InheritedObjectType = DtypUtility.ReadGuid(binary, offset + pointer);
                pointer += 16;
            }

            _SecurityIdentifier = new _SID(binary, offset + pointer);
            pointer += _SecurityIdentifier.Size;

            int appDataLength = length - pointer;
            if (appDataLength > 0)
            {
                this.applicationData = new byte[appDataLength];
                Array.Copy(binary, offset + pointer, this.applicationData, 0, appDataLength);
            }
        }

        /// <summary>
        /// Create the binary from the buffer
        /// </summary>
        /// <param name="binary">The binary</param>
        /// <param name="offset">The offset in the binary</param>
        public override void GetBinaryForm(byte[] binary, int offset)
        {
            int len = Size;
            binary[offset++] = (byte)this.AceType;
            binary[offset++] = (byte)this.AceFlags;
            DtypUtility.WriteUInt16ToByteArray((ushort)len, binary, offset); offset += 2;
            DtypUtility.WriteInt32ToByteArray(_AccessMask, binary, offset); offset += 4;
            DtypUtility.WriteInt32ToByteArray((int)ObjectFlags, binary, offset); offset += 4;

            if (ObjectFlags.HasFlag(_ObjectAceFlags.ObjectAceTypePresent))
            {
                DtypUtility.WriteGuid(ObjectType, binary, offset); 
                offset += 16;
            }
            if (ObjectFlags.HasFlag( _ObjectAceFlags.InheritedObjectAceTypePresent))
            {
                DtypUtility.WriteGuid(InheritedObjectType, binary, offset); 
                offset += 16;
            }

            _SecurityIdentifier.GetBinaryForm(binary, offset);
            offset += _SecurityIdentifier.Size;

            if (this.applicationData != null)
            {
                Array.Copy(this.applicationData, 0, binary, offset, this.applicationData.Length);
                offset += this.applicationData.Length;
            }
        }

        /// <summary>
        /// return the length of the binary
        /// </summary>
        public override int Size
        {
            get
            {
                int dataLength = this.applicationData == null ? 0 : this.applicationData.Length;
                int length = DtypUtility.SHORT_FIXED_ACE_LENGTH + DtypUtility.ACE_FLAGS_LENGTH + _SecurityIdentifier.Size + dataLength;

                if (ObjectFlags.HasFlag(_ObjectAceFlags.ObjectAceTypePresent))
                {
                    length += 16;
                }

                if (ObjectFlags.HasFlag( _ObjectAceFlags.InheritedObjectAceTypePresent))
                {
                    length += 16;
                }

                return length;
            }
        }

        public override string GetSddlForm()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "({0};{1};{2};{3};{4};{5})",
                DtypUtility.ConvertAceTypeToSDDL(AceType),
                DtypUtility.ConvertAceFlagToSDDL(AceFlags),
                DtypUtility.ConvertAccessMaskToSDDL(_AccessMask),
                ObjectFlags.HasFlag(_ObjectAceFlags.ObjectAceTypePresent) ? objectType.ToString("D") : string.Empty,
                ObjectFlags.HasFlag(_ObjectAceFlags.InheritedObjectAceTypePresent) ? inheridObjectType.ToString("D") : string.Empty,
                identifier.GetSddlForm());
        }
    }

    /// <summary>
    /// The raw acl
    /// </summary>
    public class _RawAcl
    {
        private byte revision;
        private List<_AceHeader> list;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binary">the input binary</param>
        /// <param name="offset">the offset in the binary</param>
        public _RawAcl(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            if (offset < 0 || offset > binary.Length - DtypUtility.SHORT_FIXED_ACE_LENGTH)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            revision = binary[offset];

            int pointer = offset + DtypUtility.ACL_HEADER_LENGTH;
            int aceCount = BitConverter.ToUInt16(binary, offset + 4);
            list = new List<_AceHeader>(aceCount);
            for (int i = 0; i < aceCount; ++i)
            {
                _AceHeader ace = _AceHeader.CreateAceFromBinary(binary, pointer);
                list.Add(ace);
                pointer += ace.Size;
            }
        }

        /// <summary>
        /// get the length of the binary
        /// </summary>
        public int Size
        {
            get
            {
                int length = DtypUtility.ACL_HEADER_LENGTH;
                foreach (var ace in list)
                {
                    length += ace.Size;
                }

                return length;
            }
        }

        /// <summary>
        /// return the revision
        /// </summary>
        public byte Revision
        {
            get { return revision; }
        }

        /// <summary>
        /// Create the binary from the buffer
        /// </summary>
        /// <param name="binary">The binary</param>
        /// <param name="offset">The offset in the binaryForm</param>
        public void GetBinaryForm(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            if (offset < 0 || offset > binary.Length - Size)
            {
                throw new ArgumentException(nameof(offset));
            }

            binary[offset] = Revision;
            binary[offset + 1] = 0;
            DtypUtility.WriteUInt16ToByteArray((ushort)Size, binary, offset + 2);
            DtypUtility.WriteUInt16ToByteArray((ushort)list.Count, binary, offset + 4);
            DtypUtility.WriteUInt16ToByteArray((ushort)0, binary, offset + 6);

            int pointer = offset + DtypUtility.ACL_HEADER_LENGTH;
            foreach (var ace in list)
            {
                ace.GetBinaryForm(binary, pointer);
                pointer += ace.Size;
            }
        }

        public string GetSddlForm(SECURITY_DESCRIPTOR_Control flags, bool isDacl)
        {
            StringBuilder result = new StringBuilder();

            if (isDacl)
            {
                if (flags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLProtected))
                    result.Append("P");
                if (flags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLInheritanceRequired))
                    result.Append("AR");
                if (flags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLAutoInherited))
                    result.Append("AI");
            }
            else
            {
                if (flags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLProtected))
                    result.Append("P");
                if (flags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLInheritanceRequired))
                    result.Append("AR");
                if (flags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLAutoInherited))
                    result.Append("AI");
            }

            foreach (var ace in list)
            {
                result.Append(ace.GetSddlForm());
            }

            return result.ToString();
        }
    }

    /// <summary>
    /// The raw security descriptor
    /// </summary>
    public class _RawSecurityDescriptor
    {
        private SECURITY_DESCRIPTOR_Control controlFlags;
        private _SID? ownerSid;
        private _SID? groupSid;
        private _RawAcl dacl;
        private _RawAcl sacl;

        #region Properties
        /// <summary>
        /// An unsigned 16-bit field that specifies control access bit flags
        /// </summary>
        public SECURITY_DESCRIPTOR_Control ControlFlags
        {
            get { return controlFlags; }
            set { controlFlags = value; }
        }

        /// <summary>
        /// The DACL of the object. The length of the SID MUST be a multiple of 4
        /// </summary>
        public _RawAcl DACL
        {
            get { return dacl; }
            set { dacl = value; }
        }

        /// <summary>
        /// The SACL of the object. The length of the SID MUST be a multiple of 4
        /// </summary>
        public _RawAcl SACL 
        {
            get { return sacl; }
            set { sacl = value; }
        }

        /// <summary>
        /// The SID of the group of the object
        /// </summary>
        public _SID? Group
        {
            get { return groupSid; }
            set { groupSid = value; }
        }

        /// <summary>
        /// The SID of the owner of the object
        /// </summary>
        public _SID? Owner
        {
            get { return ownerSid; }
            set { ownerSid = value; }
        }
        #endregion

        /// <summary>
        /// the constructor
        /// </summary>
        /// <param name="binary">the input binary</param>
        /// <param name="offset">the offset in the binary</param>
        public _RawSecurityDescriptor(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            if (offset < 0 || offset > binary.Length - 0x14)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            controlFlags = (SECURITY_DESCRIPTOR_Control)BitConverter.ToUInt16(binary, offset + 2);

            //Get owner sid
            int ownerStart = BitConverter.ToInt32(binary, offset + 4);
            if (ownerStart != 0)
            {
                ownerSid = new _SID(binary, ownerStart);
            }

            //Get group sid
            int groupStart = BitConverter.ToInt32(binary, offset + 8);
            if (groupStart != 0)
            {
                groupSid = new _SID(binary, groupStart);
            }

            //Get sacl
            int saclStart = BitConverter.ToInt32(binary, offset + 12);
            if (saclStart != 0)
            {
                sacl = new _RawAcl(binary, saclStart);
            }

            //Get dacl
            int daclStart = BitConverter.ToInt32(binary, offset + 16);
            if (daclStart != 0)
            {
                dacl = new _RawAcl(binary, daclStart);
            }
        }

        /// <summary>
        /// the constructor
        /// </summary>
        /// <param name="flags">the security descriptor control flag</param>
        /// <param name="owner">the owner sid</param>
        /// <param name="group">the group sid</param>
        /// <param name="sacl">the sacl</param>
        /// <param name="dacl">the dacl</param>
        public _RawSecurityDescriptor(SECURITY_DESCRIPTOR_Control flags,
            _SID? owner,
            _SID? group,
            _RawAcl sacl,
            _RawAcl dacl)
        {
            controlFlags = flags;
            ownerSid = owner;
            groupSid = group;
            this.sacl = sacl;
            this.dacl = dacl;
        }

        public _RawSecurityDescriptor(string sddl)
        {
            this.controlFlags |= SECURITY_DESCRIPTOR_Control.SelfRelative;
        }

        /// <summary>
        /// return the length of the binary
        /// </summary>
        public int Size
        {
            get
            {
                int length = DtypUtility.SECURITY_DESCRIPTOR_FIXED_LENGTH;
                if (Owner != null)
                {
                    length += Owner.Value.Size;
                }
                if (Group != null)
                {
                    length += Group.Value.Size;
                }
                if (this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLPresent))
                {
                    length += DACL.Size;
                }
                if (this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLPresent))
                {
                    length += SACL.Size;
                }

                return length;
            }
        }

        /// <summary>
        /// Create the binary from the buffer
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="offset">The offset in the binary</param>
        public void GetBinaryForm(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            int binaryLength = Size;
            if (offset < 0 || offset > binary.Length - binaryLength)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            SECURITY_DESCRIPTOR_Control controlFlags = this.controlFlags;

            binary[offset] = 1; //revision
            binary[offset + 1] = 0; //Sbz1
            DtypUtility.WriteUInt16ToByteArray((ushort)controlFlags, binary, offset + 2);

            int pointer = DtypUtility.SECURITY_DESCRIPTOR_FIXED_LENGTH;
            if (Owner != null)
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 4);
                Owner.Value.GetBinaryForm(binary, offset + pointer);
                pointer += Owner.Value.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 4);
            }

            if (Group != null)
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 8);
                Group.Value.GetBinaryForm(binary, offset + pointer);
                pointer += Group.Value.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 8);
            }

            _RawAcl sacl = this.SACL;
            if (this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLPresent))
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 12);
                sacl.GetBinaryForm(binary, offset + pointer);
                pointer += this.SACL.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 12);
            }

            _RawAcl dacl = this.DACL;
            if (this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLPresent))
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 16);
                dacl.GetBinaryForm(binary, offset + pointer);
                pointer += this.DACL.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 16);
            }
        }

        public string GetSddlForm(AccessControlSections sections)
        {
            StringBuilder result = new StringBuilder();

            if (Owner != null && sections.HasFlag(AccessControlSections.Owner))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "O:{0}", Owner.Value.GetSddlForm());
            }

            if (Group != null && sections.HasFlag(AccessControlSections.Group))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "G:{0}", Group.Value.GetSddlForm());
            }

            if (sections.HasFlag(AccessControlSections.Access) && this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLPresent))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "D:{0}", dacl.GetSddlForm(controlFlags, true));
            }

            if (sections.HasFlag(AccessControlSections.Audit) && this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLPresent))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "S:{0}", sacl.GetSddlForm(controlFlags, false));
            }

            return result.ToString();
        }
       
    }
}
