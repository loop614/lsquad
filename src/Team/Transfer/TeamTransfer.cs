using Lsquad.TeamName.Transfer;

namespace Lsquad.Team.Transfer;

public class TeamTransfer
{
    public int? id_team { get; set; }

    public int external_team_id { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public List<TeamNameTransfer> teamNameEntities = [];

    public void AddTeamNameTransfer(TeamNameTransfer newTeamNameTransfer)
    {
        if (teamNameEntities is null) {
            teamNameEntities = [];
        }
        teamNameEntities.Add(newTeamNameTransfer);
    }
}
