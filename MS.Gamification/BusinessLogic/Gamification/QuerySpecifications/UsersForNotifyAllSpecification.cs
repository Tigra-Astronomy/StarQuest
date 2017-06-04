// This file is part of the MS.Gamification project
// 
// File: UsersForNotifyAllSpecification.cs  Created: 2017-06-04@03:07
// Last modified: 2017-06-04@03:13

using System.Linq;
using MS.Gamification.Models;
using JetBrains.Annotations;
using System.Diagnostics.Contracts;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
{
    /// <summary>
    /// Selects all users eligible to receive notification emails
    /// </summary>
    public class UsersForNotifyAllSpecification : QuerySpecification<ApplicationUser, string>
    {
        [ItemNotNull]
        [NotNull]
        public override IQueryable<string> GetQuery([ItemNotNull][NotNull] IQueryable<ApplicationUser> users)
        {
            Contract.Requires(users != null);
            Contract.Ensures(Contract.Result<IQueryable<string>>() != null);
            var query = from user in users
                        where user.EmailConfirmed
                        where !string.IsNullOrEmpty(user.PasswordHash)
                        where !string.IsNullOrEmpty(user.Email)
                        select user.Id;
            return query;
        }
    }
}