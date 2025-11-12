namespace LevelUp.Application.Dtos.User
{
    public record UserCreateDto(string FullName, string Email, string Password, string? JobTitle, int? TeamId);
}
