using NSubstitute;
using NUnit.Framework;
using TddWebApplication.Enum;
using TddWebApplication.Model;
using TddWebApplication.Repo;

namespace TddWebApplication.Tests;

[TestFixture]
public class MatchControllerTests
{
    [SetUp]
    public void Setup()
    {
        _matchRepository = Substitute.For<IMatchRepository>();
        _controller = new MatchController(_matchRepository);
    }

    private MatchController _controller;
    private IMatchRepository _matchRepository;

    [Test]
    public void UpdateMatchResult_WhenHomeGoal_ShouldReturn1To0FirstHalf()
    {
        // Arrange
        var matchId = 90;
        _matchRepository.GetMatchResult(matchId).Returns(new Match { MatchId = matchId, CurrentResult = "" });

        // Act
        var result = _controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

        // Assert
        Assert.That(result, Is.EqualTo("1:0 (First Half)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "H");
    }

    [Test]
    public void UpdateMatchResult_WhenSecondHomeGoal_ShouldReturn2To0FirstHalf()
    {
        // Arrange
        var matchId = 90;
        _matchRepository.GetMatchResult(matchId).Returns(new Match { MatchId = matchId, CurrentResult = "H" });

        // Act
        var result = _controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

        // Assert
        Assert.That(result, Is.EqualTo("2:0 (First Half)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "HH");
    }

    [Test]
    public void UpdateMatchResult_WhenAwayGoalAfterTwoHomeGoals_ShouldReturn2To1FirstHalf()
    {
        // Arrange
        var matchId = 90;
        _matchRepository.GetMatchResult(matchId).Returns(new Match { MatchId = matchId, CurrentResult = "HH" });

        // Act
        var result = _controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal);

        // Assert
        Assert.That(result, Is.EqualTo("2:1 (First Half)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "HHA");
    }

    [Test]
    public void UpdateMatchResult_WhenNextPeriod_ShouldShowSecondHalf()
    {
        // Arrange
        var matchId = 90;
        _matchRepository.GetMatchResult(matchId).Returns(new Match { MatchId = matchId, CurrentResult = "HHA" });

        // Act
        var result = _controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);

        // Assert
        Assert.That(result, Is.EqualTo("2:1 (Second Half)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "HHA;");
    }

    [Test]
    public void UpdateMatchResult_WhenAwayCancelLastGoal_ShouldReturn2To0SecondHalf()
    {
        // Arrange
        var matchId = 90;
        _matchRepository.GetMatchResult(matchId).Returns(new Match { MatchId = matchId, CurrentResult = "HHA;" });

        // Act
        var result = _controller.UpdateMatchResult(matchId, MatchEvent.AwayCancel);

        // Assert
        Assert.That(result, Is.EqualTo("2:0 (Second Half)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "HH;");
    }

    [Test]
    public void UpdateMatchResult_WhenHomeCancelLastGoal_ShouldThrowException()
    {
        // Arrange
        var matchId = 90;
        var match = new Match { MatchId = matchId, CurrentResult = "HHA;" };
        _matchRepository.GetMatchResult(matchId).Returns(match);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            _controller.UpdateMatchResult(matchId, MatchEvent.HomeCancel));
        Assert.That(ex.Message, Is.EqualTo("Last goal is not same team's goal"));
    }

    [Test]
    public void UpdateMatchResult_WhenHomeCancel_InSecondHalf_ShouldRemoveLastHomeGoal()
    {
        // Arrange
        var matchId = 1;
        var initialResult = "HHA;H";
        var match = new Match { MatchId = matchId, CurrentResult = initialResult };
        var matchRepository = Substitute.For<IMatchRepository>();
        matchRepository.GetMatchResult(matchId).Returns(match);
        var controller = new MatchController(matchRepository);

        // Act
        var result = controller.UpdateMatchResult(matchId, MatchEvent.HomeCancel);

        // Assert
        Assert.That(result, Is.EqualTo("2:1 (Second Half)"));
        matchRepository.Received(1).UpdateMatchResult(matchId, "HHA;");
    }
}