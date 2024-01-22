using System.Text.Json.Serialization;

namespace Lsquad.DataImport.Transfer;

public class BrDomainSquad
{
    [JsonPropertyName("version")]
    public double version { get; set; } = 0.0;

    [JsonPropertyName("teamId")]
    public int team_id { get; set; }

    [JsonPropertyName("players")]
    public List<BrDomainSquadPlayer> players = [];
}

public class BrDomainSquadPlayer
{
    [JsonPropertyName("weight")]
    public string? weight { get; set; }

    [JsonPropertyName("shirtNumber")]
    public string? shirt_number { get; set; }

    [JsonPropertyName("preferredFoot")]
    public int? preferred_foot { get; set; }

    [JsonPropertyName("externalPlayerId")]
    public int? player_id { get; set; }

    [JsonPropertyName("position")]
    public int? position { get; set; }

    [JsonPropertyName("lastName")]
    public string? last_name { get; set; }

    [JsonPropertyName("height")]
    public string? height { get; set; }

    [JsonPropertyName("fullName")]
    public string? full_name { get; set; }

    [JsonPropertyName("firstName")]
    public string? first_name { get; set; }

    [JsonPropertyName("countryCode")]
    public string? country_code { get; set; }

    [JsonPropertyName("birthDate")]
    public string? birth_date { get; set; }
}
