// This file is part of the MS.Gamification project
// 
// File: PickListExtensions.cs  Created: 2016-04-04@00:13
// Last modified: 2016-04-04@01:48 by Fern

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    public static class PickListExtensions
        {
        public static IEnumerable<SelectListItem> ToSelectList<TKey>(this IEnumerable<PickListItem<TKey>> items)
            => items.Select(p => new SelectListItem {Value = p.Id.ToString(), Text = p.DisplayName});
        }
    }
