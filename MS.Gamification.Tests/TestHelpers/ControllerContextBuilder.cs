// This file is part of the MS.Gamification project
// 
// File: ControllerContextBuilder.cs  Created: 2016-05-26@03:51
// Last modified: 2016-07-03@01:33

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Effort.Extra;
using Machine.Specifications;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers.Fakes;

namespace MS.Gamification.Tests.TestHelpers
    {
    /// <summary>
    ///     Builds an instance of an MVC <see cref="Controller" />, initialized with fke data suitable for unit testing.
    /// </summary>
    /// <typeparam name="TController">The type of the controller to be constructed.</typeparam>
    class ControllerContextBuilder<TController> where TController : ControllerBase
        {
        readonly FakeHttpContext blobby = new FakeHttpContext("/", "GET");
        readonly ObjectData data = new ObjectData(TableNamingStrategy.Pluralised);
        readonly TempDataDictionary tempdata = new TempDataDictionary();
        readonly EffortUnitOfWorkBuilder uowBuilder = new EffortUnitOfWorkBuilder();
        Uri baseUri = new Uri("http://localhost:9876");
        HttpVerbs requestMethod = HttpVerbs.Get;
        string requestPath = "/";
        string requestUsername = string.Empty;
        string[] requestUserRoles;

        public IUnitOfWork UnitOfWork { get; private set; }


        /// <summary>
        ///     Adds a user to the identity store and assigns the Moderator role.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <param name="username">The user's login name.</param>
        /// <returns>
        ///     A reference to this <see cref="ControllerContextBuilder{TController}" /> that may be used to fluently
        ///     chain operations.
        /// </returns>
        public ControllerContextBuilder<TController> WithModerator(string id, string username)
            {
            CreateUserInRoles(id, username, new[] {"Moderator"});
            return this;
            }

        void CreateUserInRoles(string id, string username, IEnumerable<string> roles)
            {
            var user = new ApplicationUser {Id = id, UserName = username, Email = $"{id}@nowhere.nw", EmailConfirmed = true};
            foreach (var role in roles)
                {
                user.Roles.Add(new IdentityUserRole {RoleId = role, UserId = id});
                }
            data.Table<ApplicationUser>("AspNetUsers").Add(user);
            }

        /// <summary>
        ///     Adds a standard user to the identity store.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <param name="username">The user's login name.</param>
        /// <returns>
        ///     A reference to this <see cref="ControllerContextBuilder{TController}" /> that may be used to fluently
        ///     chain operations.
        /// </returns>
        public ControllerContextBuilder<TController> WithStandardUser(string id, string username)
            {
            CreateUserInRoles(id, username, new string[] {});
            return this;
            }


        /// <summary>
        ///     Adds an entity to the test data context, inferring the table name from the entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity, which determines to which table it is added.</typeparam>
        /// <param name="entity">
        ///     The initialized entity, including any foreign key IDs (navigation properties should not
        ///     be populated).
        /// </param>
        /// <returns>
        ///     A reference to this <see cref="ControllerContextBuilder{TController}" /> that may be used to
        ///     fluently chain operations.
        /// </returns>
        public ControllerContextBuilder<TController> WithEntity<TEntity>(TEntity entity) where TEntity : class
            {
            data.Table<TEntity>().Add(entity);
            return this;
            }

        /// <summary>
        ///     Adds an entity to the test data context and explicitly specifies the table name.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity, which determines to which table it is added.</typeparam>
        /// <param name="entity">
        ///     The initialized entity, including any foreign key IDs (navigation properties should not
        ///     be populated).
        /// </param>
        /// <param name="tableName">Specifies the table name if it cannot be inferred from the entity type name.</param>
        /// <returns>
        ///     A reference to this <see cref="ControllerContextBuilder{TController}" /> that may be used to
        ///     fluently chain operations.
        /// </returns>
        public ControllerContextBuilder<TController> WithEntity<TEntity>(TEntity entity, string tableName) where TEntity : class
            {
            data.Table<TEntity>(tableName).Add(entity);
            return this;
            }

        public ControllerContextBuilder<TController> WithRoute(string relativeUrl, HttpVerbs method = HttpVerbs.Get)
            {
            requestPath = relativeUrl;
            requestMethod = method;
            return this;
            }

        /// <summary>
        ///     Builds a controller of the required type using any data previously supplied (or defaults).
        /// </summary>
        /// <returns>An initialized controller of type <typeparamref name="TController" />.</returns>
        /// <exception cref="SpecificationException">Thrown if the controller cannot be built.</exception>
        public TController Build()
            {
            var dataLoader = new ObjectDataLoader(data);
            UnitOfWork = uowBuilder.WithData(dataLoader).Build();
            var controller = Activator.CreateInstance(typeof(TController), UnitOfWork) as TController;
            if (controller == null)
                throw new SpecificationException(
                    $"ControllerContextBuilder: Unable to create controller instance of type {nameof(TController)}");
            var httpContext = new FakeHttpContext(requestPath, requestMethod.ToString("G"));
            var fakeIdentity = new FakeIdentity(requestUsername);
            var fakePrincipal = new FakePrincipal(fakeIdentity, requestUserRoles);
            httpContext.User = fakePrincipal;
            var context = new ControllerContext {HttpContext = httpContext};
            controller.ControllerContext = context;
            controller.TempData = tempdata;
            return controller;
            }

        public ControllerContextBuilder<TController> WithRequestingUser(string username, params string[] roles)
            {
            requestUsername = username;
            requestUserRoles = roles;
            return this;
            }

        public ControllerContextBuilder<TController> WithTempData(string key, object value)
            {
            tempdata.Add(key, value);
            return this;
            }
        }
    }