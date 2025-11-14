using LevelUp.Application.Dtos.Auth;
using LevelUp.Application.Dtos.User;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.User
{
    public class AuthResponseSample : IExamplesProvider<AuthResponseDto>
    {
        public AuthResponseDto GetExamples()
        {
            var user = new UserResponseDto(
                Id: 1,
                FullName: "Administrador do Sistema",
                Email: "admin@levelup.com",
                JobTitle: "SysAdmin",
                PointBalance: 9999,
                Role: "ADMIN",
                TeamId: 1
            );

            return new AuthResponseDto(
                Token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkB...",
                User: user
            );
        }
    }
}
