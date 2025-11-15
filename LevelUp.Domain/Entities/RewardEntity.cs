using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LevelUp.Domain.Entities
{
    [Table("TB_LEVELUP_REWARDS")]
    public class RewardEntity
    {
        [Key]
        [Column("reward_id")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("point_cost")]
        [Range(1, int.MaxValue, ErrorMessage = "PointCost deve ser maior que 0.")]
        public int PointCost { get; set; }

        [Required]
        [Column("stock_quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity não pode ser negativo.")]
        public int StockQuantity { get; set; } = 0;

        [Required]
        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; }

        [Column("UPDATED_AT")]
        public DateTime? UpdatedAt { get; set; }

        [Required]
        [Column("IS_ACTIVE", TypeName = "CHAR(1)")]
        public char IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<RewardRedemptionEntity> RewardRedemptions { get; set; } = new List<RewardRedemptionEntity>();
    }
}
