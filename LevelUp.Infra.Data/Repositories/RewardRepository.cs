using LevelUp.Domain.Common;
using LevelUp.Domain.Entities;
using LevelUp.Domain.Errors;
using LevelUp.Domain.Interfaces;
using LevelUp.Infra.Data.AppData;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace LevelUp.Infra.Data.Repositories
{
    public class RewardRepository : IRewardRepository
    {
        private readonly ApplicationContext _context;
        public RewardRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<RewardEntity?> CreateAsync(RewardEntity reward)
        {
            var p_reward_id = new OracleParameter("P_REWARD_ID", OracleDbType.Int32, ParameterDirection.Output);

            await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    BEGIN
                        PKG_LEVELUP_APP.PR_CREATE_REWARD(
                            P_NAME           => {reward.Name},
                            P_DESCRIPTION    => {reward.Description},
                            P_POINT_COST     => {reward.PointCost},
                            P_STOCK_QUANTITY => {reward.StockQuantity},
                            P_REWARD_ID      => {p_reward_id}
                        );
                    END;
                    """);

            reward.Id = Convert.ToInt32(p_reward_id.Value.ToString());
            reward.CreatedAt = DateTime.UtcNow;
            return reward;
        }

        public async Task<RewardEntity?> DeleteAsync(int id)
        {
            /*
                Método de soft delete para manter o histórico dos recompensas
                mas não exibir mais no sistema.
            */
            var reward = await _context.Rewards.FindAsync(id);

            if (reward is null || reward.IsActive == 'N')
            {
                throw new IdNotFoundException($"Recompensa com ID: {id} - não encontrada para deletar.");
            }

            reward.IsActive = 'N';
            reward.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return reward;
        }

        public async Task<PageResultModel<IEnumerable<RewardEntity>>> GetAllAsync(int offset = 0, int take = 10)
        {
            var query = _context.Rewards.Where(r => r.IsActive == 'Y');

            var total = await query.CountAsync();

            var data = await query
                .OrderBy(r => r.PointCost)
                .Skip(offset)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();

            return new PageResultModel<IEnumerable<RewardEntity>>
            {
                Data = data,
                Total = total,
                Offset = offset,
                Take = take
            };
        }

        public async Task<RewardEntity?> GetByIdAsync(int id)
        {
            var reward = await _context.Rewards
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive == 'Y');

            if (reward is null)
            {
                throw new NoContentException("Usuário não encontrado.");
            }

            return reward;
        }

        public async Task<RewardEntity?> UpdateAsync(int id, RewardEntity reward)
        {
            var existingReward = await _context.Rewards.FindAsync(id);
            if (existingReward != null && existingReward.IsActive == 'Y')
            {
                existingReward.Name = reward.Name;
                existingReward.Description = reward.Description;
                existingReward.PointCost = reward.PointCost;
                existingReward.StockQuantity = reward.StockQuantity;
                existingReward.UpdatedAt = DateTime.UtcNow;

                _context.Rewards.Update(existingReward);
                await _context.SaveChangesAsync();
                return existingReward;
            }
            return null;
        }
    }
}
