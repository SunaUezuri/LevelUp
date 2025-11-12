namespace LevelUp.Application.Dtos.Reward
{
    public record RewardResponseDto(
            int Id,
            string Name,
            string? Description,
            int PointCost,
            int StockQuantity
        );
}
