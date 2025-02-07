using Kata.Domain.Entities;
using Kata.Domain.Interfaces;
using Kata.Domain.Repositories;

namespace Kata.Application.Services
{
    public class BattleService : IBattleService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly IClanRepository _clanRepository;

        public BattleService(IBattleRepository battleRepository, IClanRepository clanRepository)
        {
            _battleRepository = battleRepository;
            _clanRepository = clanRepository;
        }

        /// <summary>
        /// Generates a battle between the armies of  the first and the last Clan
        /// Returns a battle report after saving it for historisation 
        /// </summary>
        /// <returns>The battle report.</returns>
        public async Task<BattleReport> Battle()
        {
            // Retrieve all clans
            IEnumerable<Clan> clans = await _clanRepository.GetAllClansAsync();

            // Validate that exactly 2 clans are available for the battle
            if (clans.Count() != 2)
                throw new InvalidOperationException("Exactly two clans are required to initiate a battle.");

            Clan clanOne = clans.First();
            Clan clanTwo = clans.Last();

            // Ensure that both clans have at least one army
            if (!clanOne.Armies.Any() || !clanTwo.Armies.Any())
                throw new InvalidOperationException("At least one army per clan is required to initiate a battle.");

            // Initialize the battle report
            BattleReport report = new BattleReport(clanOne, clanTwo);

            // Indices for the armies
            int armyOneIndex = 0;
            int armyTwoIndex = 0;

            while (armyOneIndex < clanOne.Armies.Count && armyTwoIndex < clanTwo.Armies.Count)
            {
                // Get the current armies
                var armyOne = clanOne.Armies[armyOneIndex];
                var armyTwo = clanTwo.Armies[armyTwoIndex];

                // Calculate damage dealt by each army
                int damageArmyOne = armyOne.SoldierCount * armyOne.SoldierAttack -
                                    armyTwo.SoldierCount * armyTwo.SoldierDefense;
                int damageArmyTwo = armyTwo.SoldierCount * armyTwo.SoldierAttack -
                                    armyOne.SoldierCount * armyOne.SoldierDefense;

                // Ensure that damage is not negative
                damageArmyOne = Math.Max(0, damageArmyOne);
                damageArmyTwo = Math.Max(0, damageArmyTwo);

                // Apply the computed damage to each army
                armyOne.SoldierCount -= damageArmyTwo / armyOne.SoldierHealth;
                armyTwo.SoldierCount -= damageArmyOne / armyTwo.SoldierHealth;

                // Update the number of soldiers remaining
                armyOne.SoldierCount = armyOne.SoldierCount <= 0 ? 0 : armyOne.SoldierCount;
                armyTwo.SoldierCount = armyTwo.SoldierCount <= 0 ? 0 : armyTwo.SoldierCount;

                if (armyOne.SoldierCount <= 0) armyOneIndex++;
                if (armyTwo.SoldierCount <= 0) armyTwoIndex++;
                
                // Update the armies in the clan repositories
                await _clanRepository.UpdateArmyAsync(clanOne.Name, armyOne.Name, armyOne);
                await _clanRepository.UpdateArmyAsync(clanTwo.Name, armyTwo.Name, armyTwo);

                // Add this turn to the battle report
                report.AddTurn(
                    armyOne.Name, armyTwo.Name,
                    damageArmyOne, damageArmyTwo,
                    armyOne.SoldierCount, armyTwo.SoldierCount
                );

                // If neither side can deal damage, declare a draw
                if (damageArmyOne == 0 && damageArmyTwo == 0)
                {
                    report.SetStatus("DRAW");
                    report.SetWinner("null"); // No winner
                    await _battleRepository.SaveBattleReport(report);
                    return report;
                }
            }

            // Determine the winner based on which clan still has armies remaining
            if (armyOneIndex < clanOne.Armies.Count)
            {
                report.SetStatus("WIN");
                report.SetWinner(clanOne.Name);
            }
            else if (armyTwoIndex < clanTwo.Armies.Count)
            {
                report.SetStatus("WIN");
                report.SetWinner(clanTwo.Name);
            }

            // Save the final battle report
            await _battleRepository.SaveBattleReport(report);
            
            return report;
        }
    }
}