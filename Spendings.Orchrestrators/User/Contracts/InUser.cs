using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Spendings.Orchrestrators.Users.Contracts
{
    public class InUser
    {
        [MinLength(3)]
        [MaxLength(30)]
        [BindRequired]
        public string Login { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        [BindRequired]
        public string Password { get; set; }
    }
}
