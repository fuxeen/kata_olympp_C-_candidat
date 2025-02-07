using Kata.Domain.Entities;
using Kata.Infrastructure.Data;
using Kata.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kata.Tests;

public class ClanRepositoryTest
{
    private DbContextOptions<KataDBContext> GetInMemoryOptions()
    {
        // Use in-memory database for testing
        return new DbContextOptionsBuilder<KataDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    
    [Fact]
    public void TestClanRepository()
    {
        // Arrange: Set up the DbContext and repository
        var options = GetInMemoryOptions();

        // Create the database context
        var dbContext = new KataDBContext(options);
        
        var repository = new ClanRepository(dbContext);

        // Act: Add clans and retrieve them

        var clans = repository.GetAllClansAsync().Result;
        
        // Assert: Verify the results
        Assert.NotNull(clans);
        Assert.Equal(2, clans.Count());
        Assert.Contains(clans, c => c.Name == "Athens");
        Assert.Contains(clans, c => c.Name == "Troy");

        var clan = repository.GetClanByNameAsync("Athens").Result;
        Assert.NotNull(clan);
        Assert.Equal("Athens", clan.Name);
        Assert.Equal(0, clan.Armies.Count);
        
        clan = repository.GetClanByNameAsync("Troy").Result;
        Assert.NotNull(clan);
        Assert.Equal("Troy", clan.Name);
        Assert.Equal(0, clan.Armies.Count);
        
        clan = repository.GetClanByNameAsync("Unknown").Result;
        Assert.Null(clan);
        
        repository.AddArmyAsync("Athens", new Army("armyAthens", 100, 1, 2, 3));

        clan = repository.GetClanByNameAsync("Athens").Result;
        Army army = clan.Armies.First();
        Assert.NotNull(army);
        Assert.Equal("armyAthens", army.Name);
        Assert.Equal(100, army.SoldierCount);
        Assert.Equal(1, army.SoldierHealth);
        Assert.Equal(2, army.SoldierAttack);
        Assert.Equal(3, army.SoldierDefense);

        repository.UpdateArmyAsync("Athens", "armyAthens", new Army("armyAthensUpdated", 75, 3, 4, 5));
        
        clan = repository.GetClanByNameAsync("Athens").Result;
        army = clan.Armies.First();
        Assert.NotNull(army);
        Assert.Equal("armyAthensUpdated", army.Name);
        Assert.Equal(75, army.SoldierCount);
        Assert.Equal(3, army.SoldierHealth);
        Assert.Equal(4, army.SoldierAttack);
        Assert.Equal(5, army.SoldierDefense);

        army = repository.GetArmyByNameAsync("armyAthensUpdated").Result;
        Assert.NotNull(army);
        Assert.Equal("armyAthensUpdated", army.Name);
        
        repository.DeleteArmyAsync("Athens", "armyAthensUpdated");
        
        army = repository.GetArmyByNameAsync("armyAthensUpdated").Result;
        Assert.Null(army);
        
        clan = repository.GetClanByNameAsync("Athens").Result;
        Assert.Equal(0, clan.Armies.Count);
        
        // Clean up
        dbContext.Database.EnsureDeleted();
    }
}