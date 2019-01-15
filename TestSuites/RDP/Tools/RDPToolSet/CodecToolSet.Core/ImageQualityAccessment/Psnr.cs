using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    public class Psnr : FRIQAIndexBase
    {
        public override string IndexName
        {
            get { return "PSNR"; }
        }

        protected override void CheckException(System.Drawing.Bitmap reference, System.Drawing.Bitmap distorted)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            if (distorted == null)
            {
                throw new ArgumentNullException("distorted");
            }
            if (reference.Height != distorted.Height || reference.Width != distorted.Width)
            {
                throw new ArgumentException("Cannot assess two bitmaps with different size.");
            }
        }

        public override AssessResult Assess(System.Drawing.Bitmap reference, System.Drawing.Bitmap distorted, UseComponent component)
        {
            double Max = 255;
            var mseCalc = new Mse();
            AssessResult mseResult = mseCalc.Assess(reference, distorted, UseComponent.Luma | UseComponent.Cb | UseComponent.Cr);
            double mse = (mseResult.Luma + mseResult.Cb + mseResult.Cr) / 3;
            double psnr = 20 * Math.Log10(Max) - 10 * Math.Log10(mse);
            var psnrResult = new AssessResult();
            psnrResult.Luma = psnr;
            psnrResult.Cb = psnr;
            psnrResult.Cr = psnr;
            return psnrResult;
        }
    }
}
