using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace Kata.Domain.Entities
{
    public class BattleReport
    {
        private readonly Clan _clanOne;
        private readonly Clan _clanTwo;

        public string ID { get;} // Auto-generated from the database


        private string _winner;
        private string _status;
        
        private readonly JArray _initialClans;
        
        private readonly JArray _turns;

        public BattleReport() : this(null, null)
        {
            // Empty constructor for DB init.
        }
        
        public BattleReport(Clan clanOne, Clan clanTwo){
            this._clanOne = clanOne;
            this._clanTwo = clanTwo;

            this._winner = "NULL";
            this._status = "DRAW";
            
            this.ID = Guid.NewGuid().ToString();

            this._turns = new JArray();

            this._initialClans = new JArray
            {
                GetArmyAsJson(_clanOne),
                GetArmyAsJson(_clanTwo)
            };
        }

        // Add a turn to the battle report
        public void AddTurn(string nameArmyOne, string nameArmyTwo, int damageArmyOne, int damageArmyTwo, int nbRemainingArmyOne, int nbRemainingArmyTwo)
        {
            JObject jTurn = new JObject()
            {
                ["nameArmy1"] = nameArmyOne,
                ["nameArmy2"] = nameArmyTwo,
                ["dammageArmy1"] = damageArmyOne,
                ["dammageArmy2"] = damageArmyTwo,
                ["nbRemainingSoldiersArmy1"] = nbRemainingArmyOne,
                ["nbRemainingSoldiersArmy2"] = nbRemainingArmyTwo
            };
            
            _turns.Add(jTurn);
        }

        public void SetWinner(string winner){
            this._winner = winner;
        }

        public void SetStatus(string status){
            this._status = status;
        }
        
        // Generate the battle report as a JSON.
        public JObject GetReport()
        {
            JObject report = new JObject
            {
                ["Winner"] = _winner,
                ["Status"] = _status,
                ["InitialClans"] = _initialClans,
                ["History"] = _turns
            };

            return report;
        }


        // Obtenir toutes les armées d'un clan sous forme de JSON.
        private JObject GetArmyAsJson(Clan clan)
        {
            JObject jClan = new JObject
            {
                ["name"] = clan.Name
            };

            JArray jArmyArray = new JArray();

            foreach (Army army in clan.Armies)
            {
                JObject jArmy = new JObject
                {
                    ["name"] = army.Name,
                    ["nbUnits"] = army.SoldierCount,
                    ["unitAttack"] = army.SoldierAttack,
                    ["unitDefense"] = army.SoldierDefense,
                    ["unitHealth"] = army.SoldierHealth,
                    ["armyAttack"] = army.SoldierAttack * army.SoldierCount,
                    ["armyDefense"] = army.SoldierDefense * army.SoldierCount
                };

                jArmyArray.Add(jArmy);
            }

            jClan["armies"] = jArmyArray;

            return jClan;
        }

    }
}
