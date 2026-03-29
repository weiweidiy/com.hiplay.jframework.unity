using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JFramework
{
    public class RijndaelDecrypter : IDecrypter, IProcesser
    {
        /// <summary>
        /// 32位密钥
        /// </summary>
        private string _pKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pKey">32位密钥</param>
        public RijndaelDecrypter(string pKey)
        {
            _pKey = pKey;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] bytes)
        {
            //密钥
            byte[] keyArray = Encoding.UTF8.GetBytes(_pKey);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return resultArray;
        }

        public byte[] Process(byte[] bytes)
        {
            return Decrypt(bytes);
        }
    }
}
