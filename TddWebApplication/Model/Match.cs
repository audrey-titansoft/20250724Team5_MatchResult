namespace TddWebApplication.Model;

public class Match
{
    public int MatchId { get; set; }
    public string CurrentResult { get; set; }

    public bool IsSecondHalf()
    {
        return CurrentResult.Contains(';');
    }

    public char[] GetGoals()
    {
        return CurrentResult.Replace(";", "").ToCharArray();
    }

    public bool HasEvent()
    {
        return GetGoals().Length > 0;
    }
    
    public char GetLastGoal()
    {
        return  GetGoals()[^1];;
    }

    public bool IsLastEventIsNextPeriod()
    {
        return GetLastEvent() == ';';
    }

    private char GetLastEvent()
    {
        return CurrentResult[^1];
    }

    public string GetEventWithoutLastEvent()
    {
        return CurrentResult[..^1];
    }

    public string CancelLastEvent()
    {
        return CurrentResult[..^2] + CurrentResult[^1];
    }

    public string GetMatchResultWithoutLastEvent()
    {
        string newResult;

        if (IsLastEventIsNextPeriod())
        {
            newResult = CancelLastEvent();
        }
        else
        {
            newResult = GetEventWithoutLastEvent();
        }

        return newResult;
    }
}