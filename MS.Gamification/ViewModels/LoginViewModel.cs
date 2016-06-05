using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class LoginViewModel
        {
        [Required]
        [Display(Name = "User Name or Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        }
    }