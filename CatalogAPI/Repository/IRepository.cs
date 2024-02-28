using CatalogAPI.Models;

namespace CatalogAPI.Repository
{
    public interface IRepository<T> where T : BaseModel
    {
        T Create(T item);
        T Update(T item);
        T Delete(int id);

        T FindById(int id);
        List<T> FindAll();
        List<T> DeleteAll();

        bool Exists(int id);
    }
}
