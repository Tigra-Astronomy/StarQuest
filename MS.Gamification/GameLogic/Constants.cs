// This file is part of the MS.Gamification project
// 
// File: Constants.cs  Created: 2016-07-18@16:18
// Last modified: 2016-08-20@19:05

namespace MS.Gamification.GameLogic
    {
    public static class Constants
        {
        /// <summary>
        ///     RFC822-compatible email pattern, taken from http://regexlib.com/REDetails.aspx?regexp_id=26
        /// </summary>
        public const string RFC822EmailPattern =
            @"^(?<email>([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))$";
        public const string UserNameAndEmailCanonicalPattern =
            @"(?<name>\w+(\s+\w+))\s*<\s*(?<email>([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))\s*>\s*";
        public const string UserNameAndEmailCsvPattern =
            @"(?<name>\w+(\s+\w+))\s*,\s*(?<email>([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?))\s*";
        }
    }