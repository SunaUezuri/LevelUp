namespace LevelUp.Application.Dtos.User
{
    public record UserResponseDto(
            int Id,
            string FullName,
            string Email,
            string? JobTitle,
            int PointBalance,
            string Role,
            int? TeamId
        );
}
