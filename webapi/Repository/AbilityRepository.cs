using DnDAPI.Contracts;
using DnDAPI.Models;

namespace DnDAPI.Repository
{
    public class AbilityRepository : RepositoryBase<Ability>, IAbilityRepository
    {
        private readonly RepositoryContext _context;

        public AbilityRepository(RepositoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Ability> Update(Ability entity)
        {
            entity.LastUpdatedDate = DateTime.UtcNow;
            _context.Abilities.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
