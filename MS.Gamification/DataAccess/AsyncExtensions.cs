// This file is part of the MS.Gamification project
// 
// File: AsyncExtensions.cs  Created: 2016-07-03@22:50
// Last modified: 2016-07-03@22:51

using System.Threading.Tasks;

namespace MS.Gamification.DataAccess
    {
    public static class AsyncExtensions
        {
        public static TResult WaitForResult<TResult>(this Task<TResult> task)
            {
            task.Wait();
            return task.Result;
            }
        }
    }