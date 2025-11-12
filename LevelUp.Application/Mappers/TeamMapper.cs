using LevelUp.Application.Dtos.Team;
using LevelUp.Domain.Entities;

namespace LevelUp.Application.Mappers
{
    public static class TeamMapper
    {

        public static TeamEntity ToEntity(this TeamCreateUpdateDto dto)
        {
            return new TeamEntity
            {
                TeamName = dto.TeamName
            };
        }

        public static TeamResponseDto ToResponseDto(this TeamEntity team)
        {
            return new TeamResponseDto(
                            team.Id,
                            team.TeamName
                        );
        }
    }
}
