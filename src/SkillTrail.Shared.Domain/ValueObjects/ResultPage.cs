using System;
using System.Collections.Generic;

namespace SkillTrail.Shared.Domain.ValueObjects
{
    public record ResultPage<TEntity>
    {
        public ResultPage(IReadOnlyCollection<TEntity> items, int pageNumber, int pageSize, int totalItems)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        public IReadOnlyCollection<TEntity> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItems { get; }
        public int TotalPages => (int) Math.Ceiling(TotalItems / (double) PageSize);
    }
}