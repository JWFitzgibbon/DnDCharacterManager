using DnDAPI.Contracts;
using DnDAPI.Models;

namespace DnDAPI.Repository
{
    public class CharacterRepository : RepositoryBase<Character>, ICharacterRepository
    {
        private readonly RepositoryContext _context;

        public CharacterRepository(RepositoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Character> Update(Character entity)
        {
            entity.LastUpdatedDate = DateTime.UtcNow;
            _context.Characters.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
