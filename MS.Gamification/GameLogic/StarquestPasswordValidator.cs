// This file is part of the MS.Gamification project
// 
// File: StarquestPasswordValidator.cs  Created: 2016-06-05@20:09
// Last modified: 2016-06-05@21:09

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MS.Gamification.GameLogic
    {
    public class StarquestPasswordValidator : IIdentityValidator<string>
        {
        const string DigitPattern = @"\p{Nd}";
        const string UpperCasePattern = @"\p{Ll}";
        const string LowerCasePattern = @"\p{Lu}";
        const string PunctuationPattern = @"[~`!@#£$€%^&*()-_=+\[\]{}\\|;:'"",.<>/?]";
        readonly Regex digitRegex = new Regex(DigitPattern);
        readonly Regex lowercaseRegex = new Regex(LowerCasePattern);
        readonly Regex punctuationRegex = new Regex(PunctuationPattern);
        readonly Regex uppercaseRegex = new Regex(UpperCasePattern);

        /// <summary>Minimum required length</summary>
        public int RequiredLength { get; set; } = 8;

        /// <summary>
        ///     The number of complexity factors that must be satisfied
        /// </summary>
        public int RequiredComplexityFactors { get; set; } = 3;

        /// <summary>
        ///     Ensures that the string is of the required length and meets the configured requirements
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual Task<IdentityResult> ValidateAsync(string item)
            {
            if (item == null)
                throw new ArgumentNullException("item");
            var validationErrors = new List<string>();
            if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
                validationErrors.Add($"Password is too short; requires a minimum of {RequiredLength} characters");
            var hasUppercase = uppercaseRegex.IsMatch(item);
            var hasLowerCase = lowercaseRegex.IsMatch(item);
            var hasDigit = digitRegex.IsMatch(item);
            var hasPunctuation = punctuationRegex.IsMatch(item);
            var complexityFactors = new List<bool> {hasUppercase, hasLowerCase, hasDigit, hasPunctuation};
            var complexityFactorsSatisfied = complexityFactors.Count(p => p);
            if (complexityFactorsSatisfied < RequiredComplexityFactors)
                validationErrors.Add(
                    $"Does not meet complexity requirements. Must have {RequiredComplexityFactors} out of 4: upper case, lower case, digits, punctuation");
            if (validationErrors.Any())
                return Task.FromResult(IdentityResult.Failed(string.Join(" ", validationErrors)));
            return Task.FromResult(IdentityResult.Success);
            }
        }
    }