using System;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence
{
    public class Encryptor : ValueConverter<string, string>
    {

        public static readonly string PasswordHash = "P@@Sw0rd";
        public static readonly string SaltKey = "S@LT&KEY";
        public static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        public static readonly byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        public static readonly ICryptoTransform decryptor = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 }.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
        public static readonly ICryptoTransform encryptor = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 }.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));


        public Encryptor(ConverterMappingHints mappingHints = default) : base(EncryptExpr, DecryptExpr, mappingHints)
        { }

        static Expression<Func<string, string>> DecryptExpr = x => new string(decrypt(x));
        static Expression<Func<string, string>> EncryptExpr = x => new string(encrypt(x));

        public static string encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string decrypt(string encryptedText)
        {

            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}