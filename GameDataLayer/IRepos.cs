using System;
using System.Collections.Generic;
using System.Linq;

namespace GameDataLayer
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Get { get; }

        T AddNew(T value);

        void SaveChanges();
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
            get { return _Context.Set<T>().Include("Players").Include("Games"); }
        }

        public T AddNew(T value)
        {
            var addedValue = _Context.Set<T>().Add(value);
            return addedValue;
        }

        public void SaveChanges()
        {
            _Context.SaveChanges();
        }
    }
}
