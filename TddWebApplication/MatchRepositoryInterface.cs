public enum MatchEvent
{
    HomeGoal = 1,
    AwayGoal = 2,
    NextPeriod = 3,
    HomeCancel = 4,
    AwayCancel = 5
}

public interface IMatchRepository
{
    string GetMatchResult(int matchId);
    void UpdateMatchResult(int matchId, string result);
}

public class MatchRepository : IMatchRepository
{
    public string GetMatchResult(int matchId)
    {
        throw new NotImplementedException();
    }

    public void UpdateMatchResult(int matchId, string result)
    {
        throw new NotImplementedException();
    }
}
