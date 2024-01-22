namespace Lsquad.Player.Transfer;

public class PlayerEntity
{
    public int? id_player { get; set; }

    public int? fk_team { get; set; }

    public double? version { get; set; }

    public string? weight { get; set; }

    public string? shirt_number { get; set; }

    public int? preferred_foot { get; set; }

    public int? position { get; set; }

    public int external_player_id { get; set; }

    public string? last_name { get; set; }

    public string? height { get; set; }

    public string? full_name { get; set; }

    public string? first_name { get; set; }

    public string? country_code { get; set; }

    public DateTime? birth_date { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public List<PlayerNameEntity> playerNameEntities = [];
}
