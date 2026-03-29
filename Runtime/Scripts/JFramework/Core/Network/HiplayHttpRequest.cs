using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public class HiplayHttpRequest : IHttpRequest
    {
        /// <summary>
        /// 请求headers字典
        /// </summary>
        Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// 请求内容类型
        /// </summary>
        string _contentType = null;

        #region 同步方法
        /// <summary>
        /// 同步删除方法
        /// </summary>
        /// <param name="url"></param>
        public byte[] Delete(string url)
        {
            return CommonHttpRequest(url, "DELETE", "");
        }

        /// <summary>
        /// 同步Get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] Get(string url, Encoding encoding = null)
        {
            return CommonHttpRequest(url, "GET", "", encoding);
        }


        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <returns></returns>
        public byte[] Post(string url, Dictionary<string, string> dic, Encoding encoding = null)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }

            return Post(url, builder.ToString(), encoding);
        }

        /// <summary>
        /// 指定Post地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求后台地址</param>
        /// <param name="content">Post提交数据内容(utf-8编码的)</param>
        /// <returns></returns>
        public byte[] Post(string url, string buffer = null, Encoding encoding = null)
        {
            return CommonHttpRequest(url, "POST", buffer, encoding);
        }

        #endregion

        #region 异步方法
        public async Task<byte[]> DeleteAsync(string url)
        {
            return await CommonHttpRequestAsync(url, "DELETE");
        }

        /// <summary>
        /// 异步Get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<byte[]> GetAsync(string url, Encoding encoding = null)
        {
            return await CommonHttpRequestAsync(url, "GET", "", encoding);
        }

        /// <summary>
        /// 异步post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<byte[]> PostAsync(string url, Dictionary<string, string> dic, Encoding encoding = null)
        {
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }

            return await PostAsync(url, builder.ToString(), encoding);
        }

        /// <summary>
        /// 异步Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<byte[]> PostAsync(string url, string buffer = null, Encoding encoding = null)
        {
            return await CommonHttpRequestAsync(url, "POST", buffer, encoding);
        }

        #endregion

        #region  私有方法
        private byte[] CommonHttpRequest(string url, string type, string data = "", Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            HttpWebRequest myRequest = null;
            Stream outstream = null;
            HttpWebResponse myResponse = null;
            StreamReader reader = null;
            try
            {
                //构造http请求的对象
                myRequest = (HttpWebRequest)WebRequest.Create(url);

                //设置
                myRequest.ProtocolVersion = HttpVersion.Version10;
                myRequest.Method = type;
                foreach (var key in _headers.Keys)
                {
                    myRequest.Headers.Add(key, _headers[key]);
                }

                if (data.Trim() != "")
                {
                    //myRequest.ContentType = "text/xml";
                    myRequest.ContentType = _contentType != null ? _contentType : "text/xml";
                    myRequest.ContentLength = data.Length;
                    //myRequest.Headers.Add("data", data);

                    //转成网络流
                    byte[] buf = encoding.GetBytes(data);

                    outstream = myRequest.GetRequestStream();
                    outstream.Flush();
                    outstream.Write(buf, 0, buf.Length);
                    outstream.Flush();
                    outstream.Close();
                }
                // 获得接口返回值
                myResponse = (HttpWebResponse)myRequest.GetResponse();
                reader = new StreamReader(myResponse.GetResponseStream(), encoding);
                string result = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();
                myRequest.Abort();
                return encoding.GetBytes(result);
            }
            catch (WebException ex)
            {
                // throw new Exception();
                if (outstream != null) outstream.Close();
                if (reader != null) reader.Close();
                if (myResponse != null) myResponse.Close();
                if (myRequest != null) myRequest.Abort();
                throw ex;
            }
        }

        private async Task<byte[]> CommonHttpRequestAsync(string url, string type, string data = "", Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            HttpWebRequest myRequest = null;
            Stream outstream = null;
            HttpWebResponse myResponse = null;
            StreamReader reader = null;
            try
            {
                //构造http请求的对象
                myRequest = (HttpWebRequest)WebRequest.Create(url);

                //设置
                myRequest.ProtocolVersion = HttpVersion.Version10;
                myRequest.Method = type;
                foreach (var key in _headers.Keys)
                {
                    myRequest.Headers.Add(key, _headers[key]);
                }

                if (data.Trim() != "")
                {
                    //myRequest.ContentType = "text/xml";
                    myRequest.ContentType = _contentType != null ? _contentType : "text/xml";
                    myRequest.ContentLength = data.Length;
                    //myRequest.Headers.Add("data", data);

                    //转成网络流
                    byte[] buf = encoding.GetBytes(data);

                    outstream = myRequest.GetRequestStream();
                    outstream.Flush();
                    outstream.Write(buf, 0, buf.Length);
                    outstream.Flush();
                    outstream.Close();
                }
                // 获得接口返回值
                myResponse = (HttpWebResponse)await myRequest.GetResponseAsync();

                reader = new StreamReader(myResponse.GetResponseStream(), encoding);
                string ReturnXml = reader.ReadToEnd();
                reader.Close();
                myResponse.Close();
                myRequest.Abort();
                return encoding.GetBytes(ReturnXml);
            }
            catch (WebException ex)
            {
                // throw new Exception();
                if (outstream != null) outstream.Close();
                if (reader != null) reader.Close();
                if (myResponse != null) myResponse.Close();
                if (myRequest != null) myRequest.Abort();
                throw ex;
            }
        }
        #endregion

        #region 通用请求
        /// <summary>
        /// Http通用请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public byte[] HttpRequest(string url, HttpType type, string inputData = "", Encoding encoding = null)
        {
            switch (type)
            {
                case HttpType.PUT:
                //return Put(url, inputData, encoding);
                case HttpType.GET:
                    return Get(url, encoding);
                case HttpType.POST:
                    return Post(url, inputData, encoding);
                case HttpType.DELETE:
                    return Delete(url);
                default:
                    break;
            }
            return null;
        }

        public async Task<byte[]> HttpRequestAsync(string url, HttpType type, string inputData = "", Encoding encoding = null)
        {
            switch (type)
            {
                case HttpType.PUT:
                //return Put(url, inputData, encoding);
                case HttpType.GET:
                    return await GetAsync(url, encoding);
                case HttpType.POST:
                    return await PostAsync(url, inputData, encoding);
                case HttpType.DELETE:
                    return await DeleteAsync(url);
                default:
                    break;
            }
            return null;
        }


        /// <summary>
        /// Http通用请求
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="uri"></param>
        /// <param name="type"></param>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public byte[] HttpRequest(string ip, string port, string uri, HttpType type, string inputData = "", Encoding encoding = null)
        {
            string url = "http://" + ip + ":" + port + uri;
            return HttpRequest(url, type, inputData, encoding);
        }

        public byte[] HttpsRequest(string ip, string port, string uri, HttpType type, string inputData = "", Encoding encoding = null)
        {
            string url = "https://" + ip + ":" + port + uri;
            return HttpRequest(url, type, inputData, encoding);
        }

        public async Task<byte[]> HttpRequestAsync(string ip, string port, string uri, HttpType type, string inputData = "", Encoding encoding = null)
        {
            string url = "http://" + ip + ":" + port + uri;
            return await HttpRequestAsync(url, type, inputData, encoding);
        }

        public async Task<byte[]> HttpsRequestAsync(string ip, string port, string uri, HttpType type, string inputData = "", Encoding encoding = null)
        {
            string url = "https://" + ip + ":" + port + uri;
            return await HttpRequestAsync(url, type, inputData, encoding);
        }

        /// <summary>
        /// 添加headers
        /// </summary>
        /// <param name="headers"></param>
        public void AddHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    AddHeader(key, headers[key]);
                }
            }
        }

        /// <summary>
        /// 添加header
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
            if (name != null && value != null)
            {
                _headers.Add(name, value);
            }
        }

        /// <summary>
        /// 删除header
        /// </summary>
        /// <param name="name"></param>
        public void RemoveHeader(string name)
        {
            _headers.Remove(name);
        }

        /// <summary>
        /// 设置类型
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            _contentType = contentType;
        }

        /// <summary>
        /// rpc风格请求，输入输出都是对象，内部会自动序列化和反序列化
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual Task<TResponse> HttpRequestAsync<TRequest, TResponse>(string url, TRequest requestData, Encoding encoding = null, IRunable runable = null)
        {
            return Task.FromResult(default(TResponse));
        }



        #endregion


        public enum HttpType
        {
            PUT = 0,
            GET = 1,
            POST = 2,
            DELETE = 3
        }
    }

}
