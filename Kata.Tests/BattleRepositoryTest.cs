using Kata.Domain.Entities;
using Kata.Infrastructure.Data;
using Kata.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kata.Tests;

public class BattleRepositoryTest
{
    private DbContextOptions<KataDBContext> GetInMemoryOptions()
    {
        // Use in-memory database for testing
        return new DbContextOptionsBuilder<KataDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }


    [Fact]
    public void TestBattleRepository()
    {
        // Arrange: Set up the DbContext and repository
        var options = GetInMemoryOptions();

        // Create the database context
        var dbContext = new KataDBContext(options);

        var repository = new BattleRepository(dbContext);

        var reports = repository.GetAllBattlesReport().Result;
        Assert.NotNull(reports);
        Assert.Equal(0, reports.Count());
        
        BattleReport report = new BattleReport();

        repository.SaveBattleReport(report);
        
        reports = repository.GetAllBattlesReport().Result;
        Assert.NotNull(reports);
        Assert.Equal(1, reports.Count());
        
        repository.SaveBattleReport(new BattleReport());
        repository.SaveBattleReport(new BattleReport());
        repository.SaveBattleReport(new BattleReport());
        
        reports = repository.GetAllBattlesReport().Result;
        Assert.NotNull(reports);
        Assert.Equal(4, reports.Count());
        repository.SaveBattleReport(report);

        BattleReport report2 = repository.GetBattleReportById(report.ID).Result;
        Assert.NotNull(report2);
        Assert.Equal(report.ID, report2.ID);
        Assert.Equal(report, report2);
        
    }
}