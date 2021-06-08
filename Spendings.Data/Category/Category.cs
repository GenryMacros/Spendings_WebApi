using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Spendings.Data.Records;

namespace Spendings.Data.Categories
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column("category")]
        public string Name { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
