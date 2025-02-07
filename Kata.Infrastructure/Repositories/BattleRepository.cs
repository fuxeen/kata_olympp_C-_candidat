using Kata.Domain.Entities;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kata.Infrastructure.Repositories
{
    public class BattleRepository : IBattleRepository
    {
        private readonly KataDBContext _context;
        public BattleRepository (KataDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BattleReport>> GetAllBattlesReport()
        {
            try
            {
                // Retrieves all BattleReports from the database
                return await _context.BattleReports.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (consider using ILogger for proper logging)
                Console.WriteLine($"Error while retrieving battle reports: {ex.Message}");
                return Enumerable.Empty<BattleReport>();  // Return an empty list if there's an error
            }

        }

        public async Task<BattleReport?> GetBattleReportById(string id)
        {
            // Validate input: Ensure the provided ID is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("The ID cannot be null or empty :" + id);
            }

            // Retrieve the BattleReport by ID from the repository or database
            return await _context.BattleReports.Where( report => report.ID == id ).FirstAsync();
        }


        public async Task SaveBattleReport(BattleReport battleReport)
        {
            if( battleReport == null)
            {
                throw new ArgumentNullException(nameof(battleReport), "The report object cannot be null.");
            }
            
            await _context.BattleReports.AddAsync(battleReport);
            await _context.SaveChangesAsync();
        }
    }
}
