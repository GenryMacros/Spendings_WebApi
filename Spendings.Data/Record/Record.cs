using System.ComponentModel.DataAnnotations.Schema;
using Spendings.Data.Users;
using Spendings.Data.Categories;
using System.ComponentModel.DataAnnotations;
using System;

namespace Spendings.Data.Records
{
    [Table("Records")]
    public class Record
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        
        [Column("userId")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
       
        [Column("categoryId")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Column("rDate")]
        public DateTime Date { get; set; }
        [Column("amount")]
        public int Amount { get; set; }
         
    }
}
