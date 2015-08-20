// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using System.Globalization;
using System.Text;
using IOP = System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime.Marshaling
{
    /// <summary>
    /// The marshaler class which coordinates marshaling of values from and to native representations.
    /// </summary>
    public class Marshaler : IDisposable, IEvaluationContext
    {
        static readonly object identifierBindingLock = new object();
        MarshalingConfiguration config;
        IRuntimeHost host;
        internal bool tracing;

        /// <summary>
        /// Constructs the marshaler based on test site and marshaling configuration.
        /// </summary>
        /// <param name="host">The message runtime host</param>
        /// <param name="configuration">The marshaling configuration</param>
        public Marshaler(IRuntimeHost host, MarshalingConfiguration configuration)
        {
            this.host = host;
            this.config = configuration;
            this.tracing = host != null ? host.MarshallerTrace : false;
        }

        /// <summary>
        /// Constructs the marshaler based on marshaling configuration, without test site.
        /// </summary>
        /// <param name="configuration">The marshaling configuration</param>
        public Marshaler(MarshalingConfiguration configuration)
        {
            this.config = configuration;
        }

        /// <summary>
        /// Resets the internal data structures of the marshaler (like identifier bindings),
        /// and prepares for a next test run.
        /// </summary>
        public static void Reset()
        {
            lock (identifierBindingLock)
            {
                foreach (IdentifierBinding<object> binding in bindings.Values)
                    binding.Reset();
            }
        }

        public int GetIntPtrSize()
        {
            return config.IntPtrSize;
        }

        public int GetAlignment()
        {
            return config.Alignment;
        }

        #region Message Runtime Host

        /// <summary>
        /// Gets the associated runtime host.
        /// </summary>
        public IRuntimeHost Host
        {
            get
            {
                return this.host;
            }
        }

        static string GetMessage(MarshalingDescriptor desc, string message, params object[] parameters)
        {
            string failMessage = parameters.Length > 0 ? String.Format(CultureInfo.InvariantCulture, message, parameters) : message;
            if (desc.Type != null)
            {
                failMessage = string.Format(CultureInfo.InvariantCulture, "while processing '{0}': {1}", desc, failMessage);
            }
            return failMessage;
        }

        /// <summary>
        /// Test case assert fails with marshaling description, a given message, and additional parameters.
        /// </summary>
        /// <param name="desc">Marshaling descriptor</param>
        /// <param name="message">The given message</param>
        /// <param name="parameters">Additional parameters</param>
        public void TestAssertFail(MarshalingDescriptor desc, string message, params object[] parameters)
        {
            string failedMessage = GetMessage(desc, message, parameters);
            if (host != null)
            {
                host.Assert(false, failedMessage);
            }
            else
            {
                Console.WriteLine(failedMessage);
                throw new InvalidOperationException(failedMessage);
            }
        }

        /// <summary>
        /// Test case assert fails with a given message and additional parameters.
        /// </summary>
        /// <param name="message">The given message</param>
        /// <param name="parameters">Additional parameters</param>
        public void TestAssertFail(string message, params object[] parameters)
        {
            string failedMessage = GetMessage(new MarshalingDescriptor(), message, parameters);
            if (host != null)
            {
                host.Assert(false, failedMessage);
            }
            else
            {
                Console.WriteLine(failedMessage);
                throw new InvalidOperationException(failedMessage);
            }
        }

        /// <summary>
        /// Test case assume fails with marshaling description, a given message, and additional parameters.
        /// </summary>
        /// <param name="desc">Marshaling descriptor</param>
        /// <param name="message">The given message</param>
        /// <param name="parameters">Additional parameters</param>
        public void TestAssumeFail(MarshalingDescriptor desc, string message, params object[] parameters)
        {
            string failedMessage = GetMessage(desc, message, parameters);
            if (host != null)
            {
                host.Assume(false, failedMessage);
            }
            else
            {
                Console.WriteLine(failedMessage);
                throw new InvalidOperationException(failedMessage);
            }
        }

        /// <summary>
        /// Test case assume fails with a given message and additional parameters.
        /// </summary>
        /// <param name="message">The given message</param>
        /// <param name="parameters">Additional parameters</param>
        public void TestAssumeFail(string message, params object[] parameters)
        {
            string failedMessage = GetMessage(new MarshalingDescriptor(), message, parameters);
            if (host != null)
            {
                host.Assume(false, failedMessage);
            }
            else
            {
                Console.WriteLine(failedMessage);
                throw new InvalidOperationException(failedMessage);
            }
        }

        /// <summary>
        /// Test case debug fails with a given message and additional parameters
        /// </summary>
        /// <param name="message">The given message</param>
        /// <param name="parameters">Additional parameters</param>
        public void DebugFail(string message, params object[] parameters)
        {
            string failedMessage = GetMessage(new MarshalingDescriptor(), message, parameters);
            if (host != null)
            {
                host.Debug(false, failedMessage);
            }
            else
            {
                Console.WriteLine(failedMessage);
                throw new InvalidOperationException(failedMessage);
            }
        }

        /// <summary>
        /// Test case trace with a given message and additional parameters
        /// </summary>
        /// <param name="message">The given message</param>
        /// <param name="parameters">Additional parameters</param>
        public void Trace(string message, params object[] parameters)
        {
            if (tracing)
            {
                if (host != null)
                {
                    host.AddLog(LogKind.Debug, message, parameters);
                }
                else
                {
                    Console.WriteLine("trace: " + String.Format(CultureInfo.InvariantCulture, message, parameters));
                }
            }
        }

        #endregion

        #region State

        // context for symbols
        Dictionary<string, object> symbolStore;

        /// <summary>
        /// Gets the symbol storage under current context
        /// </summary>
        public Dictionary<string, object> SymbolStore
        {
            get { return symbolStore; }
        }
        Dictionary<string, object> pointerSymbolStore;
        Dictionary<string, object> customTypes = new Dictionary<string, object>();

        Stack<SavedContext> outerContexts = new Stack<SavedContext>();
        struct SavedContext
        {
            internal Dictionary<string, object> symbolStore;
            internal Dictionary<string, object> pointerSymbolStore;
        }

        IRegion region;

        Stack<IRegion> outerRegions = new Stack<IRegion>();
        List<IntPtr> allocatedMemory = new List<IntPtr>();
        List<IntPtr> foreignMemory = new List<IntPtr>();

        /// <summary>
        /// A container which store the pointers allocated by the RPC runtime.
        /// </summary>
        public IList<IntPtr> ForeignMemory
        {
            get
            {
                return foreignMemory;
            }
        }

        static Dictionary<string, IdentifierBinding<object>> bindings =
            new Dictionary<string, IdentifierBinding<object>>();

        // context for array marshaling
        int arrayLevel;
        IList<int> arraySizes;
        IList<int> arrayLengths;
        Stack<ArrayContext> outerArrayContexts = new Stack<ArrayContext>();
        struct ArrayContext
        {
            internal int arrayLevel;
            internal IList<int> arraySizes;
            internal IList<int> arrayLengths;
        }

        internal void EnterArrayLevel()
        {
            arrayLevel++;
        }

        internal void ExitArrayLevel()
        {
            arrayLevel--;
            if (arrayLevel == 0)
            {
                arraySizes = null;
                arrayLengths = null;
            }
        }

        internal int ArrayLevel
        {
            get
            {
                return arrayLevel;
            }
        }

        internal IList<int> ArraySizes
        {
            get { return arraySizes; }
            set { arraySizes = value; }
        }

        internal IList<int> ArrayLengths
        {
            get { return arrayLengths; }
            set { arrayLengths = value; }
        }

        internal bool ArrayContextAvailable
        {
            get
            {
                return (arraySizes != null && arrayLengths != null);
            }
        }

        // context for sequence marshaling
        int sequenceLevel;
        IList<int> sequenceSizes;
        IList<int> sequenceLengths;
        Stack<SequenceContext> outerSequenceContexts = new Stack<SequenceContext>();
        struct SequenceContext
        {
            internal int sequenceLevel;
            internal IList<int> sequenceSizes;
            internal IList<int> sequenceLengths;
        }

        internal void EnterSequenceLevel()
        {
            sequenceLevel++;
        }

        internal void ExitSequenceLevel()
        {
            sequenceLevel--;
            if (sequenceLevel == 0)
            {
                sequenceSizes = null;
                sequenceLengths = null;
            }
        }

        internal int SequenceLevel
        {
            get
            {
                return sequenceLevel;
            }
        }

        internal IList<int> SequenceSizes
        {
            get { return sequenceSizes; }
            set { sequenceSizes = value; }
        }

        internal IList<int> SequenceLengths
        {
            get { return sequenceLengths; }
            set { sequenceLengths = value; }
        }

        internal bool SequenceContextAvailable
        {
            get
            {
                return (sequenceSizes != null && sequenceLengths != null);
            }
        }

        // State for struct and union unmarshaling
        bool isProbingUnmarshaling;

        /// <summary>
        /// Internal use only. 
        /// State for structure and union unmarshaling.
        /// </summary>
        public bool IsProbingUnmarshaling
        {
            get
            {
                return isProbingUnmarshaling;
            }

            set
            {
                isProbingUnmarshaling = value;
            }
        }

        // State for bit marshaling

        Queue<int> bitQueue = new Queue<int>();

        /// <summary>
        /// Gets the number of bits in the bit queue.
        /// </summary>
        public int CurrentBitCount
        {
            get { return bitQueue.Count; }
        }

        /// <summary>
        /// Check bit marshaling alignment
        /// </summary>
        public void CheckBitAlignment()
        {
            if (bitQueue.Count != 0)
            {
                TestAssumeFail("Bit alignment failed");
            }
        }

        #endregion

        #region Context

        /// <summary>
        /// Enters a new context. 
        /// The context contains an environment for symbol lookup in attribute expressions. 
        /// </summary>
        public void EnterContext()
        {
            // Save symbol context
            SavedContext c = new SavedContext();
            c.symbolStore = symbolStore;
            c.pointerSymbolStore = pointerSymbolStore;
            outerContexts.Push(c);
            symbolStore = null;
            pointerSymbolStore = null;

            // Save array context
            ArrayContext arrayContext = new ArrayContext();
            arrayContext.arrayLevel = arrayLevel;
            arrayContext.arraySizes = arraySizes;
            arrayContext.arrayLengths = arrayLengths;
            outerArrayContexts.Push(arrayContext);

            arrayLevel = 0;
            arraySizes = null;
            arrayLengths = null;

            // Save sequence context
            SequenceContext sequenceContext = new SequenceContext();
            sequenceContext.sequenceLevel = sequenceLevel;
            sequenceContext.sequenceSizes = sequenceSizes;
            sequenceContext.sequenceLengths = sequenceLengths;
            outerSequenceContexts.Push(sequenceContext);

            sequenceLevel = 0;
            sequenceSizes = null;
            sequenceLengths = null;
        }

        /// <summary>
        /// Exits context and restores the old one.
        /// </summary>
        public void ExitContext()
        {
            SavedContext c = outerContexts.Pop();
            symbolStore = c.symbolStore;
            pointerSymbolStore = c.pointerSymbolStore;

            ArrayContext arrayContext = outerArrayContexts.Pop();
            arrayLevel = arrayContext.arrayLevel;
            arraySizes = arrayContext.arraySizes;
            arrayLengths = arrayContext.arrayLengths;

            SequenceContext sequenceContext = outerSequenceContexts.Pop();
            sequenceLevel = sequenceContext.sequenceLevel;
            sequenceSizes = sequenceContext.sequenceSizes;
            sequenceLengths = sequenceContext.sequenceLengths;
        }

        /// <summary>
        /// Defines a custom and its value in the current context.
        /// </summary>
        /// <param name="typeName">The custom type to be defined</param>
        /// <param name="value">The value corresponding to the custom type</param>
        public void DefineCustomType(string typeName, object value)
        {
            if (customTypes == null)
                customTypes = new Dictionary<string, object>();
            customTypes[typeName] = value;
        }

        /// <summary>
        /// Defines a symbol and its value in the current context.
        /// </summary>
        /// <param name="symbol">The symbol to be defined</param>
        /// <param name="value">The value corresponding to the symbol</param>
        public void DefineSymbol(string symbol, object value)
        {
            if (symbolStore == null)
                symbolStore = new Dictionary<string, object>();
            symbolStore[symbol] = value;
        }

        /// <summary>
        /// Define a pointer symbol and its value in the current context.
        /// </summary>
        /// <param name="symbol">The pointerlevel symbol to be defined</param>
        /// <param name="value">The value for this symbol</param>
        public void DefinePointerSymbol(string symbol, object value)
        {
            if (pointerSymbolStore == null)
                pointerSymbolStore = new Dictionary<string, object>();
            pointerSymbolStore[symbol] = value;
        }

        /// <summary>
        /// Defines symbols
        /// </summary>
        /// <param name="fields">The fields corresponding to the symbols</param>
        /// <param name="value">The values corresponding to the symbols</param>
        public void DefineSymbols(FieldInfo[] fields, object value)
        {
            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }

            foreach (FieldInfo field in fields)
            {
                MarshalingDescriptor fdesc = new MarshalingDescriptor(field.FieldType, field);
                object fieldValue = field.GetValue(value);

                Type nt;
                try
                {
                    nt = GetNativeType(fdesc);
                }
                catch (InvalidOperationException) // some types does not have native type.
                {
                    nt = null;
                }

                if (nt == typeof(IntPtr))
                {
                    DefinePointerSymbol(field.Name, fieldValue);
                }
                else
                {
                    DefineSymbol(field.Name, fieldValue);
                }
            }
        }

        /// <summary>
        /// Gets or set the number of bytes to the region
        /// </summary>
        public int RegionOffset
        {
            get
            {
                return region.Offset;
            }

            set
            {
                region.Offset = value;
            }
        }

        /// <summary>
        /// Gets or set the boolean value which indicate
        /// whether check left space when do marshaling/unmarshaling
        /// </summary>
        public bool UseSpaceChecking
        {
            get
            {
                return region.UseSpaceChecking;
            }

            set
            {
                region.UseSpaceChecking = value;
            }
        }

        /// <summary>
        /// Tries to resolve a symbol to an integer value.
        /// </summary>
        /// <param name="symbol">The symbol to be resolved</param>
        /// <param name="value">The value corresponding to the symbol</param>
        /// <returns>Return true if the symbol is defined and has a value</returns>
        public bool TryResolveSymbol(string symbol, out int value)
        {
            if (symbolStore != null)
            {
                object valueObj;
                if (symbolStore.TryGetValue(symbol, out valueObj))
                {
                    if (valueObj == null)
                    {
                        value = 0;
                        return true;
                    }
                    else if (valueObj is IntPtr)
                    {
                        value = ((IntPtr)valueObj).ToInt32();
                        return true;
                    }
                    else
                    {
                        try
                        {
                            long temp = Convert.ToInt64(valueObj, CultureInfo.InvariantCulture);
                            value = (int)temp;

                            return true;
                        }
                        catch (ArgumentException)
                        {
                            value = 0;
                            return false;
                        }
                        catch (OverflowException)
                        {
                            value = 0;
                            return false;
                        }
                        catch (FormatException)
                        {
                            value = 0;
                            return false;
                        }
                    }
                }
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// Inherit from IEvaluationContext, currently not used in Marshaler.
        /// </summary>
        /// <param name="typeName">The custom type to be resolved</param>
        /// <param name="size">The actual size of the custom type</param>
        /// <returns>Returns true if it resolves the type successfully.</returns>
        public bool TryResolveCustomType(string typeName, out string size)
        {
            object type;
            if (customTypes.TryGetValue(typeName, out type))
            {
                Type customType = (Type)type;
                MarshalingDescriptor descriptor = new MarshalingDescriptor(customType);
                ITypeMarshaler msr = GetMarshaler(descriptor);
                size = msr.GetSize(this, descriptor, null).ToString(CultureInfo.InvariantCulture);
                return true;
            }
            else
            {
                size = "0";
                return false;
            }
        }

        #endregion

        #region Region

        /// <summary>
        /// Implements IRegion in memory.
        /// </summary>
        class MemoryRegion : IRegion
        {
            Marshaler marshaler;
            IntPtr region;
            int regionOffs;
            int regionSize;
            bool useSpaceChecking = true;

            internal MemoryRegion(Marshaler marshaler, IntPtr ptr, int size)
            {
                this.marshaler = marshaler;
                this.region = ptr;
                this.regionSize = size;
            }


            void CheckSpace(int s)
            {
                if (useSpaceChecking)
                {
                    if (regionOffs + s > regionSize)
                        marshaler.TestAssumeFail("attempt to read/write {0} bytes behind allocated memory region",
                                            regionOffs + s - regionSize);
                }
            }


            #region IRegion members

            public IntPtr NativeMemory
            {
                get { return region; }
            }

            public void WriteByte(byte x)
            {
                CheckSpace(1);
                IOP.Marshal.WriteByte(region, regionOffs, x);
                regionOffs += 1;
            }

            public void WriteInt16(short x)
            {
                CheckSpace(2);
                IOP.Marshal.WriteInt16(region, regionOffs, x);
                regionOffs += 2;
            }

            public void WriteInt32(int x)
            {
                CheckSpace(4);
                IOP.Marshal.WriteInt32(region, regionOffs, x);
                regionOffs += 4;
            }

            public void WriteInt64(long x)
            {
                CheckSpace(8);
                IOP.Marshal.WriteInt64(region, regionOffs, x);
                regionOffs += 8;
            }

            public void WriteIntPtr(IntPtr x)
            {
                CheckSpace(IntPtr.Size);
                IOP.Marshal.WriteIntPtr(region, regionOffs, x);
                regionOffs += IntPtr.Size;
            }

            public byte ReadByte()
            {
                CheckSpace(1);
                byte r = IOP.Marshal.ReadByte(region, regionOffs);
                regionOffs += 1;
                return r;
            }

            public short ReadInt16()
            {
                CheckSpace(2);
                short r = IOP.Marshal.ReadInt16(region, regionOffs);
                regionOffs += 2;
                return r;
            }

            public int ReadInt32()
            {
                CheckSpace(4);
                int r = IOP.Marshal.ReadInt32(region, regionOffs);
                regionOffs += 4;
                return r;
            }

            public long ReadInt64()
            {
                CheckSpace(8);
                long r = IOP.Marshal.ReadInt64(region, regionOffs);
                regionOffs += 8;
                return r;
            }

            public IntPtr ReadIntPtr()
            {
                int s = IntPtr.Size;
                CheckSpace(s);
                IntPtr r = IOP.Marshal.ReadIntPtr(region, regionOffs);
                regionOffs += s;
                return r;
            }

            public int Offset
            {
                get
                {
                    return regionOffs;
                }

                set
                {
                    regionOffs = value;
                }
            }

            public int SpaceLeft
            {
                get
                {
                    return regionSize - regionOffs;
                }
            }

            public bool TryReset()
            {
                regionOffs = 0;
                return true;
            }

            public bool UseSpaceChecking
            {
                get
                {
                    return useSpaceChecking;
                }

                set
                {
                    useSpaceChecking = value;
                }
            }

            #endregion

        }

        /// <summary>
        /// Allocates region which is automatically freed by Dispose or 
        /// a call to <see cref="FreeMemory">FreeMemory</see>.
        /// </summary>
        /// <param name="size">The size of memory to be allocated in the region</param>
        /// <returns>The allocated region</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public IRegion AllocateRegion(int size)
        {
            IntPtr r = IOP.Marshal.AllocHGlobal(size);
            allocatedMemory.Add(r);
            return new MemoryRegion(this, r, size);
        }

        /// <summary>
        /// Allocates a region from the given unmanaged memory.
        /// </summary>
        /// <param name="ptr">The pointer which points to native memory.</param>
        /// <param name="size">The size of the unmanaged memory to be allocated in the region</param>
        /// <returns>The allocated region</returns>
        public IRegion MakeRegion(IntPtr ptr, int size)
        {
            return new MemoryRegion(this, ptr, size);
        }

        /// <summary>
        /// Marks an externally allocated region of memory for dispose.
        /// </summary>
        /// <param name="ptr">The pointer which points to native memory</param>        
        public void MarkMemoryForDispose(IntPtr ptr)
        {
            allocatedMemory.Add(ptr);
        }

        /// <summary>
        /// Marks an region of memory allocated by RPC runtime for dispose.
        /// </summary>
        /// <param name="ptr">The pointer which points to native memory</param>
        public void MarkForeignMemoryForDispose(IntPtr ptr)
        {
            if (!IsProbingUnmarshaling && ptr != IntPtr.Zero)
            {
                ForeignMemory.Add(ptr);
            }
        }

        /// <summary>
        /// Disposes the marshaler and frees all allocated regions.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes this object.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        ~Marshaler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <param name="disposing">Indicates if Dispose is called by user.</param>
        [SecurityPermission(SecurityAction.Demand)]
        protected virtual void Dispose(bool disposing)
        {
            FreeMemory();
        }

        /// <summary>
        /// Frees allocated regions and leaves the marshaler functional for future use.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        public void FreeMemory()
        {
            foreach (IntPtr r in allocatedMemory)
                IOP.Marshal.FreeHGlobal(r);
            allocatedMemory.Clear();
        }

        /// <summary>
        /// Enters a region into which marshaling calls should write. 
        /// The current active region is to be saved.
        /// </summary>
        /// <param name="newRegion">The new region to be entered</param>
        public void EnterRegion(IRegion newRegion)
        {

            outerRegions.Push(region);
            region = newRegion;
        }

        /// <summary>
        /// Exits the last entered region and restores the old region.
        /// </summary>
        public void ExitRegion()
        {
            int s = region.SpaceLeft;
            if (s > 0)
                this.Trace("region has unused space of {0} bytes", s);
            region = outerRegions.Pop();
        }

        #endregion

        #region Abstract identifier binding

        /// <summary>
        /// Gets the identifier binding for the given binding name.
        /// </summary>
        /// <param name="name">The binding name.</param>
        /// <returns>The identifier binding.</returns>
        public IdentifierBinding<object> GetBinding(string name)
        {
            IdentifierBinding<object> binding;
            lock (identifierBindingLock)
            {
                if (!bindings.TryGetValue(name, out binding))
                {
                    if (Host != null)
                    {
                        bindings[name] = binding = new IdentifierBinding<object>(Host);
                    }
                    else
                    {
                        bindings[name] = binding = new IdentifierBinding<object>();
                    }
                }
            }

            return binding;
        }

        #endregion

        #region Reading and Writing

        /// <summary>
        /// Writes a bit to the current region.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        public void WriteBit(int value)
        {
            bitQueue.Enqueue(value);

            while (bitQueue.Count >= 8)
            {
                byte outgoingByte = 0;

                for (int i = 0; i < 8; i++)
                {
                    int bitValue = bitQueue.Dequeue();
                    outgoingByte |= (byte)(bitValue << (7 - i));
                }

                region.WriteByte(outgoingByte);
            }
        }

        /// <summary>
        /// Writes a byte to the current region.
        /// </summary>
        /// <param name="value">The value to be written</param>
        public void WriteByte(byte value)
        {
            if (tracing)
                Trace("writing 0x{0:x2}", value);
            region.WriteByte(value);
        }

        /// <summary>
        /// Writes a 16-bit integer to the current region.
        /// </summary>
        /// <param name="value">The value to be written</param>
        public void WriteInt16(short value)
        {
            if (tracing)
                Trace("writing 0x{0:x4}", value);
            region.WriteInt16(value);
        }

        /// <summary>
        /// Writes a 32-bit integer to the current region.
        /// </summary>
        /// <param name="value">The value to be written</param>
        public void WriteInt32(int value)
        {
            if (tracing)
                Trace("writing 0x{0:x8}", value);
            region.WriteInt32(value);
        }

        /// <summary>
        /// Writes a 64-bit integer to the current region.
        /// </summary>
        /// <param name="value">The value to be written</param>
        public void WriteInt64(long value)
        {
            if (tracing)
                Trace("writing 0x{0:x16}", value);
            region.WriteInt64(value);
        }

        /// <summary>
        /// Writes an IntPtr to the current region.
        /// </summary>
        /// <param name="value">The value to be written</param>
        public void WriteIntPtr(IntPtr value)
        {
            if (tracing)
                Trace("writing @0x{0:x4}", value);
            region.WriteIntPtr(value);
        }

        /// <summary>
        /// Writes a managed structure to the current region.
        /// </summary>
        /// <param name="value">The value to be written</param>
        /// <param name="size">The unmanaged size of the structure</param>
        [SecurityPermission(SecurityAction.Demand)]
        public void WriteStructure(object value, int size)
        {
            IntPtr rawBuffer = IOP.Marshal.AllocHGlobal(size);
            MarkMemoryForDispose(rawBuffer);
            IOP.Marshal.StructureToPtr(value, rawBuffer, false);

            byte[] bytes = new byte[size];
            IOP.Marshal.Copy(rawBuffer, bytes, 0, size);
            foreach (byte b in bytes)
            {
                WriteByte(b);
            }
        }

        /// <summary>
        /// Reads a bit from the current region.
        /// </summary>
        /// <returns>The value.</returns>
        public int ReadBit()
        {
            while (bitQueue.Count <= 0)
            {
                byte incomingByte = 0;
                incomingByte = region.ReadByte();

                for (int i = 0; i < 8; i++)
                {
                    int bitValue = (incomingByte >> (7 - i)) & 0x1;
                    bitQueue.Enqueue(bitValue);
                }
            }

            return bitQueue.Dequeue();
        }

        /// <summary>
        /// Reads a byte from the current region.
        /// </summary>
        /// <returns>The value</returns>
        public byte ReadByte()
        {
            byte r = region.ReadByte();
            if (tracing)
                Trace("reading 0x{0:x2}", r);
            return r;
        }

        /// <summary>
        /// Reads a 16-bit integer from the current region and increments the region pointer.
        /// </summary>
        /// <returns>The value</returns>
        public short ReadInt16()
        {
            short r = region.ReadInt16();
            if (tracing)
                Trace("reading 0x{0:x4}", r);
            return r;
        }

        /// <summary>
        /// Reads a 32-bit integer from the current region and increments the region pointer.
        /// </summary>
        /// <returns>The value</returns>
        public int ReadInt32()
        {
            int r = region.ReadInt32();
            if (tracing)
                Trace("reading 0x{0:x8}", r);
            return r;
        }

        /// <summary>
        /// Reads a 64-bit integer from the current region and increments the region pointer.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadInt64()
        {
            long r = region.ReadInt64();
            if (tracing)
                Trace("reading 0x{0:x16}", r);
            return r;
        }

        /// <summary>
        /// Reads an IntPtr from the current region and increments the region pointer.
        /// </summary>
        /// <returns>The value</returns>
        public IntPtr ReadIntPtr()
        {
            int size = GetIntPtrSize();
            IntPtr r = new IntPtr(size == 4 ? region.ReadInt32() : region.ReadInt64());
            if (tracing)
                Trace("reading @0x{0:x4}", r);
            return r;
        }

        /// <summary>
        /// Reads a managed structure from the current region.
        /// </summary>
        /// <param name="type">The type of the structure to be read from current region</param>
        /// <param name="size">The number of bytes to be read from current region</param>
        /// <returns>The value</returns>
        [SecurityPermission(SecurityAction.Demand)]
        public object ReadStructure(Type type, int size)
        {
            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                bytes[i] = ReadByte();
            }

            IntPtr rawBuffer = IOP.Marshal.AllocHGlobal(size);
            MarkMemoryForDispose(rawBuffer);

            IOP.Marshal.Copy(bytes, 0, rawBuffer, size);
            return IOP.Marshal.PtrToStructure(rawBuffer, type);
        }

        /// <summary>
        /// Skips the given number of bytes for reading.
        /// </summary>
        /// <param name="bytes">The number of bytes which are skipped</param>
        public void Skip(int bytes)
        {
            while (bytes-- > 0)
                ReadByte();
        }

        /// <summary>
        /// Writes zeros for the given number of bytes.
        /// </summary>
        /// <param name="bytes">The number of bytes to be written.</param>
        public void Clear(int bytes)
        {
            while (bytes-- > 0)
                WriteByte(0);
        }

        internal void AlignWrite(int align)
        {
            int offset = region.Offset;
            int newOffset = Pad(offset, align);
            Clear(newOffset - offset);
        }

        internal void AlignRead(int align)
        {
            int offset = region.Offset;
            int newOffset = Pad(offset, align);
            Skip(newOffset - offset);
        }

        internal static int Pad(int offset, int alignment)
        {
            if (alignment == 0)
                return offset;

            while (offset % alignment != 0)
                offset++;
            return offset;
        }

        internal static int GetAlignment(Type type)
        {
            int size = IOP.Marshal.SizeOf(type);
            if (size > 4) // FIXME: x64 alignment rules?
                return 4;
            else
                return size;
        }

        #endregion

        #region Marshaling

        /// <summary>
        /// Returns the type marshaler determined by the marshaling configuration.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The type marshaler.</returns>
        public ITypeMarshaler GetMarshaler(MarshalingDescriptor descriptor)
        {
            return config.GetMarshaler(this, descriptor);
        }

        /// <summary>
        /// Computes the size of the given value. 
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The size of the given value.</returns>
        public int GetSize(object value)
        {
            if (value == null)
                return IntPtr.Size;

            return GetSize(new MarshalingDescriptor(value.GetType()), value);
        }

        /// <summary>
        /// Computes the size of the given marshaling descriptor and value. 
        /// It is the delegate to the configured type marshaler.
        /// If the size depends on the value and the value is unknown (null), a negative value maybe returned.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="value">The value.</param>
        /// <returns>The size of the given value.</returns>
        public int GetSize(MarshalingDescriptor descriptor, object value)
        {
            ITypeMarshaler msr = GetMarshaler(descriptor);
            return msr.GetSize(this, descriptor, value);
        }

        /// <summary>
        /// Computes the alignment of the given marshaling descriptor. 
        /// It is the delegate to the configured type marshaler.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The alignment.</returns>
        public int GetAlignment(MarshalingDescriptor descriptor)
        {
            ITypeMarshaler msr = GetMarshaler(descriptor);
            return msr.GetAlignment(this, descriptor);
        }

        /// <summary>
        /// Gets the native type of the given marshaling descriptor, if any. 
        /// It is the delegate to the configured type marshaler.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The native type.</returns>
        public Type GetNativeType(MarshalingDescriptor descriptor)
        {
            ITypeMarshaler msr = GetMarshaler(descriptor);
            return msr.GetNativeType(this, descriptor);
        }

        /// <summary>
        /// Marshals the given value. 
        /// It is the delegate to the configured type marshaler.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="value">The value to be marshaled.</param>
        /// <returns></returns>
        public void Marshal(MarshalingDescriptor descriptor, object value)
        {
            Trace("marshaling {0}", descriptor);
            ITypeMarshaler msr = GetMarshaler(descriptor);
            if (msr.GetType() != typeof(BitMarshaler))
            {
                CheckBitAlignment();
            }
            msr.Marshal(this, descriptor, value);
        }

        /// <summary>
        /// Marshals the given value into the specified region.
        /// This method enters the region, delegates to the configured type marshaler, and exits the region.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="customRegion">The custom region.</param>
        /// <param name="value">The value to be marshaled.</param>
        public void MarshalInto(MarshalingDescriptor descriptor, IRegion customRegion, object value)
        {
            Trace("marshaling {0}", descriptor);
            EnterRegion(customRegion);
            ITypeMarshaler msr = GetMarshaler(descriptor);
            msr.Marshal(this, descriptor, value);
            CheckBitAlignment();
            ExitRegion();
        }

        /// <summary>
        /// Unmarshals the given value. 
        /// It is the delegate to the configured type marshaler.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <returns>The unmarshaled value.</returns>
        public object Unmarshal(MarshalingDescriptor descriptor)
        {
            Trace("unmarshaling {0}", descriptor);
            ITypeMarshaler msr = GetMarshaler(descriptor);
            if (msr.GetType() != typeof(BitMarshaler))
            {
                CheckBitAlignment();
            }
            return msr.Unmarshal(this, descriptor);
        }

        /// <summary>
        /// Unmarshals the given value from the specified region. 
        /// This method enters the region, delegates to the configured type marshaler, and exits the region.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="customRegion">The custom region.</param>
        /// <returns>The unmarshaled value.</returns>
        public object UnmarshalFrom(MarshalingDescriptor descriptor, IRegion customRegion)
        {
            Trace("unmarshaling {0}", descriptor);
            EnterRegion(customRegion);
            ITypeMarshaler msr = GetMarshaler(descriptor);
            object r = msr.Unmarshal(this, descriptor);
            CheckBitAlignment();
            ExitRegion();
            return r;
        }

        #endregion

        #region Size/Length attribute evaluation

        /// <summary>
        /// Retrieves the advocated size and length of a marshaling descriptor by inspecting the
        /// attributes attached to it.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="size">The advocated size.</param>
        /// <param name="length">The advocated length.</param>
        public void GetAdvocatedSize(MarshalingDescriptor descriptor,
                                     out int size, out int length)
        {
            string sizeExpr;
            length = -1;
            if (MarshalingDescriptor.TryGetAttribute<int>(descriptor.Attributes, typeof(StaticSizeAttribute),
                                          "size", out size))
            {
                StaticSizeMode mode;
                if (!MarshalingDescriptor.TryGetAttribute<StaticSizeMode>(descriptor.Attributes,
                                                                                typeof(StaticSizeAttribute),
                                          "mode", out mode))
                    throw new InvalidOperationException("expected mode field in static size attribute");
                if (mode == StaticSizeMode.Bytes)
                {
                    Type elementType;
                    if (descriptor.Type.IsArray)
                        elementType = descriptor.Type.GetElementType();
                    else
                        elementType = descriptor.Type.GetGenericArguments()[0];
                    int s = GetSize(new MarshalingDescriptor(elementType), Activator.CreateInstance(elementType));
                    if (s < 0)
                        TestAssumeFail(descriptor, "must be able to determine size of element type");
                    size /= s;
                }

                if (size < 0)
                    TestAssumeFail(descriptor, "invalide static size '{0}'",
                                                size);
            }
            else
                size = -1;
            if (MarshalingDescriptor.TryGetAttribute<string>(
                            descriptor.Attributes, typeof(SizeAttribute),
                            "expr", out sizeExpr))
            {
                if (size >= 0)
                    TestAssumeFail(descriptor, "invalid combination of static with dynamic size");

                IList<int?> sizes = this.EvaluateSizeExpression(sizeExpr, descriptor);
                if (sizes.Count != 1)
                {
                    TestAssumeFail(descriptor, "size expression is invalid (expect one size value in the expression).");
                }

                if (sizes[0] == null)
                {
                    TestAssumeFail(descriptor, "evaluating size expression failed");
                }

                size = sizes[0].Value;

                if (size < 0)
                    TestAssertFail(descriptor, "invalid dynamic size '{0}'",
                                                size);
            }
            if (MarshalingDescriptor.TryGetAttribute<string>(
                            descriptor.Attributes, typeof(LengthAttribute),
                            "expr", out sizeExpr))
            {
                length = this.Evaluate(sizeExpr);
                if (length < 0)
                    TestAssertFail(descriptor, "invalid length '{0}'",
                                                length);
            }
            if (size < 0)
                size = length;
            if (size < 0)
                size = 1;
            if (length < 0)
                length = size;
            if (length > size)
                TestAssertFail(descriptor, "length '{0}' greater than size '{1}'",
                                            length, size);
        }

        /// <summary>
        /// Retrieves the advocated size list and length list of a marshaling descriptor by inspecting the
        /// attributes attached to it.
        /// </summary>
        /// <param name="descriptor">The marshaling descriptor.</param>
        /// <param name="sizes">The advocated size list.</param>
        /// <param name="lengths">The advocated length list.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public void GetMultipleAdvocatedSizes(MarshalingDescriptor descriptor,
                                     out IList<int> sizes, out IList<int> lengths)
        {
            string sizeExpr;
            sizes = new List<int>();
            lengths = new List<int>();
            int size;

            // Evaluate static size expression
            if (MarshalingDescriptor.TryGetAttribute<int>(descriptor.Attributes, typeof(StaticSizeAttribute),
                                          "size", out size))
            {
                StaticSizeMode mode;
                if (!MarshalingDescriptor.TryGetAttribute<StaticSizeMode>(descriptor.Attributes,
                                                                                typeof(StaticSizeAttribute),
                                          "mode", out mode))
                    throw new InvalidOperationException("expected mode field in static size attribute");
                if (mode == StaticSizeMode.Bytes)
                {
                    Type elementType;
                    if (descriptor.Type.IsArray)
                        elementType = descriptor.Type.GetElementType();
                    else
                        elementType = descriptor.Type.GetGenericArguments()[0];
                    int s = GetSize(new MarshalingDescriptor(elementType), Activator.CreateInstance(elementType));
                    if (s < 0)
                        TestAssumeFail(descriptor, "must be able to determine size of element type");
                    size /= s;
                }

                if (size < 0)
                    TestAssumeFail(descriptor, "invalide static size '{0}'",
                                                size);
                sizes.Add(size);
            }

            // Evaluate size expression
            if (MarshalingDescriptor.TryGetAttribute<string>(
                            descriptor.Attributes, typeof(SizeAttribute),
                            "expr", out sizeExpr))
            {
                if (sizes.Count > 0)
                    TestAssumeFail(descriptor, "invalid combination of static with dynamic size");

                IList<int?> nullableSizes = this.EvaluateSizeExpression(sizeExpr, descriptor);

                if (nullableSizes.Count > 0 && nullableSizes[0] == null)
                {
                    nullableSizes[0] = 1;
                }
                foreach (int? value in nullableSizes)
                {
                    if (value == null)
                    {
                        TestAssertFail(descriptor, "invalid dynamic size expression '{0}'", sizeExpr);
                    }

                    if (value < 0)
                    {
                        TestAssertFail(descriptor, "invalid dynamic size '{0}'", value);
                    }


                    sizes.Add((int)value);
                }
            }

            // Evaluate length expression
            if (MarshalingDescriptor.TryGetAttribute<string>(
                            descriptor.Attributes, typeof(LengthAttribute),
                            "expr", out sizeExpr))
            {
                IList<int?> nullableLengths = this.EvaluateMultipleExpressions(sizeExpr);
                if (nullableLengths.Count != sizes.Count)
                {
                    TestAssertFail(descriptor, "the number of size values in size attribute is different from the number of length values in length attribute");
                }

                if (nullableLengths.Count > 0 && nullableLengths[0] == null)
                {
                    nullableLengths[0] = 1;
                }

                for (int i = 0; i < nullableLengths.Count; i++)
                {
                    if (nullableLengths[i] == null)
                    {
                        TestAssertFail(descriptor, "invalid length expression '{0}'", sizeExpr);
                    }

                    lengths.Add((int)nullableLengths[i]);

                    if (lengths[i] < 0)
                        TestAssertFail(descriptor, "invalid length '{0}'", lengths[i]);

                    if (lengths[i] > sizes[i])
                        TestAssertFail(descriptor, "length '{0}' is greater than size '{1}'",
                                                    lengths[i], sizes[i]);
                }
            }

            if (sizes.Count == 0)
            {
                size = 1;
                sizes.Add(size);
                lengths.Add(size);
            }

            if (lengths.Count == 0)
            {
                lengths = sizes;
            }
        }

        IList<int?> EvaluateSizeExpression(string sizeExpr, MarshalingDescriptor descriptor)
        {
            IList<int?> nullableSizes = new List<int?>();

            // Do custom calculation when the first char is '@'
            if (sizeExpr.StartsWith("@", StringComparison.OrdinalIgnoreCase))
            {
                if (descriptor.ContainerType == null)
                {
                    TestAssumeFail(descriptor, "cannot find the type which contains custom calculation method");
                }

                if (sizeExpr.Length < 2)
                {
                    TestAssumeFail(descriptor, "cannot find method name in the size expression");
                }
                // remove the first char '@' to get method name.
                string methodName = sizeExpr.Substring(1);

                MethodInfo method = descriptor.ContainerType.GetMethod(
                    methodName,
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                if (method == null)
                {
                    TestAssumeFail(descriptor, "cannot find custom calculation method");
                }

                // Check the method signature.
                ParameterInfo[] paramInfos = method.GetParameters();
                if (paramInfos.Length == 1
                    && paramInfos[0].ParameterType == typeof(IEvaluationContext)
                    && method.ReturnParameter.ParameterType == typeof(int))
                {
                    int customSize = (int)method.Invoke(null, new object[] { this });
                    nullableSizes.Add(customSize);
                }
                else
                {
                    TestAssumeFail(descriptor, "the custom calculation method signature is invalid");
                }
            }
            else
            {
                nullableSizes = this.EvaluateMultipleExpressions(sizeExpr);
            }

            return nullableSizes;
        }
        #endregion

        #region Expression evaluation

        /// <summary>
        /// Evaluates the given attribute expression to an integer value by using the current
        /// context for symbol resolution.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>The value for the expression.</returns>
        public int Evaluate(string expression)
        {
            MultipleExpressionEvaluator evaluator = new MultipleExpressionEvaluator(this, expression);
            IList<object> result = evaluator.Evaluate();
            if (result.Count != 1 || result[0] == null)
                TestAssertFail("expected exactly one expression in attribute '{0}'", expression);
            return (int)result[0];
        }

        /// <summary>
        /// Evaluates the given attribute expression to a set of integer value by using the current
        /// context for symbol resolution.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>The value list for the expression.</returns>
        public IList<int?> EvaluateMultipleExpressions(string expression)
        {
            MultipleExpressionEvaluator evaluator = new MultipleExpressionEvaluator(this, expression);
            IList<object> result = evaluator.Evaluate();
            if (result.Count == 0)
                TestAssertFail("expected at least one expression in attribute '{0}'", expression);
            IList<int?> intResult = new List<int?>();
            foreach (object s in result)
            {
                intResult.Add(s as int?);
            }
            return intResult;
        }

        /// <summary>
        /// Reports error when evaluating the expression.
        /// </summary>
        /// <param name="message">The error message.</param>
        public void ReportError(string message)
        {
            TestAssertFail("expression evaluation failed: {0}", message);
        }

        /// <summary>
        /// Evaluates the given constant attribute expression to a list of integer values.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <returns>The list of integer values.</returns>
        public IList<int?> EvaluateConstant(string expression)
        {
            EnterContext(); // hide symbols so we only see constants
            MultipleExpressionEvaluator evaluator = new MultipleExpressionEvaluator(this, expression);
            IList<object> result = evaluator.Evaluate();
            ExitContext();
            IList<int?> intResult = new List<int?>();
            foreach (object s in result)
            {
                intResult.Add(s as int?);
            }
            return intResult;
        }

        /// <summary>
        /// Symbols defined in the current symbol context.
        /// </summary>
        public IDictionary<string, object> Variables
        {
            get
            {
                IDictionary<string, object> variables = new Dictionary<string, object>();
                if (symbolStore != null)
                {
                    foreach (KeyValuePair<string, object> kvp in symbolStore)
                    {
                        variables.Add(kvp);
                    }
                }

                if (pointerSymbolStore != null)
                {
                    foreach (KeyValuePair<string, object> kvp in pointerSymbolStore)
                    {
                        variables.Add(kvp);
                    }
                }
                return variables;
            }
        }

        /// <summary>
        /// Tries to dereference the pointer.
        /// </summary>
        /// <param name="variable">The pointer symbol name</param>
        /// <param name="value">The dereferenced value</param>
        /// <param name="pointerValue">The pointer value</param>
        /// <returns>Returns </returns>
        public bool TryResolveDereference(string variable, out int value, out int pointerValue)
        {
            value = 0;
            pointerValue = 1;
            object valueObj;
            if (pointerSymbolStore != null)
            {
                if (pointerSymbolStore.TryGetValue(variable, out valueObj))
                {
                    try
                    {
                        if (valueObj == null)
                        {
                            value = 0;
                            pointerValue = 0;
                        }
                        else
                        {
                            Type vt = valueObj.GetType();
                            if (vt.IsArray)
                            {
                                value = Convert.ToInt32(((Array)valueObj).GetValue(0), CultureInfo.InvariantCulture);
                                pointerValue = 1;
                            }
                            else
                            {
                                value = Convert.ToInt32(valueObj, CultureInfo.InvariantCulture);
                                pointerValue = 1;
                            }
                        }
                        return true;
                    }
                    catch (ArgumentException)
                    {
                        value = 0;
                        return false;
                    }
                    catch (OverflowException)
                    {
                        value = 0;
                        return false;
                    }
                    catch (FormatException)
                    {
                        value = 0;
                        return false;
                    }
                }
            }

            return false;
        }

        #endregion
    }

}
