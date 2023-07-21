using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("name")]
        [StringLength(255)]
        public string? Name { get; set; }

        [Column("comments")]
        public string ? Comments { get; set; }

        [Column("score")]
        public int ? score { get; set; }

        [Column("image")]
        [StringLength(255)]
        public string? Image { get; set; }
    }
}
