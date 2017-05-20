// This file is part of the MS.Gamification project
// 
// File: UnitTestWorkItem.cs  Created: 2017-05-20@01:14
// Last modified: 2017-05-20@01:47

using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    /// <summary>
    ///     A degenerate work item used for unit testing.
    /// </summary>
    /// <seealso cref="MS.Gamification.Models.QueuedWorkItem" />
    /// <remarks>
    ///     This class really belongs in the unit test project, but is here for subtle reasons related to how
    ///     Entity Framework discovers class hierarchies. Note that because this class inherits from
    ///     an abstract model class, any changes to this class will result in a change to the database schema.
    /// </remarks>
    public sealed class UnitTestWorkItem : QueuedWorkItem
        {
        public UnitTestWorkItem()
            {
            QueueName = "Test";
            }
        }
    }