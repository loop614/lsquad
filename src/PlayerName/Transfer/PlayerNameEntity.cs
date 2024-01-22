namespace Lsquad.Player.Transfer;

public class PlayerNameEntity
{
    public int? id_player_name { get; set; }

    public int? fk_player { get; set; }

    public int? fk_language { get; set; }

    public double? version { get; set; }

    public string name { get; set; } = String.Empty;

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }
}
