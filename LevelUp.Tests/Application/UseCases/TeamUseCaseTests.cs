using LevelUp.Application.Dtos.Team;
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
    public class TeamUseCaseTests
    {
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly ITeamUseCase _teamUseCase;

        public TeamUseCaseTests()
        {
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _teamUseCase = new TeamUseCase(_teamRepositoryMock.Object);
        }

        private TeamEntity CreateValidTeamEntity(int id = 1, string name = "Equipe Teste")
        {
            return new TeamEntity
            {
                Id = id,
                TeamName = name
            };
        }

        private TeamCreateUpdateDto CreateValidTeamDto(string name = "Nova Equipe")
        {
            return new TeamCreateUpdateDto(name);
        }


        [Fact(DisplayName = "GetByIdAsync com ID existente deve retornar Success")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByIdAsyncComIdExistenteDeveRetornarSuccess()
        {
            // Arrange
            var team = CreateValidTeamEntity(1, "Equipe Alpha");
            _teamRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(team);

            // Act
            var result = await _teamUseCase.GetByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(team.Id, result.Value.Id);
            Assert.Equal(team.TeamName, result.Value.TeamName);
        }

        [Fact(DisplayName = "GetByIdAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetByIdAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _teamRepositoryMock.Setup(r => r.GetByIdAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Time não encontrado"));

            // Act
            var result = await _teamUseCase.GetByIdAsync(99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Time não encontrado", result.Error);
        }


        [Fact(DisplayName = "GetAllAsync deve retornar Success e mapear DTOs")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task GetAllAsyncDeveRetornarSuccessEMapearDTOs()
        {
            // Arrange
            var teams = new List<TeamEntity>
            {
                CreateValidTeamEntity(1, "Equipe A"),
                CreateValidTeamEntity(2, "Equipe B")
            };
            var pageResult = new PageResultModel<IEnumerable<TeamEntity>>
            {
                Data = teams,
                Total = 2,
                Offset = 0,
                Take = 10
            };
            _teamRepositoryMock.Setup(r => r.GetAllAsync(0, 10)).ReturnsAsync(pageResult);

            // Act
            var result = await _teamUseCase.GetAllAsync(0, 10);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Total);
            Assert.Equal(2, result.Value.Data.Count());
            Assert.Equal("Equipe A", result.Value.Data.First().TeamName);
        }


        [Fact(DisplayName = "CreateAsync deve retornar Success 201")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task CreateAsyncDeveRetornarSuccess201()
        {
            // Arrange
            var createDto = CreateValidTeamDto("Nova Equipe");

            _teamRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<TeamEntity>()))
                               .ReturnsAsync((TeamEntity team) =>
                               {
                                   team.Id = 5;
                                   return team;
                               });

            // Act
            var result = await _teamUseCase.CreateAsync(createDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Id);
            Assert.Equal("Nova Equipe", result.Value.TeamName);

            _teamRepositoryMock.Verify(r => r.CreateAsync(It.Is<TeamEntity>(t => t.TeamName == createDto.TeamName)), Times.Once);
        }

        [Fact(DisplayName = "UpdateAsync com ID existente deve retornar Success 200")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task UpdateAsyncComIdExistenteDeveRetornarSuccess200()
        {
            // Arrange
            var updateDto = CreateValidTeamDto("Nome Atualizado");
            var updatedTeam = CreateValidTeamEntity(1, "Nome Atualizado");

            _teamRepositoryMock.Setup(r => r.UpdateAsync(1, It.Is<TeamEntity>(t => t.TeamName == updateDto.TeamName)))
                               .ReturnsAsync(updatedTeam);

            // Act
            var result = await _teamUseCase.UpdateAsync(1, updateDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
            Assert.Equal("Nome Atualizado", result.Value.TeamName);
        }

        [Fact(DisplayName = "UpdateAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task UpdateAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            var updateDto = CreateValidTeamDto("Nome Atualizado");

            _teamRepositoryMock.Setup(r => r.UpdateAsync(99, It.IsAny<TeamEntity>()))
                               .ReturnsAsync((TeamEntity)null); // Simula o repositório retornando nulo

            // Act
            var result = await _teamUseCase.UpdateAsync(99, updateDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("não encontrado para atualizar", result.Error);
        }

        [Fact(DisplayName = "DeleteAsync com ID existente deve retornar Success 200")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task DeleteAsyncComIdExistenteDeveRetornarSuccess200()
        {
            // Arrange
            var teamToDelete = CreateValidTeamEntity(1);
            _teamRepositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(teamToDelete);

            // Act
            var result = await _teamUseCase.DeleteAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact(DisplayName = "DeleteAsync com ID inexistente deve retornar Failure 404")]
        [Trait("Categoria", "Application - UseCase")]
        public async Task DeleteAsyncComIdInexistenteDeveRetornarFailure404()
        {
            // Arrange
            _teamRepositoryMock.Setup(r => r.DeleteAsync(99))
                               .ThrowsAsync(new IdNotFoundException("Time não encontrado"));

            // Act
            var result = await _teamUseCase.DeleteAsync(99);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains("Time não encontrado", result.Error);
        }
    }
}
