using System;
using System.ComponentModel.DataAnnotations;

namespace Authorize.Models
{
    public class UserVM
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTimestamp { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string EmailAddress { get; set; }
        public bool IsUserEditor { get; set; }
    }
}
