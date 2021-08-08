using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SkillTrail.Shared.Domain.Abstract;
using SkillTrail.Shared.Domain.ValueObjects;

namespace SkillTrail.Shared.Infrastructure.Database
{
    public interface IRepository<TEntity, TId> where TEntity : AggregateRoot<TId>
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> GetAllByIdsAsync(IEnumerable<TId> ids,
            CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> FindAllAsync(Specification<TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<ResultPage<TEntity>> FindPageAsync(Specification<TEntity> specification, Paginator<TEntity> paginator,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> FindAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> expression,
            CancellationToken cancellationToken = default);
    }
}