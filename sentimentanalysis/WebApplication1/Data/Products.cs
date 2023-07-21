using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication1.Data
{
    [Table("Sheet1$")]
    public partial class Products
    {
        [Key]
        [Column("id")]
        public double? Id { get; set; }

        [Column("title")]
        [StringLength(255)]
        public string? Title { get; set; }

        [Column("price")]
        public double? Price { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("category")]
        [StringLength(255)]
        public string? Category { get; set; }

        [Column("image")]
        [StringLength(255)]
        public string? Image { get; set; }
    }
}
