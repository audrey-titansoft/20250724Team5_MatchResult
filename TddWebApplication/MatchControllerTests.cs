using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace TddWebApplication.Tests;

[TestFixture]
public class MatchControllerTests
{
    private MatchController _controller;
    private IMatchRepository _matchRepository;

    [SetUp]
    public void Setup()
    {
        _matchRepository = Substitute.For<IMatchRepository>();
        _controller = new MatchController(_matchRepository);
    }

    [Test]
    public void UpdateMatchResult_WhenHomeGoal_ShouldReturn1To0FirstHalf()
    {
        // Arrange
        var matchId = 90;
        _matchRepository.GetMatchResult(matchId).Returns("");

        // Act
        var result = _controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("1:0 (上半场)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "H");
    }
[Test]
public void UpdateMatchResult_WhenSecondHomeGoal_ShouldReturn2To0FirstHalf()
{
    // Arrange
    var matchId = 90;
    _matchRepository.GetMatchResult(matchId).Returns("H");

    // Act
    var result = _controller.UpdateMatchResult(matchId, MatchEvent.HomeGoal);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result, Is.EqualTo("2:0 (上半场)"));
    _matchRepository.Received(1).UpdateMatchResult(matchId, "HH");
}
[Test]
public void UpdateMatchResult_WhenAwayGoalAfterTwoHomeGoals_ShouldReturn2To1FirstHalf()
{
    // Arrange
    var matchId = 90;
    _matchRepository.GetMatchResult(matchId).Returns("HH");

    // Act
    var result = _controller.UpdateMatchResult(matchId, MatchEvent.AwayGoal); 

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result, Is.EqualTo("2:1 (上半场)"));
    _matchRepository.Received(1).UpdateMatchResult(matchId, "HHA");
}
[Test]
public void UpdateMatchResult_WhenNextPeriod_ShouldShowSecondHalf()
{
    // Arrange
    var matchId = 90;
    _matchRepository.GetMatchResult(matchId).Returns("HHA");

    // Act
    var result = _controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod);

    // Assert
    Assert.That(result, Is.EqualTo("2:1 (下半场)"));
    _matchRepository.Received(1).UpdateMatchResult(matchId, "HHA;");
}

[Test]
public void UpdateMatchResult_WhenNextPeriodInSecondHalf_ShouldThrowException()
{
    // Arrange
    var matchId = 90;
    _matchRepository.GetMatchResult(matchId).Returns("HHA;");

    // Act & Assert
    var ex = Assert.Throws<InvalidOperationException>(() => 
        _controller.UpdateMatchResult(matchId, MatchEvent.NextPeriod));
    Assert.That(ex.Message, Is.EqualTo("比赛已经在下半场"));
}
}