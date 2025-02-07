using Kata.Domain.Entities;

namespace Kata.Tests
{
    public class ClanTests
    {
        [Fact]
        public void TestClan()
        {   
            // Testing Clan and Army objects

            Clan clanAthens = new Clan("Athens");
            Clan clanTroys = new Clan("Troy");

            Assert.Equal(clanTroys.Armies.Count, 0);
            Assert.Equal(clanAthens.Name, "Athens");
            
            clanAthens.AddArmy("armyAthens", 100, 10, 10, 10);
            clanTroys.AddArmy("armyTroys", 100, 10, 10, 10);

            Assert.Equal(clanTroys.Armies.Count, 1);
            Assert.Equal(clanAthens.Armies.Count, 1);
            
            Army armyTroys = clanTroys.Armies[0];
            
            Assert.Equal(armyTroys.SoldierCount, 100);
            Assert.Equal(armyTroys.SoldierAttack, 10);
            Assert.Equal(armyTroys.SoldierHealth, 10);
            Assert.Equal(armyTroys.SoldierDefense, 10);
            Assert.Equal(armyTroys.Name, "armyTroys");
            
            armyTroys.SoldierCount = 0;
            
            Assert.Equal(armyTroys.SoldierCount, 0);
            
            clanTroys.AddArmy("armyTroys2", 200, 20, 20, 20);
            
            Assert.Equal(clanTroys.Armies.Count, 2);
            
            armyTroys = clanTroys.Armies[1];
            
            Assert.Equal(armyTroys.SoldierCount, 200);
            Assert.Equal(armyTroys.SoldierAttack, 20);
            Assert.Equal(armyTroys.SoldierHealth, 20);
            Assert.Equal(armyTroys.SoldierDefense, 20);
            Assert.Equal(armyTroys.Name, "armyTroys2");
            
            bool result = clanTroys.AddArmy("armyTroys", 200, 20, 20, 20);
            
            Assert.False(result);
            
            clanTroys.RemoveArmy("armyTroys");
            
            Assert.Equal(clanTroys.Armies.Count, 1);
            
            armyTroys = clanTroys.Armies[0];
            
            Assert.Equal(armyTroys.SoldierCount, 200);
            Assert.Equal(armyTroys.SoldierAttack, 20);
            Assert.Equal(armyTroys.SoldierHealth, 20);
            Assert.Equal(armyTroys.SoldierDefense, 20);
            Assert.Equal(armyTroys.Name, "armyTroys2");
        }
    }
}