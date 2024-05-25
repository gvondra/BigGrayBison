using System.ComponentModel.DataAnnotations;

namespace Authorize.Models
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ResponseType { get; set; }
        public string ClientId { get; set; }
        public string RedirectUrl { get; set; }
        public string State { get; set; }
    }
}
