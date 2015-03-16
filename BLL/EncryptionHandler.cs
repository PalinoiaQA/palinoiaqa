using System;
using System.Data;
using System.Security.Cryptography;
using System.IO;

namespace BLL
{    
    /// <summary>
    /// class to hold code for EncryptionHandler
    /// </summary>
    public class EncryptionHandler
    {
        #region properties and variables

        // Change these keys
        private byte[] Key = { 133, 216, 19, 12, 24, 86, 85, 45, 112, 184, 27, 162, 37, 112, 222, 209, 251, 20, 175, 144, 173, 53, 196, 89, 23, 26, 13, 218, 131, 236, 63, 208 };
        private byte[] Vector = { 186, 74, 191, 119, 23, 3, 113, 111, 231, 121, 32, 112, 79, 32, 134, 197 };
        private ICryptoTransform EncryptorTransform, DecryptorTransform;
        private System.Text.UTF8Encoding UTFEncoder;

        #endregion properties and variables

        #region constructors
                
        /// <summary>
        /// constructor with no parameters
        /// </summary>
        public EncryptionHandler()
        {
            //This is our encryption method
            RijndaelManaged rm = new RijndaelManaged();

            //Create an encryptor and a decryptor using our encryption method, key, and vector.
            EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
            DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);

            //Used to translate bytes to text and vice versa
            UTFEncoder = new System.Text.UTF8Encoding();
        }

        #endregion constructors

        #region instance methods
              
        /// <summary>
        /// Generates an encryption key
        /// </summary>
        /// <returns>byte</returns>
        static public byte[] GenerateEncryptionKey()
        {
            //Generate a Key.
            RijndaelManaged rm = new RijndaelManaged();
            rm.GenerateKey();
            return rm.Key;
        }
                
        /// <summary>
        /// Generates an encryption vector
        /// </summary>
        /// <returns>byte</returns>
        static public byte[] GenerateEncryptionVector()
        {
            //Generate a Vector
            RijndaelManaged rm = new RijndaelManaged();
            rm.GenerateIV();
            return rm.IV;
        }
                        
        /// <summary>
        /// Encrypt some text and return a string suitable for passing in a URL
        /// </summary>
        /// <param name="TextValue">string</param>
        /// <returns>string</returns>
        public string EncryptToString(string TextValue)
        {
            return ByteArrToString(Encrypt(TextValue));
        }
                
        /// <summary>
        /// Translate our text value into a byte array. (used to stream the data in and out of the CryptoStream)
        /// </summary>
        /// <param name="TextValue">string</param>
        /// <returns>byte[]</returns>
        public byte[] Encrypt(string TextValue)
        {
            //Translates our text value into a byte array.
            Byte[] bytes = UTFEncoder.GetBytes(TextValue);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            /*
             * We will have to write the unencrypted bytes to the stream,
             * then read the encrypted result back from the stream.
             */
            #region Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            #endregion

            #region Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }
                
        /// <summary>
        /// decrypt string
        /// </summary>
        /// <param name="EncryptedString">string</param>
        /// <returns>string</returns>
        public string DecryptString(string EncryptedString)
        {
            return Decrypt(StrToByteArray(EncryptedString));
        }
                
        /// <summary>
        /// decrypt byte array
        /// </summary>
        /// <param name="EncryptedValue">byte[]</param>
        /// <returns>string</returns>
        public string Decrypt(byte[] EncryptedValue)
        {
            #region Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
            decryptStream.FlushFinalBlock();
            #endregion

            #region Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();
            #endregion
            return UTFEncoder.GetString(decryptedBytes);
        }

               
        /// <summary>
        /// Convert a string to a byte array
        ///   NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        ///      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        ///      return encoding.GetBytes(str);
        /// However, this results in character values that cannot be passed in a URL.  So, instead, I just
        /// lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>byte[]</returns>
        public byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }
                
        /// <summary>
        /// Convert a byte array to a string
        /// Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
        ///      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        ///      return enc.GetString(byteArr); 
        /// </summary>
        /// <param name="byteArr">byte[]</param>
        /// <returns>string</returns>
        public string ByteArrToString(byte[] byteArr)
        {
            byte val;
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                val = byteArr[i];
                if (val < (byte)10)
                    tempStr += "00" + val.ToString();
                else if (val < (byte)100)
                    tempStr += "0" + val.ToString();
                else
                    tempStr += val.ToString();
            }
            return tempStr;
        }

        #endregion instance methods
    }
}
