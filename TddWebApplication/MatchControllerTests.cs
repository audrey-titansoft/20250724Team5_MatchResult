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
        var result = _controller.UpdateMatchResult(matchId, (int)MatchEvent.HomeGoal) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo("1:0 (上半场)"));
        _matchRepository.Received(1).UpdateMatchResult(matchId, "H");
    }
}
