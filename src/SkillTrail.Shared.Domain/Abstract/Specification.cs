using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SkillTrail.Shared.Domain.Abstract
{
    public abstract class Specification<TEntity> where TEntity : IAggregateRoot
    {
        private readonly List<Expression<Func<TEntity, bool>>> _filters = new();
        private readonly List<Expression<Func<TEntity, object>>> _includes = new();

        public IReadOnlyCollection<Expression<Func<TEntity, bool>>> Filters => _filters;
        public IReadOnlyCollection<Expression<Func<TEntity, object>>> Includes => _includes;
        
        protected void AddFilter(Expression<Func<TEntity, bool>> filter)
        {
            _filters.Add(filter);
        }

        public void AddInclude(Expression<Func<TEntity, object>> include)
        {
            _includes.Add(include);
        }
    }
}