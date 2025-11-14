using LevelUp.Application.Dtos.User;
using LevelUp.Domain.Common;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.User
{
    public class UserListResponseSample : IExamplesProvider<PageResultModel<IEnumerable<UserResponseDto>>>
    {
        public PageResultModel<IEnumerable<UserResponseDto>> GetExamples()
        {
            var userList = new List<UserResponseDto>
            {
                new UserResponseDto(
                    Id: 1,
                    FullName: "Administrador do Sistema",
                    Email: "admin@levelup.com",
                    JobTitle: "SysAdmin",
                    PointBalance: 9999,
                    Role: "ADMIN",
                    TeamId: 1
                ),
                new UserResponseDto(
                    Id: 2,
                    FullName: "Usuário Comum",
                    Email: "usuario.comum@levelup.com",
                    JobTitle: "Desenvolvedor .NET Pleno",
                    PointBalance: 350,
                    Role: "USER",
                    TeamId: 1
                )
            };

            return new PageResultModel<IEnumerable<UserResponseDto>>
            {
                Data = userList,
                Offset = 0,
                Take = 10,
                Total = 2
            };
        }
    }
}
