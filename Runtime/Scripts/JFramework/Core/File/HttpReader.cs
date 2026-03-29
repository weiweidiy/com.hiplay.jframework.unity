using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    /// <summary>
    /// 远程Http加载配置 to do: 添加httpRequest类 负责实际的请求
    /// </summary>
    public class HttpReader : Reader
    {
        /// <summary>
        /// webRequest对象
        /// </summary>
        IHttpRequest _webRequest;

        public HttpReader(IHttpRequest webRequest) : this(webRequest, null) { }

        public HttpReader(IHttpRequest webRequest, JDataProcesserManager processer) : base(processer)
        {
            _webRequest = webRequest;
        }

        public override Task<bool> ExistsAsync(string location)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override byte[] Read(string url)
        {
            try
            {
                return _webRequest.Get(url);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override async Task<T> ReadAsync<T>(string url, IDeserializer converter)
        {
            try
            {
                return converter.ToObject<T>(await _webRequest.GetAsync(url));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return default;
            }
        }


        ///// <summary>
        ///// 指定Post地址使用Get 方式获取全部字符串
        ///// </summary>
        ///// <param name="url">请求后台地址</param>
        ///// <returns></returns>
        //string Post(string url)
        //{
        //    string result = "";
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.Method = "POST";
        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    //获取内容
        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        result = reader.ReadToEnd();
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 指定Post地址使用Get 方式获取全部字符串
        ///// </summary>
        ///// <param name="url">请求后台地址</param>
        ///// <returns></returns>
        //public static string Post(string url, Dictionary<string, string> dic)
        //{
        //    string result = "";
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    #region 添加Post 参数
        //    StringBuilder builder = new StringBuilder();
        //    int i = 0;
        //    foreach (var item in dic)
        //    {
        //        if (i > 0)
        //            builder.Append("&");
        //        builder.AppendFormat("{0}={1}", item.Key, item.Value);
        //        i++;
        //    }
        //    byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
        //    req.ContentLength = data.Length;
        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(data, 0, data.Length);
        //        reqStream.Close();
        //    }
        //    #endregion
        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    //获取响应内容
        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        result = reader.ReadToEnd();
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 指定Post地址使用Get 方式获取全部字符串
        ///// </summary>
        ///// <param name="url">请求后台地址</param>
        ///// <param name="content">Post提交数据内容(utf-8编码的)</param>
        ///// <returns></returns>
        //public static string Post(string url, string content)
        //{
        //    string result = "";
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";

        //    #region 添加Post 参数
        //    byte[] data = Encoding.UTF8.GetBytes(content);
        //    req.ContentLength = data.Length;
        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(data, 0, data.Length);
        //        reqStream.Close();
        //    }
        //    #endregion

        //    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //    Stream stream = resp.GetResponseStream();
        //    //获取响应内容
        //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //    {
        //        result = reader.ReadToEnd();
        //    }
        //    return result;
        //}


    }
}
