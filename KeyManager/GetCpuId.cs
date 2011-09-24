using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace GetCpuId
{
    public class GetCpuId
    {
        public static string GetId()
        {
            ManagementObject dsk = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
            dsk.Get();

            string a = dsk["VolumeSerialNumber"].ToString();

            return "_" + Md5Encrypt("+meepo"+a+"mootoo") + "_";
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
