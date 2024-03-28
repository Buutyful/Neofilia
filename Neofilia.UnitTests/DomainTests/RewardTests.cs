using Neofilia.Domain;

namespace Neofilia.UnitTests.DomainTests;

public class RewardTests
{
    [Fact]
    public void ReedemReward_WhenIsAlreadyReedemed_ShouldThrowInvalidOperation()
    {
        //Arrange
        var reward = Reward.NewMoneyReward(5.255m);
        reward.Redeem();
        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => reward.Redeem());
    }

    [Fact]
    public void ReedemReward_WhenIsReedemed_ShouldBeSetRedeemedToTrue()
    {
        //Arrange
        var reward = Reward.NewMoneyReward(5.255m);
        //Act
        reward.Redeem();
        //Assert
        Assert.True(reward.IsRedeemed);
    }

    [Fact]
    public void ReedemReward_WhenIsReedemed_ShouldSetRedeemedAtDateCorrectly()
    {
        //Arrange
        var reward = Reward.NewMoneyReward(5.255m);
        var now = DateTimeOffset.Now;
        //Act
        reward.Redeem();
        //Assert
        Assert.True(Math.Abs((now - reward.RedeemedAt).TotalSeconds) < 5);
    }
}