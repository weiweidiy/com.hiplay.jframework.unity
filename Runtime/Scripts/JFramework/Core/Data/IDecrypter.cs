using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    public interface IDecrypter
    {
        byte[] Decrypt(byte[] bytes);
    }
}
