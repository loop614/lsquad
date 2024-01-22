using System.Text.Json.Serialization;

namespace Lsquad.DataImport.Transfer;

public class BrDomainTeam
{
    [JsonPropertyName("version")]
    public int version { get; set; }

    [JsonPropertyName("sport_id")]
    public int? sport_id { get; set; }

    [JsonPropertyName("names")]
    public BrDomainTeamName? names;

    [JsonPropertyName("id")]
    public int id { get; set; }
}

public class BrDomainTeamName
{
    [JsonPropertyName("stats")]
    public List<BrDomainTeamNameStat> stats = [];

    [JsonPropertyName("livescore")]
    public List<BrDomainTeamNameLivescore> livescore = [];
}

public class BrDomainTeamNameStat
{
    [JsonPropertyName("name")]
    public string? name { get; set; }

    [JsonPropertyName("lang")]
    public string? lang { get; set; }
}

public class BrDomainTeamNameLivescore
{
    [JsonPropertyName("name")]
    public string? name { get; set; }


    [JsonPropertyName("lang")]
    public string? lang { get; set; }
}
