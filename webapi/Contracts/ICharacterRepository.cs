using DnDAPI.Models;

namespace DnDAPI.Contracts
{
    public interface ICharacterRepository : IRepositoryBase<Character>
    {
        Task<Character> Update(Character entity);
    }
}
