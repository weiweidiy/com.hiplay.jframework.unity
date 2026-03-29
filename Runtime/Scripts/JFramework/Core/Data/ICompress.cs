using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    public interface ICompress
    {
        byte[] Compress(byte[] bytes);
    }
}
