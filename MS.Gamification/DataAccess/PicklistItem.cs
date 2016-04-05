// This file is part of the MS.Gamification project
// 
// File: PicklistItem.cs  Created: 2016-04-03@23:33
// Last modified: 2016-04-03@23:36 by Fern

namespace MS.Gamification.DataAccess
    {
    /// <summary>
    ///   Represents a key-value pair that can be used in a select (drop-down) field in a view
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class PickListItem<TKey>
        {
        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public PickListItem(TKey id, string displayName)
            {
            Id = id;
            DisplayName = displayName;
            }

        public TKey Id { get; }
        public string DisplayName { get; }
        }
    }
