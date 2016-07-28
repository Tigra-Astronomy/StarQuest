// This file is part of the MS.Gamification project
// 
// File: AccountController.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-22@14:43

using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MS.Gamification.EmailTemplates;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using RazorEngine.Templating;
using Constants = MS.Gamification.GameLogic.Constants;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public class AccountController : Controller
        {
        private readonly IAuthenticationManager authManager;
        private readonly IRazorEngineService razorEngine;
        private readonly ApplicationSignInManager signInManager;
        private readonly ApplicationUserManager userManager;

        public AccountController(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IAuthenticationManager authManager,
            IRazorEngineService razorEngine)
            {
            this.authManager = authManager;
            this.razorEngine = razorEngine;
            this.userManager = userManager;
            this.signInManager = signInManager;
            }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
            {
            ViewBag.ReturnUrl = returnUrl;
            return View();
            }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
            {
            if (!ModelState.IsValid)
                return View(model);

            ApplicationUser userDetails;
            try
                {
                userDetails = FindUserByNameOrEmail(model.UserName);
                }
            catch (InvalidOperationException)
                {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
                }

            if (!userDetails.EmailConfirmed)
                {
                return View("PendingEmailConfirmation", new ResendVerificationEmailViewModel {UserId = userDetails.Id});
                }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await signInManager.PasswordSignInAsync(userDetails.UserName, model.Password, model.RememberMe, true);
            switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, model.RememberMe});
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }

        /// <summary>
        ///     Finds the user by name or email.
        /// </summary>
        /// <param name="userNameOrEmail">The user name or email.</param>
        /// <returns>The <see cref="ApplicationUser" />, if found.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there was not exactly one user found.</exception>
        private ApplicationUser FindUserByNameOrEmail(string userNameOrEmail)
            {
            /*
            1. Decide if we have a user name or an email address.
            2. If it's a username, just log in as normal.
            3. If it's an email, look the username up in the database, then log in with the username.
            */
            var signInName = userNameOrEmail;
            var emailRegex = new Regex(Constants.emailPattern);
            var isEmail = emailRegex.IsMatch(userNameOrEmail);
            if (isEmail)
                {
                var query = from user in userManager.Users
                            where user.Email == userNameOrEmail
                            where user.EmailConfirmed
                            select user.UserName;
                if (query.Any())
                    {
                    signInName = query.First();
                    }
                }

            // When we get here, signInName must contain the username, not the email.
            // Users who have not verified their email address are not allowed to log in.
            var userDetails = userManager.Users.Single(p => p.UserName == signInName);
            return userDetails;
            }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
            {
            // Require that the user has already logged in via username/password or external login
            if (!await signInManager.HasBeenVerifiedAsync())
                return View("Error");
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
            }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
            {
            if (!ModelState.IsValid)
                return View(model);

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result =
                await
                    signInManager.TwoFactorSignInAsync(
                        model.Provider,
                        model.Code,
                        model.RememberMe,
                        model.RememberBrowser);
            switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                }
            }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
            {
            return View();
            }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
            {
            if (ModelState.IsValid)
                {
                var user = new ApplicationUser {UserName = model.UserName, Email = model.Email};
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    {
                    await SendVerificationEmail(user.Id, model.Email);
                    return RedirectToAction("RegistrationConfirmed", "Account");
                    }
                AddErrors(result);
                }

            // If we got this far, something failed, redisplay form
            return View(model);
            }

        private async Task SendVerificationEmail(string userId, string email)
            {
            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            var code = await userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId, code}, Request.Url.Scheme);
            var emailBody = RenderTokenVerificationEmail("EmailVerificationRequest.cshtml", callbackUrl, code, email);
            await userManager.SendEmailAsync(userId, "Star Quest: Confirm your email address", emailBody);
            }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
            {
            if (userId == null || code == null)
                return View("Error");
            var result = await userManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
            {
            return View();
            }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
            {
            if (ModelState.IsValid)
                {
                var user = FindUserByNameOrEmail(model.Email);
                var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user.Id);
                if (user == null || !isEmailConfirmed)
                    {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                    }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code}, Request.Url.Scheme);
                var emailBody = RenderTokenVerificationEmail("ResetPassword.cshtml", callbackUrl, code, user.Email);
                await userManager.SendEmailAsync(user.Id, "Star Quest: Reset password", emailBody);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

            // If we got this far, something failed, redisplay form
            return View(model);
            }

        private string RenderTokenVerificationEmail(string template, string callbackUrl, string code, string email)
            {
            var emailModel = new VerificationTokenEmailModel
                {
                CallbackUrl = callbackUrl,
                InformationUrl = Url.Action("Index", "Home", new {}, Request.Url.Scheme),
                VerificationToken = code,
                Recipient = email
                };
            var emailBody = razorEngine.RunCompile(template, typeof(VerificationTokenEmailModel), emailModel);
            return emailBody;
            }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
            {
            return View();
            }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
            {
            return code == null ? View("Error") : View();
            }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
            {
            if (!ModelState.IsValid)
                return View(model);
            ApplicationUser user;
            try
                {
                user = FindUserByNameOrEmail(model.Email);
                }
            catch (InvalidOperationException) // Did not find exactly one user.
                {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
            var result = await userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            AddErrors(result);
            return View();
            }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
            {
            return View();
            }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
            {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
            }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
            {
            var userId =
                await signInManager.GetVerifiedUserIdAsync();
            if (userId == null)
                return View("Error");
            var userFactors = await userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
            }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
            {
            if (!ModelState.IsValid)
                return View();

            // Generate the token and send it
            if (!await signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                return View("Error");
            return RedirectToAction("VerifyCode",
                new {Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe});
            }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
            {
            var loginInfo = await authManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
                return RedirectToAction("Login");

            // Sign in the user with this external login provider if the user already has a login
            var result = await signInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new {ReturnUrl = returnUrl, RememberMe = false});
                    case SignInStatus.Failure:
                    default:
                        // If the user does not have an account, then prompt the user to create an account
                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        return View("ExternalLoginConfirmation",
                            new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
                }
            }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
            {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Manage");

            if (ModelState.IsValid)
                {
                // Get the information about the user from the external login provider
                var info = await authManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return View("ExternalLoginFailure");
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                    {
                    result = await userManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                        {
                        await signInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                        }
                    }
                AddErrors(result);
                }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
            }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
            {
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
            }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult RegistrationConfirmed()
            {
            return View();
            }

        [AllowAnonymous]
        public async Task<ActionResult> ResendVerificationEmail(string id)
            {
            if (string.IsNullOrWhiteSpace(id))
                {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            await SendVerificationEmail(id, string.Empty);
            return View("RegistrationConfirmed");
            }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
            {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
            }

        private ActionResult RedirectToLocal(string returnUrl)
            {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
            }

        internal class ChallengeResult : HttpUnauthorizedResult
            {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null) {}

            public ChallengeResult(string provider, string redirectUri, string userId)
                {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
                }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
                {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                    properties.Dictionary[XsrfKey] = UserId;
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
                }
            }
        #endregion
        }
    }