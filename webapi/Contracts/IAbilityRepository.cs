using DnDAPI.Models;

namespace DnDAPI.Contracts
{
    public interface IAbilityRepository : IRepositoryBase<Ability>
    {
        Task<Ability> Update(Ability ability);
    }
}
