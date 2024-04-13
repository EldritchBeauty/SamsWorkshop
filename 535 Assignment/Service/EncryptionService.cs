using System.Security.Cryptography;

namespace _535_Assignment.Service
{
    public class EncryptionService
    {
        private readonly string _secretKey;

        public EncryptionService(IConfiguration configuration)
        {
            _secretKey = configuration["SecretKey"];
        }

        /// <summary>
        /// Encrypt's image byte array using AES.
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public byte[] EncryptByteArray(byte[] fileData)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(_secretKey);

                ICryptoTransform encrypt = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, 16);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encrypt, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(fileData, 0, fileData.Length);
                        csEncrypt.FlushFinalBlock();
                        return msEncrypt.ToArray();
                    }
                }
            }

        }

        /// <summary>
        /// Decrypt's image byte array using AES.
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public byte[] DecryptByteArray(byte[] eFileData)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(_secretKey) ;
            

                byte[] IV = new byte[16];

                Array.Copy(eFileData, 0, IV, 0, 16);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, IV);

                using (MemoryStream msDecrypt = new MemoryStream(eFileData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(eFileData, 16, eFileData.Length - 16);
                        csDecrypt.FlushFinalBlock();
                        return msDecrypt.ToArray();
                    }
                }
            }
        }
    }
}
