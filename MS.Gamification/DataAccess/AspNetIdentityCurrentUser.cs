﻿// This file is part of the MS.Gamification project
// 
// File: AspNetIdentityCurrentUser.cs  Created: 2016-07-03@22:08
// Last modified: 2016-07-05@04:59

using System.Security.Principal;
using Microsoft.AspNet.Identity;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess
    {
    /// <summary>
    ///     A service for accessing details of the current user. In the case of a web application, the 'current' user
    ///     would typically be the user making the request.
    /// </summary>
    /// <remarks>
    ///     Note that a user may be 'current' without necessarily being logged in. The presence of a 'current user'
    ///     simply means the user has been identified and is no guarantee that they have been authenticated.
    ///     <see cref="IsAuthenticated" />.
    /// </remarks>
    public interface ICurrentUser
        {
        /// <summary>
        ///     Gets the display name of the user.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; }

        /// <summary>
        ///     Gets the login name of the user. This is typically what the user would enter in the login screen, but may be
        ///     something different.
        /// </summary>
        /// <value>The name of the login.</value>
        string LoginName { get; }

        /// <summary>
        ///     Gets the unique identifier of the user. Typically this is used as the Row ID in whatever store is used to persist
        ///     the user's details.
        /// </summary>
        /// <value>The unique identifier.</value>
        string UniqueId { get; }

        /// <summary>
        ///     Gets a value indicating whether the user has been authenticated.
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        bool IsAuthenticated { get; }
        }

    /// <summary>
    ///     Encapsulates the concept of a 'current user' based on ASP.Net Identity.
    /// </summary>
    /// <seealso cref="MS.Gamification.DataAccess.ICurrentUser" />
    public class AspNetIdentityCurrentUser : ICurrentUser
        {
        private readonly IIdentity identity;
        private readonly UserManager<ApplicationUser, string> manager;
        private ApplicationUser user;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AspNetIdentityCurrentUser" /> class.
        /// </summary>
        /// <param name="manager">The ASP.Net Identity User Manager.</param>
        /// <param name="identity">The identity as reported by the HTTP Context.</param>
        public AspNetIdentityCurrentUser(ApplicationUserManager manager, IIdentity identity)
            {
            this.manager = manager;
            this.identity = identity;
            }

        /// <summary>
        ///     Gets the display name of the user. This implementation returns the login name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName => identity.Name;

        /// <summary>
        ///     Gets the login name of the user.
        ///     something different.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName => identity.Name;

        /// <summary>
        ///     Gets the unique identifier of the user, which can be used to look the user up in a database.
        ///     the user's details.
        /// </summary>
        /// <value>The unique identifier.</value>
        public string UniqueId
            {
            get
                {
                if (user == null)
                    user = GetApplicationUser();
                return user.Id;
                }
            }

        /// <summary>
        ///     Gets a value indicating whether the user has been authenticated.
        /// </summary>
        /// <value><c>true</c> if the user is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated => identity.IsAuthenticated;

        private ApplicationUser GetApplicationUser()
            {
            return manager.FindByName(LoginName);
            }
        }
    }