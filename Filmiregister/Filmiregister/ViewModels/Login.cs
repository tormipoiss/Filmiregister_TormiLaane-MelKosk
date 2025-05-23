using System.ComponentModel.DataAnnotations;

namespace Filmiregister.ViewModels
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember login?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
