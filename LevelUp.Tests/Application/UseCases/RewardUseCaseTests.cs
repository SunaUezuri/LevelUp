
using LevelUp.Application.Dtos.Reward;
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
    public class RewardUseCaseTests
    {
        private readonly Mock<IRewardRepository> _rewardRepositoryMock;
        private readonly IRewardUseCase _rewardUseCase;

        public RewardUseCaseTests()
        {
            _rewardRepositoryMock = new Mock<IRewardRepository>();
            _rewardUseCase = new RewardUseCase(_rewardRepositoryMock.Object);
        }

        private RewardEntity CreateValidRewardEntity(int id = 1)
        {
            return new RewardEntity
            {
                Id = id,
                Name = "Gift Card Teste",
                Description = "Descrição Teste",
                PointCost = 100,
                StockQuantity = 50,
                IsActive = 'Y',
                CreatedAt = System.DateTime.UtcNow
            };
        }

        private RewardCreateUpdateDto CreateValidRewardDto()
        {
            return new RewardCreateUpdateDto(
                "Nova Recompensa",
                "Descrição DTO",
                150,
                75
            );
        }


        [Fact(DisplayName = "GetByIdAsync com ID existente deve retornar Success")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByIdAsyncComIdExistenteDeveRetornarSuccess()
        {
            // Arrange
            var reward = CreateValidRewardEntity(1);
            _rewardRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reward);

            // Act
            var result = await _rewardUseCase.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(reward.Id, result.Value.Id);
            Assert.Equal(reward.Name, result.Value.Name);
        }

        [Fact(DisplayName = "GetByIdAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByIdAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _rewardRepositoryMock.Setup(r => r.GetByIdAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Recompensa não encontrada"));

            // Act
            var result = await _rewardUseCase.GetByIdAsync(99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Recompensa não encontrada", result.Error);
        }


        [Fact(DisplayName = "GetAllAsync deve retornar Success e mapear DTOs")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetAllAsyncDeveRetornarSuccessEMapearDTOs()
        {
            // Arrange
            var rewards = new List<RewardEntity>
            {
                CreateValidRewardEntity(1),
                CreateValidRewardEntity(2)
            };
            var pageResult = new PageResultModel<IEnumerable<RewardEntity>>
            {
                Data = rewards,
                Total = 2,
                Offset = 0,
                Take = 10
            };
            _rewardRepositoryMock.Setup(r => r.GetAllAsync(0, 10)).ReturnsAsync(pageResult);

            // Act
            var result = await _rewardUseCase.GetAllAsync(0, 10);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Total);
            Assert.Equal(2, result.Value.Data.Count());
        }


        [Fact(DisplayName = "CreateAsync deve retornar Success 201")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task CreateAsyncDeveRetornarSuccess201()
        {
            // Arrange
            var createDto = CreateValidRewardDto();

            _rewardRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RewardEntity>()))
                               .ReturnsAsync((RewardEntity reward) =>
                               {
                                   reward.Id = 10;
                                   return reward;
                               });

            // Act
            var result = await _rewardUseCase.CreateAsync(createDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(10, result.Value.Id);
            Assert.Equal("Nova Recompensa", result.Value.Name);
        }


        [Fact(DisplayName = "UpdateAsync com ID existente deve retornar Success 200")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task UpdateAsyncComIdExistenteDeveRetornarSuccess200()
        {
            // Arrange
            var updateDto = CreateValidRewardDto();
            var updatedReward = CreateValidRewardEntity(1);
            updatedReward.Name = updateDto.Name;

            _rewardRepositoryMock.Setup(r => r.UpdateAsync(1, It.Is<RewardEntity>(r => r.Name == updateDto.Name)))
                               .ReturnsAsync(updatedReward);

            // Act
            var result = await _rewardUseCase.UpdateAsync(1, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal(updateDto.Name, result.Value.Name);
        }

        [Fact(DisplayName = "UpdateAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task UpdateAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            var updateDto = CreateValidRewardDto();

            _rewardRepositoryMock.Setup(r => r.UpdateAsync(99, It.IsAny<RewardEntity>()))
                               .ReturnsAsync((RewardEntity)null);

            // Act
            var result = await _rewardUseCase.UpdateAsync(99, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("não encontrada para atualizar", result.Error);
        }

        [Fact(DisplayName = "DeleteAsync com ID existente deve retornar Success 200")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task DeleteAsyncComIdExistenteDeveRetornarSuccess200()
        {
            // Arrange
            var rewardToDelete = CreateValidRewardEntity(1);
            _rewardRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(rewardToDelete);

            // Act
            var result = await _rewardUseCase.DeleteAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            _rewardRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact(DisplayName = "DeleteAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task DeleteAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _rewardRepositoryMock.Setup(r => r.DeleteAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Recompensa não encontrada"));

            // Act
            var result = await _rewardUseCase.DeleteAsync(99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Recompensa não encontrada", result.Error);
        }
    }
}
