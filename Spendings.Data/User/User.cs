using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Spendings.Data.Records;

namespace Spendings.Data.Users
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column("ulogin")]
        public string Login { get; set; }
        [Column("upassword")]
        public string Password { get; set; }
        [Column("isDeleted")]
        public bool IsDeleted { get; set; }

        public ICollection<Record> Records { get; set; }
    }
}
