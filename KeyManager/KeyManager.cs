using System;
using System.Collections.Generic;
using System.Text;

namespace KeyManager
{
    public sealed class KeyManager
    {
        public static string GenerateKey(string UniqueId)
        {
            string md5OfUniqueId = Md5Encrypt("meepo" + UniqueId + "mootoo");
            string parity = Md5Encrypt("meepomeepo" + md5OfUniqueId + "mootoomootoo");
            return md5OfUniqueId + "" + parity;
        }

        public static bool IsKeyValid(string KEY)
        {
#if SKIP_KEY
            if (true) return true;
#endif
            try
            {
                string[] tokens = new string[2] { KEY.Substring(0, 32), KEY.Substring(32) };

                string parity = Md5Encrypt("meepomeepo" + tokens[0] + "mootoomootoo");
                if (parity != tokens[1])
                {
                    return false;
                }

                string UniqueId = GetCpuId.GetCpuId.GetId();

                string md5OfUniqueId = Md5Encrypt("meepo" + UniqueId + "mootoo");

                if (md5OfUniqueId == tokens[0]) return true;
                else return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string Md5Encrypt(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();

        }
    }
}
