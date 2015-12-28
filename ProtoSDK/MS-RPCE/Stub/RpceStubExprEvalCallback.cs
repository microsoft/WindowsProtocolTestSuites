// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// Callback stub for EXPR_EVAL.
    /// </summary>
    internal class RpceStubExprEvalCallback
    {
        // Stub and EXPR_EVAL.
        private RpceStub RpceStub;
        private RpceStubExprEval exprEval;


        /// <summary>
        /// Initialize a RpceStubExprEvalCallback.
        /// </summary>
        /// <param name="stub">RpceStub which contain the EXPR_EVAL.</param>
        /// <param name="exprEvalRoutine">A EXPR_EVAL.</param>
        internal RpceStubExprEvalCallback(RpceStub stub, RpceStubExprEval exprEvalRoutine)
        {
            RpceStub = stub;
            exprEval = exprEvalRoutine;
        }


        /// <summary>
        /// Callback function.
        /// </summary>
        /// <param name="stubMsg">MIDL_STUB_MESSAGE structure.</param>
        internal void EXPR_EVAL(IntPtr stubMsg)
        {
            if (stubMsg != RpceStub.pStubMsg)
            {
                throw new InvalidOperationException("Expected and actual MIDL_STUB_MESSAGE don't match.");
            }

            exprEval.Invoke(RpceStub);
        }
    }
}
