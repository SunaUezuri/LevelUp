using LevelUp.Application.Dtos.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.User
{
    public class AuthRequestSample : IExamplesProvider<AuthRequestDto>
    {
        public AuthRequestDto GetExamples()
        {
            return new AuthRequestDto(
                Email: "admin@levelup.com",
                Password: "AdminPassword123!"
            );
        }
    }
}
