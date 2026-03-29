using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    public interface IEncrypter
    {
        byte[] Encrypt(byte[] bytes);
    }
}
