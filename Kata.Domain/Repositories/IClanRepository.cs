using Kata.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Repositories
{
    public interface IClanRepository
    {
        Task<IEnumerable<Clan>> GetAllClansAsync();
        Task<Clan?> GetClanByNameAsync(string name);
        Task<Army?> GetArmyByNameAsync(string armyName);
        Task AddArmyAsync(string nameClan,Army army);
        Task UpdateArmyAsync(string nameClan,string armyName, Army army);

        Task DeleteArmyAsync(string nameClan, string armyName);
    }
}
