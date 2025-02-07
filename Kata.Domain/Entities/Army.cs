using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Domain.Entities
{
    public class Army
    {
        public string ID { get;} // Auto-generated from the database
        public string Name{get; set;}

        public int SoldierCount{get;set;}
        public int SoldierHealth{get;set;}
        public int SoldierAttack{get;set;}
        public int SoldierDefense{get;set;}

        public Army(string name, int soldierCount, int soldierHealth, int soldierAttack, int soldierDefense)
        {
            Name = name;
            SoldierCount = soldierCount;
            SoldierHealth = soldierHealth;
            SoldierAttack = soldierAttack;
            SoldierDefense = soldierDefense;
            
            this.ID = Guid.NewGuid().ToString();
        }
    }
}
