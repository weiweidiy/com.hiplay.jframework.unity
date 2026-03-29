using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IHttpRequest 
    {
        byte[] Post(string url, Dictionary<string, string> dic, Encoding encoding = null);

        byte[] Post(string url, string content = null, Encoding encoding = null);

        Task<byte[]> PostAsync(string url, Dictionary<string, string> dic, Encoding encoding = null);

        Task<byte[]> PostAsync(string url, string content = null, Encoding encoding = null);

        byte[] Get(string url, Encoding encoding = null);

        Task<byte[]> GetAsync(string url, Encoding encoding = null);

        byte[] Delete(string url);

        Task<byte[]> DeleteAsync(string url);

        void AddHeaders(Dictionary<string, string> headers);

        void AddHeader(string name, string value);

        void RemoveHeader(string name);

        void SetContentType(string contentType);

        Task<TResponse> HttpRequestAsync<TRequest, TResponse>(string url, TRequest requestData, Encoding encoding = null, IRunable runable = null);
    }
}
