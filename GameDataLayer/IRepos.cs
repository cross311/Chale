using System;
using System.Collections.Generic;
using System.Linq;

namespace GameDataLayer
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get { get; }

        T Save(T value);
    }

    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        private TournamentContext _Context;

        public EntityFrameworkRepository(TournamentContext context)
        {
            _Context = context;
        }

        IQueryable<T> IRepository<T>.Get
        {
            get { return _Context.Set<T>(); }
        }

        public T Save(T value)
        {
            var addedValue = _Context.Set<T>().Add(value);
            _Context.SaveChanges();
            return addedValue;
        }
    }
}
