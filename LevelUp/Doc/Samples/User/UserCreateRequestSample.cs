using LevelUp.Application.Dtos.User;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.User
{
    public class UserCreateRequestSample : IExamplesProvider<UserCreateDto>
    {
        public UserCreateDto GetExamples()
        {
            return new UserCreateDto(
                FullName: "Novo Usuário da Silva",
                Email: "novo.usuario@levelup.com",
                Password: "NovaSenha@123",
                JobTitle: "Analista de QA Jr.",
                TeamId: 1
            );
        }
    }
}
