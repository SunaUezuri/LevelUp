using LevelUp.Application.Dtos.User;

namespace LevelUp.Application.Dtos.Auth
{
    public record AuthResponseDto(
            string Token,
            UserResponseDto User
        );
}
