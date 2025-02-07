using Kata.Domain.Entities;
using Kata.Domain.Repositories;
using Kata.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kata.Infrastructure.Repositories
{
    public class ClanRepository : IClanRepository
    {
        private readonly KataDBContext _context;
        public ClanRepository(KataDBContext context)
        {
            _context = context;
            
            // initiate the clans
            if (_context.Clans.Count() != 2)
            {
                // Clear existing clans only if necessary
                if (_context.Clans.Any())
                {
                    _context.Clans.RemoveRange(_context.Clans);
                }

                // Add clans
                var clansToAdd = new List<Clan>
                {
                    new Clan("Athens"),
                    new Clan("Troy")
                };

                _context.Clans.AddRange(clansToAdd);
                _context.SaveChanges();
            }
        }
        public async Task<Army?> GetArmyByNameAsync(string armyName)
        {
            // Validate input to ensure the name is not null, or whitespace
            if (string.IsNullOrWhiteSpace(armyName))
            {
                throw new ArgumentException("The name cannot be null or empty.", nameof(armyName));
            }
            
            // Use LINQ to search for an Army with the given name across all Clans
            return await _context.Clans
                .Include(c => c.Armies) // Ensure Armies are included in the query
                .SelectMany(c => c.Armies) // Flatten Armies to a single collection
                .FirstOrDefaultAsync(a => a.Name == armyName); // Find the Army by name
        }

        public async Task<Clan?> GetClanByNameAsync(string name)
        {
            // Ensure the name is not null or empty
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The clan name cannot be null or empty.", nameof(name));
            }
            
            return await _context.Clans.FirstOrDefaultAsync(c => c.Name == name);
        }


        public async Task AddArmyAsync(string clanName, Army army)
        {
            if (string.IsNullOrWhiteSpace(clanName))
            {
                throw new ArgumentException("The clan name cannot be null or empty.", nameof(clanName));
            }
            
            if (army == null)
            {
                throw new ArgumentNullException(nameof(army), "The army object cannot be null.");
            }
            
            var clan = await _context.Clans.FirstOrDefaultAsync(c => c.Name == clanName);

            if (clan != null)
            {
                clan.Armies.Add(army);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"Clan with name '{clanName}' was not found.");
            }
        }

        public async Task UpdateArmyAsync(string nameClan, string armyName, Army army)
        {
            if (string.IsNullOrWhiteSpace(nameClan))
            {
                throw new ArgumentException("The clan name cannot be null or empty.", nameof(nameClan));
            }
            if (string.IsNullOrWhiteSpace(armyName))
            {
                throw new ArgumentException("The army name cannot be null or empty.", nameof(armyName));
            }
            if (army == null)
            {
                throw new ArgumentNullException(nameof(army), "The updated army object cannot be null.");
            }

            // Search the clan by its name
            var clan = await _context.Clans
                .Include(c => c.Armies) // Load the collection of associated armies
                .FirstOrDefaultAsync(c => c.Name == nameClan);

            if (clan == null)
            {
                throw new InvalidOperationException($"Clan with name '{nameClan}' was not found.");
            }
            
            var oldArmy = clan.Armies.FirstOrDefault(a => a.Name == armyName);
            if (oldArmy == null)
            {
                throw new InvalidOperationException($"Army with name '{armyName}' in clan '{nameClan}' was not found.");
            }

            // Update the properties of the army
            oldArmy.Name = army.Name;
            oldArmy.SoldierCount = army.SoldierCount;
            oldArmy.SoldierAttack = army.SoldierAttack;
            oldArmy.SoldierDefense = army.SoldierDefense;
            oldArmy.SoldierHealth = army.SoldierHealth;

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
        public async Task DeleteArmyAsync(string nameClan, string nameArmy)
        {
            // Ensure clan name is not null or empty
            if (string.IsNullOrWhiteSpace(nameClan))
                throw new ArgumentException("The clan name cannot be null or empty.", nameof(nameClan));

            // Ensure army name is not null or empty
            if (string.IsNullOrWhiteSpace(nameArmy))
                throw new ArgumentException("The army name cannot be null or empty.", nameof(nameArmy));

            // Find the clan entity by its name, including its associated armies
            var clan = await _context.Clans
                .Include(c => c.Armies) // Load associated armies for manipulation
                .FirstOrDefaultAsync(c => c.Name == nameClan);

            if (clan == null)
                throw new InvalidOperationException($"Clan with name '{nameClan}' not found.");

            // Find the specific army within the clan by its name
            var army = clan.Armies.FirstOrDefault(a => a.Name == nameArmy);

            if (army == null)
                throw new InvalidOperationException($"Army with name '{nameArmy}' not found in clan '{nameClan}'.");

            // Remove the army from the clan's collection of armies
            clan.Armies.Remove(army);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Clan>> GetAllClansAsync()
        {
            // Retrieve all clans from the database
            return await _context.Clans
                .AsNoTracking() // Use AsNoTracking for read-only queries to improve performance
                .Include(c => c.Armies) // Include associated armies
                .ToListAsync();
        }
    }
}