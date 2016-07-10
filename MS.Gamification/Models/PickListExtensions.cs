// This file is part of the MS.Gamification project
// 
// File: PickListExtensions.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-10@02:08

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public static class PickListExtensions
        {
        public static IEnumerable<SelectListItem> ToSelectList<TKey>(this IEnumerable<PickListItem<TKey>> items)
            => items.Select(p => new SelectListItem {Value = p.Id.ToString(), Text = p.DisplayName});

        /// <exception cref="InvalidOperationException">Only valid on enum types</exception>
        public static IEnumerable<PickListItem<int>> FromEnum<TEnum>() where TEnum : struct
            {
            var sourceType = typeof(TEnum);
            if (!sourceType.IsEnum)
                throw new InvalidOperationException("Only valid on enum types");
            var names = Enum.GetNames(sourceType);
            var values = Enum.GetValues(sourceType);
            var output = new List<PickListItem<int>>();
            for (var i = 0; i < names.Length; i++)
                {
                var element = names[i];
                var value = (int) values.GetValue(i);
                try
                    {
                    // Try to get a display name for the element
                    var memInfo = sourceType.GetMember(element);
                    var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                    var displayName = ((DisplayAttribute) attributes[0]).Name;
                    var item = new PickListItem<int>(value, displayName);
                    output.Add(item);
                    }
                catch
                    {
                    // Otherwise, use the name 'as-is'.
                    var item = new PickListItem<int>(value, element);
                    output.Add(item);
                    }
                }
            return output;
            }

        public static string RenderDisplayName(this Enum e)
            {
            var memberName = e.ToString();
            var type = e.GetType();
            var memberInfo = type.GetMember(memberName); // Can be null
            if (memberInfo.Length < 1)
                return memberName;
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length < 1)
                return memberName;
            var displayName = (attributes[0] as DisplayAttribute)?.Name ?? memberName;
            return displayName;
            }
        }
    }