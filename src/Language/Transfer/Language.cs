using System.Text.Json.Serialization;

namespace Lsquad.Language.Transfer;

public class Language
{
    [JsonPropertyName("idLanguage")]
    public int id_language { get; set; }

    [JsonPropertyName("name")]
    public string? name { get; set; }
}
