using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using CodecToolSet.Core;
using RDPToolSet.Web.Controllers;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CodecToolSet.Tests
{
    [TestClass]
    public class FunctionalTest
    {
        #region parameters
        static QuantizationFactors DEFAULT_QUANT = new QuantizationFactors
        {
            // Media
            LL3 = 0x6,
            HL3 = 0x6,
            LH3 = 0x6,
            HH3 = 0x6,
            HL2 = 0x8,
            LH2 = 0x7,
            HH2 = 0x7,
            HL1 = 0x7,
            LH1 = 0x9,
            HH1 = 0x8
        };

        public static QuantizationFactorsArray DEFAULT_QUANT_ARRAY = new QuantizationFactorsArray
        {
            quants = new[]{
                DEFAULT_QUANT,
                DEFAULT_QUANT,
                DEFAULT_QUANT
            }
        };

        EntropyAlgorithm ENTROPY_ALG = new EntropyAlgorithm
        {
            Algorithm = EntropyAlgorithm.AlgorithmEnum.RLGR1
        };

        UseReduceExtrapolate DEFAULT_USE_REDUCE_EXTRAPOLATE = new UseReduceExtrapolate
        {
            Enabled = true
        };

        UseDifferenceTile DEFAULT_USE_DIFFERENCE_TILE = new UseDifferenceTile
        {
            Enabled = true
        };
        #endregion

        [TestMethod]
        public void Dynamic_Conversion_Test()
        {
            
        }

        [TestMethod]
        public void TryRFXPEncodeEachStep()
        {
            string[] codecs = new[]{
                "rgbToYUV",
                "dwt",
                "quantization",
                "linearization",
                "subbandDiffing",
                "progQuantization",
                "rlgrSRLEncode"
            };

            var CodecDic = new Dictionary<string, CodecToolSet.Core.RFXPEncode.RFXPEncodeBase>()
            {
                {"rgbToYUV", new CodecToolSet.Core.RFXPEncode.RGBToYUV()},
                {"dwt", new CodecToolSet.Core.RFXPEncode.DWT()},
                {"quantization", new CodecToolSet.Core.RFXPEncode.Quantization()},
                {"linearization", new CodecToolSet.Core.RFXPEncode.Linearization()},
                {"subbandDiffing", new CodecToolSet.Core.RFXPEncode.SubBandDiffing()},
                {"progQuantization", new CodecToolSet.Core.RFXPEncode.ProgressiveQuantization()},
                {"rlgrSRLEncode", new CodecToolSet.Core.RFXPEncode.RLGR_SRLEncode()}
            };

            foreach (var key in CodecDic.Keys)
            {
                CodecDic[key].Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = DEFAULT_QUANT_ARRAY;
                CodecDic[key].Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = ENTROPY_ALG;
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] = DEFAULT_USE_DIFFERENCE_TILE;
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = DEFAULT_USE_REDUCE_EXTRAPOLATE;
            }


            #region Handle the Previous Frame
            Tile pretile = Tile.FromFile("D:\\Sandbox\\CodecInput\\test.bmp");
            
            Tile[] prergbToYUVOut = CodecDic["rgbToYUV"].DoAction(new[] { pretile });
            Tile[] predwtOut = CodecDic["dwt"].DoAction(prergbToYUVOut);            
            Tile[] prequantOut = CodecDic["quantization"].DoAction(predwtOut);            
            Tile[] prelinearOut = CodecDic["linearization"].DoAction(prequantOut);            
            Frame preFrame = new Frame { Tile = prelinearOut.FirstOrDefault() };

            pretile.SaveToFile(@"D:\TEMP\pretile.txt");
            prergbToYUVOut.FirstOrDefault().SaveToFile(@"D:\TEMP\prergbToYUVOut.txt");
            predwtOut.FirstOrDefault().SaveToFile(@"D:\TEMP\predwtOut.txt");
            prequantOut.FirstOrDefault().SaveToFile(@"D:\TEMP\prequantOut.txt");
            prelinearOut.FirstOrDefault().SaveToFile(@"D:\TEMP\prelinearOut.txt");
            #endregion

            Tile tile = Tile.FromFile("D:\\Sandbox\\CodecInput\\test2.bmp");
            Tile[] rgbToYUVOut = CodecDic["rgbToYUV"].DoAction(new[] { tile });            
            Tile[] dwtOut = CodecDic["dwt"].DoAction(rgbToYUVOut);            
            Tile[] quantOut = CodecDic["quantization"].DoAction(dwtOut);            
            Tile[] linearOut = CodecDic["linearization"].DoAction(quantOut);            

            tile.SaveToFile(@"D:\TEMP\tile.txt");
            rgbToYUVOut.FirstOrDefault().SaveToFile(@"D:\TEMP\rgbToYUVOut.txt");
            dwtOut.FirstOrDefault().SaveToFile(@"D:\TEMP\dwtOut.txt");
            quantOut.FirstOrDefault().SaveToFile(@"D:\TEMP\quantOut.txt");
            linearOut.FirstOrDefault().SaveToFile(@"D:\TEMP\linearOut.txt");

            // Add the previous frame as an input parameter
            CodecDic["subbandDiffing"].Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;
            Tile[] subbandOut = CodecDic["subbandDiffing"].DoAction(linearOut);
            subbandOut.FirstOrDefault().SaveToFile(@"D:\TEMP\subbandOut.txt");

            Tile[] progQuantOut = CodecDic["progQuantization"].DoAction(subbandOut);
            int count = 1;
            foreach (var e in progQuantOut)
            {
                e.SaveToFile(String.Format(@"D:\TEMP\progQuantOut{0}.txt", count++));
            }

            Tile[] encodeOutput = CodecDic["rlgrSRLEncode"].DoAction(progQuantOut);
            for (int i = 0; i < encodeOutput.Length; i++)
            {
                encodeOutput[i].SaveToFile(String.Format(@"D:\TEMP\encodeOutput{0}.txt", i));
            }
        }

        [TestMethod]
        public void TryRFXPEncode()
        {
            Tile tile = Tile.FromFile("D:\\Sandbox\\CodecInput\\test2.bmp");
            // Frame preFrame = GetPreviousFrame("D:\\Sandbox\\CodecInput\\test.bmp");

            Frame preFrame = null;

            var rfxPEncode = new CodecToolSet.Core.RFXPEncode.RFXPEncode();
            ICodecAction diffing = rfxPEncode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PENCODE_NAME_SUBBANDDIFFING));
            diffing.Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;
            Tile[] rfxPEncodeOut = rfxPEncode.DoAction(new[] { tile });
            for (int i = 0; i < rfxPEncodeOut.Length; i++)
            {
                rfxPEncodeOut[i].SaveToFile(String.Format(@"D:\TEMP\rfxPEncodeOut{0}.txt", i));
            }
        }

        [TestMethod]
        public void TryRFXPDecodeEachStep()
        {
            string[] codecs = new[]{
                "rlgrSRLDecode",
                "progDeQuantization",
                "subbandDiffing",
                "subbandReconstruction",
                "DeQuantization",
                "inverse-Dwt",
                "yuvToRGB",
                "rgbToImage"
            };

            var CodecDic = new Dictionary<string, CodecToolSet.Core.RFXPDecode.RFXPDecodeBase>()
            {
                {"rlgrSRLDecode", new CodecToolSet.Core.RFXPDecode.RLGR_SRLDecode()},
                {"progDeQuantization", new CodecToolSet.Core.RFXPDecode.ProgressiveDequantization()},
                {"subbandDiffing", new CodecToolSet.Core.RFXPDecode.SubBandDiffing()},
                {"subbandReconstruction", new CodecToolSet.Core.RFXPDecode.SubBandReconstruction()},
                {"DeQuantization", new CodecToolSet.Core.RFXPDecode.DeQuantization()},
                {"inverse-Dwt", new CodecToolSet.Core.RFXPDecode.InverseDWT()},
                {"yuvToRGB", new CodecToolSet.Core.RFXPDecode.YUVToRGB()},
                {"rgbToImage", new CodecToolSet.Core.RFXPDecode.RGBToImage()}
            };

            foreach (var key in CodecDic.Keys)
            {
                CodecDic[key].Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = DEFAULT_QUANT_ARRAY;
                CodecDic[key].Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = ENTROPY_ALG;
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] = DEFAULT_USE_DIFFERENCE_TILE;
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = DEFAULT_USE_REDUCE_EXTRAPOLATE;
            }

            // Frame preFrame = GetPreviousFrame("D:\\Sandbox\\CodecInput\\test.bmp");
            Frame preFrame = null;
            Tile DAS = Tile.FromArrays<short>(new Triplet<short[]>(new short[RdpegfxTileUtils.TileSize*RdpegfxTileUtils.TileSize],new short[RdpegfxTileUtils.TileSize*RdpegfxTileUtils.TileSize],new short[RdpegfxTileUtils.TileSize*RdpegfxTileUtils.TileSize]));

            // fisrtPass
            Tile fisrtPass = ReadTileFromFile(@"D:\TEMP\rfxPEncodeOut0.txt");
            CodecDic["rlgrSRLDecode"].Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.FirstPass };
            Tile[] rlgrSRLDecodeOut = CodecDic["rlgrSRLDecode"].DoAction(new[] { fisrtPass });
            DAS.Add(rlgrSRLDecodeOut.FirstOrDefault());
            rlgrSRLDecodeOut.FirstOrDefault().SaveToFile("D:\\TEMP\\rlgrSRLDecodeOut0.txt");

            QuantizationFactorsArray quant = GetProgQuantizationFactorsForChunk(ProgressiveChunk_Values.kChunk_25);
            CodecDic["progDeQuantization"].Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = quant;
            Tile[] progDeQuantizationOut = CodecDic["progDeQuantization"].DoAction(rlgrSRLDecodeOut);
            progDeQuantizationOut.FirstOrDefault().SaveToFile("D:\\TEMP\\progDeQuantizationOut0.txt");

            CodecDic["subbandDiffing"].Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;
            CodecDic["subbandDiffing"].Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.FirstPass };
            Tile[] subbandDiffingOut = CodecDic["subbandDiffing"].DoAction(progDeQuantizationOut);
            preFrame = new Frame { Tile = subbandDiffingOut.FirstOrDefault() };
            subbandDiffingOut.FirstOrDefault().SaveToFile("D:\\TEMP\\subbandDiffingOut0.txt");

            Tile[] subbandReconstructionOut = CodecDic["subbandReconstruction"].DoAction(subbandDiffingOut);
            subbandReconstructionOut.FirstOrDefault().SaveToFile("D:\\TEMP\\subbandReconstructionOut0.txt");

            QuantizationFactorsArray quantArray = new QuantizationFactorsArray();
            quantArray.quants = new QuantizationFactors[3] { DEFAULT_QUANT, DEFAULT_QUANT, DEFAULT_QUANT };
            CodecDic["DeQuantization"].Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
            Tile[] DeQuantizationOut = CodecDic["DeQuantization"].DoAction(subbandReconstructionOut);
            DeQuantizationOut.FirstOrDefault().SaveToFile("D:\\TEMP\\DeQuantizationOut0.txt");

            Tile[] inverseDwtOut = CodecDic["inverse-Dwt"].DoAction(DeQuantizationOut);
            Tile[] yuvToRGBOut = CodecDic["yuvToRGB"].DoAction(inverseDwtOut);
            yuvToRGBOut.FirstOrDefault().SaveToFile("D:\\TEMP\\yuvToRGBOutOut0.txt");

            Bitmap bitmap = yuvToRGBOut.FirstOrDefault().GetBitmap();
            bitmap.Save("D:\\Temp\\FirstPass.bmp");

            ProgressiveChunk_Values[] chunks = new[] { ProgressiveChunk_Values.kChunk_25, ProgressiveChunk_Values.kChunk_50, ProgressiveChunk_Values.kChunk_75, ProgressiveChunk_Values.kChunk_100 };
            for (int i = 1; i < chunks.Length; i++)
            {
                Tile encodedTile = ReadTileFromFile(String.Format(@"D:\TEMP\rfxPEncodeOut{0}.txt", i * 2 - 1));
                Tile rawTile = ReadTileFromFile(String.Format(@"D:\TEMP\rfxPEncodeOut{0}.txt", i * 2));
                CodecDic["rlgrSRLDecode"].Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.UpgradePass };
                CodecDic["rlgrSRLDecode"].Parameters[Constants.PARAM_NAME_DAS] = new Frame { Tile = DAS };
                CodecDic["rlgrSRLDecode"].Parameters[Constants.PARAM_NAME_PREVIOUS_PROGRESSIVE_QUANTS] = GetProgQuantizationFactorsForChunk(chunks[i-1]);
                CodecDic["rlgrSRLDecode"].Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = GetProgQuantizationFactorsForChunk(chunks[i]);
                rlgrSRLDecodeOut = CodecDic["rlgrSRLDecode"].DoAction(new[] { encodedTile, rawTile });
                DAS.Add(rlgrSRLDecodeOut.FirstOrDefault());
                rlgrSRLDecodeOut.FirstOrDefault().SaveToFile(String.Format("D:\\TEMP\\rlgrSRLDecodeOut{0}.txt", i));

                quant = GetProgQuantizationFactorsForChunk(chunks[i]);
                CodecDic["progDeQuantization"].Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = quant;
                progDeQuantizationOut = CodecDic["progDeQuantization"].DoAction(rlgrSRLDecodeOut);
                progDeQuantizationOut.FirstOrDefault().SaveToFile(String.Format("D:\\TEMP\\progDeQuantizationOut{0}.txt", i));

                CodecDic["subbandDiffing"].Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;
                CodecDic["subbandDiffing"].Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.UpgradePass };
                subbandDiffingOut = CodecDic["subbandDiffing"].DoAction(progDeQuantizationOut);
                preFrame = new Frame { Tile = subbandDiffingOut.FirstOrDefault() };
                subbandDiffingOut.FirstOrDefault().SaveToFile(String.Format("D:\\TEMP\\subbandDiffingOut{0}.txt", i));

                subbandReconstructionOut = CodecDic["subbandReconstruction"].DoAction(subbandDiffingOut);

                CodecDic["DeQuantization"].Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;
                DeQuantizationOut = CodecDic["DeQuantization"].DoAction(subbandReconstructionOut);

                inverseDwtOut = CodecDic["inverse-Dwt"].DoAction(DeQuantizationOut);
                yuvToRGBOut = CodecDic["yuvToRGB"].DoAction(inverseDwtOut);
                yuvToRGBOut.FirstOrDefault().SaveToFile(String.Format("D:\\TEMP\\yuvToRGBOutOut{0}.txt", i));

                bitmap = yuvToRGBOut.FirstOrDefault().GetBitmap();
                bitmap.Save(String.Format("D:\\Temp\\upgradePass{0}.bmp", i));
            }
        }

        [TestMethod]
        public void TryRFXPDecode()
        {
            var rfxPDecode = new CodecToolSet.Core.RFXPDecode.RFXPDecode();

            //Frame preFrame = GetPreviousFrame("D:\\Sandbox\\CodecInput\\test.bmp");
            Frame preFrame = null;
            Tile DAS = Tile.FromArrays<short>(new Triplet<short[]>(new short[RdpegfxTileUtils.TileSize * RdpegfxTileUtils.TileSize], new short[RdpegfxTileUtils.TileSize * RdpegfxTileUtils.TileSize], new short[RdpegfxTileUtils.TileSize * RdpegfxTileUtils.TileSize]));
            
            // fisrt pass
            Tile fisrtPass = ReadTileFromFile(@"D:\TEMP\rfxPEncodeOut0.txt");
            
            ICodecAction rlgrSRLDecode = rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_RLGRSRLDECODE));
            rlgrSRLDecode.Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.FirstPass };

            QuantizationFactorsArray quant = GetProgQuantizationFactorsForChunk(ProgressiveChunk_Values.kChunk_25);
            ICodecAction progDeQuantization = rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_PROGRESSIVEDEQUANTIZATION));
            progDeQuantization.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = quant;

            ICodecAction subbandDiffing = rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_SUBBANDDIFFING));
            subbandDiffing.Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;
            subbandDiffing.Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.FirstPass };

            ICodecAction DeQuantization = rfxPDecode.SubActions.SingleOrDefault(c => c.Name.Equals(Constants.PDECODE_NAME_DEQUANTIZATION));
            QuantizationFactorsArray quantArray = new QuantizationFactorsArray();
            quantArray.quants = new QuantizationFactors[3] { DEFAULT_QUANT, DEFAULT_QUANT, DEFAULT_QUANT };
            DeQuantization.Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = quantArray;

            Tile[] rgbImage = rfxPDecode.DoAction(new[] { fisrtPass });
            rgbImage.FirstOrDefault().GetBitmap().Save("D:\\TEMP\\rfxPDecode0.bmp");

            DAS.Add(rlgrSRLDecode.Result.FirstOrDefault());
            preFrame = new Frame { Tile = subbandDiffing.Result.FirstOrDefault() };

            ProgressiveChunk_Values[] chunks = new[] { ProgressiveChunk_Values.kChunk_25, ProgressiveChunk_Values.kChunk_50, ProgressiveChunk_Values.kChunk_75, ProgressiveChunk_Values.kChunk_100 };

            for (int i = 1; i < chunks.Length; i++)
            {
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.UpgradePass };
                subbandDiffing.Parameters[Constants.PARAM_NAME_ENCODED_TILE_TYPE] = new CodecToolSet.Core.EncodedTileType { Type = CodecToolSet.Core.EncodedTileType.EncodedType.UpgradePass };
                subbandDiffing.Parameters[Constants.PARAM_NAME_PREVIOUS_FRAME] = preFrame;
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_DAS] = new Frame { Tile = DAS };
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_PREVIOUS_PROGRESSIVE_QUANTS] = GetProgQuantizationFactorsForChunk(chunks[i - 1]);
                rlgrSRLDecode.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = GetProgQuantizationFactorsForChunk(chunks[i]);
                quant = GetProgQuantizationFactorsForChunk(chunks[i]);
                progDeQuantization.Parameters[Constants.PARAM_NAME_PROGRESSIVE_QUANTS] = quant;

                Tile encodedTile = ReadTileFromFile(String.Format(@"D:\TEMP\rfxPEncodeOut{0}.txt", i * 2 - 1));
                Tile rawTile = ReadTileFromFile(String.Format(@"D:\TEMP\rfxPEncodeOut{0}.txt", i * 2));

                rgbImage = rfxPDecode.DoAction(new[] { encodedTile, rawTile });
                rgbImage.FirstOrDefault().GetBitmap().Save(String.Format("D:\\TEMP\\rfxPDecode{0}.bmp", i));

                DAS.Add(rlgrSRLDecode.Result.FirstOrDefault());
                preFrame = new Frame { Tile = subbandDiffing.Result.FirstOrDefault() };
            }
        }

        #region private method
        private Frame GetPreviousFrame(string path)
        {
            Tile pretile = Tile.FromFile(path);

            string[] codecs = new[]{
                "rgbToYUV",
                "dwt",
                "quantization",
                "linearization",
            };

            var CodecDic = new Dictionary<string, CodecToolSet.Core.RFXPEncode.RFXPEncodeBase>()
            {
                {"rgbToYUV", new CodecToolSet.Core.RFXPEncode.RGBToYUV()},
                {"dwt", new CodecToolSet.Core.RFXPEncode.DWT()},
                {"quantization", new CodecToolSet.Core.RFXPEncode.Quantization()},
                {"linearization", new CodecToolSet.Core.RFXPEncode.Linearization()},
            };

            foreach (var key in CodecDic.Keys)
            {
                CodecDic[key].Parameters[Constants.PARAM_NAME_QUANT_FACTORS_ARRAY] = DEFAULT_QUANT_ARRAY;
                CodecDic[key].Parameters[Constants.PARAM_NAME_ENTROPY_ALGORITHM] = ENTROPY_ALG;
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_DIFFERENCE_TILE] = DEFAULT_USE_DIFFERENCE_TILE;
                CodecDic[key].Parameters[Constants.PARAM_NAME_USE_REDUCE_EXTRAPOLATE] = DEFAULT_USE_REDUCE_EXTRAPOLATE;
            }

            Tile[] prergbToYUVOut = CodecDic["rgbToYUV"].DoAction(new[] { pretile });
            Tile[] predwtOut = CodecDic["dwt"].DoAction(prergbToYUVOut);
            Tile[] prequantOut = CodecDic["quantization"].DoAction(predwtOut);
            Tile[] prelinearOut = CodecDic["linearization"].DoAction(prequantOut);
            Frame preFrame = new Frame { Tile = prelinearOut.FirstOrDefault() };

            return preFrame;
        }

        private Tile ReadTileFromFile(string path){
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            var arrayList = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                int[] x;
                string line = sr.ReadLine();
                if (line.Equals("NULL")) x = null;
                else if (line.Equals("")) x = new int[0];
                else
                {
                    string[] strs = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    x = new int[strs.Length];
                    for (int j = 0; j < strs.Length; j++)
                    {
                        x[j] = Convert.ToInt16(strs[j]);
                    }
                }
                arrayList.Add(x);
            }
            Triplet<int[]> triplet = new Triplet<int[]>(arrayList[0], arrayList[1], arrayList[2]);
            return Tile.FromArrays<int>(triplet);
        }

        private QuantizationFactorsArray GetProgQuantizationFactorsForChunk(ProgressiveChunk_Values chunk)
        {
            RFX_PROGRESSIVE_CODEC_QUANT quant = RdpegfxTileUtils.GetProgCodecQuant(chunk);
            QuantizationFactorsArray factors = ConvertQuant(quant);

            return factors;
        }

        private QuantizationFactorsArray ConvertQuant(RFX_PROGRESSIVE_CODEC_QUANT quant)
        {
            var fators = new QuantizationFactorsArray();
            fators.quants = new QuantizationFactors[3];

            fators.quants[0] = new QuantizationFactors();
            fators.quants[0].LL3 = (byte)(quant.yQuantValues.LL3_HL3 & 0x0F);
            fators.quants[0].HL3 = (byte)(quant.yQuantValues.LL3_HL3  >> 4);
            fators.quants[0].LH3 = (byte)(quant.yQuantValues.LH3_HH3 & 0x0F);
            fators.quants[0].HH3 = (byte)(quant.yQuantValues.LH3_HH3 >> 4);            
            fators.quants[0].HL2 = (byte)(quant.yQuantValues.HL2_LH2 & 0x0F);
            fators.quants[0].LH2 = (byte)(quant.yQuantValues.HL2_LH2 >> 4);
            fators.quants[0].HH2 = (byte)(quant.yQuantValues.HH2_HL1 & 0x0F);
            fators.quants[0].HL1 = (byte)(quant.yQuantValues.HH2_HL1 >> 4);
            fators.quants[0].LH1 = (byte)(quant.yQuantValues.LH1_HH1 & 0x0F);
            fators.quants[0].HH1 = (byte)(quant.yQuantValues.LH1_HH1 >> 4);

            fators.quants[1] = new QuantizationFactors();
            fators.quants[1].LL3 = (byte)(quant.cbQuantValues.LL3_HL3 & 0x0F);
            fators.quants[1].HL3 = (byte)(quant.cbQuantValues.LL3_HL3 >> 4);
            fators.quants[1].LH3 = (byte)(quant.cbQuantValues.LH3_HH3 & 0x0F);
            fators.quants[1].HH3 = (byte)(quant.cbQuantValues.LH3_HH3 >> 4);
            fators.quants[1].HL2 = (byte)(quant.cbQuantValues.HL2_LH2 & 0x0F);
            fators.quants[1].LH2 = (byte)(quant.cbQuantValues.HL2_LH2 >> 4);
            fators.quants[1].HH2 = (byte)(quant.cbQuantValues.HH2_HL1 & 0x0F);
            fators.quants[1].HL1 = (byte)(quant.cbQuantValues.HH2_HL1 >> 4);
            fators.quants[1].LH1 = (byte)(quant.cbQuantValues.LH1_HH1 & 0x0F);
            fators.quants[1].HH1 = (byte)(quant.cbQuantValues.LH1_HH1 >> 4);

            fators.quants[2] = new QuantizationFactors();
            fators.quants[2].LL3 = (byte)(quant.crQuantValues.LL3_HL3 & 0x0F);
            fators.quants[2].HL3 = (byte)(quant.crQuantValues.LL3_HL3 >> 4);
            fators.quants[2].LH3 = (byte)(quant.crQuantValues.LH3_HH3 & 0x0F);
            fators.quants[2].HH3 = (byte)(quant.crQuantValues.LH3_HH3 >> 4);
            fators.quants[2].HL2 = (byte)(quant.crQuantValues.HL2_LH2 & 0x0F);
            fators.quants[2].LH2 = (byte)(quant.crQuantValues.HL2_LH2 >> 4);
            fators.quants[2].HH2 = (byte)(quant.crQuantValues.HH2_HL1 & 0x0F);
            fators.quants[2].HL1 = (byte)(quant.crQuantValues.HH2_HL1 >> 4);
            fators.quants[2].LH1 = (byte)(quant.crQuantValues.LH1_HH1 & 0x0F);
            fators.quants[2].HH1 = (byte)(quant.crQuantValues.LH1_HH1 >> 4);

            return fators;
        }
        #endregion
    }

}
