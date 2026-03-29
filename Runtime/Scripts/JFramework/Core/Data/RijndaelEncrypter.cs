using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JFramework
{

    public class RijndaelEncrypter : IEncrypter, IProcesser
    {
        /// <summary>
        /// 加密密码：32位字符串
        /// </summary>
        private string _pKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pKey">32位字符串加密密码</param>
        public RijndaelEncrypter(string pKey)
        {
            _pKey = pKey;
        }

        /// <summary>
        /// 加密字节数组
        /// </summary>
        /// <param name="toEncryptArray"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] toEncryptArray)
        {
            //密匙
            byte[] keyArray = Encoding.UTF8.GetBytes(_pKey);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public byte[] Process(byte[] bytes)
        {
            return Encrypt(bytes);
        }
    }
}
