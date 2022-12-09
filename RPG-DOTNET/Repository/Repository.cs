using Microsoft.EntityFrameworkCore;
using RPG_DOTNET.Data;
using RPG_DOTNET.Models;

namespace RPG_DOTNET.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<T> entities;
        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
            entities = _dataContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
         
        public T FindById(int id)
        {
            return entities.Find(id);
        }

        public void Insert(T entity)
        {
            entities.Add(entity);
            return;
        }

        public void Delete(T entity)
        {
            entities.Remove(entity);
            return;
        }
    }
}
