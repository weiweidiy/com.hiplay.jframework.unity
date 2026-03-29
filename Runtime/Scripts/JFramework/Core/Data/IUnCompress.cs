using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    public interface IUnCompress
    {
        byte[] UnCompress(byte[] bytes, Encoding coding = null);
    }
}
