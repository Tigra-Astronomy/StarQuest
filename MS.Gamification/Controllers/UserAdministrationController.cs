// This file is part of the MS.Gamification project
// 
// File: UserAdministrationController.cs  Created: 2016-06-07@02:20
// Last modified: 2016-07-17@10:11

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MS.Gamification.EmailTemplates;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using RazorEngine.Templating;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public class UserAdministrationController : RequiresAdministratorRights
        {
        private readonly IRazorEngineService razor;
        private readonly ApplicationUserManager userManager;

        public UserAdministrationController(ApplicationUserManager userManager, IRazorEngineService razor)
            {
            this.userManager = userManager;
            this.razor = razor;
            }

        public ActionResult UserProfile()
            {
            return View();
            }

        public ActionResult AdminDashboard()
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
            var sourceEmails = emails.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
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
            var user = new ApplicationUser {UserName = emailAddress, Email = emailAddress};
            var result = await userManager.CreateAsync(user);
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
                InformationUrl = Url.Action("Index", "Home"),
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
                throw new InvalidOperationException(result.Errors.FirstOrDefault());
            var token = await userManager.GeneratePasswordResetTokenAsync(userId); // Can't re-use a token
            code = HttpUtility.UrlEncode(token);
            return RedirectToAction("ResetPassword", "Account", new {code});
            }
        }
    }