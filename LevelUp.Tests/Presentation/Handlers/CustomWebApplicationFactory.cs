using LevelUp.Application.Interfaces;
using LevelUp.Tests.Presentation.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Collections.Generic;

namespace LevelUp.Tests.Presentation.Handlers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IAuthUseCase> AuthUseCaseMock { get; }
        public Mock<IUserUseCase> UserUseCaseMock { get; }
        public Mock<ITeamUseCase> TeamUseCaseMock { get; }
        public Mock<IRewardUseCase> RewardUseCaseMock { get; }
        public Mock<IRewardRedemptionUseCase> RedemptionRewardUseCaseMock { get; }

        public CustomWebApplicationFactory()
        {
            AuthUseCaseMock = new Mock<IAuthUseCase>();
            UserUseCaseMock = new Mock<IUserUseCase>();
            TeamUseCaseMock = new Mock<ITeamUseCase>();
            RewardUseCaseMock = new Mock<IRewardUseCase>();
            RedemptionRewardUseCaseMock = new Mock<IRewardRedemptionUseCase>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IAuthUseCase));
                services.RemoveAll(typeof(IUserUseCase));
                services.RemoveAll(typeof(ITeamUseCase));
                services.RemoveAll(typeof(IRewardUseCase));
                services.RemoveAll(typeof(IRewardRedemptionUseCase));

                // Adiciona os Mocks como Singletons
                services.AddSingleton(AuthUseCaseMock.Object);
                services.AddSingleton(UserUseCaseMock.Object);
                services.AddSingleton(TeamUseCaseMock.Object);
                services.AddSingleton(RewardUseCaseMock.Object);
                services.AddSingleton(RedemptionRewardUseCaseMock.Object);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                    options.DefaultChallengeScheme = TestAuthHandler.Scheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.Scheme, _ => { });

                var testConfig = new Dictionary<string, string>
                {
                    ["Jwt:SecretKey"] = "UMA_CHAVE_SECRETA_DE_TESTE_BEM_LONGA_E_SEGURA_123",
                    ["Jwt:ExpiresInHours"] = "1"
                };

                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(testConfig)
                    .Build();

                services.RemoveAll(typeof(IConfiguration));
                services.AddSingleton<IConfiguration>(config);
            });
        }
    }
}
