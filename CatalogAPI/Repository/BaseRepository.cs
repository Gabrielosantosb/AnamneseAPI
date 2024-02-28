using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repository
{
    public class Repository
    {
        public class BaseRepository<T> : IRepository<T> where T : BaseModel
        {
            private readonly MySQLContext _context;
            private readonly DbSet<T> dataset;

            public BaseRepository(MySQLContext context)
            {
                _context = context;
                dataset = _context.Set<T>();
            }

            public List<T> FindAll()
            {
                return dataset.ToList();
            }

            public T FindById(int id)
            {
                var result = dataset.SingleOrDefault(p => p.Id.Equals(id));
                return result;
            }

            public T Create(T item)
            {
                try
                {
                    dataset.Add(item);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return item;
            }

            public T Update(T item)
            {
                var result = dataset.SingleOrDefault(p => p.Id.Equals(item.Id));
                if (result != null)
                    try
                    {
                        _context.Entry(result).CurrentValues.SetValues(item);
                        _context.SaveChanges();
                        return result;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                else
                {
                    return null;
                }
            }

            public T Delete(int id)
            {
                var result = dataset.SingleOrDefault(p => p.Id == id);
                if (result != null)
                    try
                    {
                        dataset.Remove(result);
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                return null;
            }

            public List<T> DeleteAll()
            {
                throw new NotImplementedException();
            }

            public bool ExistsByEmail(string email)
            {
                return _context.Users.Any(u => u.Email == email);
            }
            public bool Exists(int id)
            {
                return dataset.Any(p => p.Id == id);
            }
        }
        }
    }

