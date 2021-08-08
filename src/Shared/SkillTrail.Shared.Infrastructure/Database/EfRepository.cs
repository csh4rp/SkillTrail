using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillTrail.Shared.Domain.Abstract;
using SkillTrail.Shared.Domain.ValueObjects;

namespace SkillTrail.Shared.Infrastructure.Database
{
    internal sealed class EfRepository<TDbContext, TEntity, TId> : IRepository<TEntity, TId> where TEntity : AggregateRoot<TId> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TEntity> _set;
        private readonly DbSet<DomainEventSnapshot> _snapshotsSet;
        private readonly IDomainEventSnapshotFactory _domainEventSnapshotFactory;

        public EfRepository(TDbContext dbContext, IDomainEventSnapshotFactory domainEventSnapshotFactory)
        {
            _dbContext = dbContext;
            _set = dbContext.Set<TEntity>();
            _snapshotsSet = dbContext.Set<DomainEventSnapshot>();
            _domainEventSnapshotFactory = domainEventSnapshotFactory;
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _set.Add(entity);
            if (!entity.DomainEvents.Any())
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return;
            }
            
            if (_dbContext.Database.CurrentTransaction is null)
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = _domainEventSnapshotFactory.Create(entity);
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = _domainEventSnapshotFactory.Create(entity);
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            
            entity.ClearDomainEvents();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entityList = entities as IReadOnlyCollection<TEntity> ?? entities.ToList();
            _set.AddRange(entityList);
            if (_dbContext.Database.CurrentTransaction is null)
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = entityList.SelectMany(e => _domainEventSnapshotFactory.Create(e));
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = entityList.SelectMany(e => _domainEventSnapshotFactory.Create(e));
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            
            foreach (var aggregateRoot in entityList)
            {
                aggregateRoot.ClearDomainEvents();
            }
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (!entity.DomainEvents.Any())
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return;
            }
            
            if (_dbContext.Database.CurrentTransaction is null)
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = _domainEventSnapshotFactory.Create(entity);
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = _domainEventSnapshotFactory.Create(entity);
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            
            entity.ClearDomainEvents();
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var entityList = entities as IReadOnlyCollection<TEntity> ?? entities.ToList();
            if (_dbContext.Database.CurrentTransaction is null)
            {
                var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = entityList.SelectMany(e => _domainEventSnapshotFactory.Create(e));
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                var snapshots = entityList.SelectMany(e => _domainEventSnapshotFactory.Create(e));
                _snapshotsSet.AddRange(snapshots);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            
            foreach (var aggregateRoot in entityList)
            {
                aggregateRoot.ClearDomainEvents();
            }
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (typeof(TEntity).IsAssignableTo(typeof(ISoftDeletable)))
            {
                var entry = _dbContext.Entry(entity);
                entry.Property<DateTime?>("DeletedAt").CurrentValue = DateTime.UtcNow;
            }
            else
            {
                _set.Remove(entity);
            }

            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (typeof(TEntity).IsAssignableTo(typeof(ISoftDeletable)))
            {
                foreach (var entity in entities)
                {
                    var entry = _dbContext.Entry(entity);
                    entry.Property<DateTime?>("DeletedAt").CurrentValue = DateTime.UtcNow;
                }
            }
            else
            {
                _set.RemoveRange(entities);
            }

            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default)
            => _set.CountAsync(cancellationToken);

        public Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
            => _set.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
        

        public async Task<IReadOnlyList<TEntity>> GetAllByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default)
            => await _set.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);

        public Task<TEntity> FindAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            var query = FilterQuery(_set, specification);
            return query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TEntity>> FindAllAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            var query = FilterQuery(_set, specification);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<ResultPage<TEntity>> FindPageAsync(Specification<TEntity> specification, Paginator<TEntity> paginator, CancellationToken cancellationToken = default)
        {
            var query = FilterQuery(_set, specification);
            
            for (var i = 0; i < paginator.SortExpressions.Count; i ++)
            {
                var sortExpression = paginator.SortExpressions.Skip(i).First();
                if (i == 0)
                {
                    query = sortExpression.SortOrder == SortOrder.Asc
                        ? query.OrderBy(sortExpression.PropertyExpression)
                        : query.OrderByDescending(sortExpression.PropertyExpression);
                }
                else
                {
                    query = sortExpression.SortOrder == SortOrder.Asc
                        ? ((IOrderedQueryable<TEntity>) query).ThenBy(sortExpression.PropertyExpression)
                        : ((IOrderedQueryable<TEntity>) query).ThenByDescending(sortExpression.PropertyExpression);
                }
            }

            var items = await query.Skip(paginator.Offset)
                .Take(paginator.PageSize)
                .ToListAsync(cancellationToken);

            var totalItems = await query.CountAsync(cancellationToken);
            return new ResultPage<TEntity>(items, paginator.PageNumber, paginator.PageSize, totalItems);
        }

        public async Task<IReadOnlyList<TEntity>> FindAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> filter,
            CancellationToken cancellationToken = default)
            => await filter(_set).ToListAsync(cancellationToken);

        private static IQueryable<TEntity> FilterQuery(IQueryable<TEntity> queryable,
            Specification<TEntity> specification)
        {
            var query = specification.Filters.Aggregate(queryable,
                (current, filter) => current.Where(filter));

            return specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}