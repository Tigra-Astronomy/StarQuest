// This file is part of the MS.Gamification project
// 
// File: IdentityConfig.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-24@15:42

using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using NLog;
using SendGrid;

namespace MS.Gamification
    {
    public class EmailService : IIdentityMessageService
        {
        private readonly Logger log = LogManager.GetLogger("mail");

        public Task SendAsync(IdentityMessage message)
            {
            // Plug in your email service here to send an email.
            return configSendGridasync(message);
            }

        private Task configSendGridasync(IdentityMessage message)
            {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            var fromAddress = ConfigurationManager.AppSettings["mailFromAddress"];
            var fromName = ConfigurationManager.AppSettings["mailFromName"];
            myMessage.From = new MailAddress(fromAddress, fromName);
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                ConfigurationManager.AppSettings["mailAccount"],
                ConfigurationManager.AppSettings["mailPassword"]
                );

            // Create a Web transport for sending email.
            try
                {
                log.Info($"Sending registration confirmation mail to {message.Destination}:\n{message.Body}");
                var transportWeb = new Web(credentials);
                return transportWeb.DeliverAsync(myMessage);
                }
            catch (Exception ex)
                {
                log.Error(ex, $"Failed to send message to: {message.Destination}");
                return Task.FromResult(0);
                }
            }
        }

    public class SmsService : IIdentityMessageService
        {
        public Task SendAsync(IdentityMessage message)
            {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
            }
        }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
        {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IDataProtectionProvider dataProtectionProvider)
            : base(store)
            {
            //var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
                {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
                };

            // Configure validation logic for passwords
            PasswordValidator = new StarquestPasswordValidator
                {
                RequiredLength = 8, RequiredComplexityFactors = 3
                };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
                {
                MessageFormat = "Your security code is {0}"
                });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
                {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
                });
            EmailService = new EmailService();
            SmsService = new SmsService();
            if (dataProtectionProvider != null)
                {
                UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                }
            }
        }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
        {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
            {
            return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
            }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
            {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
            }
        }
    }