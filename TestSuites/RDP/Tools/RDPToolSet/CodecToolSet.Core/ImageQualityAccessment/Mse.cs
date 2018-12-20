using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.Rdp.ImageQualityAccessment
{
    public class Mse : FRIQAIndexBase
    {

        public override string IndexName
        {
            get { return "MSE"; }
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
            var result = new AssessResult();
            InitBitmaps(reference, distorted);

            // MSE
            double sum = 0.0;
            for (int i = 0; i < LumaReference.GetLength(0); i++)
            {
                for (int j = 0; j < LumaReference.GetLength(1); j++)
                {
                    sum += (LumaReference[i, j] - LumaDistorted[i, j]) * (LumaReference[i, j] - LumaDistorted[i, j]);
                }
            }
            result.Luma = sum / LumaReference.Length;

            sum = 0.0;
            for (int i = 0; i < CbReference.GetLength(0); i++)
            {
                for (int j = 0; j < CbReference.GetLength(1); j++)
                {
                    sum += (CbReference[i, j] - CbDistorted[i, j]) * (CbReference[i, j] - CbDistorted[i, j]);
                }
            }
            result.Cb = sum / CbReference.Length;

            sum = 0.0;
            for (int i = 0; i < CrReference.GetLength(0); i++)
            {
                for (int j = 0; j < CrReference.GetLength(1); j++)
                {
                    sum += (CrReference[i, j] - CrDistorted[i, j]) * (CrReference[i, j] - CrDistorted[i, j]);
                }
            }
            result.Cr = sum / CrReference.Length;

            return result;
        }

    }
}
