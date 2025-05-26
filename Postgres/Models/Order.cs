using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Postgres.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("customer_name")]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [Column("phone")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "new";

        [Column("task_id")]
        public string TaskId { get; set; } = string.Empty;

        public ICollection<OrderFile> Files { get; set; } = new List<OrderFile>();
    }
}