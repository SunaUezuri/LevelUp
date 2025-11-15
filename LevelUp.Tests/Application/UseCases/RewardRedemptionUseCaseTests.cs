using LevelUp.Application.Interfaces;
using LevelUp.Application.UseCases;
using LevelUp.Domain.Common;
using LevelUp.Domain.Entities;
using LevelUp.Domain.Errors;
using LevelUp.Domain.Interfaces;
using Moq;
using System.Net;

namespace LevelUp.Tests.Application.UseCases
{
    public class RewardRedemptionUseCaseTests
    {
        private readonly Mock<IRewardRedemptionRepository> _redemptionRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRewardRepository> _rewardRepositoryMock;
        private readonly IRewardRedemptionUseCase _redemptionUseCase;

        public RewardRedemptionUseCaseTests()
        {
            _redemptionRepositoryMock = new Mock<IRewardRedemptionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _rewardRepositoryMock = new Mock<IRewardRepository>();

            _redemptionUseCase = new RewardRedemptionUseCase(
                _redemptionRepositoryMock.Object,
                _userRepositoryMock.Object,
                _rewardRepositoryMock.Object
            );
        }
        private UserEntity CreateValidUser(int id = 1, int points = 1000)
        {
            return new UserEntity { Id = id, FullName = "Usuário Teste", Email = "teste@user.com", PointBalance = points, IsActive = 'Y', Role = "USER" };
        }

        private RewardEntity CreateValidReward(int id = 1, int cost = 100, int stock = 5)
        {
            return new RewardEntity { Id = id, Name = "Gift Card", PointCost = cost, StockQuantity = stock, IsActive = 'Y' };
        }

        [Fact(DisplayName = "RedeemAsync com saldo e estoque deve retornar Success 201")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task RedeemAsyncComSaldoEEstoqueDeveRetornarSuccess201()
        {
            // Arrange
            var user = CreateValidUser(id: 1, points: 1000);
            var reward = CreateValidReward(id: 5, cost: 100, stock: 5);
            int novoSaldo = 900;
            int novoEstoque = 4;

            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            _rewardRepositoryMock.Setup(r => r.GetByIdAsync(reward.Id)).ReturnsAsync(reward);

            // 3. Configura o "UpdateUserPointsAsync" para aceitar o novo saldo
            _userRepositoryMock.Setup(r => r.UpdateUserPointsAsync(user.Id, novoSaldo)).ReturnsAsync(true);

            _rewardRepositoryMock.Setup(r => r.UpdateAsync(reward.Id, It.Is<RewardEntity>(r => r.StockQuantity == novoEstoque))).ReturnsAsync(reward);

            _redemptionRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RewardRedemptionEntity>()))
                                     .ReturnsAsync((RewardRedemptionEntity r) => {
                                         r.Id = 99;
                                         r.Reward = reward;
                                         return r;
                                     });

            // Act
            var result = await _redemptionUseCase.RedeemAsync(user.Id, reward.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(reward.Name, result.Value.RewardName);
            Assert.Equal(reward.PointCost, result.Value.PointsSpent);
            _userRepositoryMock.Verify(r => r.UpdateUserPointsAsync(user.Id, novoSaldo), Times.Once);
            _rewardRepositoryMock.Verify(r => r.UpdateAsync(reward.Id, It.Is<RewardEntity>(re => re.StockQuantity == novoEstoque)), Times.Once);
            _redemptionRepositoryMock.Verify(r => r.CreateAsync(It.Is<RewardRedemptionEntity>(r => r.UserId == user.Id && r.RewardId == reward.Id)), Times.Once);
        }

        [Fact(DisplayName = "RedeemAsync com pontos insuficientes deve retornar Failure 400")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task RedeemAsyncComPontosInsuficientesDeveRetornarFailure400()
        {
            // Arrange
            var user = CreateValidUser(points: 50);
            var reward = CreateValidReward(cost: 100);

            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _rewardRepositoryMock.Setup(r => r.GetByIdAsync(reward.Id)).ReturnsAsync(reward);

            // Act
            var result = await _redemptionUseCase.RedeemAsync(user.Id, reward.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("Pontos insuficientes", result.Error);
            _redemptionRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<RewardRedemptionEntity>()), Times.Never);
            _userRepositoryMock.Verify(r => r.UpdateUserPointsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = "RedeemAsync com recompensa fora de estoque deve retornar Failure 400")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task RedeemAsyncComRecompensaForaDeEstoqueDeveRetornarFailure400()
        {
            // Arrange
            var user = CreateValidUser(points: 1000);
            var reward = CreateValidReward(cost: 100, stock: 0);

            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _rewardRepositoryMock.Setup(r => r.GetByIdAsync(reward.Id)).ReturnsAsync(reward);

            // Act
            var result = await _redemptionUseCase.RedeemAsync(user.Id, reward.Id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Contains("fora de estoque", result.Error);
        }

        [Fact(DisplayName = "RedeemAsync com usuário inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task RedeemAsyncComUsuarioInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByIdAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _redemptionUseCase.RedeemAsync(99, 1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Usuário não encontrado", result.Error);
        }

        [Fact(DisplayName = "RedeemAsync com recompensa inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task RedeemAsyncComRecompensaInexistenteDeveRetornarFailure404()
        {
            // Arrange
            var user = CreateValidUser();
            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            _rewardRepositoryMock.Setup(r => r.GetByIdAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Recompensa não encontrada"));

            // Act
            var result = await _redemptionUseCase.RedeemAsync(user.Id, 99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Recompensa não encontrada", result.Error);
        }


        [Fact(DisplayName = "GetUserRedemptionsAsync deve retornar Success e a lista de DTOs")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetUserRedemptionsAsyncDeveRetornarSuccessEListaDTOs()
        {
            // Arrange
            var user = CreateValidUser(id: 5);
            var reward = CreateValidReward();
            var redemptions = new List<RewardRedemptionEntity>
            {
                new RewardRedemptionEntity { Id = 1, UserId = user.Id, RewardId = reward.Id, PointsSpent = 100, RedeemedAt = System.DateTime.Now, Reward = reward }
            };
            var pageResult = new PageResultModel<IEnumerable<RewardRedemptionEntity>>
            {
                Data = redemptions,
                Total = 1,
                Offset = 0,
                Take = 10
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _redemptionRepositoryMock.Setup(r => r.GetByUserIdAsync(user.Id, 0, 10)).ReturnsAsync(pageResult);

            // Act
            var result = await _redemptionUseCase.GetUserRedemptionsAsync(user.Id, 0, 10);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Total);
            Assert.Equal(reward.Name, result.Value.Data.First().RewardName);
        }

        [Fact(DisplayName = "GetUserRedemptionsAsync com usuário inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetUserRedemptionsAsyncComUsuarioInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByIdAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _redemptionUseCase.GetUserRedemptionsAsync(99, 0, 10);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Usuário não encontrado", result.Error);
        }
    }
}
