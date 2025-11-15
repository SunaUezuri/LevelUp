using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LevelUp.Domain.Entities
{
    [Table("TB_LEVELUP_REWARD_REDEMPTIONS")]
    public class RewardRedemptionEntity
    {
        [Key]
        [Column("REDEMPTION_ID")]
        public int Id { get; set; }

        [Required]
        [Column("USER_ID")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId deve ser válido.")]
        public int UserId { get; set; }

        [Required]
        [Column("REWARD_ID")]
        [Range(1, int.MaxValue, ErrorMessage = "RewardId deve ser válido.")]
        public int RewardId { get; set; }

        [Required]
        [Column("REDEEMED_AT")]
        public DateTime RedeemedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("POINTS_SPENT")]
        [Range(1, int.MaxValue, ErrorMessage = "PointsSpent deve ser maior que 0.")]
        public int PointsSpent { get; set; }

        [JsonIgnore]
        public virtual UserEntity User { get; set; }

        [JsonIgnore]
        public virtual RewardEntity Reward { get; set; }
    }
}
