// This file is part of the MS.Gamification project
// 
// File: ValidationImageControllerSpecs.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-16@01:33

using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.Controllers
    {
    /*
     * Validation Image Controller, ValidationImage action
     * 
     * When called with no parameter,
     * + It should return the NoImage.png placeholder image
     * 
     * When called with a well-formed filename and the file does not exist on disk
     * + It should return the NoImage.png placeholder image
     * 
     * When called with a well-formed filename and the image exists on disk
     * + It should return the existing image from disk
     * + It should set the mime type based on the image file extension
     * 
     * When called with a malformed filename
     * + It should return the NoImage.png placeholder
     */

    class with_fake_web_server
        {
        protected const string RootPath = @"C:\UnitTestFakePath";
        protected static ValidationImageController controller;
        protected static FilePathResult result;
        protected static UnitTestImageStore FakeImageStore;

        Cleanup after = () =>
            {
            controller = null;
            FakeImageStore = null;
            result = null;
            };

        Establish context = () =>
            {
            FakeImageStore = new UnitTestImageStore(RootPath);
            controller = new ValidationImageController(FakeImageStore);
            };
        }

    [Subject(typeof(ValidationImageController), "ValidationImage")]
    class when_called_with_a_null_identifier : with_fake_web_server
        {
        Because of = () => result = controller.GetImage(null) as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldContain(Challenge.NoImagePlaceholder);
        }

    [Subject(typeof(ValidationImageController), "ValidationImage")]
    class when_called_with_a_well_formed_id_which_does_not_exist_in_the_store : with_fake_web_server
        {
        Because of = () => result = controller.GetImage("doesnotexist") as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldContain(Challenge.NoImagePlaceholder);
        }

    [Subject(typeof(ValidationImageController), "ValidationImage")]
    class when_called_with_a_well_formed_identifier_which_exists_in_the_store : with_fake_web_server
        {
        Establish context = () => FakeImageStore["unittest"] = UnittestPng;
        Because of = () => result = controller.GetImage("unittest") as FilePathResult;
        It should_return_the_image = () => result.FileName.ShouldEndWith(UnittestPng);
        It should_set_the_correct_mime_type_for_the_image_file = () => result.ContentType.ShouldEqual("image/png");
        const string UnittestPng = "unittest.png";
        }
    }
