using LevelUp.Application.Dtos.Auth;
using LevelUp.Application.Dtos.User;
using LevelUp.Domain.Common;

namespace LevelUp.Application.Interfaces
{
    public interface IAuthUseCase
    {
        Task<OperationResult<AuthResponseDto?>> LoginAsync(AuthRequestDto request);

        Task<OperationResult<UserResponseDto?>> RegisterAsync(UserCreateDto request);
    }
}
