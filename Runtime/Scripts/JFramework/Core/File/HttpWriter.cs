using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public class HttpWriter : Writer
    {
        /// <summary>
        /// webRequest对象
        /// </summary>
        IHttpRequest _webRequest;

        /// <summary>
        /// 返回结果
        /// </summary>
        string _response = null;

        public HttpWriter(IHttpRequest httpRequest, JDataProcesserManager processer = null) : base(processer)
        {
            _webRequest = httpRequest;
        }

        /// <summary>
        /// 同步写入远程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        public override void Write(string url, byte[] buffer, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            string content = encoding.GetString(buffer);
            Write(url, content, encoding);
        }

        /// <summary>
        /// 同步写入远程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        public override void Write(string url, string buffer, Encoding encoding = null)
        {
            //Console.WriteLine(url);
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(buffer);
            bytes = GetProcessResult(bytes);
            buffer = encoding.GetString(bytes);
            //Console.WriteLine("readyToSave " + buffer);
            byte[] response = _webRequest.Post(url, buffer);
            _response = encoding.GetString(response);
        }

        /// <summary>
        /// 异步写入远程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public override async Task WriteAsync(string url, byte[] buffer, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            string content = encoding.GetString(buffer);
            await WriteAsync(url, content, encoding);
        }

        /// <summary>
        /// 异步写入远程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public override async Task WriteAsync(string url, string buffer, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(buffer);
            bytes = GetProcessResult(bytes);
            buffer = encoding.GetString(bytes);
            //Console.WriteLine("readyToSave " + buffer);
            byte[] response = await _webRequest.PostAsync(url, buffer);
            _response = encoding.GetString(response);
        }
    }
}
