using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Images.Repository.DataContracts
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// With tracking for the entity changes enabled.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync(long id);

        /// <summary>
        /// GetAll entities AsNotracking detection
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
