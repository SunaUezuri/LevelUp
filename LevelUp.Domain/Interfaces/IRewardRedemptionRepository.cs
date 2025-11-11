using LevelUp.Domain.Common;
using LevelUp.Domain.Entities;

namespace LevelUp.Infra.Data.Repositories
{
    public interface IRewardRedemptionRepository
    {
        Task<RewardRedemptionEntity?> CreateAsync(RewardRedemptionEntity redemption);
        Task<PageResultModel<IEnumerable<RewardRedemptionEntity>>> GetByUserIdAsync(int userId, int offset = 0, int take = 10);
    }
}
