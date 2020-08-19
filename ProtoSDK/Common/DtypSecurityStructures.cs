// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Dtyp
{
    /// <summary>
    /// A security identifier (SID) uniquely identifies a security principal.
    /// </summary>
    public class _SecurityIdentifier
    {
        private byte[] buffer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="binary">The binary to be encoded</param>
        /// <param name="offset">The offset in the binary</param>
        unsafe public _SecurityIdentifier(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }

            if ((offset < 0) || (offset > binary.Length - 2))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            fixed (byte* binaryPtr = binary)CreateFromBinaryForm((IntPtr)(binaryPtr + offset), binary.Length - offset);
        }

        /// <summary>
        /// Create the buffer from the binary input
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="length">The length of the binary</param>
        void CreateFromBinaryForm(IntPtr binary, int length)
        {
            int revision = Marshal.ReadByte(binary, 0);
            int subAuthorities = Marshal.ReadByte(binary, 1);
            if (revision != 1 || subAuthorities > 15)
            {
                throw new ArgumentException(nameof(revision));
            }
            if (length < (8 + (subAuthorities * 4)))
            {
                throw new ArgumentException(nameof(length));
            }

            buffer = new byte[8 + (subAuthorities * 4)];
            Marshal.Copy(binary, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// The binary length of the buffer
        /// </summary>
        public int Size
        {
            get { return buffer.Length; }
        }

        /// <summary>
        /// Create the binary from the buffer
        /// </summary>
        /// <param name="binary">The binary></param>
        /// <param name="offset">The offset in the binary</param>
        public void GetBinaryForm(byte[] binary, int offset)
        {
            if (binary == null)
            {
                throw new ArgumentNullException(nameof(binary));
            }
            if ((offset < 0) || (offset > binary.Length - buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            Array.Copy(buffer, 0, binary, offset, buffer.Length);
        }

        public string GetSddlForm()
        {
            StringBuilder result = new StringBuilder();
            _SID sid = TypeMarshal.ToStruct<_SID>(buffer);
            string sddl = DtypUtility.ToSddlString(sid);
            result.AppendFormat(CultureInfo.InvariantCulture, "O:{0}", sddl);

            return result.ToString();
        }
    }

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

        public abstract string GetSddlForm();
    }

    /// <summary>
    /// The non object ace inherited from the ace header
    /// </summary>
    public class _NonObjectAce : _AceHeader
    {
        private int access_mask;
        private _SecurityIdentifier identifier;
        private byte[] applicationData;

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
        public _SecurityIdentifier _SecurityIdentifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binary">The input binary</param>
        /// <param name="offset">The offset in the binary</param>
        public _NonObjectAce(byte[] binary, int offset) : base(binary, offset)
        {
            int length = BitConverter.ToUInt16(binary, offset + 2);

            if (offset > binary.Length - length)
            {
                throw new ArgumentException(nameof(offset));
            }

            if (length < 8 + DtypUtility.MinLengthOfSecurityIdentifier)
            {
                throw new ArgumentException(nameof(length));
            }

            _AccessMask = BitConverter.ToInt32(binary, offset + 4);
            _SecurityIdentifier = new _SecurityIdentifier(binary, offset + 8);

            int dataLength = length - (8 + _SecurityIdentifier.Size);
            if (dataLength > 0)// If there is still other application data exists
            {
                this.applicationData = new byte[dataLength];
                Array.Copy(binary, offset + 8 + _SecurityIdentifier.Size, this.applicationData, 0, dataLength);
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
                return 8 + _SecurityIdentifier.Size + dataLength;
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
            DtypUtility.WriteInt32ToByteArray(_AccessMask, binary, offset + 4);

            _SecurityIdentifier.GetBinaryForm(binary, offset + 8);

            if (this.applicationData != null)
            {
                Array.Copy(this.applicationData, 0, binary, offset + 8 + _SecurityIdentifier.Size, this.applicationData.Length);
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
        private _SecurityIdentifier identifier;
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
        public _SecurityIdentifier _SecurityIdentifier
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

            _AccessMask = BitConverter.ToInt32(binary, offset + 4);
            ObjectFlags = (_ObjectAceFlags)BitConverter.ToInt32(binary, offset + 8);

            int pointer = 12;
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

            _SecurityIdentifier = new _SecurityIdentifier(binary, offset + pointer);
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
                DtypUtility.WriteGuid(ObjectType, binary, offset); offset += 16;
            }
            if (ObjectFlags.HasFlag( _ObjectAceFlags.InheritedObjectAceTypePresent))
            {
                DtypUtility.WriteGuid(InheritedObjectType, binary, offset); offset += 16;
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
                int length = 12 + _SecurityIdentifier.Size + dataLength;

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

            if (offset < 0 || offset > binary.Length - 8)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            revision = binary[offset];

            int pointer = offset + 8;
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
                int length = 8;
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

            int pointer = offset + 8;
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
        private _SecurityIdentifier ownerSid;
        private _SecurityIdentifier groupSid;
        private _RawAcl dacl;
        private _RawAcl sacl;

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
        public _SecurityIdentifier Group
        {
            get { return groupSid; }
            set { groupSid = value; }
        }

        /// <summary>
        /// The SID of the owner of the object
        /// </summary>
        public _SecurityIdentifier Owner
        {
            get { return ownerSid; }
            set { ownerSid = value; }
        }

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

            controlFlags = (SECURITY_DESCRIPTOR_Control)BitConverter.ToUInt16(binary, offset + 0x02);

            //Get owner sid
            int ownerStart = BitConverter.ToInt32(binary, offset + 0x04);
            if (ownerStart != 0)
            {
                ownerSid = new _SecurityIdentifier(binary, ownerStart);
            }

            //Get group sid
            int groupStart = BitConverter.ToInt32(binary, offset + 0x08);
            if (groupStart != 0)
            {
                groupSid = new _SecurityIdentifier(binary, groupStart);
            }

            //Get sacl
            int saclStart = BitConverter.ToInt32(binary, offset + 0x0C);
            if (saclStart != 0)
            {
                sacl = new _RawAcl(binary, saclStart);
            }

            //Get dacl
            int daclStart = BitConverter.ToInt32(binary, offset + 0x10);
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
            _SecurityIdentifier owner,
            _SecurityIdentifier group,
            _RawAcl sacl,
            _RawAcl dacl)
        {
            controlFlags = flags;
            ownerSid = owner;
            groupSid = group;
            this.sacl = sacl;
            this.dacl = dacl;
        }

        /// <summary>
        /// return the length of the binary
        /// </summary>
        public int Size
        {
            get
            {
                int length = 0x14;
                if (Owner != null)
                {
                    length += Owner.Size;
                }
                if (Group != null)
                {
                    length += Group.Size;
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

            binary[offset + 0x00] = 1; //revision
            binary[offset + 0x01] = 0; //reserved
            DtypUtility.WriteUInt16ToByteArray((ushort)controlFlags, binary, offset + 0x02);

            int pointer = 0x14;
            if (Owner != null)
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 0x04);
                Owner.GetBinaryForm(binary, offset + pointer);
                pointer += Owner.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 0x04);
            }

            if (Group != null)
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 0x08);
                Group.GetBinaryForm(binary, offset + pointer);
                pointer += Group.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 0x08);
            }

            _RawAcl sacl = this.SACL;
            if (this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLPresent))
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 0x0C);
                sacl.GetBinaryForm(binary, offset + pointer);
                pointer += this.SACL.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 0x0C);
            }

            _RawAcl dacl = this.DACL;
            if (this.controlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLPresent))
            {
                DtypUtility.WriteInt32ToByteArray(pointer, binary, offset + 0x10);
                dacl.GetBinaryForm(binary, offset + pointer);
                pointer += this.DACL.Size;
            }
            else
            {
                DtypUtility.WriteInt32ToByteArray(0, binary, offset + 0x10);
            }
        }

        public string GetSddlForm(AccessControlSections sections)
        {
            StringBuilder result = new StringBuilder();

            if (Owner != null && sections.HasFlag(AccessControlSections.Owner))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "O:{0}", Owner.GetSddlForm());
            }

            if (Group != null && sections.HasFlag(AccessControlSections.Group))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "G:{0}", Group.GetSddlForm());
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
