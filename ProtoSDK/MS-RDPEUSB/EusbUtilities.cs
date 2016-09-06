// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// The builder is used to build various URB objects.
    /// </summary>
    public class UrbBuilder
    {
        #region Private Members

        private URB_FUNCTIONID function;
        private uint requestId;
        private byte noAck;

        #endregion

        #region Build Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="function">The URB function.</param>
        /// <param name="requestId">An ID that uniquely identifies the TRANSFER_IN_REQUEST or TRANSFER_OUT_REQUEST.</param>
        /// <param name="noAck">f this bit is nonzero the client should not send a URB_COMPLETION message for this 
        /// TRANSFER_OUT_REQUEST. This bit can be nonzero only if the NoAckIsochWriteJitterBufferSizeInMs field 
        /// in USB_DEVICE_CAPABILITIES is nonzero and URB Function is set to URB_FUNCTION_ISOCH_TRANSFER. 
        /// If the RequestId field is set to TRANSFER_IN_REQUEST, this field MUST be set to zero.</param>
        public UrbBuilder(URB_FUNCTIONID function, uint requestId, byte noAck)
        {
            this.function = function;
            this.requestId = requestId;
            this.noAck = noAck;
        }

        /// <summary>
        /// Builds a particular request for a string descriptor from the device.
        /// </summary>
        /// <param name="index">Specifies the device-defined index of the descriptor that is being retrieved or set.</param>
        /// <returns>The string descriptor request.</returns>
        public TS_URB_CONTROL_DESCRIPTOR_REQUEST BuildStringDescriptorRequest(byte index)
        {
            return BuildStringDescriptorRequest(index, 0x0409);
        }

        /// <summary>
        /// Builds a particular request for a string descriptor from the device.
        /// </summary>
        /// <param name="index">Specifies the device-defined index of the descriptor that is being retrieved or set.</param>
        /// <param name="languageId">Locale ID of the language.</param>
        /// <returns>The string descriptor request.</returns>
        public TS_URB_CONTROL_DESCRIPTOR_REQUEST BuildStringDescriptorRequest(byte index, ushort languageId)
        {
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new TS_URB_CONTROL_DESCRIPTOR_REQUEST();
            des.Header.Size = 12;
            des.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE;
            des.Header.RequestId = this.requestId;
            des.Header.NoAck = this.noAck;
            des.Index = index;
            des.DescriptorType = (byte)UsbDescriptorTypes.USB_STRING_DESCRIPTOR_TYPE;
            // English (U.S.)
            des.LanguageId = languageId;
            return des;
        }

        /// <summary>
        /// Builds a descriptor request to get the information of the whole device.
        /// </summary>
        /// <returns>The device descriptor request</returns>
        public TS_URB_CONTROL_DESCRIPTOR_REQUEST BuildDeviceDescriptorRequest()
        {
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new TS_URB_CONTROL_DESCRIPTOR_REQUEST();
            des.Header.Size = 12;
            des.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE;
            des.Header.RequestId = this.requestId;
            des.Header.NoAck = this.noAck;
            des.Index = 0; // this parameter not used for device descriptors
            des.DescriptorType = (byte)UsbDescriptorTypes.USB_DEVICE_DESCRIPTOR_TYPE;
            des.LanguageId = 0; // this parameter not used for device descriptors
            return des;
        }

        /// <summary>
        /// Builds a descriptor request to get the information of the configurations.
        /// </summary>
        /// <param name="index">number of configuration descriptor</param>
        /// <returns>The configuration descriptor request</returns>
        public TS_URB_CONTROL_DESCRIPTOR_REQUEST BuildConfigurationDescriptorRequest(byte index)
        {
            TS_URB_CONTROL_DESCRIPTOR_REQUEST des = new TS_URB_CONTROL_DESCRIPTOR_REQUEST();
            des.Header.Size = 12;
            des.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_GET_DESCRIPTOR_FROM_DEVICE;
            des.Header.RequestId = this.requestId;
            des.Header.NoAck = this.noAck;
            des.Index = index;
            des.DescriptorType = (byte)UsbDescriptorTypes.USB_CONFIGURATION_DESCRIPTOR_TYPE;
            des.LanguageId = 0; // this parameter not used for configuration descriptors
            return des;
        }

        /// <summary>
        /// Builds a configuration-selection request.
        /// </summary>
        /// <param name="interfaces">The interface information field.</param>
        /// <param name="configDescriptor">The configuration descriptor.</param>
        /// <returns>The configuration-selection request.</returns>
        public TS_URB_SELECT_CONFIGURATION BuildSelectConfigRequest(
            TS_USBD_INTERFACE_INFORMATION[] interfaces,
            byte[] configDescriptor
            )
        {
            TS_URB_SELECT_CONFIGURATION req = new TS_URB_SELECT_CONFIGURATION(
                this.requestId,
                this.noAck
                );
            // TS_URB_HEADER (8 bytes) + ConfigurationDescriptorIsValid (1 byte) + Padding (3 bytes)
            // NumInterfaces (4 bytes) + TS_USBD_INTERFACE_INFORMATION + USB_CONFIGURATION_DESCRIPTOR
            int interfaceLength = 0;
            foreach (TS_USBD_INTERFACE_INFORMATION i in interfaces)
            {
                interfaceLength += i.Length;
            }
            req.Header.Size = (ushort)(8 + 1 + 3 + 4 + interfaceLength + configDescriptor.Length);
            req.Header.URB_Function = URB_FUNCTIONID.URB_FUNCTION_SELECT_CONFIGURATION;
            req.Interfaces = interfaces;
            req.NumInterfaces = (uint)interfaces.Length;
            req.UsbConfDesc = configDescriptor;
            req.ConfigurationDescriptorIsValid = (configDescriptor != null ? (byte)1 : (byte)0);
            return req;
        }

        /// <summary>
        /// Builds an interface-selection request.
        /// </summary>
        /// <param name="configuration">The result of configuration-selection.</param>
        /// <param name="index">The index of the interface to be selected.</param>
        /// <returns>The interface-selection request.</returns>
        public TS_URB_SELECT_INTERFACE BuildSelectionInterfaceRequest(TS_URB_SELECT_CONFIGURATION_RESULT configuration, int index)
        {
            if (null == configuration)
            {
                throw new ArgumentNullException("configuration");
            }
            if (index >= configuration.Interface.Length || null == configuration.Interface[index])
            {
                throw new ArgumentException("index specified interface doesn't exist.");
            }

            TS_URB_SELECT_INTERFACE urb = new TS_URB_SELECT_INTERFACE(this.requestId, this.noAck);
            urb.ConfigurationHandle = configuration.ConfigurationHandle;
            urb.Header.Size = 8 + 4; // Header + ConfigurationHandle
            urb.TsUsbdIInfo = new TS_USBD_INTERFACE_INFORMATION();
            TS_USBD_INTERFACE_INFORMATION_RESULT inf = configuration.Interface[index];
            if (inf.NumberOfPipes != inf.Pipes.Length)
            {
                throw new ArgumentException(String.Format(
                    "The selecting interface count doesn't match. NumberOfPipes: {0}, Pipes count: {1}.",
                    inf.NumberOfPipes,
                    inf.Pipes.Length
                    ));
            }
            
            urb.TsUsbdIInfo.Length = 2 + 2 + 1 + 1 + 2 + 4;
            urb.TsUsbdIInfo.NumberOfPipesExpected = (ushort)inf.NumberOfPipes;
            urb.TsUsbdIInfo.InterfaceNumber = inf.InterfaceNumber;
            urb.TsUsbdIInfo.AlternateSetting = inf.AlternateSetting;
            urb.TsUsbdIInfo.Padding = PaddingGenerator.GeneratePadding();
            urb.TsUsbdIInfo.NumberOfPipes = inf.NumberOfPipes;

            urb.TsUsbdIInfo.Infomations = new TS_USBD_PIPE_INFORMATION[inf.NumberOfPipes];
            for (int i = 0; i < inf.NumberOfPipes; i++)
			{
                urb.TsUsbdIInfo.Length += 2 + 2 + 4 + 4; // count Pipe sizes
                urb.TsUsbdIInfo.Infomations[i] = new TS_USBD_PIPE_INFORMATION();
                urb.TsUsbdIInfo.Infomations[i].MaximumPacketSize = inf.Pipes[i].MaximumPacketSize;
                urb.TsUsbdIInfo.Infomations[i].Padding = PaddingGenerator.GeneratePadding();
                urb.TsUsbdIInfo.Infomations[i].MaximumTransferSize = inf.Pipes[i].MaximumTransferSize;
                urb.TsUsbdIInfo.Infomations[i].PipeFlags = inf.Pipes[i].PipeFlags;
			}

            urb.Header.Size += urb.TsUsbdIInfo.Length; // interface information size.
            return urb;
        }

        /// <summary>
        /// Builds a vendor command request.
        /// </summary>
        /// <param name="request">Specifies the USB or vendor-defined request code for the device, interface, endpoint, or other device-defined target. </param>
        /// <param name="value">Specifies a value, specific to Request, that becomes part of the USB-defined setup packet for the target. This value is defined by the creator of the code used in Request.</param>
        /// <param name="flag">Specifies zero, one, or a combination of the flags of type TransferFlags.</param>
        /// <returns>The vendor command request.</returns>
        public TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST BuildVendorCommandRequest(byte request, ushort value, TransferFlags flag)
        {
            TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST req = new TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST(
                this.requestId,
                this.noAck,
                URB_FUNCTIONID.URB_FUNCTION_VENDOR_DEVICE
                );
            req.Header.Size = 8 + 4 + 1 + 1 + 2 + 2 + 2;
            req.TransferFlags = (uint)flag;
            req.RequestTypeReservedBits = 0;
            req.Request = request;
            req.Value = value;
            req.Index = 0;
            req.Padding = 0;// PaddingGenerator.GeneratePadding();

            return req;
        }

        /// <summary>
        /// Overloaded method to build a partial vendor command request.
        /// </summary>
        /// <returns>The partial vendor command request.</returns>
        public TS_URB_CONTROL_VENDOR_OR_CLASS_REQUEST BuildVendorCommandRequest()
        {
            return BuildVendorCommandRequest(0, 0, TransferFlags.USBD_TRANSFER_DIRECTION_OUT);
        }

        /// <summary>
        /// Builds a request to receive data on a bulk pipe, or receive data on an interrupt pipe.
        /// </summary>
        /// <param name="endpoints">All information of the pipes to be searched.</param>
        /// <param name="endpointNumber">The endpoint number.</param>
        /// <param name="flag">The endpoint flag.</param>
        /// <param name="type">The type of the endpoint.</param>
        /// <returns>The request containing the pipe handle found to receive data on a bulk pipe, or receive data on an interrupt pipe.</returns>
        public TS_URB_BULK_OR_INTERRUPT_TRANSFER BuildInterruptTransferRequest(TS_USBD_PIPE_INFORMATION_RESULT[] endpoints, byte endpointNumber, TransferFlags flag, USBD_PIPE_TYPE type)
        {
            if (((flag & TransferFlags.USBD_TRANSFER_DIRECTION_IN) != TransferFlags.USBD_TRANSFER_DIRECTION_IN)
                && ((flag & TransferFlags.USBD_TRANSFER_DIRECTION_OUT) != TransferFlags.USBD_TRANSFER_DIRECTION_OUT))
            {
                throw new ArgumentException("flag must contain the direction flag.");
            }

            TS_URB_BULK_OR_INTERRUPT_TRANSFER req = new TS_URB_BULK_OR_INTERRUPT_TRANSFER(
                this.requestId,
                this.noAck
                );
            req.Header.URB_Function = this.function;

            req.Header.Size = 8 + 4 + 4;
            req.TransferFlags = (uint)flag;

            bool found = false;
            // Looking for the correct pipe handle.
            foreach (TS_USBD_PIPE_INFORMATION_RESULT e in endpoints)
            {
                byte num = (byte)(e.EndpointAddress & 0x0F);
                if (num == endpointNumber)
                {
                    VerifyEndpointDirection(e.EndpointAddress, flag);
                    
                    VerifyType(e.PipeType, type);

                    req.PipeHandle = e.PipeHandle;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new ArgumentException(String.Format("Specified endpoint ({0}) cannot be found.", endpointNumber));
            }

            return req;
        }

        /// <summary>
        /// Builds a request to clear a stall condition on an endpoint.
        /// </summary>
        /// <param name="pipeHandle">Specifies an opaque handle to the bulk or interrupt pipe.</param>
        /// <returns></returns>
        public TS_URB_PIPE_REQUEST BuildPipeRequest(uint pipeHandle)
        {
            TS_URB_PIPE_REQUEST req = new TS_URB_PIPE_REQUEST(
                this.requestId,
                this.noAck,
                this.function);
            req.Header.Size = 8 + 4;
            req.PipeHandle = pipeHandle;
            return req;
        }

        /// <summary>
        /// Builds a request to retrieve the current alternate interface setting for an interface in the current configuration.
        /// </summary>
        /// <param name="interfaceIndex">Specifies the device-defined index of the interface descriptor being retrieved.</param>
        /// <returns>The request to retrieve the current alternate interface setting.</returns>
        public TS_URB_CONTROL_GET_INTERFACE_REQUEST BuildGetInterfaceRequest(ushort interfaceIndex)
        {
            TS_URB_CONTROL_GET_INTERFACE_REQUEST req = new TS_URB_CONTROL_GET_INTERFACE_REQUEST(
                this.requestId,
                0);
            req.Interface = interfaceIndex;
            req.Padding = PaddingGenerator.GeneratePadding();
            return req;
        }

        /// <summary>
        /// Builds a request to retrieve Microsoft OS Feature Descriptors from a USB device or an interface on a USB device.
        /// </summary>
        /// <param name="recipient">Specifies whether the recipient is the USB device or an interface on the USB device.</param>
        /// <param name="interfaceNumber">Indicates the interface number that is the recipient of the request, if the Recipient member value is 1. Must be set to 0 if the USB device is the recipient.</param>
        /// <param name="msPageIndex">Must be set to 0. Page index of the 64K page of the MS OS Feature Descriptor to be returned. Current implementation only supports a maximum descriptor size of 4K.</param>
        /// <param name="msFeatureDescriptorIndex">Index for MS OS Feature Descriptor to be requested.</param>
        /// <returns>The request to retrieve Microsoft OS Feature Descriptors from a USB device or an interface on a USB device.</returns>
        public TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST BuildOSFeatureDescriptor(Recipient recipient, byte interfaceNumber, byte msPageIndex, ushort msFeatureDescriptorIndex)
        {
            TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST req = new TS_URB_OS_FEATURE_DESCRIPTOR_REQUEST(
                this.requestId,
                this.noAck
                );
            req.Recipient = (byte)recipient;
            req.InterfaceNumber = recipient == Recipient.Interface ? (byte)interfaceNumber : (byte)0;
            req.MS_PageIndex = msPageIndex;
            req.MS_FeatureDescriptorIndex = msFeatureDescriptorIndex;
            return req;
        }

        #endregion

        #region Private Methods

        private void VerifyEndpointDirection(byte endpointAddress, TransferFlags expectedFlag)
        {
            // Verify direction
            bool isFlagDirIn =
                (expectedFlag & TransferFlags.USBD_TRANSFER_DIRECTION_IN) == TransferFlags.USBD_TRANSFER_DIRECTION_IN;
            bool isEndpointDirIn =
                (endpointAddress >> 7) == 1;
            if (isFlagDirIn != isEndpointDirIn)
            {
                throw new ArgumentException(String.Format(
                    "flag is not consistent to endpoint direction. flag: {0}, Endpoint: {1}.",
                    expectedFlag,
                    isEndpointDirIn ? "In" : "Out"
                    ));
            }
        }

        private void VerifyType(uint pipeType, USBD_PIPE_TYPE expectedType)
        {
            if ((USBD_PIPE_TYPE)pipeType != expectedType)
            {
                throw new ArgumentException(String.Format(
                    "type is not consistent to endpoint type. type: {0}, Endpoint: {1}.",
                    (USBD_PIPE_TYPE)pipeType,
                    expectedType
                ));
            }
        }

        #endregion
    }

    /// <summary>
    /// Used to parse a structure instance from a buffer.
    /// </summary>
    public class UsbStructParser
    {
        /// <summary>
        /// Parses the structure instance from an EusbUrbCompletionPdu object.
        /// </summary>
        /// <typeparam name="T">The type of the structure</typeparam>
        /// <param name="pdu">The an EusbUrbCompletionPdu object containing the buffer to be parsed</param>
        /// <returns>The structure instance.</returns>
        public static T Parse<T>(EusbUrbCompletionPdu pdu) where T : UsbStructure, new()
        {
            if (null == pdu.OutputBuffer || 
                pdu.OutputBufferSize <= 0 ||
                pdu.HResult != (uint)HRESULT_FROM_WIN32.ERROR_SUCCESS)
            {
                return default(T);
            }

            T us = new T();
            if (!PduMarshaler.Unmarshal(pdu.OutputBuffer, us))
            {
                return default(T);
            }
            return us;
        }
    }

    /// <summary>
    /// The parser used to parse the result of USB configuration.
    /// </summary>
    public class UsbConfigurationParser
    {
        #region Private Members

        private List<TS_USBD_INTERFACE_INFORMATION> interfaces = new List<TS_USBD_INTERFACE_INFORMATION>();

        private MemoryStream ms = null;

        // In Windows XP, Windows Server 2003 and later operating system, the MaximumTransferSize member of 
        // the USBD_PIPE_INFORMATION structure is obsolete.
        private const uint USBD_DEFAULT_MAXIMUM_TRANSFER_SIZE = uint.MaxValue;

        #endregion

        #region Public Members

        /// <summary>
        /// The parsed TS_USBD_INTERFACE_INFORMATION result.
        /// </summary>
        public TS_USBD_INTERFACE_INFORMATION[] Interfaces
        {
            get { return interfaces.ToArray(); }
        }

        /// <summary>
        /// The parsed USB_CONFIGURATION_DESCRIPTOR result.
        /// </summary>
        public byte[] configDescriptor { get; private set; }

        /// <summary>
        /// Parses all information of the configuration result.
        /// </summary>
        /// <param name="pdu">The EusbUrbCompletionPdu to be parsed.</param>
        /// <returns>true indicates successful.</returns>
        public bool ParseAll(EusbUrbCompletionPdu pdu)
        {
            if (pdu.OutputBuffer == null)
            {
                return false;
            }
            ms = new MemoryStream(pdu.OutputBuffer);

            USB_CONFIGURATION_DESCRIPTOR cd = UsbStructParser.Parse<USB_CONFIGURATION_DESCRIPTOR>(pdu);
            ms.Seek(USB_CONFIGURATION_DESCRIPTOR.DefaultSize, SeekOrigin.Begin);

            if (ms.Length != cd.wTotalLength)
            {
                return false;
            }

            configDescriptor = pdu.OutputBuffer;

            USB_INTERFACE_DESCRIPTOR interfaceDes = (USB_INTERFACE_DESCRIPTOR)GetNextDescriptor(UsbDescriptorTypes.USB_INTERFACE_DESCRIPTOR_TYPE);
            while (null != interfaceDes)
            {
                TS_USBD_INTERFACE_INFORMATION interfaceInfo = new TS_USBD_INTERFACE_INFORMATION();
                interfaceInfo.NumberOfPipesExpected = interfaceDes.bNumEndpoints;
                interfaceInfo.InterfaceNumber = interfaceDes.bInterfaceNumber;
                interfaceInfo.AlternateSetting = interfaceDes.bAlternateSetting;
                // Padding can be set to any value and MUST be ignored upon receipt.
                interfaceInfo.Padding = PaddingGenerator.GeneratePadding();
                GeneratePipeInformation(interfaceDes, interfaceInfo);
                interfaces.Add(interfaceInfo);

                interfaceDes = (USB_INTERFACE_DESCRIPTOR)GetNextDescriptor(UsbDescriptorTypes.USB_INTERFACE_DESCRIPTOR_TYPE);
            }

            return true;
        }

        #endregion

        #region Private Methods

        private void GeneratePipeInformation(USB_INTERFACE_DESCRIPTOR interfaceDes, TS_USBD_INTERFACE_INFORMATION interfaceInfo)
        {
            uint pipeCount = interfaceDes.bNumEndpoints;
            interfaceInfo.NumberOfPipes = pipeCount;
            interfaceInfo.Infomations = new TS_USBD_PIPE_INFORMATION[pipeCount];

            ushort lenExpectPipeInfo = 2 + 2 + 1 + 1 + 2 + 4;
            interfaceInfo.Length = (ushort)(lenExpectPipeInfo + pipeCount * (2 + 2 + 4 + 4));

            for (int i = 0; i < pipeCount; i++)
            {
                interfaceInfo.Infomations[i] = new TS_USBD_PIPE_INFORMATION();
                // TODO:
                // In Windows XP, Windows Server 2003 and later operating system, Alt settings for an interface or restrict the maximum packet size by setting 
                // MaximumPacketSize to some value less than or equal to the value of of wMaxPacketSize defined in device firmware for the current Alt settings.
                interfaceInfo.Infomations[i].MaximumPacketSize = 0;
                interfaceInfo.Infomations[i].Padding = PaddingGenerator.GeneratePadding();
                // In Windows XP, Windows Server 2003 and later operating system, the MaximumTransferSize member of the USBD_PIPE_INFORMATION structure is obsolete.
                interfaceInfo.Infomations[i].MaximumTransferSize = USBD_DEFAULT_MAXIMUM_TRANSFER_SIZE;
                interfaceInfo.Infomations[i].PipeFlags = 0;
            }
        }

        private UsbDescriptorStructure GetNextDescriptor(UsbDescriptorTypes descriptorType)
        {
            byte[] temp = new byte[2];
            UsbDescriptorStructure result = null;

            while (ms.Position < ms.Length - 2)
            {
                if (2 != ms.Read(temp, 0, 2))
                {
                    return null;
                }

                UsbDescriptorStructure des = new UsbDescriptorStructure();
                PduMarshaler.Unmarshal(temp, des);

                if (des.bDescriptorType == descriptorType)
                {
                    ms.Seek(-2, SeekOrigin.Current);
                    
                    switch (des.bDescriptorType)
                    {

                        case UsbDescriptorTypes.USB_INTERFACE_DESCRIPTOR_TYPE:
                            result = new USB_INTERFACE_DESCRIPTOR();
                            if (ParseDescriptor(USB_INTERFACE_DESCRIPTOR.DefaultSize, result))
                            {
                                return result;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                            //break;
                    }
                }
                else
                {
                    ms.Seek(des.bLength - 1, SeekOrigin.Current);
                }
            }
            return result;
        }

        private bool ParseDescriptor(int length, UsbDescriptorStructure des)
        {
            byte[] temp = new byte[length];
            if(length != ms.Read(temp, 0, length))
            {
                return false;
            }

            return PduMarshaler.Unmarshal(temp, des);
        }

        #endregion
    }

    /// <summary>
    /// Test helper class used to generate a unique unsigned integer ID.
    /// </summary>
    public static class IdGenerator
    {
        private static uint NextId = 0;

        /// <summary>
        /// Generates a new ID.
        /// </summary>
        /// <returns>Next avaliable ID.</returns>
        public static uint NewId()
        {
            if (NextId == uint.MaxValue)
            {
                NextId = 0;
                return uint.MaxValue;
            }
            return NextId++;
        }
    }

    /// <summary>
    /// Test helper class used to generate random paddings.
    /// </summary>
    public static class PaddingGenerator
    {
        /// <summary>
        /// Generates randmon ushort padding.
        /// </summary>
        /// <returns>Padding</returns>
        public static ushort GeneratePadding()
        {
            return (ushort)(new Random().Next());
        }

        /// <summary>
        /// Generates random byte array padding
        /// </summary>
        /// <param name="length">The length of the padding buffer.</param>
        /// <returns>Padding</returns>
        public static byte[] GeneratePadding(int length)
        {
            byte[] p = new byte[length];
            new Random().NextBytes(p);
            return p;
        }
    }
}
