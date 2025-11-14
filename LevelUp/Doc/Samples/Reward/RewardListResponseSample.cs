using LevelUp.Application.Dtos.Reward;
using LevelUp.Domain.Common;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Reward
{
    public class RewardListResponseSample : IExamplesProvider<PageResultModel<IEnumerable<RewardResponseDto>>>
    {
        public PageResultModel<IEnumerable<RewardResponseDto>> GetExamples()
        {
            var rewardList = new List<RewardResponseDto>
            {
                new RewardResponseDto(
                    Id: 1,
                    Name: "Gift Card Steam R$50",
                    Description: "R$50 em créditos para usar na plataforma Steam.",
                    PointCost: 350,
                    StockQuantity: 100
                ),
                new RewardResponseDto(
                    Id: 2,
                    Name: "Gift Card iFood R$100",
                    Description: "R$100 em créditos no iFood.",
                    PointCost: 600,
                    StockQuantity: 75
                ),
                new RewardResponseDto(
                    Id: 3,
                    Name: "Day-off de Bem-Estar",
                    Description: "Dia de folga focado em desconexão e recuperação.",
                    PointCost: 1200,
                    StockQuantity: 20
                )
            };

            return new PageResultModel<IEnumerable<RewardResponseDto>>
            {
                Data = rewardList,
                Offset = 0,
                Take = 10,
                Total = 3
            };
        }
    }
}
