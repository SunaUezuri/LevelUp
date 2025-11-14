using LevelUp.Application.Dtos.Reward;
using Swashbuckle.AspNetCore.Filters;

namespace LevelUp.Doc.Samples.Reward
{
    public class RewardUpdateRequestSample : IExamplesProvider<RewardCreateUpdateDto>
    {
        public RewardCreateUpdateDto GetExamples()
        {
            return new RewardCreateUpdateDto(
                Name: "Gift Card Steam R$75",
                Description: "Valor atualizado para R$75 em créditos na Steam.",
                PointCost: 500,
                StockQuantity: 50
            );
        }
    }
}
