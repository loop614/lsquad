namespace Lsquad.TeamName.Transfer;

public class TeamNameEntity
{
    public int? id_team_name { get; set; }

    public int? fk_team { get; set; }

    public int? fk_language { get; set; }

    public double? version { get; set; }

    public string? name { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }
}
