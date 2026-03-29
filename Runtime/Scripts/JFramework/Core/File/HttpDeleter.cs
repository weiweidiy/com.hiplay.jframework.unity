using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public class HttpDeleter : IDelete
    {

        private IHttpRequest _webRequest;

        public HttpDeleter(IHttpRequest request)
        {
            _webRequest = request;
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public void Delete(string location)
        {
            _webRequest.Delete(location);
        }

        public async Task<bool> DeleteAsync(string location)
        {
            try
            {
                await _webRequest.DeleteAsync(location);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
