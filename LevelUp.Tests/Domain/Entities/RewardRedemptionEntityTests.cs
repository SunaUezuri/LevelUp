using LevelUp.Domain.Entities;

namespace LevelUp.Tests.Domain.Entities
{
    public class RewardRedemptionEntityTests
    {
        private RewardRedemptionEntity CreateValidRedemption()
        {
            return new RewardRedemptionEntity
            {
                Id = 1,
                UserId = 1,
                RewardId = 1,
                PointsSpent = 500,
                RedeemedAt = DateTime.UtcNow
            };
        }

        [Fact(DisplayName = "RewardRedemptionEntity com dados válidos deve passar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardRedemptionEntityComDadosValidosDevePassarNaValidacao()
        {
            // Arrange
            var redemption = CreateValidRedemption();

            // Act
            var validationResults = ValidationHelper.Validate(redemption);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact(DisplayName = "RewardRedemptionEntity com UserId zero deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardRedemptionEntityComUserIdZeroDeveFalharNaValidacao()
        {
            // Arrange
            var redemption = CreateValidRedemption();
            redemption.UserId = 0;

            // Act
            var validationResults = ValidationHelper.Validate(redemption);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
                v.MemberNames.Contains("UserId") &&
                (v.ErrorMessage.Contains("range") || v.ErrorMessage.Contains("value"))
            );
        }

        [Fact(DisplayName = "RewardRedemptionEntity com RewardId zero deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardRedemptionEntityComRewardIdZeroDeveFalharNaValidacao()
        {
            // Arrange
            var redemption = CreateValidRedemption();
            redemption.RewardId = 0;

            // Act
            var validationResults = ValidationHelper.Validate(redemption);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
                v.MemberNames.Contains("RewardId") &&
                (v.ErrorMessage.Contains("range") || v.ErrorMessage.Contains("value"))
            );
        }

        [Fact(DisplayName = "RewardRedemptionEntity com PointsSpent zero deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardRedemptionEntityComPointsSpentZeroDeveFalharNaValidacao()
        {
            // Arrange
            var redemption = CreateValidRedemption();
            redemption.PointsSpent = 0;

            // Act
            var validationResults = ValidationHelper.Validate(redemption);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
                v.MemberNames.Contains("PointsSpent") &&
                (v.ErrorMessage.Contains("range") || v.ErrorMessage.Contains("value"))
            );
        }
    }
}
