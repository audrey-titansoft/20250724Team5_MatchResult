using TddWebApplication.Model;

namespace TddWebApplication.Repo;

public class MatchRepository : IMatchRepository
{
    public Match GetMatchResult(int matchId)
    {
        return new Match();
    }

    public void UpdateMatchResult(int matchId, string result)
    {
        throw new NotImplementedException();
    }
}