using Microsoft.AspNetCore.Mvc;

namespace TddWebApplication;

public class MatchController : Controller
{
    private readonly IMatchRepository _matchRepository;

    public MatchController(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    /*
     * UpdateMatchResult(matchId, matchEvent)
     * matchId: unique int
     * matchEvent: 1. HomeGoal, 2. AwayGoal 3. NextPeriod, 4. HomeCancel, 5. AwayCancel
     * call DB through IMatchRepository
     *
     * **example**
     *
     * 1st call:
     * HomeGoal => return "1:0 (First Half)"
     * DB:
     * MatchId | MatchResult
     * --------|------------
     * 90      | H
     *
     * 2nd call:
     * HomeGoal => return "2:0 (First Half)"
     * DB:
     * MatchId | MatchResult
     * --------|------------
     * 90      | HH
     *
     * 3rd call:
     * AwayGoal => return "2:1 (First Half)"
     * DB:
     * MatchId | MatchResult
     * --------|------------
     * 90      | HHA
     *
     * 4th call:
     * NextPeriod => return "2:1 (Second Half)"
     * DB:
     * MatchId | MatchResult
     * --------|------------
     * 90      | HHA;
     *
     * 5th call (1) if:
     * AwayCancel => return "2:0 (Second Half)"
     * DB:
     * MatchId | MatchResult
     * --------|------------
     * 90      | HH;
     *
     * 5th call (2) if:
     * HomeCancel => throw exception
     * DB:
     * MatchId | MatchResult
     * --------|------------
     * 90      | HHA;
     */
     
    // let's implement the logic one by one, step by step, with the respective test

    [HttpPost]
    public string UpdateMatchResult(int matchId, MatchEvent matchEvent)
    {
        var currentResult = _matchRepository.GetMatchResult(matchId);
        var isSecondHalf = currentResult.Contains(";");
        string newResult;
        
        switch (matchEvent)
        {
            case MatchEvent.HomeGoal:
                newResult = currentResult + "H";
                break;
            case MatchEvent.AwayGoal:
                newResult = currentResult + "A";
                break;
            case MatchEvent.NextPeriod:
                if (isSecondHalf)
                    throw new InvalidOperationException("比赛已经在下半场");
                newResult = currentResult + ";";
                break;
            default:
                return string.Empty;
        }

        _matchRepository.UpdateMatchResult(matchId, newResult);
        
        var homeGoals = newResult.Count(c => c == 'H');
        var awayGoals = newResult.Count(c => c == 'A');
        var period = isSecondHalf || matchEvent == MatchEvent.NextPeriod ? "下半场" : "上半场";
        
        return $"{homeGoals}:{awayGoals} ({period})";
    }
}