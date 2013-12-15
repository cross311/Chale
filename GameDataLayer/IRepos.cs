using System;
using System.Collections.Generic;
using System.Linq;

namespace GameDataLayer
{
    public interface IRepository<T>
    {
        IQueryable<T> Get { get; }

        T Save(T value);
    }
}
