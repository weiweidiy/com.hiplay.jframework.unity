using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    /// <summary>
    /// 数据加工器
    /// </summary>
    public interface IProcesser
    {
        byte[] Process(byte[] bytes);
    }
}
