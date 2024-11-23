using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ClassFindrDataAccessLibrary.Utils
{
    public class Encryption
    {
        private static string key = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")
                             ?? "default-development-key";

        // Encrypt the connection string
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Encryption key is missing or invalid.");
            }

            // Ensure the key is exactly 32 bytes (AES-256 requires 32 bytes)
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 32)
            {
                Array.Resize(ref keyBytes, 32); // Truncate or pad to 32 bytes if necessary
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes; // Set the key to the 32-byte array
                aesAlg.GenerateIV();  // Generate a random IV
                byte[] iv = aesAlg.IV; // Save the IV

                aesAlg.Padding = PaddingMode.PKCS7; // Explicitly use PKCS7 padding

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    // Combine the IV and encrypted data before converting to Base64
                    byte[] encrypted = msEncrypt.ToArray();
                    byte[] result = new byte[iv.Length + encrypted.Length];
                    Array.Copy(iv, 0, result, 0, iv.Length);
                    Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);

                    return Convert.ToBase64String(result); // Return the combined IV + encrypted data
                }
            }
        }

        // Decrypt the connection string
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Encryption key is missing or invalid.");
            }


            // Ensure key is 32 bytes long for AES-256
            if (key.Length < 32)
            {
                key = key.PadRight(32, '0'); // Pad with '0' if it's shorter than 32 bytes
            }
            else if (key.Length > 32)
            {
                key = key.Substring(0, 32); // Truncate to 32 bytes if it's longer
            }

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Extract the IV (first 16 bytes)
            byte[] iv = new byte[16];
            Array.Copy(cipherBytes, 0, iv, 0, iv.Length);

            // Extract the encrypted data (everything after the IV)
            byte[] encryptedData = new byte[cipherBytes.Length - iv.Length];
            Array.Copy(cipherBytes, iv.Length, encryptedData, 0, encryptedData.Length);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv; // Use the extracted IV for decryption
                aesAlg.Padding = PaddingMode.PKCS7; // Ensure PKCS7 padding is used

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}

// Retrieve the encrypted connection string from the config
//string encryptedConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

// Decrypt the connection string
//string decryptedConnectionString = Encryption.Decrypt(encryptedConnectionString);
