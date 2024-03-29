using System.Text.Json.Serialization;

namespace Lsquad.Squad.Transfer;

public class SquadResponse
{
    [JsonPropertyName("team")]
    public SquadResponseTeam team { get; set; } = new();

    [JsonPropertyName("players")]
    public List<SquadResponsePlayer> players { get; set; } = [];

    [JsonPropertyName("errors")]
    public List<string> errors { get; set; } = [];
}

public class SquadResponseTeam
{
    [JsonPropertyName("id")]
    public int id;

    [JsonPropertyName("name")]
    public string name = string.Empty;
}

public class SquadResponsePlayer
{
    [JsonPropertyName("name")]
    public string? fullName { get; set; }

    [JsonPropertyName("player_id")]
    public int? player_id { get; set; }

    [JsonPropertyName("weight")]
    public string? weight { get; set; }

    [JsonPropertyName("shirt_number")]
    public string? shirt_number { get; set; }

    [JsonPropertyName("preferred_foot")]
    public int? preferred_foot { get; set; }

    [JsonPropertyName("position")]
    public int? position { get; set; }

    [JsonPropertyName("last_name")]
    public string? last_name { get; set; }

    [JsonPropertyName("height")]
    public string? height { get; set; }

    [JsonPropertyName("first_name")]
    public string? first_name { get; set; }

    [JsonPropertyName("country_code")]
    public string? country_code { get; set; }

    [JsonPropertyName("birth_date")]
    public DateTime? birth_date { get; set; }
}
