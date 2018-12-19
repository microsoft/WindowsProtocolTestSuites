using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace CodecToolSet.Core.RFXDecode
{
    class RFXDecoderWrapper : RemoteFXDecoder
    {
        public static void RLGRDecode(byte[] input, EntropyAlgorithm mode, out short[] output)
        {
            var rfxMode = (Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx.EntropyAlgorithm)mode.Algorithm;
            var RLGRDecoder = new RLGRDecoder();
            int comLength = RdpegfxTileUtils.TileSize * RdpegfxTileUtils.TileSize;
            output = RLGRDecoder.Decode(input, rfxMode, comLength);
        }

        public static void SubBandReconstruction(short[] input, out short[,] output)
        {
            reconstruction_Component(input, out output);
        }

        public static void Dequantization(short[,] input, QuantizationFactors quant, out short[,] output)
        {
            var rfxQuant = Utility.ConvertRFXQuants(quant);

            output = new short[input.GetLength(0), input.GetLength(1)];
            Array.Copy(input, output, input.Length);
            dequantization_Component(output, rfxQuant);
        }

        public static void InverseDWT(short[,] input, out short[,] output)
        {
            output = new short[input.GetLength(0), input.GetLength(1)];
            Array.Copy(input, output, input.Length);
            InverseDWT_Component(output);
        }

        public static void YUVToRGB(short[,] inputX, short[,] inputY, short[,] inputZ, out byte[,] outputX, out byte[,] outputY, out byte[,] outputZ)
        {
            YCbCrToRGB(inputX, inputY, inputZ, out outputX, out outputY, out outputZ);
        }
    }
}
