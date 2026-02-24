using JFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Common
{
    public class CustomCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // 这里可以添加自定义的证书验证逻辑，或者直接返回true来接受所有证书
            return true;
        }
    }

    public class ErrorMessage
    {
        public string message;
    }

    public class HttpResponseException : Exception
    {
        public string ResponseText { get; }
        public long StatusCode { get; }

        public HttpResponseException(string message, string responseText, long statusCode)
            : base(message)
        {
            ResponseText = responseText;
            StatusCode = statusCode;
        }
    }

    public class DefaultHttpRequest : IHttpRequest
    {
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private string contentType = "application/json";
        CustomCertificateHandler customCertificateHandler;
        ISerializer serializer;
        IDeserializer deserializer;

        public DefaultHttpRequest(ISerializer serializer, IDeserializer deserializer, CustomCertificateHandler customCertificateHandler = null)
        {
            this.customCertificateHandler = customCertificateHandler;
        }
        public void AddHeader(string name, string value)
        {
            if (headers.ContainsKey(name))
            {
                headers[name] = value;
            }
            else
            {
                headers.Add(name, value);
            }
        }

        public void AddHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                AddHeader(header.Key, header.Value);
            }
        }

        public void SetContentType(string contentType)
        {
            this.contentType = contentType;
        }

        public byte[] Get(string url, Encoding encoding = null)
        {
            return GetAsync(url, encoding).GetAwaiter().GetResult();
        }

        public async Task<byte[]> GetAsync(string url, Encoding encoding = null)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                SetupRequest(webRequest);
                await SendRequest(webRequest);
                return webRequest.downloadHandler.data;
            }
        }

        public byte[] Post(string url, Dictionary<string, string> dic, Encoding encoding = null)
        {
            return PostAsync(url, dic, encoding).GetAwaiter().GetResult();
        }

        public byte[] Post(string url, string content = null, Encoding encoding = null)
        {
            return PostAsync(url, content, encoding).GetAwaiter().GetResult();
        }

        public async Task<byte[]> PostAsync(string url, Dictionary<string, string> dic, Encoding encoding = null)
        {
            WWWForm form = new WWWForm();
            foreach (var kvp in dic)
            {
                form.AddField(kvp.Key, kvp.Value);
            }

            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
            {
                webRequest.certificateHandler = customCertificateHandler;
                SetupRequest(webRequest);
                await SendRequest(webRequest);
                return webRequest.downloadHandler.data;
            }
        }

        public async Task<byte[]> PostAsync(string url, string content = null, Encoding encoding = null)
        {
            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                webRequest.certificateHandler = customCertificateHandler;
                if (!string.IsNullOrEmpty(content))
                {
                    byte[] bodyRaw = (encoding ?? Encoding.UTF8).GetBytes(content);
                    webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                }
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                SetupRequest(webRequest);
                webRequest.SetRequestHeader("Content-Type", contentType);
                await SendRequest(webRequest);
                return webRequest.downloadHandler.data;
            }
        }

        public byte[] Delete(string url)
        {
            return DeleteAsync(url).GetAwaiter().GetResult();
        }

        public async Task<byte[]> DeleteAsync(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
            {
                SetupRequest(webRequest);
                await SendRequest(webRequest);
                return webRequest.downloadHandler?.data ?? new byte[0];
            }
        }

        public void RemoveHeader(string name)
        {

            if (headers.ContainsKey(name))
            {
                headers.Remove(name);
            }
        }

        private void SetupRequest(UnityWebRequest webRequest)
        {
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }
        }

        private async Task SendRequest(UnityWebRequest webRequest)
        {
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // 抛出自定义异常，带上响应内容
                throw new HttpResponseException(
                    $"HTTP Request failed: {webRequest.error}\nURL: {webRequest.url}",
                    webRequest.downloadHandler?.text,
                    webRequest.responseCode
                );

                //throw new Exception($"HTTP Request failed: {webRequest.error}\nURL: {webRequest.url}\nResponse: {webRequest.downloadHandler?.text}");
            }
        }

        public async Task<TResponse> HttpRequestAsync<TRequest, TResponse>(string url, TRequest requestData, Encoding encoding = null,IRunable runable = null)
        {
            try
            {
                var response = await PostBodyAsync<TRequest, TResponse>(url, requestData, encoding, runable);
                if (response == null)
                {
                    throw new System.Exception("Draw failed, response is null");
                }
                return response;
            }
            catch (HttpResponseException ex)
            {
                // 解析服务器返回的错误信息
                var errorJson = ex.ResponseText;
                string message = null;
                try
                {
                    // 用你自己的反序列化工具
                    var errorObj = deserializer.ToObject<ErrorMessage>(errorJson);
                    message = errorObj?.message;
                }
                catch
                {
                    // 解析失败
                    throw new Exception(message ?? ex.Message);
                }
                throw new Exception(message ?? ex.Message);
            }
        }

        public async Task<TResp> PostBodyAsync<TReq, TResp>(string url, TReq body, Encoding encoding = null, IRunable runable = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            string json = null;
            if (body != null)
            {
                json = serializer.Serialize(body);
            }

            runable?.Start(null);
            var result = await PostAsync(url, json, encoding);
            runable?.Stop();
            var response = encoding.GetString(result);
            TResp responseObject = deserializer.ToObject<TResp>(response);
            return responseObject;
        }
    }
}