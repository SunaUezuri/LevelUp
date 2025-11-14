using LevelUp.Application.Dtos.Team;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Team
{
    public class TeamResponseSample : IExamplesProvider<TeamResponseDto>
    {
        public TeamResponseDto GetExamples()
        {
            return new TeamResponseDto(
                Id: 1,
                TeamName: "Equipe de Engenharia Alpha"
            );
        }
    }
}
