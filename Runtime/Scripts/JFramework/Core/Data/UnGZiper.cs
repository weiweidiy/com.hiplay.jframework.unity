using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace JFramework
{
    public class UnGZiper : IUnCompress, IProcesser
    {
        public byte[] Process(byte[] bytes)
        {
            return UnCompress(bytes);
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="bytes">需要解压的字节数组</param>
        /// <param name="encoding">默认UTF8</param>
        /// <returns></returns>
        public byte[] UnCompress(byte[] bytes, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (GZipStream decompressedStream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(decompressedStream, encoding))
                    {
                        string result = reader.ReadToEnd();//重点
                        return encoding.GetBytes(result);
                    }
                }
            }
        }
    }
}
