using System.ComponentModel.DataAnnotations;

namespace Authorize.Models
{
    public class UsersVM
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
    }
}
