using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Postgres.Models
{
    [Table("order_files")]
    public class OrderFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        [Column("order_id")]
        public int OrderId { get; set; }

        [Required]
        [Column("file_name")]
        [StringLength(255)]
        public string FileName { get; set; }

        [Column("file_path")]
        [StringLength(512)]
        public string FilePath { get; set; }

        [Column("yougile_file_id")]
        [StringLength(100)]
        public string YougileFileId { get; set; }

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public Order Order { get; set; }
    }
}