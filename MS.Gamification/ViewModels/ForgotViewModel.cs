using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ForgotViewModel
        {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        }
    }