using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace U2F.Core.Crypto
{
    public interface ICryptoService
    {
        /// <summary>
        /// Generated a securely generated byte array 
        /// </summary>
        /// <returns>Securely generated random bytes</returns>
        byte[] GenerateChallenge();
        
        /// <summary>
        /// Checks the signature.
        /// </summary>
        /// <param name="certificate">The key.</param>
        /// <param name="signedBytes">The source.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        bool CheckSignature(X509Certificate2 certificate, byte[] signedBytes, byte[] signature);


        /// <summary>
        /// Checks the signature.
        /// </summary>
        /// <param name="certificate">The key.</param>
        /// <param name="signedBytes">The source.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        bool CheckSignature(CngKey certificate, byte[] signedBytes, byte[] signature);

        /// <summary>
        /// Decodes the public key.
        /// </summary>
        /// <param name="encodedPublicKey">The encoded public key.</param>
        /// <returns></returns>
        CngKey DecodePublicKey(X509Certificate2 encodedPublicKey);

        /// <summary>
        /// Encoding the raw byte[] key to Cngkey.
        /// </summary>
        /// <param name="rawKey">Raw byte[] should be 65 bytes in length.</param>
        /// <returns></returns>
        CngKey EncodePublicKey(byte[] rawKey);

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>byte array of hashed byte array</returns>
        byte[] Hash(byte[] bytes);

        /// <summary>
        /// Hashes the specified string with sha256.
        /// </summary>
        /// <param name="stringToHash">The string to be hased.</param>
        /// <returns>byte array of hashed string</returns>
        byte[] Hash(string stringToHash);
    }
}