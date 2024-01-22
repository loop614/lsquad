using System.Text.Json.Serialization;

namespace Lsquad.DataImport.Transfer;

public class BrDomainPlayer
{
    [JsonPropertyName("version")]
    public double version { get; set; }

    [JsonPropertyName("names")]
    public BrDomainPlayerNames? names;

    [JsonPropertyName("id")]
    public int id { get; set; }
}

public class BrDomainPlayerNames
{
    [JsonPropertyName("stats")]
    public List<BrDomainPlayerNameStat> stats = [];

    [JsonPropertyName("livescore")]
    public List<BrDomainPlayerNameLiveScore> livescore = [];

}

public class BrDomainPlayerNameStat
{
    [JsonPropertyName("name")]
    public string? name { get; set; }

    [JsonPropertyName("lang")]
    public string? lang { get; set; }
}

public class BrDomainPlayerNameLiveScore
{
    [JsonPropertyName("name")]
    public string? name { get; set; }

    [JsonPropertyName("lang")]
    public string? lang { get; set; }
}
