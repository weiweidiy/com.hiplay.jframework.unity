using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Game
{
    public interface IConfigTable<T> : IEnumerable<T> where T : IUnique
    {
        void Initialize(T[] lst);
    }


}