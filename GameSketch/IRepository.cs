using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSketch
{
    public interface IRepository<T>
    {
        IQueryable<T> Get { get; }

        T Save(T value);
    }
}
