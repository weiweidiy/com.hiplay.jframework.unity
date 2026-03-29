using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public class LocalWriter : Writer
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public LocalWriter() : base() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processer"></param>
        public LocalWriter(JDataProcesserManager processer) : base(processer) { }


        /// <summary>
        /// 同步写入
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="bytes"></param>
        public override void Write(string toPath, byte[] bytes, Encoding encoding = null)
        {
            bytes = GetProcessResult(bytes);

            using (FileStream fs = new FileStream(toPath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 同步写入
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="buffer"></param>
        public override void Write(string toPath, string buffer, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            byte[] bytes = encoding.GetBytes(buffer);

            Write(toPath, bytes);
        }

        /// <summary>
        /// 把字节数组写到文件
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="bytes"></param>
        public override async Task WriteAsync(string toPath, byte[] bytes, Encoding encoding = null)
        {
            //数据加工
            bytes = GetProcessResult(bytes);

            using (FileStream fs = new FileStream(toPath, FileMode.Create))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 把字符串写到文件
        /// </summary>
        /// <param name="toPath"></param>
        /// <param name="buffer"></param>
        public override async Task WriteAsync(string toPath, string buffer, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            byte[] bytes = encoding.GetBytes(buffer);

            await WriteAsync(toPath, bytes);
        }
    }
}
