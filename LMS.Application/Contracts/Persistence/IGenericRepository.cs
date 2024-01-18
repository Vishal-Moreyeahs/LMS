using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models;

namespace LMS.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task AddRange(List<T> entity);
        Task Update(T entity);
        Task Delete(T entity);
        IEnumerable<T> GetAllRelatedEntity();
    }
}
