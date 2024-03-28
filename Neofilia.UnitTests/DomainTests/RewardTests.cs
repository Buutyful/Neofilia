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
}