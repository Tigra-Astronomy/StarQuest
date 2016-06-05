using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ExternalLoginConfirmationViewModel
        {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        }
    }