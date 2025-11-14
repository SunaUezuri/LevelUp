using LevelUp.Application.Dtos.Team;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Team
{
    public class TeamCreateRequestSample : IExamplesProvider<TeamCreateUpdateDto>
    {
        public TeamCreateUpdateDto GetExamples()
        {
            return new TeamCreateUpdateDto(
                TeamName: "Equipe de Engenharia Alpha"
            );
        }
    }
}
