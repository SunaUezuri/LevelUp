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
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationContext _context;

        public TeamRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<TeamEntity?> CreateAsync(TeamEntity team)
        {
            var p_team_id = new OracleParameter("P_TEAM_ID", OracleDbType.Int32, ParameterDirection.Output);

            await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        BEGIN
                            PKG_LEVELUP_APP.PR_CREATE_TEAM(
                                P_TEAM_NAME => {team.TeamName},
                                P_TEAM_ID   => {p_team_id}
                            );
                        END;
                        """);

            team.Id = Convert.ToInt32(p_team_id.Value.ToString());
            return team;
        }

        public async Task<TeamEntity?> DeleteAsync(int id)
        {
            var team = await GetByIdAsync(id);
            if (team is null)
            {
                throw new IdNotFoundException($"Time com ID: {id} - não encontrado.");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<PageResultModel<IEnumerable<TeamEntity>>> GetAllAsync(int offset = 0, int take = 10)
        {
            var total = await _context.Teams.CountAsync();

            var data = await _context.Teams
                .OrderBy(t => t.TeamName)
                .Skip(offset)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();

            return new PageResultModel<IEnumerable<TeamEntity>>
            {
                Data = data,
                Total = total,
                Offset = offset,
                Take = take
            };
        }

        public async Task<TeamEntity?> GetByIdAsync(int id)
        {
            var team = await _context.Teams
                .Include(t => t.Users)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team is null)
            {
                throw new IdNotFoundException($"Time com ID: {id} - não encontrado.");
            }

            return team;
        }

        public async Task<TeamEntity?> UpdateAsync(int id, TeamEntity team)
        {
            var existingTeam = await _context.Teams.FindAsync(id);
            if (existingTeam != null)
            {
                existingTeam.TeamName = team.TeamName;
                _context.Teams.Update(existingTeam);
                await _context.SaveChangesAsync();
                return existingTeam;
            }
            return null;
        }
    }
}
