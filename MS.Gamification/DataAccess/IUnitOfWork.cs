// This file is part of the MS.Gamification project
// 
// File: IUnitOfWork.cs  Created: 2016-03-17@00:52
// Last modified: 2016-03-17@02:54 by Fern

using MS.Gamification.Models;

namespace MS.Gamification.DataAccess
    {
    public interface IUnitOfWork
        {
        IRepository<Challenge> Challenges { get; }
        IRepository<ApplicationUser> Users { get; }
        void Commit();
        void Cancel();
        }
    }
