using LevelUp.Application.Dtos.Team;
using LevelUp.Domain.Common;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Team
{
    public class TeamListResponseSample : IExamplesProvider<PageResultModel<IEnumerable<TeamResponseDto>>>
    {
        public PageResultModel<IEnumerable<TeamResponseDto>> GetExamples()
        {
            var teamList = new List<TeamResponseDto>
            {
                new TeamResponseDto(
                    Id: 1,
                    TeamName: "Equipe de Engenharia Alpha"
                ),
                new TeamResponseDto(
                    Id: 2,
                    TeamName: "Equipe de Produto"
                ),
                new TeamResponseDto(
                    Id: 3,
                    TeamName: "Equipe de RH e Bem-Estar"
                )
            };

            return new PageResultModel<IEnumerable<TeamResponseDto>>
            {
                Data = teamList,
                Offset = 0,
                Take = 10,
                Total = 3
            };
        }
    }
}
