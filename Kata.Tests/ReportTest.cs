using Kata.Domain.Entities;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace Kata.Tests;

public class ReportTest
{
    private readonly ITestOutputHelper _output;

    public ReportTest(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void TestReport()
    {
        Clan clanAthens = new Clan("Athens");
        Clan clanTroys = new Clan("Troy");
        
        clanAthens.AddArmy("armyAthens", 100, 10, 10, 10);
        clanTroys.AddArmy("armyTroys", 103, 12, 13, 4);
        
        BattleReport b = new BattleReport(clanAthens, clanTroys);
        
        // Add fake turn just for the test.
        b.AddTurn("nameArmyOne", "nameArmyTwo", 1, 2, 3, 4);
        b.AddTurn("t2nameArmyOne", "t2nameArmyTwo", 5, 6, 7, 8);
        b.AddTurn("t3nameArmyOne", "t3nameArmyTwo", 9, 10, 11, 12);
        b.AddTurn("t4nameArmyOne", "t4nameArmyTwo", 13, 14, 15, 16);
        
        b.SetStatus("status");
        b.SetWinner("winner");
        
        
        JObject jReport = b.GetReport();
        
        // Check that the report is not null
        Assert.NotNull(jReport);
        
        // Check if basic information is present in the report
        Assert.True(jReport.ContainsKey("Status"), "The report must contain a 'Status' field.");
        Assert.Equal("status", jReport["Status"]?.ToString());

        Assert.True(jReport.ContainsKey("Winner"), "The report must contain a 'Winner' field.");
        Assert.Equal("winner", jReport["Winner"]?.ToString());
        
        // Check that battle turns are present in the report
        Assert.True(jReport.ContainsKey("History"), "The report must contain 'Turns'.");
        
        var turns = jReport["History"] as JArray;
        Assert.NotNull(turns);
        Assert.Equal(4, turns!.Count); // Ensure that there are 4 turns

        // Validate the details of the first two turns
        Assert.Equal("nameArmyOne", turns[0]["nameArmy1"]?.ToString());
        Assert.Equal("nameArmyTwo", turns[0]["nameArmy2"]?.ToString());
        Assert.Equal("1", turns[0]["dammageArmy1"]?.ToString());
        Assert.Equal("2", turns[0]["dammageArmy2"]?.ToString());
        
        Assert.Equal("t2nameArmyOne", turns[1]["nameArmy1"]?.ToString());
        Assert.Equal("t2nameArmyTwo", turns[1]["nameArmy2"]?.ToString());
        Assert.Equal("5", turns[1]["dammageArmy1"]?.ToString());
        Assert.Equal("6", turns[1]["dammageArmy2"]?.ToString());

        // Validate the details of the last turn
        Assert.Equal("t4nameArmyOne", turns[3]["nameArmy1"]?.ToString());
        Assert.Equal("t4nameArmyTwo", turns[3]["nameArmy2"]?.ToString());
        Assert.Equal("13", turns[3]["dammageArmy1"]?.ToString());
        Assert.Equal("14", turns[3]["dammageArmy2"]?.ToString());
        
        
        // Check that battle turns are present in the report
        Assert.True(jReport.ContainsKey("InitialClans"));
        
        var jClans = jReport["InitialClans"] as JArray;
        Assert.NotNull(jClans);
        Assert.Equal(2, jClans.Count); // Ensure that there are 2 clans
        
        var jClanAthens = jClans[0] as JObject;
        
        Assert.True( jClanAthens.ContainsKey("name"));
        Assert.Equal("Athens", jClanAthens["name"]?.ToString());
        
        Assert.True( jClanAthens.ContainsKey("armies"));
        var jArmyAthen = jClanAthens["armies"] as JArray;
        Assert.NotNull(jArmyAthen);
        Assert.Equal(1, jArmyAthen.Count);
        
        var jOneArmy = jArmyAthen[0] as JObject;
        Assert.True(jOneArmy.ContainsKey("name"));
        Assert.Equal("armyAthens", jOneArmy["name"]?.ToString());
        Assert.Equal(100, jOneArmy["nbUnits"]);
        Assert.Equal(10, jOneArmy["unitAttack"]);
        Assert.Equal(10, jOneArmy["unitDefense"]);
        Assert.Equal(10, jOneArmy["unitHealth"]);
        Assert.Equal(100, jOneArmy["armyAttack"]);
        Assert.Equal(100, jOneArmy["armyDefense"]);
        
        // Write report
        var stringWriter = new StringWriter();
        var jsonWriter = new Newtonsoft.Json.JsonTextWriter(stringWriter)
            { Formatting = Newtonsoft.Json.Formatting.Indented };
        
        jReport.WriteTo(jsonWriter);
        _output.WriteLine("Here is the created battle report:");
        _output.WriteLine(stringWriter.ToString());
    }
}