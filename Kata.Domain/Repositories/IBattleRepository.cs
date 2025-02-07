using Kata.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Repositories
{
    public interface IBattleRepository
    {
        Task SaveBattleReport(BattleReport battleReport);
        Task<IEnumerable<BattleReport>> GetAllBattlesReport();
        Task<BattleReport?> GetBattleReportById(String id);
    }
}
