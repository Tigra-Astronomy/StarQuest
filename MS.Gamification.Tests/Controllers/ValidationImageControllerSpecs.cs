// This file is part of the MS.Gamification project
// 
// File: ValidationImageControllerSpecs.cs  Created: 2016-05-11@23:16
// Last modified: 2016-05-12@00:25

using System.IO;
using System.Web;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.HtmlHelpers;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.Controllers
    {
    /*
     * Validation Image Controller, GetImage action
     * 
     * When called with no parameter,
     * + It should return the NoImage.png placeholder image
     * 
     * When called with a well-formed filename and the file does not exist on disk
     * + It should return the NoImage.png placeholder image
     * 
     * When called with a well-formed filename and the image exists on disk
     * + It should return the existing image from disk
     * - It should set the mime type based on the image file extension
     * 
     * When called with a malformed filename
     * - It should return the NoImage.png placeholder
     */

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_no_filename
        {
        Establish context = () =>
            {
            var fakeServer = A.Fake<HttpServerUtilityBase>();
            A.CallTo(() => fakeServer.MapPath(A<string>.Ignored)).Returns(@"C:\UnitTestFakePath");
            var fakeFilesystem = A.Fake<IImageStore>();
            A.CallTo(() => fakeFilesystem.FileExists(A<string>.Ignored)).Returns(true);
            controller = new ValidationImageController(fakeServer, fakeFilesystem);
            };
        Because of = () => result = controller.GetImage(null) as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldEndWith(Challenge.NoImagePlaceholder);
        static ValidationImageController controller;
        static FilePathResult result;
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_a_well_formed_filename_which_does_not_exist
        {
        Establish context = () =>
            {
            var fakeServer = A.Fake<HttpServerUtilityBase>();
            A.CallTo(() => fakeServer.MapPath(A<string>.Ignored)).Returns(@"C:\UnitTestFakePath");
            var fakeFilesystem = A.Fake<IImageStore>();
            A.CallTo(() => fakeFilesystem.FileExists(A<string>.Ignored)).Returns(false);
            controller = new ValidationImageController(fakeServer, fakeFilesystem);
            };
        Because of = () => result = controller.GetImage("unittest.png") as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldEndWith(Challenge.NoImagePlaceholder);
        static ValidationImageController controller;
        static FilePathResult result;
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_a_well_formed_filename_which_exists
        {
        const string UnittestPng = "unittest.png";
        Establish context = () =>
        {
            var fakeServer = A.Fake<HttpServerUtilityBase>();
            A.CallTo(() => fakeServer.MapPath(A<string>.Ignored)).Returns(@"C:\UnitTestFakePath");
            var fakeFilesystem = A.Fake<IImageStore>();
            A.CallTo(() => fakeFilesystem.FileExists(A<string>.Ignored)).Returns(true);
            controller = new ValidationImageController(fakeServer, fakeFilesystem);
        };
        Because of = () => result = controller.GetImage(UnittestPng) as FilePathResult;
        It should_return_the_image = () => result.FileName.ShouldEndWith(UnittestPng);
        It should_set_the_correct_mime_type_for_the_image_file = () => result.ContentType.ShouldEqual("image/png");
        static ValidationImageController controller;
        static FilePathResult result;
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_a_malformed_filename
        {
        Establish context = () =>
        {
            var fakeServer = A.Fake<HttpServerUtilityBase>();
            A.CallTo(() => fakeServer.MapPath(A<string>.Ignored)).Returns(@"C:\UnitTestFakePath");
            var fakeFilesystem = A.Fake<IImageStore>();
            A.CallTo(() => fakeFilesystem.FileExists(A<string>.Ignored)).Returns(false);
            controller = new ValidationImageController(fakeServer, fakeFilesystem);
        };
        Because of = () => result = controller.GetImage(string.Empty) as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldEndWith(Challenge.NoImagePlaceholder);
        static ValidationImageController controller;
        static FilePathResult result;
        }

    }