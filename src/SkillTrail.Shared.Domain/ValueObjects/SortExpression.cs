using System;
using System.Linq.Expressions;

namespace SkillTrail.Shared.Domain.ValueObjects
{
    public class SortExpression<TEntity>
    {
        public SortExpression(Expression<Func<TEntity, object>> propertyExpression, SortOrder sortOrder)
        {
            PropertyExpression = propertyExpression;
            SortOrder = sortOrder;
        }

        public Expression<Func<TEntity, object>> PropertyExpression { get; }
        public SortOrder SortOrder { get; }
    }
}