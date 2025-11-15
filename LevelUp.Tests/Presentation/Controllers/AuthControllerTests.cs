using LevelUp.Application.Dtos.Auth;
using LevelUp.Application.Dtos.User;
using LevelUp.Application.Interfaces;
using LevelUp.Domain.Common;
using LevelUp.Tests.Presentation.Handlers;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace LevelUp.Tests.Presentation.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IAuthUseCase> _authUseCaseMock;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _authUseCaseMock = _factory.AuthUseCaseMock;

            _authUseCaseMock.Reset();
        }

        private UserResponseDto CreateValidUserResponseDto()
        {
            return new UserResponseDto(1, "Teste", "teste@levelup.com", "Tester", 100, "USER", 1);
        }

        [Fact(DisplayName = "Login com credenciais válidas deve retornar 200 OK e Token")]
        [Trait("Categoria", "Presentation - Controller")]
        public async Task LoginComCredenciaisValidasDeveRetornar200OKEToken()
        {
            // Arrange
            var loginDto = new AuthRequestDto("teste@levelup.com", "senha123");
            var userResponse = CreateValidUserResponseDto();
            var authResponse = new AuthResponseDto("TOKEN_FALSO", userResponse);

            _authUseCaseMock.Setup(u => u.LoginAsync(It.IsAny<AuthRequestDto>()))
                            .ReturnsAsync(OperationResult<AuthResponseDto>.Success(authResponse));

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject(jsonString);

            string returnedToken = (string)content.data.token;
            Assert.NotNull(returnedToken);
            Assert.NotEmpty(returnedToken);
            Assert.StartsWith("ey", returnedToken);
            Assert.Equal(userResponse.Email, (string)content.data.user.email);
        }

        [Fact(DisplayName = "Login com credenciais inválidas deve retornar 401 Unauthorized")]
        [Trait("Categoria", "Presentation - Controller")]
        public async Task LoginComCredenciaisInvalidasDeveRetornar401Unauthorized()
        {
            // Arrange
            var loginDto = new AuthRequestDto("errado@levelup.com", "senha-errada");

            _authUseCaseMock.Setup(u => u.LoginAsync(It.IsAny<AuthRequestDto>()))
                            .ReturnsAsync(OperationResult<AuthResponseDto>.Failure(
                                "Email ou senha inválidos.", (int)HttpStatusCode.Unauthorized));

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "Register com dados válidos deve retornar 201 Created")]
        [Trait("Categoria", "Presentation - Controller")]
        public async Task RegisterComDadosValidosDeveRetornar201Created()
        {
            // Arrange
            var registerDto = new UserCreateDto("Novo User", "novo@email.com", "senha123", "QA", 1);
            var userResponse = CreateValidUserResponseDto();
            _authUseCaseMock.Setup(u => u.RegisterAsync(It.IsAny<UserCreateDto>()))
                            .ReturnsAsync(OperationResult<UserResponseDto?>.Success(
                                userResponse, (int)HttpStatusCode.Created));

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "Register com email duplicado deve retornar 409 Conflict")]
        [Trait("Categoria", "Presentation - Controller")]
        public async Task RegisterComEmailDuplicadoDeveRetornar409Conflict()
        {
            // Arrange
            var registerDto = new UserCreateDto("Duplicado", "duplicado@email.com", "senha123", "QA", 1);

            _authUseCaseMock.Setup(u => u.RegisterAsync(It.IsAny<UserCreateDto>()))
                            .ReturnsAsync(OperationResult<UserResponseDto?>.Failure(
                                "Este e-mail já está em uso.", (int)HttpStatusCode.Conflict));

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/register", registerDto);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
