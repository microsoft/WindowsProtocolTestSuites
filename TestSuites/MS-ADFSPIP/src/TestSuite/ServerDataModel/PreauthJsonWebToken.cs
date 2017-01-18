// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// The class to encode or decode the JWT used in ADFS preauthentication.
    /// </summary>
    internal class PreauthJsonWebToken
    {
        /// <summary>
        /// The signature MUST use SHA256 as the hash algorithm
        /// </summary>
        private const string HashAlgorithm = "SHA256";

        /// <summary>
        /// The signing algorithm MUST be RSA-SHA256.
        /// Any other algorithm will not work.
        /// </summary>
        private const string SigningAlgorithm = "RS256";

        private const string Type = "JWT";

        /// <summary>
        /// The JWT header used internally by the preauth-string.
        /// This is not the complete JWT header.
        /// </summary>
        internal class JwtHeader : JSONObject
        {
            public string typ;
            public string alg;
            public string x5t;
        }

        /// <summary>
        /// Encode a ProxyToken to base64 JWT, and sign it with the private key 
        /// provided by the certificate.
        /// </summary>
        /// <param name="payload">
        /// The ProxyToken to be encoded.
        /// </param>
        /// <param name="signingCertificate">
        /// The X509Certificate which includes the private key to sign the JWT.
        /// </param>
        /// <returns>
        /// The base64 encoded JWT with signature appended.
        /// </returns>
        public static string Encode(ProxyToken payload, X509Certificate2 signingCertificate)
        {
            // header, payload and signature are joint by dot
            // header.payload.signature
            var segments = new List<string>();

            // RS256 is the ONLY algorithm that works
            var header = new JwtHeader { typ = Type, alg = SigningAlgorithm };
            // x5t is the base64 encoded SHA1 hash of the certificate
            // must use SHA1 hash algorithm here
            header.x5t = Base64Helper.Base64Encode(signingCertificate.GetCertHash());

            // header and payload should be encoded using UTF8
            var base64header = Base64Helper.Base64Encode(Encoding.UTF8.GetBytes(header.ToString()));
            var base64payload = Base64Helper.Base64Encode(Encoding.UTF8.GetBytes(payload.ToString()));

            segments.Add(base64header);
            segments.Add(base64payload);

            // what we sign is the base64 encoded result
            var stringToSign = string.Join(".", segments.ToArray());

            // sign the data: header and payload
            var signature = RSASigningHelper.SignData(
                // data to be signed should use ASCII encoding
                Encoding.ASCII.GetBytes(stringToSign), 
                signingCertificate, HashAlgorithm);

            segments.Add(Base64Helper.Base64Encode(signature));

            return string.Join(".", segments.ToArray());
        }

        /// <summary>
        /// Decodes the base64 JWT and veries it with the public key 
        /// in the certificate provided.
        /// </summary>
        /// <param name="token">
        /// The base64-encoded JWT.
        /// </param>
        /// <param name="signingCertificate">
        /// The certificate with the public key to verify the JWT.
        /// </param>
        /// <param name="verity">
        /// If this value is true, signature verification will be proceeded.
        /// </param>
        /// <returns>
        /// The decoded ProxyToken.
        /// </returns>
        public static ProxyToken Decode(string token, X509Certificate2 signingCertificate, bool verity = false)
        {
            var segments = token.Split('.');         
            
            if (verity) {
                // get the signed data by decoding the string with ASCII
                // because when signing it, we use ASCII
                var signedData = Encoding.ASCII.GetBytes(string.Concat(segments[0], ".", segments[1]));
                var signature  = Base64Helper.Base64Decode(segments[2]);

                // veify the signature
                if(!RSASigningHelper.VerifySignature(signedData, signature, signingCertificate, HashAlgorithm))
                    throw new CryptographicException("Invalid signature");
            }

            // decode header and payload
            var header  = Encoding.UTF8.GetString(Base64Helper.Base64Decode(segments[0]));
            var payload = Encoding.UTF8.GetString(Base64Helper.Base64Decode(segments[1]));

            return JSONObject.Parse<ProxyToken>(payload);
        }
    }

    internal static class RSASigningHelper
    {
        /// <summary>
        /// Sign the data using RSA algorithm with the private key
        /// provided in a certificate.
        /// </summary>
        /// <param name="data">
        /// Data to be signed.
        /// </param>
        /// <param name="cert">
        /// Certificate containing a private key.
        /// </param>
        /// <param name="halgo">
        /// Hash algorithm to use.
        /// </param>
        /// <returns>
        /// The signature.
        /// </returns>
        public static byte[] SignData(byte[] data, X509Certificate2 cert, string halgo)
        {
            // Make sure the certificate contains a private key
            if (!cert.HasPrivateKey)
                throw new CryptographicException("The certificate does not contain a private key");

            // Reconstruct the certificate (and don't ask why).
            // This may be required by the way the certificate creates it's private key. 
            // Anyway we do this by first exporting the key and then re-importing it.
            // Round-trip the key to XML and back, there might be a better way but this works.
            var key = new RSACryptoServiceProvider();
            key.FromXmlString(cert.PrivateKey.ToXmlString(true));

            // Make sure the hash algorithm is valid
            var oid = CryptoConfig.MapNameToOID(halgo);
            if (oid == null)
            {
                throw new CryptographicException("Invalid hash algorithm specified");
            }

            return key.SignData(data, oid);
        }

        /// <summary>
        /// Verify the RSA signature with the public key 
        /// provided in a certificate.
        /// </summary>
        /// <param name="data">
        /// The data against which the signature is made.
        /// </param>
        /// <param name="sig">
        /// The signature.
        /// </param>
        /// <param name="cert">
        /// The certificate.
        /// </param>
        /// <param name="halgo">
        /// The hash algorithm used when signing the data.
        /// </param>
        /// <returns>
        /// True if the data is valid; False, otherwise.
        /// </returns>
        public static bool VerifySignature(byte[] data, byte[] sig, X509Certificate2 cert, string halgo)
        {
            var key = (RSACryptoServiceProvider)cert.PublicKey.Key;

            // Make sure the hash algorithm is valid
            var oid = CryptoConfig.MapNameToOID(halgo);
            if (oid == null)
            {
                throw new CryptographicException("Invalid hash algorithm specified");
            }

            return key.VerifyData(data, CryptoConfig.MapNameToOID(halgo), sig);
        }
    }
}
