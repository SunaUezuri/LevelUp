using LevelUp.Domain.Entities;

namespace LevelUp.Tests.Domain.Entities
{
    public class RewardEntityTests
    {
        private RewardEntity CreateValidReward()
        {
            return new RewardEntity
            {
                Id = 1,
                Name = "Gift Card R$50",
                Description = "Gift card para usar na loja X.",
                PointCost = 500,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow,
                IsActive = 'Y'
            };
        }

        [Fact(DisplayName = "RewardEntity com dados válidos deve passar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntityComDadosValidosDevePassarNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact(DisplayName = "RewardEntity sem Name deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntitySemNameDeveFalharNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();
            reward.Name = string.Empty;

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
                v.MemberNames.Contains("Name") &&
                v.ErrorMessage.Contains("required")
            );
        }

        [Fact(DisplayName = "RewardEntity com Name muito longo deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntityComNameMuitoLongoDeveFalharNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();
            reward.Name = new string('A', 256);

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
                v.MemberNames.Contains("Name") &&
                v.ErrorMessage.Contains("length")
            );
        }

        [Fact(DisplayName = "RewardEntity com PointCost negativo deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntityComPointCostNegativoDeveFalharNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();
            reward.PointCost = -100;

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
               v.MemberNames.Contains("PointCost") &&
               (v.ErrorMessage.Contains("deve ser") || v.ErrorMessage.Contains("maior que 0."))
           );
        }

        [Fact(DisplayName = "RewardEntity com PointCost zero deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntityComPointCostZeroDeveFalharNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();
            reward.PointCost = 0;

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
               v.MemberNames.Contains("PointCost") &&
               (v.ErrorMessage.Contains("deve ser") || v.ErrorMessage.Contains("maior que 0."))
           );
        }

        [Fact(DisplayName = "RewardEntity com StockQuantity negativo deve falhar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntityComStockQuantityNegativoDeveFalharNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();
            reward.StockQuantity = -1;

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, v =>
                v.MemberNames.Contains("StockQuantity") &&
                (v.ErrorMessage.Contains("não pode") || v.ErrorMessage.Contains("ser negativo."))
            );
        }

        [Fact(DisplayName = "RewardEntity com StockQuantity zero deve passar na validação")]
        [Trait("Categoria", "Domain - Entity")]
        public void RewardEntityComStockQuantityZeroDevePassarNaValidacao()
        {
            // Arrange
            var reward = CreateValidReward();
            reward.StockQuantity = 0;

            // Act
            var validationResults = ValidationHelper.Validate(reward);

            // Assert
            Assert.Empty(validationResults);
        }
    }
}
