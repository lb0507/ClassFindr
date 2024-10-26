/*
 *  Class that provides security functions for the application.
 *  
 *  Version 10.26.24
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Utils
{
    /// <summary> Provides methods for security applications throughout ClassFindr </summary>
    public class Security
    {

        /// <summary>
        ///     Hashes the inputted string (Output will always be 64 characters)
        /// </summary>
        /// <param name="input"> Inputted string to be hashed </param>
        /// <returns> Output of the SHA-256 hashed input </returns>
        public static string Hash(string? input)
        {
            SHA256 encryptor = SHA256.Create();     // Creates the hashing class to use SHA-256
            byte[] buffer = Encoding.UTF8.GetBytes(input ?? "");    // Gets the individual bytes from the password
            byte[] hashedPW = encryptor.ComputeHash(buffer);    // The actual hash computation

            StringBuilder sb = new StringBuilder();     // Create a new string builder for output

            // Append and format each character to the output
            foreach (byte b in hashedPW)
            {
                sb.Append(b.ToString("x2"));    // Append and format the character to the output
            }

            encryptor.Clear();  // Similar to .Close() when closing a file.  Releases memory being used by the hash class

            return sb.ToString();
        }

    }
}
