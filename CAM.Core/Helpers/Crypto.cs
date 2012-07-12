using System;
using System.Security.Cryptography;
using System.Text;

namespace CAM.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Functions were grabbed from http://www.dijksterhuis.org/encrypting-decrypting-string/
    /// </remarks>
    public class Crypto
    {
        public static string EncryptString(string message, string passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] dataToEncrypt = utf8.GetBytes(message);

            // Step 5. Attempt to encrypt the string
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(results);
        }

        public static string DecryptString(string message, string passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] dataToDecrypt = Convert.FromBase64String(message);

            // Step 5. Attempt to decrypt the string
            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return utf8.GetString(results);
        }
    }
}
