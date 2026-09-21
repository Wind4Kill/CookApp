using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Application.Interfaces.Caching
{
    public interface ICacheService<T> where T : class
    {
        Task<T?> GetValueAsync(int id, CancellationToken cancellationToken);
        Task AddValueAsync(T entity, int id, CancellationToken cancellationToken);
        Task RemoveAsync(int id, CancellationToken cancellationToken);
    }
}