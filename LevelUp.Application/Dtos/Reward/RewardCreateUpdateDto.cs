namespace LevelUp.Application.Dtos.Reward
{
    public record RewardCreateUpdateDto(
            string Name,
            string? Description,
            int PointCost,
            int StockQuantity
        );
}
