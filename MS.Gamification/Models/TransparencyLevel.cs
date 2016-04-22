using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.Models
    {
    public enum TransparencyLevel
        {
        Unknown,

        [Display(Name = "Extremely Clear")]
        ExtremelyClear,
        Clear,

        [Display(Name = "Mostly Clear")]
        MostlyClear,

        [Display(Name = "Somewhat Clear")]
        SomewhatClear,
        Poor
        }
    }