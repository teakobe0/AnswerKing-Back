﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JzAPI.tool
{
    /// <summary>
    /// 数据加密类
    /// </summary>
    public class DES
    {

        public const string key = "AKadmins";
        //加密
        public static string Encode(string str)

        {

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            byte[] bytes = Encoding.UTF8.GetBytes(str);

            MemoryStream stream = new MemoryStream();

            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);

            stream2.Write(bytes, 0, bytes.Length);

            stream2.FlushFinalBlock();

            StringBuilder builder = new StringBuilder();

            foreach (byte num in stream.ToArray())

            {

                builder.AppendFormat("{0:X2}", num);

            }

            stream.Close();

            return builder.ToString();

        }
        //解密
        public static string Decode(string str)

        {

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

            provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            provider.IV = Encoding.ASCII.GetBytes(key.Substring(0, 8));

            byte[] buffer = new byte[str.Length / 2];

            for (int i = 0; i < (str.Length / 2); i++)

            {

                int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);

                buffer[i] = (byte)num2;

            }

            MemoryStream stream = new MemoryStream();

            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);

            stream2.Write(buffer, 0, buffer.Length);

            stream2.FlushFinalBlock();

            stream.Close();

            return Encoding.UTF8.GetString(stream.ToArray());

        }
    }
}
