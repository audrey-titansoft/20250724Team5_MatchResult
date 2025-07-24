using TddWebApplication.Model;

namespace TddWebApplication.Repo;

public interface IMatchRepository
{
    Match GetMatchResult(int matchId);
    void UpdateMatchResult(int matchId, string result);
}