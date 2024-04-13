using Microsoft.AspNetCore.SignalR;
using Moq;
using Neofilia.Server.Services.Quiz;

namespace Neofilia.UnitTests.ChallengeTests;

public class QuizGameTests
{
    [Fact]
    public async Task Run_WhenGameIsRunning_ShouldNotThrowExceptions()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(70));
        var cancellationToken = cancellationTokenSource.Token;

        var mockHubContext = new Mock<IHubContext<QuizHub>>();
        var game = new Game(mockHubContext.Object);
        
        // Act & Assert
        try
        {
            await game.Run(cancellationToken);
        }
        catch (Exception ex)
        {
            if(cancellationToken.IsCancellationRequested)
            Assert.Fail($"Unexpected exception: {ex}");
        }        
    }

}

