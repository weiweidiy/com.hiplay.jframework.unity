using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public abstract class Writer : IWriter
    {
        public abstract void Write(string toPath, byte[] buffer, Encoding encoding = null);
        public abstract void Write(string toPath, string buffer, Encoding encoding = null);
        public abstract Task WriteAsync(string toPath, byte[] buffer, Encoding encoding = null);
        public abstract Task WriteAsync(string toPath, string buffer, Encoding encoding = null);

        /// <summary>
        /// 数据加工处理器
        /// </summary>
        private JDataProcesserManager _processer;

        /// <summary>
        /// 无参构造器
        /// </summary>
        public Writer() : this(null) { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="processer">数据加工处理器</param>
        public Writer(JDataProcesserManager processer)
        {
            _processer = processer;
        }

        /// <summary>
        /// 返回加工处理结果
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected byte[] GetProcessResult(byte[] bytes)
        {
            byte[] result = (byte[])bytes.Clone();
            if (_processer != null)
                return _processer.GetResult(result);

            return result;
        }
    }
}
