using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Entities
{
    public class Clan
    {
        public string Name{get; set;}

        public List<Army> Armies{get; set;}

        public Clan(string name)
        {
            this.Name = name;
            this.Armies = new List<Army>();
        }

        public bool AddArmy(string name, int soldierCount, int soldierHealth, int soldierAttack, int soldierDefense){            
            
            bool creationOk = true;

            foreach(Army army in this.Armies){
                if(army.Name == name){
                    creationOk = false;
                }
            }
            
            if(creationOk){
                this.Armies.Add(new Army(name, soldierCount, soldierHealth, soldierAttack, soldierDefense));
            }

            return creationOk;
        }

        public bool RemoveArmy(string name){
            bool found = false;
            
            foreach(Army army in this.Armies){
                if(army.Name == name){
                    this.Armies.Remove(army);
                    found = true;
                    break;
                }
            }

            return found;
        }
    }
}
