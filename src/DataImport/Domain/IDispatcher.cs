using Lsquad.DataImport.Transfer;

namespace Lsquad.DataImport.Domain;

public interface IDispatcher
{
    public void Dispatch(BrDomainPlayer brDomainPlayer);

    public void Dispatch(BrDomainSquad brDomainSquad);

    public void Dispatch(BrDomainTeam brDomainTeam);

    public void FlushCache(string topic);
}
