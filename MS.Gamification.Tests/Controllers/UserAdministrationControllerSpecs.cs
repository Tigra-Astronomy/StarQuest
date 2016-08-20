// This file is part of the MS.Gamification project
// 
// File: UserAdministrationControllerSpecs.cs  Created: 2016-08-20@17:43
// Last modified: 2016-08-20@18:55

using System;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.Controllers;
using MS.Gamification.Models;
using static MS.Gamification.Areas.Admin.Controllers.UserAdministrationController;

namespace MS.Gamification.Tests.Controllers
    {
    class with_user_admin_controller
        {
        protected static ApplicationUser User;
        protected static Exception Exception;
        Cleanup after = () =>
            {
            User = null;
            Exception = null;
            };
        }

    [Subject(typeof(UserAdministrationController), "Parse Username and Email")]
    class when_creating_a_user_from_an_invalid_email_string : with_user_admin_controller
        {
        Because of = () => Exception = Catch.Exception(() => User = CreateUserFromEmailString("invalid"));
        It should_reject_invalid_input = () => Exception.ShouldNotBeNull();
        }

    [Subject(typeof(UserAdministrationController), "Parse Username and Email")]
    class when_creating_a_user_from_a_simple_valid_email_address : with_user_admin_controller
        {
        Because of = () => Exception = Catch.Exception(() => User = CreateUserFromEmailString(" Joe@user.com "));
        It should_recognise_the_username = () => User.UserName.ShouldEqual("Joe@user.com");
        It should_recognise_the_email = () => User.Email.ShouldEqual("Joe@user.com");
        }

    [Subject(typeof(UserAdministrationController), "Parse Username and Email")]
    class when_creating_a_user_from_valid_comma_separated_values : with_user_admin_controller
        {
        Because of = () => Exception = Catch.Exception(() => User = CreateUserFromEmailString("Joe User, Joe@user.com"));
        It should_recognise_the_username = () => User.UserName.ShouldEqual("Joe User");
        It should_recognise_the_email = () => User.Email.ShouldEqual("Joe@user.com");
        }

    [Subject(typeof(UserAdministrationController), "Parse Username and Email")]
    class when_creating_a_user_from_valid_comma_separated_values_with_spurious_white_space : with_user_admin_controller
        {
        Because of = () => Exception = Catch.Exception(() => User = CreateUserFromEmailString("Joe User   ,   Joe@user.com"));
        It should_recognise_the_username = () => User.UserName.ShouldEqual("Joe User");
        It should_recognise_the_email = () => User.Email.ShouldEqual("Joe@user.com");
        }

    [Subject(typeof(UserAdministrationController), "Parse Username and Email")]
    class when_creating_a_user_from_valid_email_string_in_canonical_form : with_user_admin_controller
        {
        Because of = () => Exception = Catch.Exception(() => User = CreateUserFromEmailString("Joe User<Joe@user.com>"));
        It should_recognise_the_username = () => User.UserName.ShouldEqual("Joe User");
        It should_recognise_the_email = () => User.Email.ShouldEqual("Joe@user.com");
        }

    [Subject(typeof(UserAdministrationController), "Parse Username and Email")]
    class when_creating_a_user_from_valid_email_string_in_canonical_form_with_spurious_white_space : with_user_admin_controller
        {
        Because of = () => Exception = Catch.Exception(() => User = CreateUserFromEmailString(" Joe User < Joe@user.com > "));
        It should_recognise_the_username = () => User.UserName.ShouldEqual("Joe User");
        It should_recognise_the_email = () => User.Email.ShouldEqual("Joe@user.com");
        }
    }