// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
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
        /// The maximum length of the binary
        /// </summary>
        public static readonly int MaxLength = 68;

        /// <summary>
        /// The minimum length of the binary
        /// </summary>
        public static readonly int MinLength = 8;

        /// <summary>
        /// Cnnstructor
        /// </summary>
        /// <param name="binaryForm">The binary form to be encoded</param>
        /// <param name="offset">The offset in the binary</param>
        unsafe public _SecurityIdentifier(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            if ((offset < 0) || (offset > binaryForm.Length - 2))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            fixed (byte* binaryFormPtr = binaryForm)
                CreateFromBinaryForm((IntPtr)(binaryFormPtr + offset), binaryForm.Length - offset);
        }

        /// <summary>
        /// Create the buffer from thr binaryForm input
        /// </summary>
        /// <param name="binaryForm">The input binary form</param>
        /// <param name="length">The length of the binary</param>
        void CreateFromBinaryForm(IntPtr binaryForm, int length)
        {
            int revision = Marshal.ReadByte(binaryForm, 0);
            int numSubAuthorities = Marshal.ReadByte(binaryForm, 1);
            if (revision != 1 || numSubAuthorities > 15)
            {
                throw new ArgumentException(nameof(revision));
            }
            if (length < (8 + (numSubAuthorities * 4)))
            {
                throw new ArgumentException(nameof(length));
            }

            buffer = new byte[8 + (numSubAuthorities * 4)];
            Marshal.Copy(binaryForm, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// The binary length of the buffer
        /// </summary>
        public int BinaryLength
        {
            get { return buffer.Length; }
        }

        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public void GetBinaryForm(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }
            if ((offset < 0) || (offset > binaryForm.Length - buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            Array.Copy(buffer, 0, binaryForm, offset, buffer.Length);
        }
    }

    /// <summary>
    /// The generic acr to be inherited
    /// </summary>
    public abstract class _GenericAce
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binaryForm">The input binaryForm</param>
        /// <param name="offset">The offset in the binaryForm</param>
        internal _GenericAce(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            if (offset < 0 || offset > binaryForm.Length - 2)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            AceType = (ACE_TYPE)binaryForm[offset];
            AceFlags = (ACE_FLAGS)binaryForm[offset + 1];
        }

        /// <summary>
        /// The ace flags
        /// </summary>
        public ACE_FLAGS AceFlags { get; set; }

        /// <summary>
        /// The ace type
        /// </summary>
        public ACE_TYPE AceType { get; }

        /// <summary>
        /// The binary length
        /// </summary>
        public abstract int BinaryLength { get; }

        /// <summary>
        /// Create the buffer from thr binaryForm input
        /// </summary>
        /// <param name="binaryForm">The input binary form</param>
        /// <param name="offset">The offset of the binary</param>
        public static _GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            if (offset < 0 || offset > binaryForm.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            ACE_TYPE type = (ACE_TYPE)binaryForm[offset];

            if (DtypUtility.IsAceObjectType(type))
            {
                return new _ObjectAce(binaryForm, offset);
            }
            else
            {
                return new _CommonAce(binaryForm, offset);
            }
        }

        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public abstract void GetBinaryForm(byte[] binaryForm, int offset);
    }

    /// <summary>
    /// The abstract known ace inherited from generic ace
    /// </summary>
    public abstract class _KnownAce : _GenericAce
    {
        private int access_mask;
        private _SecurityIdentifier identifier;

        internal _KnownAce(byte[] binaryForm, int offset) : base(binaryForm, offset)
        { }

        /// <summary>
        /// The access mask contained in the ace
        /// </summary>
        public int _AccessMask
        {
            get { return access_mask; }
            set { access_mask = value; }
        }

        /// <summary>
        /// the security identifier contained in the ace
        /// </summary>
        public _SecurityIdentifier _SecurityIdentifier
        {
            get { return identifier; }
            set { identifier = value; }
        }
    }

    /// <summary>
    /// The qualified ace inherited from the known ace
    /// </summary>
    public abstract class _QualifiedAce : _KnownAce
    {
        private byte[] opaque;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binaryForm">THe input binaryForm</param>
        /// <param name="offset">The offset in the binaryForm</param>
        internal _QualifiedAce(byte[] binaryForm, int offset) : base(binaryForm, offset)
        { }

        /// <summary>
        /// Return the opaque
        /// </summary>
        /// <returns></returns>
        public byte[] GetOpaque()
        {
            return opaque == null ? null : (byte[])opaque.Clone();
        }

        /// <summary>
        /// Set the value of opaque with the input value
        /// </summary>
        /// <param name="opaque">the input value</param>
        public void SetOpaque(byte[] opaque)
        {
            this.opaque = (opaque == null ? null : (byte[])opaque.Clone());
        }

        /// <summary>
        /// Get the length of the opaque
        /// </summary>
        /// <returns></returns>
        public int GetOpaqueLength()
        {
            return opaque == null ? 0 : opaque.Length;
        }
    }

    /// <summary>
    /// The common ace inherited from qualified ace
    /// </summary>
    public class _CommonAce : _QualifiedAce
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binaryForm">The input binaryForm</param>
        /// <param name="offset">The offset in the binaryForm</param>
        internal _CommonAce(byte[] binaryForm, int offset) : base(binaryForm, offset)
        {
            int len = DtypUtility.ReadUInt16(binaryForm, offset + 2);

            if (offset > binaryForm.Length - len)
            {
                throw new ArgumentException(nameof(offset));
            }

            if (len < 8 + _SecurityIdentifier.MinLength)
            {
                throw new ArgumentException(nameof(len));
            }

            _AccessMask = DtypUtility.ReadInt32(binaryForm, offset + 4);
            _SecurityIdentifier = new _SecurityIdentifier(binaryForm, offset + 8);

            int opaqueLen = len - (8 + _SecurityIdentifier.BinaryLength);
            if (opaqueLen > 0)
            {
                byte[] opaque = new byte[opaqueLen];
                Array.Copy(binaryForm, offset + 8 + _SecurityIdentifier.BinaryLength, opaque, 0, opaqueLen);
                SetOpaque(opaque);
            }
        }

        /// <summary>
        /// The binary length of the common ace
        /// </summary>
        public override int BinaryLength
        {
            get
            {
                return 8 + _SecurityIdentifier.BinaryLength + GetOpaqueLength();
            }
        }

        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public override void GetBinaryForm(byte[] binaryForm, int offset)
        {
            int len = BinaryLength;
            binaryForm[offset] = (byte)this.AceType;
            binaryForm[offset + 1] = (byte)this.AceFlags;
            DtypUtility.WriteUInt16((ushort)len, binaryForm, offset + 2);
            DtypUtility.WriteInt32(_AccessMask, binaryForm, offset + 4);

            _SecurityIdentifier.GetBinaryForm(binaryForm, offset + 8);

            byte[] opaque = GetOpaque();

            if (opaque != null)
                Array.Copy(opaque, 0, binaryForm, offset + 8 + _SecurityIdentifier.BinaryLength, opaque.Length);
        }
    }

    /// <summary>
    /// The object ace inherited from the qualified ace
    /// </summary>
    public class _ObjectAce : _QualifiedAce
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binaryForm">The input binaryForm</param>
        /// <param name="offset">The offset in the binaryForm</param>
        internal _ObjectAce(byte[] binaryForm, int offset) : base(binaryForm, offset)
        {
            int length = DtypUtility.ReadUInt16(binaryForm, offset + 2);
            int minLength = 12 + _SecurityIdentifier.MinLength;

            if (offset > binaryForm.Length - length)
            {
                throw new ArgumentException(nameof(offset));
            }

            if (length < minLength)
            {
                throw new ArgumentException(nameof(length));
            }

            _AccessMask = DtypUtility.ReadInt32(binaryForm, offset + 4);

            ObjectAceFlags = (_ObjectAceFlags)DtypUtility.ReadInt32(binaryForm, offset + 8);

            if (ObjectAceTypePresent)
            {
                minLength += 16;
            }

            if (InheritedObjectAceTypePresent)
            {
                minLength += 16;
            }

            if (length < minLength)
            {
                throw new ArgumentException(nameof(length));
            }

            int pos = 12;
            if (ObjectAceTypePresent)
            {
                ObjectAceType = DtypUtility.ReadGuid(binaryForm, offset + pos);
                pos += 16;
            }
            if (InheritedObjectAceTypePresent)
            {
                InheritedObjectAceType = DtypUtility.ReadGuid(binaryForm, offset + pos);
                pos += 16;
            }

            _SecurityIdentifier = new _SecurityIdentifier(binaryForm, offset + pos);
            pos += _SecurityIdentifier.BinaryLength;

            int opaqueLength = length - pos;
            if (opaqueLength > 0)
            {
                byte[] opaque = new byte[opaqueLength];
                Array.Copy(binaryForm, offset + pos, opaque, 0, opaqueLength);
                SetOpaque(opaque);
            }
        }

        /// <summary>
        /// The ace flags of the object ace
        /// </summary>
        public _ObjectAceFlags ObjectAceFlags { get; set; }

        /// <summary>
        /// The ace type of the object ace
        /// </summary>
        public Guid ObjectAceType { get; set; }

        /// <summary>
        /// The ace type of the inheritied object ace
        /// </summary>
        public Guid InheritedObjectAceType { get; set; }

        /// <summary>
        /// return whether objectacetype present
        /// </summary>
        bool ObjectAceTypePresent
        {
            get { return 0 != (ObjectAceFlags & _ObjectAceFlags.ObjectAceTypePresent); }
        }

        /// <summary>
        /// return whether inheritiedobjectacetype present
        /// </summary>
        bool InheritedObjectAceTypePresent
        {
            get { return 0 != (ObjectAceFlags & _ObjectAceFlags.InheritedObjectAceTypePresent); }
        }

        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public override void GetBinaryForm(byte[] binaryForm, int offset)
        {
            int len = BinaryLength;
            binaryForm[offset++] = (byte)this.AceType;
            binaryForm[offset++] = (byte)this.AceFlags;
            DtypUtility.WriteUInt16((ushort)len, binaryForm, offset); offset += 2;
            DtypUtility.WriteInt32(_AccessMask, binaryForm, offset); offset += 4;
            DtypUtility.WriteInt32((int)ObjectAceFlags, binaryForm, offset); offset += 4;

            if (0 != (ObjectAceFlags & _ObjectAceFlags.ObjectAceTypePresent))
            {
                DtypUtility.WriteGuid(ObjectAceType, binaryForm, offset); offset += 16;
            }
            if (0 != (ObjectAceFlags & _ObjectAceFlags.InheritedObjectAceTypePresent))
            {
                DtypUtility.WriteGuid(InheritedObjectAceType, binaryForm, offset); offset += 16;
            }

            _SecurityIdentifier.GetBinaryForm(binaryForm, offset);
            offset += _SecurityIdentifier.BinaryLength;

            byte[] opaque = GetOpaque();
            if (opaque != null)
            {
                Array.Copy(opaque, 0, binaryForm, offset, opaque.Length);
                offset += opaque.Length;
            }
        }

        /// <summary>
        /// return the length of the binary
        /// </summary>
        public override int BinaryLength
        {
            get
            {
                int length = 12 + _SecurityIdentifier.BinaryLength + GetOpaqueLength();

                if (ObjectAceTypePresent)
                {
                    length += 16;
                }

                if (InheritedObjectAceTypePresent)
                {
                    length += 16;
                }

                return length;
            }
        }
    }

    public sealed class _AceEnumerator : IEnumerator
    {
        _GenericAcl owner;
        int current = -1;
        internal _AceEnumerator(_GenericAcl owner)
        {
            this.owner = owner;
        }
        public _GenericAce Current
        {
            get { return current < 0 ? null : owner[current]; }
        }
        object IEnumerator.Current
        {
            get { return Current; }
        }
        public bool MoveNext()
        {
            if (current + 1 == owner.Count)
                return false;
            current++;
            return true;
        }
        public void Reset()
        {
            current = -1;
        }
    }

    /// <summary>
    /// The generic acl to be inheritied
    /// </summary>
    public abstract class _GenericAcl : ICollection, IEnumerable
    {
        public static readonly byte AclRevision;
        public static readonly byte AclRevisionDS;
        public static readonly int MaxBinaryLength;

        /// <summary>
        /// constructor
        /// </summary>
        static _GenericAcl()
        {
            //AclRevision = 2;
            //AclRevisionDS = 4;
            //MaxBinaryLength = 0x10000;
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected _GenericAcl()
        {
        }

        /// <summary>
        /// Get the length of the binary
        /// </summary>
        public abstract int BinaryLength { get; }

        /// <summary>
        /// Get the count
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Inheritied property 
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Inherited peoperty
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract _GenericAce this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Revision
        /// </summary>
        public abstract byte Revision { get; }


        public virtual object SyncRoot
        {
            get
            { return this; }
        }

        /// <summary>
        /// Inherited function
        /// </summary>
        /// <param name="array">the array to be copied to</param>
        /// <param name="index">the index of the array</param>
        public void CopyTo(_GenericAce[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || array.Length - index < Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            for (int i = 0; i < Count; i++)
            {
                array[i + index] = this[i];
            }
        }

        /// <summary>
        /// inherited function
        /// </summary>
        /// <param name="array">the array to be copied to</param>
        /// <param name="index">the index of the array</param>
        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((_GenericAce[])array, index);
        }


        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public abstract void GetBinaryForm(byte[] binaryForm, int offset);

        /// <summary>
        /// inherited function
        /// </summary>
        /// <returns></returns>
        public _AceEnumerator GetEnumerator()
        {
            return new _AceEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// The raw acl
    /// </summary>
    public class _RawAcl : _GenericAcl
    {
        private byte revision;
        private List<_GenericAce> list;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="binaryForm">the input binaryForm</param>
        /// <param name="offset">the offset in the binaryForm</param>
        public _RawAcl(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            if (offset < 0 || offset > binaryForm.Length - 8)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            revision = binaryForm[offset];

            int binaryLength = DtypUtility.ReadUInt16(binaryForm, offset + 2);
            if (offset > binaryForm.Length - binaryLength)
            {
                throw new ArgumentException(nameof(offset));
            }

            int pos = offset + 8;
            int numAces = DtypUtility.ReadUInt16(binaryForm, offset + 4);

            list = new List<_GenericAce>(numAces);
            for (int i = 0; i < numAces; ++i)
            {
                _GenericAce newAce = _GenericAce.CreateFromBinaryForm(binaryForm, pos);
                list.Add(newAce);
                pos += newAce.BinaryLength;
            }
        }

        /// <summary>
        /// get the length of the binary
        /// </summary>
        public override int BinaryLength
        {
            get
            {
                int length = 8;
                foreach (var ace in list)
                {
                    length += ace.BinaryLength;
                }

                return length;
            }
        }

        /// <summary>
        /// get the count of the list
        /// </summary>
        public override int Count
        {
            get { return list.Count; }
        }

        /// <summary>
        /// inherited property
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override _GenericAce this[int index]
        {
            get { return list[index]; }

            set { list[index] = value; }
        }

        /// <summary>
        /// inherited property
        /// </summary>
        public override byte Revision
        {
            get { return revision; }
        }

        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public override void GetBinaryForm(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            if (offset < 0 || offset > binaryForm.Length - BinaryLength)
            {
                throw new ArgumentException(nameof(offset));
            }

            binaryForm[offset] = Revision;
            binaryForm[offset + 1] = 0;
            DtypUtility.WriteUInt16((ushort)BinaryLength, binaryForm, offset + 2);
            DtypUtility.WriteUInt16((ushort)list.Count, binaryForm, offset + 4);
            DtypUtility.WriteUInt16(0, binaryForm, offset + 6);

            int pos = offset + 8;
            foreach (var ace in list)
            {
                ace.GetBinaryForm(binaryForm, pos);
                pos += ace.BinaryLength;
            }
        }
    }

    /// <summary>
    /// The generic security descriptor
    /// </summary>
    public abstract class _GenericSecurityDescriptor
    {
        /// <summary>
        /// the constructor
        /// </summary>
        protected _GenericSecurityDescriptor()
        { }


        /// <summary>
        /// return the length of the binary
        /// </summary>
        public int BinaryLength
        {
            get
            {
                int length = 0x14;
                if (Owner != null)
                {
                    length += Owner.BinaryLength;
                }
                if (Group != null)
                {
                    length += Group.BinaryLength;
                }
                if (DaclPresent && !DaclIsUnmodifiedAefa)
                {
                    length += InternalDacl.BinaryLength;
                }
                if (SaclPresent)
                {
                    length += InternalSacl.BinaryLength;
                }

                return length;
            }
        }

        /// <summary>
        /// the security descriptor control flag
        /// </summary>
        public abstract SECURITY_DESCRIPTOR_Control ControlFlags { get; }

        /// <summary>
        /// the group security identifier of this secrurity descriptor
        /// </summary>
        public abstract _SecurityIdentifier Group { get; set; }

        /// <summary>
        /// the owner security identifier of this security descriptor
        /// </summary>
        public abstract _SecurityIdentifier Owner { get; set; }

        /// <summary>
        /// the revision
        /// </summary>
        public static byte Revision
        {
            get { return 1; }
        }

        /// <summary>
        /// the discretionary access control list 
        /// </summary>
        internal virtual _GenericAcl InternalDacl
        {
            get { return null; }
        }

        /// <summary>
        /// the system access control list
        /// </summary>
        internal virtual _GenericAcl InternalSacl
        {
            get { return null; }
        }

        /// <summary>
        /// the reserved field
        /// </summary>
        internal virtual byte InternalReservedField
        {
            get { return 0; }
        }

        /// <summary>
        /// Create the binaryForm from the buffer
        /// </summary>
        /// <param name="binaryForm">The binaryForm></param>
        /// <param name="offset">The offset in the binaryForm</param>
        public void GetBinaryForm(byte[] binaryForm, int offset)
        {
            if (null == binaryForm)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            int binaryLength = BinaryLength;
            if (offset < 0 || offset > binaryForm.Length - binaryLength)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            SECURITY_DESCRIPTOR_Control controlFlags = ControlFlags;
            if (DaclIsUnmodifiedAefa)
            {
                controlFlags &= ~SECURITY_DESCRIPTOR_Control.DACLPresent;
            }

            binaryForm[offset + 0x00] = Revision;
            binaryForm[offset + 0x01] = InternalReservedField;
            DtypUtility.WriteUInt16((ushort)controlFlags, binaryForm, offset + 0x02);

            int pos = 0x14;
            if (Owner != null)
            {
                DtypUtility.WriteInt32(pos, binaryForm, offset + 0x04);
                Owner.GetBinaryForm(binaryForm, offset + pos);
                pos += Owner.BinaryLength;
            }
            else
            {
                DtypUtility.WriteInt32(0, binaryForm, offset + 0x04);
            }

            if (Group != null)
            {
                DtypUtility.WriteInt32(pos, binaryForm, offset + 0x08);
                Group.GetBinaryForm(binaryForm, offset + pos);
                pos += Group.BinaryLength;
            }
            else
            {
                DtypUtility.WriteInt32(0, binaryForm, offset + 0x08);
            }

            _GenericAcl sysAcl = InternalSacl;
            if (SaclPresent)
            {
                DtypUtility.WriteInt32(pos, binaryForm, offset + 0x0C);
                sysAcl.GetBinaryForm(binaryForm, offset + pos);
                pos += InternalSacl.BinaryLength;
            }
            else
            {
                DtypUtility.WriteInt32(0, binaryForm, offset + 0x0C);
            }

            _GenericAcl discAcl = InternalDacl;
            if (DaclPresent && !DaclIsUnmodifiedAefa)
            {
                DtypUtility.WriteInt32(pos, binaryForm, offset + 0x10);
                discAcl.GetBinaryForm(binaryForm, offset + pos);
                pos += InternalDacl.BinaryLength;
            }
            else
            {
                DtypUtility.WriteInt32(0, binaryForm, offset + 0x10);
            }
        }

        internal virtual bool DaclIsUnmodifiedAefa
        {
            get { return false; }
        }

        /// <summary>
        /// return true if dacl is present
        /// </summary>
        bool DaclPresent
        {
            get
            {
                return InternalDacl != null && (ControlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.DACLPresent));
            }
        }

        /// <summary>
        /// return true if sacl is present
        /// </summary>
        bool SaclPresent
        {
            get
            {
                return InternalSacl != null && (ControlFlags.HasFlag(SECURITY_DESCRIPTOR_Control.SACLPresent));

            }
        }
    }

    /// <summary>
    /// The raw security descriptor
    /// </summary>
    public class _RawSecurityDescriptor : _GenericSecurityDescriptor
    {
        private SECURITY_DESCRIPTOR_Control controlFlags;
        private _SecurityIdentifier ownerSid;
        private _SecurityIdentifier groupSid;

        /// <summary>
        /// the constructor
        /// </summary>
        /// <param name="binaryForm"></param>
        /// <param name="offset"></param>
        public _RawSecurityDescriptor(byte[] binaryForm, int offset)
        {
            if (binaryForm == null)
            {
                throw new ArgumentNullException(nameof(binaryForm));
            }

            if (offset < 0 || offset > binaryForm.Length - 0x14)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            ResourceManagerControl = binaryForm[offset + 0x01];

            controlFlags = (SECURITY_DESCRIPTOR_Control)DtypUtility.ReadUInt16(binaryForm, offset + 0x02);
            int ownerPos = DtypUtility.ReadInt32(binaryForm, offset + 0x04);
            int groupPos = DtypUtility.ReadInt32(binaryForm, offset + 0x08);
            int saclPos = DtypUtility.ReadInt32(binaryForm, offset + 0x0C);
            int daclPos = DtypUtility.ReadInt32(binaryForm, offset + 0x10);

            if (ownerPos != 0)
            {
                ownerSid = new _SecurityIdentifier(binaryForm, ownerPos);
            }
            if (groupPos != 0)
            {
                groupSid = new _SecurityIdentifier(binaryForm, groupPos);
            }
            if (saclPos != 0)
            {
                SystemAcl = new _RawAcl(binaryForm, saclPos);
            }
            if (daclPos != 0)
            {
                DiscretionaryAcl = new _RawAcl(binaryForm, daclPos);
            }
        }

        /// <summary>
        /// the constructor
        /// </summary>
        /// <param name="flags">the security descriptor control flag</param>
        /// <param name="owner">the owner sid</param>
        /// <param name="group">the group sid</param>
        /// <param name="sacl"></param>
        /// <param name="dacl"></param>
        public _RawSecurityDescriptor(SECURITY_DESCRIPTOR_Control flags,
            _SecurityIdentifier owner,
            _SecurityIdentifier @group,
            _RawAcl sacl,
            _RawAcl dacl)
        {
            controlFlags = flags;
            ownerSid = owner;
            groupSid = @group;
            SystemAcl = sacl;
            DiscretionaryAcl = dacl;
        }

        /// <summary>
        /// the security control flags
        /// </summary>
        public override SECURITY_DESCRIPTOR_Control ControlFlags
        {
            get { return controlFlags; }
        }

        /// <summary>
        /// the dacl
        /// </summary>
        public _RawAcl DiscretionaryAcl { get; set; }

        /// <summary>
        /// the group sid
        /// </summary>
        public override _SecurityIdentifier Group
        {
            get { return groupSid; }
            set { groupSid = value; }
        }

        /// <summary>
        /// the owner sid
        /// </summary>
        public override _SecurityIdentifier Owner
        {
            get { return ownerSid; }
            set { ownerSid = value; }
        }

        /// <summary>
        /// the resource manager control
        /// </summary>
        public byte ResourceManagerControl { get; set; }

        /// <summary>
        /// the sacl
        /// </summary>
        public _RawAcl SystemAcl { get; set; }

        /// <summary>
        /// set the security descriptor control flags
        /// </summary>
        /// <param name="flags">the input flag</param>
        public void SetFlags(SECURITY_DESCRIPTOR_Control flags)
        {
            controlFlags = flags | SECURITY_DESCRIPTOR_Control.SelfRelative;
        }

        internal override _GenericAcl InternalDacl
        {
            get { return this.DiscretionaryAcl; }
        }

        internal override _GenericAcl InternalSacl
        {
            get { return this.SystemAcl; }
        }

        internal override byte InternalReservedField
        {
            get { return this.ResourceManagerControl; }
        }
    }
}
