using RPG_DOTNET.Models;

namespace RPG_DOTNET.Repository
{
    public interface IRepository<T> where T : class
    {
         IEnumerable<T> GetAll();
         T FindById(int id);
        void Insert(T entity);
        void Delete(T entity);
    }
}
