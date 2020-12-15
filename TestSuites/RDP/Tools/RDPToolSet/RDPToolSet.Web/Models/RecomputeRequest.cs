// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace RDPToolSet.Web.Models
{
    public class RecomputeRequest
    {
        public string Action { get; set; }

        public RecomputeParams Params { get; set; }

        public string[] Inputs { get; set; }

        public int Layer { get; set; }
    }

    public class RecomputeParams
    {
        public string EntropyAlgorithm { get; set; }

        public string[] QuantizationFactorsArray { get; set; }

        public string[] ProgQuantizationArray { get; set; }

        public string UseReduceExtrapolate { get; set; }

        public string UseDifferenceTile { get; set; }

        public string UseDataFormat { get; set; }
    }
}
