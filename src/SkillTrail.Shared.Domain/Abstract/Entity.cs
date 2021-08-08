namespace SkillTrail.Shared.Domain.Abstract
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; protected set; }
    }
}