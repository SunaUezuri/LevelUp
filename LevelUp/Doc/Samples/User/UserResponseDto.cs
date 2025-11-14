using LevelUp.Application.Dtos.User;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.User
{
    public class UserResponseSample : IExamplesProvider<UserResponseDto>
    {
        public UserResponseDto GetExamples()
        {
            return new UserResponseDto(
                Id: 2,
                FullName: "Usuário Comum",
                Email: "usuario.comum@levelup.com",
                JobTitle: "Desenvolvedor .NET Pleno",
                PointBalance: 350,
                Role: "USER",
                TeamId: 1
            );
        }
    }
}
