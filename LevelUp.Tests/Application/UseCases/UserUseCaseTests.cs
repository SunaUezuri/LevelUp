using LevelUp.Application.Dtos.User;
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
    public class UserUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserUseCase _userUseCase;

        public UserUseCaseTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userUseCase = new UserUseCase(_userRepositoryMock.Object);
        }

        private UserEntity CreateValidUserEntity(int id = 1, string name = "Usuário Teste")
        {
            return new UserEntity
            {
                Id = id,
                FullName = name,
                Email = $"{name.ToLower().Replace(' ', '.')}@levelup.com",
                Role = "USER",
                PointBalance = 100,
                IsActive = 'Y',
                CreatedAt = System.DateTime.UtcNow
            };
        }

        private UserUpdateDto CreateValidUserUpdateDto()
        {
            return new UserUpdateDto(
                "Nome Atualizado",
                "email@atualizado.com",
                "Cargo Atualizado",
                "ADMIN",
                2,
                500
            );
        }

        [Fact(DisplayName = "GetByIdAsync com ID existente deve retornar Success")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByIdAsyncComIdExistenteDeveRetornarSuccess()
        {
            // Arrange
            var user = CreateValidUserEntity(1);
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _userUseCase.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Id, result.Value.Id);
            Assert.Equal(user.FullName, result.Value.FullName);
        }

        [Fact(DisplayName = "GetByIdAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByIdAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _userRepositoryMock.Setup(r => r.GetByIdAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _userUseCase.GetByIdAsync(99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Usuário não encontrado", result.Error);
        }

        [Fact(DisplayName = "GetByEmailAsync com Email existente deve retornar Success")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByEmailAsyncComEmailExistenteDeveRetornarSuccess()
        {
            // Arrange
            var user = CreateValidUserEntity(1);
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            // Act
            var result = await _userUseCase.GetByEmailAsync(user.Email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Email, result.Value.Email);
        }

        [Fact(DisplayName = "GetAllAsync deve retornar Success e mapear DTOs")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetAllAsyncDeveRetornarSuccessEMapearDTOs()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                CreateValidUserEntity(1, "Usuário A"),
                CreateValidUserEntity(2, "Usuário B")
            };
            var pageResult = new PageResultModel<IEnumerable<UserEntity>>
            {
                Data = users,
                Total = 2,
                Offset = 0,
                Take = 10
            };
            _userRepositoryMock.Setup(r => r.GetAllAsync(0, 10)).ReturnsAsync(pageResult);

            // Act
            var result = await _userUseCase.GetAllAsync(0, 10);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Total);
            Assert.Equal(2, result.Value.Data.Count());
            Assert.Equal("Usuário A", result.Value.Data.First().FullName);
        }

        [Fact(DisplayName = "UpdateAsync com ID existente deve retornar Success 200")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task UpdateAsyncComIdExistenteDeveRetornarSuccess200()
        {
            // Arrange
            var updateDto = CreateValidUserUpdateDto();
            var updatedUser = CreateValidUserEntity(1);

            _userRepositoryMock.Setup(r => r.UpdateAsync(1, It.IsAny<UserEntity>()))
                               .ReturnsAsync(updatedUser);

            // Act
            var result = await _userUseCase.UpdateAsync(1, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            _userRepositoryMock.Verify(r => r.UpdateAsync(1, It.Is<UserEntity>(
                u => u.FullName == updateDto.FullName &&
                     u.Email == updateDto.Email &&
                     u.Role == updateDto.Role &&
                     u.PointBalance == updateDto.PointBalance
            )), Times.Once);
        }

        [Fact(DisplayName = "UpdateAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task UpdateAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            var updateDto = CreateValidUserUpdateDto();

            _userRepositoryMock.Setup(r => r.UpdateAsync(99, It.IsAny<UserEntity>()))
                               .ThrowsAsync(new IdNotFoundException("Usuário não encontrado"));

            // Act
            var result = await _userUseCase.UpdateAsync(99, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Usuário não encontrado", result.Error);
        }

        [Fact(DisplayName = "DeleteAsync com ID existente deve retornar Success 200")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task DeleteAsyncComIdExistenteDeveRetornarSuccess200()
        {
            // Arrange
            var userToDelete = CreateValidUserEntity(1);
            _userRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(userToDelete);

            // Act
            var result = await _userUseCase.DeleteAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1, result.Value.Id);
            _userRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
