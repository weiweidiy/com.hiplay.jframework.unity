using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IDeleteAsync
    {
        Task<bool> DeleteAsync(string location);

        Task ClearAsync();
    }
}
