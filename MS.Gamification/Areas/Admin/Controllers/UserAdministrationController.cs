// This file is part of the MS.Gamification project
// 
// File: UserAdministrationController.cs  Created: 2016-08-20@23:12
// Last modified: 2016-11-01@19:22

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Areas.Admin.ViewModels;
using MS.Gamification.Areas.Admin.ViewModels.UserAdministration;
using MS.Gamification.DataAccess;
using MS.Gamification.EmailTemplates;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using NLog;
using RazorEngine.Templating;
using Constants = MS.Gamification.GameLogic.Constants;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    [Authorize]
    public class UserAdministrationController : RequiresAdministratorRights
        {
        private static readonly Regex SimpleEmailRegex = new Regex(Constants.RFC822EmailPattern, RegexOptions.ExplicitCapture);
        private static readonly Regex UserNameEmailCsvRegex = new Regex(Constants.UserNameAndEmailCsvPattern,
            RegexOptions.ExplicitCapture);
        private static readonly Regex UserNameEmailCanonicalRegex = new Regex(Constants.UserNameAndEmailCanonicalPattern,
            RegexOptions.ExplicitCapture);
        private readonly IGameEngineService gameEngine;
        private readonly ILogger log;
        private readonly IMapper mapper;
        private readonly IRazorEngineService razor;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork uow;
        private readonly ApplicationUserManager userManager;

        public UserAdministrationController(ApplicationUserManager userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork uow,
            IGameEngineService gameEngine,
            IRazorEngineService razor,
            IMapper mapper)
            {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.uow = uow;
            this.gameEngine = gameEngine;
            this.razor = razor;
            this.mapper = mapper;
            log = LogManager.GetCurrentClassLogger();
            }

        public ActionResult UserProfile()
            {
            return View();
            }

        public ActionResult Index()
            {
            return View();
            }

        public ActionResult CreateUserAccounts()
            {
            return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUserAccounts(BulkUserEntryViewModel bulkCreateModel)
            {
            var successfulEmails = new List<string>();
            var failedEmails = new Dictionary<string, string>();
            var sourceEmails = bulkCreateModel.UserNamesAndEmails.Split(new[] {'\r', '\n', ';'},
                StringSplitOptions.RemoveEmptyEntries);
            foreach (var emailAddress in sourceEmails)
                {
                var cleanedEmail = emailAddress.Trim();
                try
                    {
                    await CreateAndNotifyUser(cleanedEmail);
                    successfulEmails.Add(cleanedEmail);
                    }
                catch (ArgumentException ex)
                    {
                    failedEmails.Add(cleanedEmail, ex.Message);
                    }
                catch (Exception ex)
                    {
                    failedEmails.Add(cleanedEmail, $"Unexpected error: {ex.Message}");
                    }
                }
            var model = new CreateUsersConfirmationViewModel
                {
                FailedAddresses = failedEmails,
                SuccessfulAddresses = successfulEmails,
                FailedTotal = failedEmails.Count,
                SucceededTotal = successfulEmails.Count
                };
            return View("CreateUserAccountsConfirmation", model);
            }

        private async Task CreateAndNotifyUser(string emailAddress)
            {
            log.Info($"Provisioning user account for email address {emailAddress}");
            // The user is created with an empty password, which must be reset
            var user = CreateUserFromEmailString(emailAddress);
            var result = await userManager.CreateAsync(user /*, password*/);
            if (!result.Succeeded)
                {
                var builder = new StringBuilder($"Unable to provision user account for {emailAddress}:");
                foreach (var error in result.Errors)
                    {
                    builder.Append($"\n... {error}");
                    }
                log.Error(builder.ToString());
                throw new InvalidOperationException(builder.ToString());
                }
            await SendNotificationEmail(user.Id, emailAddress);
            }

        /// <summary>
        ///     Tries to recognize various user name and email formats and, if successful, creates a new
        ///     <see cref="ApplicationUser" /> from the parsed results.
        /// </summary>
        /// <param name="userNameAndEmail">
        ///     The input user name and email in one of the recognised formats:
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Format</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Simple RFC822 email address</term>
        ///             <description>
        ///                 <para>A simple email address in RFC-822 format.</para>
        ///                 <para>Example: <c><![CDATA[Joe@user.com]]></c></para>
        ///                 <para>
        ///                     Since there is no user name component in this format, the parsed value is used  as both
        ///                     the email address and the user name.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Canonical user name format</term>
        ///             <description>
        ///                 <para>A user name and email address specified in canonical format.</para>
        ///                 <para>Example: <c><![CDATA[Joe User<Joe@user.com>]]></c></para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Comma Separated Values (CSV) format</term>
        ///             <description>
        ///                 <para>A user name and email address separated with a comma.</para>
        ///                 <para>Example: <c><![CDATA[Joe User, Joe@user.com]]></c></para>
        ///             </description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <exception cref="ArgumentException">Not in any of the recognized formats.</exception>
        internal static ApplicationUser CreateUserFromEmailString(string userNameAndEmail)
            {
            var source = userNameAndEmail.Trim();
            // Try to match Canonical format "User Name<recipient@email.domain>"
            var canonicalMatch = UserNameEmailCanonicalRegex.Match(source);
            if (canonicalMatch.Success)
                return new ApplicationUser
                        {UserName = canonicalMatch.Groups["name"].Value, Email = canonicalMatch.Groups["email"].Value};
            // Try to match CSV format "Joe User, Joe@user.com"
            var csvMatch = UserNameEmailCsvRegex.Match(source);
            if (csvMatch.Success)
                return new ApplicationUser {UserName = csvMatch.Groups["name"].Value, Email = csvMatch.Groups["email"].Value};
            // Try to match a simple RFC822 email address and use it as the username and the email address.
            var rfc822Match = SimpleEmailRegex.Match(source);
            if (rfc822Match.Success)
                {
                var email = rfc822Match.Groups["email"].Value;
                return new ApplicationUser
                        {UserName = email, Email = email};
                }
            throw new ArgumentException("Not in any of the recognised formats.");
            }

        private async Task SendNotificationEmail(string userId, string email)
            {
            log.Info($"Sending invitation to user {userId}");
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userId);
            var emailModel = new VerificationTokenEmailModel
                {
                CallbackUrl =
                    Url.Action("ConfirmEmail", "UserAdministration", new {userId, code, area = string.Empty}, Request.Url.Scheme),
                InformationUrl = Url.Action("Index", "Home", new {area = string.Empty}, Request.Url.Scheme),
                VerificationToken = code,
                Recipient = email
                };
            var emailBody = razor.RunCompile("NewUserInvitation.cshtml", typeof(VerificationTokenEmailModel), emailModel);
            await userManager.SendEmailAsync(userId, "Invitation to Star Quest by Monkton Stargazers", emailBody);
            log.Info($"Successfully sent invitation email to user id {userId}");
            }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
            {
            if (userId == null || code == null)
                return View("Error");
            var result = await userManager.ConfirmEmailAsync(userId, code);
            if (!result.Succeeded)
                //throw new InvalidOperationException(result.Errors.FirstOrDefault());
                return View("TokenExpired");
            code = await userManager.GeneratePasswordResetTokenAsync(userId);
            return RedirectToAction("ResetPassword", "Account", new {code});
            }

        public ActionResult SimulateExpiredToken()
            {
            return View("TokenExpired");
            }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReplacementToken(ResendInvitationViewModel model)
            {
            var forgotModel = new ForgotViewModel {Email = model.Email};
            if (!ModelState.IsValid)
                {
                return View("TokenExpired", forgotModel);
                }
            try
                {
                var user = await userManager.FindByEmailAsync(model.Email);
                await SendNotificationEmail(user.Id, model.Email);
                return RedirectToAction("Index");
                }
            catch (Exception e)
                {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(forgotModel);
                }
            }

        public ActionResult ResendInvitation()
            {
            return View();
            }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ResendInvitation(ResendInvitationViewModel model)
            {
            if (!ModelState.IsValid)
                {
                return View(model);
                }
            try
                {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    throw new ArgumentException("No such user");
                await SendNotificationEmail(user.Id, model.Email);
                }
            catch (Exception e)
                {
                ModelState.AddModelError(nameof(model.Email), e.Message);
                return View(model);
                }
            return View("ReplacementToken");
            }


        public ActionResult ManageUsers()
            {
            var model = CreateManageUsersViewModel();
            return View(model);
            }

        private List<ManageUserViewModel> CreateManageUsersViewModel()
            {
            var query = from user in userManager.Users
                        select new ManageUserViewModel
                            {
                            Id = user.Id,
                            Email = user.Email,
                            Username = user.UserName,
                            AccountLocked = user.LockoutEnabled,
                            HasValidPassword = user.PasswordHash != null,
                            EmailVerified = user.EmailConfirmed
                            };
            return query.ToList();
            }

        public async Task<ActionResult> ManageUser(string id)
            {
            if (string.IsNullOrWhiteSpace(id))
                {
                ModelState.AddModelError("", "Invalid user ID");
                var model = CreateManageUsersViewModel();
                return View("ManageUsers", model);
                }
            try
                {
                var user = await userManager.FindByIdAsync(id);
                var roles = await userManager.GetRolesAsync(id);
                var model = mapper.Map<ApplicationUser, ManageUserViewModel>(user);
                model.Roles = roles;
                AddAvailableRolesToModel(model);
                return View(model);
                }
            catch (Exception ex)
                {
                ModelState.AddModelError("", ex);
                var model = CreateManageUsersViewModel();
                return View("ManageUsers", model);
                }
            }

        private void AddAvailableRolesToModel(ManageUserViewModel model)
            {
            var availableRoles = roleManager.Roles
                .Select(p => new PickListItem<string> {Id = p.Name, DisplayName = p.Name})
                .ToList();
            model.RolePicker = availableRoles.ToSelectList();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(ManageUserViewModel model)
            {
            try
                {
                if (model == null)
                    throw new InvalidOperationException("Invalid model");
                if (string.IsNullOrWhiteSpace(model.RoleToAdd))
                    throw new InvalidOperationException($"Invalid role '{model.RoleToAdd}'");
                var result = await userManager.AddToRoleAsync(model.Id, model.RoleToAdd);
                AddIdentityErrors(result);
                }
            catch (Exception ex)
                {
                ModelState.AddModelError("", ex);
                }
            return RedirectToAction("ManageUser", new {id = model.Id});
            }

        public async Task<ActionResult> RemoveRole(string id, string role)
            {
            var result = await userManager.RemoveFromRoleAsync(id, role);
            AddIdentityErrors(result);
            return RedirectToAction("ManageUser", new {id});
            }

        private void AddIdentityErrors(IdentityResult result)
            {
            if (result.Succeeded)
                return;
            foreach (var error in result.Errors)
                {
                ModelState.AddModelError("", error);
                }
            }

        public ActionResult BatchObservations()
            {
            var users = userManager.Users.Select(p => new BatchObservationUserViewModel
                {
                Selected = false,
                UserId = p.Id,
                UserName = p.UserName
                });
            var model = users.ToList();
            return View(model);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchObservations(List<BatchObservationUserViewModel> model)
            {
            var selectedUsers = model.Where(p => p.Selected).Select(s => s.UserId).ToList();
            var observationModel = new BatchObservationViewModel
                {
                Users = selectedUsers,
                // Assume sensible defaults for group observing
                ObservationDateTimeUtc = DateTime.UtcNow,
                Equipment = ObservingEquipment.Telescope,
                Notes = "Group observation entered by the administrator."
                };
            ViewBag.Message =
                $"Enter observation details and click Submit to create an observation for {selectedUsers.Count()} users.";

            PrepareBatchObservationViewModel(observationModel);
            return View("BatchObservationDetails", observationModel);
            }

        private BatchObservationViewModel PrepareBatchObservationViewModel(BatchObservationViewModel model)
            {
            model.ChallengePicker = uow.Challenges.PickList.ToSelectList();
            model.EquipmentPicker = PickListExtensions.FromEnum<ObservingEquipment>().ToSelectList();
            model.SeeingPicker = PickListExtensions.FromEnum<AntoniadiScale>().ToSelectList();
            model.TransparencyPicker = PickListExtensions.FromEnum<TransparencyLevel>().ToSelectList();
            return model;
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchObservationDetails(BatchObservationViewModel model)
            {
            var observation = mapper.Map<BatchObservationViewModel, Observation>(model);
            var results = gameEngine.BatchCreateObservations(observation, model.Users);
            PrepareBatchObservationViewModel(model);
            ViewData["Message"] =
                $"Successfully created {results.Succeeded}; Failed to create {results.Failed} observations.";

            foreach (var key in results.Errors.Keys)
                {
                ModelState.AddModelError(null, $"{key}: {results.Errors[key]}");
                }
            return View(model);
            }
        }
    }