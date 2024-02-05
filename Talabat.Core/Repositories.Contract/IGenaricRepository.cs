using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenaricRepository<T> where T:BaseEntity
    {
        Task<T?> GetAsync(int id);
        public Task<IReadOnlyList<T>> GetAllAsync();

        Task<T?> GetWithSpecificationAsync(ISpecifictaions<T> spec);
        public Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecifictaions<T> spec);

        Task<int> GetCountAsync(ISpecifictaions<T> spec);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
