using Kata.Application.Services;
using Kata.Domain.Entities;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace Kata.Tests;
using Kata.Infrastructure.Data;
using Kata.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class BattleServiceTest
{
    private readonly ITestOutputHelper _output;

    public BattleServiceTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    private DbContextOptions<KataDBContext> GetInMemoryOptions()
    {
        // Use in-memory database for testing
        return new DbContextOptionsBuilder<KataDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }


    [Fact]
    public void TestBattleService()
    {
        // Arrange: Set up the DbContext and repository
        var options = GetInMemoryOptions();

        // Create the database context
        var dbContext = new KataDBContext(options);

        var battleRepository = new BattleRepository(dbContext);
        var clanRepository = new ClanRepository(dbContext);

        BattleService service = new BattleService( battleRepository, clanRepository);
        
        clanRepository.AddArmyAsync("Athens", new Army("armyAthens1", 1000, 10, 10, 10));
        clanRepository.AddArmyAsync("Troy", new Army("armyTroy1", 1000, 20, 10, 0));
        clanRepository.AddArmyAsync("Troy", new Army("armyTroy2", 1000, 10, 100, 100));
        
        // Act: Generate a battle
        BattleReport report = service.Battle().Result;
        
        BattleReport reportFromDb = battleRepository.GetAllBattlesReport().Result.First();

        Assert.NotNull(reportFromDb);
        Assert.Equal(report, reportFromDb);
        
        JObject jReport = reportFromDb.GetReport();
        
        // Write report
        var stringWriter = new StringWriter();
        var jsonWriter = new Newtonsoft.Json.JsonTextWriter(stringWriter)
            { Formatting = Newtonsoft.Json.Formatting.Indented };
        
        jReport.WriteTo(jsonWriter);
        _output.WriteLine("Here is the created battle report:");
        _output.WriteLine(stringWriter.ToString());
    }
}