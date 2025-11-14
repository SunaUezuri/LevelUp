using LevelUp.Application.Dtos.Reward;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Reward
{
    public class RewardCreateRequestSample : IExamplesProvider<RewardCreateUpdateDto>
    {
        public RewardCreateUpdateDto GetExamples()
        {
            return new RewardCreateUpdateDto(
                Name: "Gift Card Steam R$50",
                Description: "R$50 em créditos para usar na plataforma Steam.",
                PointCost: 350,
                StockQuantity: 100
            );
        }
    }
}
