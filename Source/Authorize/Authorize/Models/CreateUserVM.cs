using System.ComponentModel.DataAnnotations;

namespace Authorize.Models
{
    public class CreateUserVM
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password1 { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password2 { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Success { get; set; }
    }
}
