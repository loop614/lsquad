using Lsquad.TeamName.Transfer;

namespace Lsquad.Team.Transfer;

public class TeamEntity
{
    public int? id_team { get; set; }

    public int external_team_id { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public List<TeamNameEntity> teamNameEntities = [];

    public void AddTeamNameEntity(TeamNameEntity newTeamNameEntity)
    {
        if (teamNameEntities is null) {
            teamNameEntities = [];
        }
        teamNameEntities.Add(newTeamNameEntity);
    }
}
