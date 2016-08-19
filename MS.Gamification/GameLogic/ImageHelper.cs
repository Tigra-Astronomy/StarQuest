using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace MS.Gamification.GameLogic
    {
    public static class ImageHelper
        {
        private const string ImageIdentifierAllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWZYZabcdefghijklmnopqrstuvwxyz0123456789-";

        public static string ToImageIdentifier(this string fileName)
            {
            var result = Path.GetFileNameWithoutExtension(fileName);
            result = result.ToLower(CultureInfo.InvariantCulture);
            result = result.Keep(ImageIdentifierAllowedCharacters, '-');
            return result;
            }

        }
    }