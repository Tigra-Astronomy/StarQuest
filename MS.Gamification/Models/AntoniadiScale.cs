using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.Models
    {
    public enum AntoniadiScale
        {
        Unknown,

        [Display(Name = "I - Perfectly Stable")]
        PerfectlyStable,

        [Display(Name = "II - MostlyStable")]
        MostlyStable,

        [Display(Name = "III - Mostly Stable")]
        SomewhatStable,

        [Display(Name = "IV - Unstable")]
        Unstable,

        [Display(Name = "V - Very Unstable")]
        VeryUnstable
        }
    }