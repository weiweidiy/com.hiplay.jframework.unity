using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IWriterAsync
    {
        /// <summary>
        /// 异步写入
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="buffer"></param>
        Task WriteAsync(string toPath, byte[] buffer, Encoding encoding = null);

        /// <summary>
        /// 异步写入
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="buffer"></param>
        Task WriteAsync(string toPath, string buffer, Encoding encoding = null);
    }
}
