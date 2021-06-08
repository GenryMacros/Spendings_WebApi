using System.ComponentModel.DataAnnotations;

namespace Spendings.Orchrestrators.Categories.Contracts
{
   public class InCategory
    {
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}
