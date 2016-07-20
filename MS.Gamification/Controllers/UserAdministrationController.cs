// This file is part of the MS.Gamification project
// 
// File: UserAdministrationController.cs  Created: 2016-07-18@03:02
// Last modified: 2016-07-20@01:37

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.DataAccess;
using MS.Gamification.EmailTemplates;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using RazorEngine.Templating;
using Constants = MS.Gamification.GameLogic.Constants;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public class UserAdministrationController : RequiresAdministratorRights
        {
        private readonly IMapper mapper;
        private readonly IRazorEngineService razor;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationUserManager userManager;

        public UserAdministrationController(ApplicationUserManager userManager,
            RoleManager<IdentityRole> roleManager,
            IRazorEngineService razor,
            IMapper mapper)
            {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.razor = razor;
            this.mapper = mapper;
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
        public async Task<ActionResult> CreateUserAccounts(string emails)
            {
            var emailRegex = new Regex(Constants.emailPattern);
            var wellFormedEmails = new List<string>();
            var successfulEmails = new List<string>();
            var failedEmails = new Dictionary<string, string>();
            var sourceEmails = emails.Split(new[] {'\r', '\n', ' ', ';', ','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var candidate in sourceEmails)
                {
                var isValid = emailRegex.IsMatch(candidate);
                if (isValid)
                    {
                    wellFormedEmails.Add(candidate);
                    }
                else
                    {
                    failedEmails.Add(candidate, "Contains invalid characters per RFC-822");
                    }
                }

            foreach (var emailAddress in wellFormedEmails)
                {
                try
                    {
                    await CreateAndNotifyUser(emailAddress);
                    successfulEmails.Add(emailAddress);
                    }
                catch (Exception ex)
                    {
                    failedEmails.Add(emailAddress, $"Exception: {ex.Message}");
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
            // The user is created with an 'un-utterable' password, which must be reset
            var user = new ApplicationUser {UserName = emailAddress, Email = emailAddress};
            var password = $"Aa1@#{new Guid()}";
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(result.Errors.First());
            await SendNotificationEmail(user.Id);
            }

        private async Task SendNotificationEmail(string userId)
            {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userId);
            var emailModel = new VerificationTokenEmailModel
                {
                ApplicationName = "Star Quest",
                CallbackUrl = Url.Action("ConfirmEmail", "UserAdministration", new {userId, code}, Request.Url.Scheme),
                InformationUrl = Url.Action("Index", "Home", new {}, Request.Url.Scheme),
                VerificationToken = code
                };
            var emailBody = razor.RunCompile("NewUserInvitation.cshtml", typeof(VerificationTokenEmailModel), emailModel);
            await userManager.SendEmailAsync(userId, "Invitation to Star Quest by Monkton Stargazers", emailBody);
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
        public async Task<ActionResult> ReplacementToken(ForgotViewModel model)
            {
            if (!ModelState.IsValid)
                {
                return View("TokenExpired", model);
                }
            try
                {
                var user = await userManager.FindByEmailAsync(model.Email);
                await SendNotificationEmail(user.Id);
                }
            catch (Exception e)
                {
                Console.WriteLine(e);
                }
            return View();
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
                AddAvailableRolesToViewData();
                return View(model);
                }
            catch (Exception ex)
                {
                ModelState.AddModelError("", ex);
                var model = CreateManageUsersViewModel();
                return View("ManageUsers", model);
                }
            }

        private void AddAvailableRolesToViewData()
            {
            var availableRoles = roleManager.Roles
                .Select(p => new PickListItem<string> {Id = p.Name, DisplayName = p.Name})
                .ToList();
            var rolePicker = availableRoles.ToSelectList();
            ViewData["Roles"] = rolePicker;
            }

        [HttpPost]
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
        }
    }
