using Kata.Domain.Entities;
using Kata.Domain.Interfaces;
using Kata.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Application.Services
{
    public class ClanService : IClanService
    {
        private readonly IClanRepository _clanRepository;

        public ClanService (IClanRepository clanRepository)
        {
            _clanRepository=clanRepository;
        }

        /// <summary>
        /// List all Clan and their armies
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Clan>> GetAllClansAsync()
        {
            return await this._clanRepository.GetAllClansAsync();
        }

        /// <summary>
        /// Get the details of a clan
        /// </summary>
        /// <param name="name">Name of the Clan</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Clan?> GetClanByNameAsync(string name)
        {
            // Validate input: Check if the clan name is null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Clan name cannot be null or empty.", nameof(name));
            }
            
            return await this._clanRepository.GetClanByNameAsync(name);
        }

        public async Task<Army> GetArmyByNameAsync(string armyName)
        {
            return await this._clanRepository.GetArmyByNameAsync(armyName);
        }

        /// <summary>
        /// Add an army to an existing Clan 
        /// </summary>
        /// <param name="nameClan">Name of the clan</param>
        /// <param name="army">Army to add into the clan</param>
        /// <returns></returns>
        public async Task<int> AddArmyAsync(string nameClan, Army army)
        {
            // Validate input: Check if nameClan is null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(nameClan))
            {
                throw new ArgumentException("Clan name cannot be null or empty.", nameof(nameClan));
            }

            // Validate input: Check if the army object is null
            if (army is null)
            {
                throw new ArgumentNullException(nameof(army), "Army object cannot be null.");
            }

            // Main call to add the army to the clan repository
            await this._clanRepository.AddArmyAsync(nameClan, army);

            return 201;
        }
       
        /// <summary>
        /// Remove an army from a clan
        /// </summary>
        /// <param name="nameClan">name of the clan</param>
        /// <param name="armyClan">name of the army</param>
        /// <returns></returns>
        public async Task<int> RemoveArmyAsync(string nameClan, string armyClan)
        {
            // Argument verification
            if (string.IsNullOrWhiteSpace(nameClan))
            {
                throw new ArgumentException("Clan name cannot be null or empty.\n", nameof(nameClan));
            }

            if (string.IsNullOrWhiteSpace(armyClan))
            {
                throw new ArgumentException("Army name cannot be null or empty.\n", nameof(armyClan));
            }
            
            // Main execution
            
            await this._clanRepository.DeleteArmyAsync(nameClan, armyClan);

            return 200;
        }
        
        /// <summary>
        /// Update an army from a clan
        /// </summary>
        /// <param name="nameClan">name of the clan</param>
        /// <param name="armyName">name of the army</param>
        /// <param name="army">updated army</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> UpdateArmyAsync(string nameClan, string armyName, Army army)
        {
            // Entry verification
            if (string.IsNullOrWhiteSpace(nameClan))
            {
                throw new ArgumentException("Clan name cannot be null or empty.\n", nameof(nameClan));
            }

            if (string.IsNullOrWhiteSpace(armyName))
            {
                throw new ArgumentException("Army name cannot be null or empty.\n", nameof(armyName));
            }
            
            if (army == null)
            {
                throw new ArgumentNullException(nameof(army), "'Army' object cannot be null.");
            }

            // Main execution
            await this._clanRepository.UpdateArmyAsync(nameClan, armyName, army);

            return 200;
        }
    }
}
