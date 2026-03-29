using System;
using System.Collections.Generic;

namespace JFramework
{
    public interface ITypeRegister
    {
        Dictionary<int, Type> GetTypes();
    }
}
