using System.Collections.Generic;

namespace SkillTrail.Shared.Domain.ValueObjects
{
    public record Paginator<TEntity>
    {
        public Paginator(int pageSize, int pageNumber, IReadOnlyCollection<SortExpression<TEntity>> sortExpressions)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            SortExpressions = sortExpressions;
        }

        public int PageSize { get; }
        public int PageNumber { get; }
        
        public IReadOnlyCollection<SortExpression<TEntity>> SortExpressions { get; }
        public int Offset => PageSize * PageNumber;
    }
}