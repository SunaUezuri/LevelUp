using LevelUp.Application.Dtos.Reward;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Reward
{
    public class RewardResponseSample : IExamplesProvider<RewardResponseDto>
    {
        public RewardResponseDto GetExamples()
        {
            return new RewardResponseDto(
                Id: 1,
                Name: "Gift Card Steam R$50",
                Description: "R$50 em créditos para usar na plataforma Steam.",
                PointCost: 350,
                StockQuantity: 100
            );
        }
    }
}
