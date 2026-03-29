using System;
using System.Collections.Generic;
using System.Text;

namespace JFramework
{
    public interface IReaderSync
    {
        byte[] Read(string location);
    }
}
