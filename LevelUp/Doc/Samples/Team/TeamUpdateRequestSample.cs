using LevelUp.Application.Dtos.Team;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Team
{
    public class TeamUpdateRequestSample : IExamplesProvider<TeamCreateUpdateDto>
    {
        public TeamCreateUpdateDto GetExamples()
        {
            return new TeamCreateUpdateDto(
                TeamName: "Equipe de Engenharia"
            );
        }
    }
}
