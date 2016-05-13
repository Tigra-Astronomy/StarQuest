// This file is part of the MS.Gamification project
// 
// File: ValidationImageControllerSpecs.cs  Created: 2016-05-11@23:16
// Last modified: 2016-05-13@17:34

using System.Collections.Generic;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
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
     * + It should set the mime type based on the image file extension
     * 
     * When called with a malformed filename
     * + It should return the NoImage.png placeholder
     */

    class with_fake_web_server
        {
        protected const string RootPath = @"C:\UnitTestFakePath";
        protected static ValidationImageController controller;
        protected static List<string> validFiles;
        protected static FilePathResult result;
        Cleanup after = () =>
            {
            controller = null;
            validFiles = null;
            result = null;
            };
        Establish context = () => { validFiles = new List<string> {Challenge.NoImagePlaceholder}; };
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_no_filename : with_fake_web_server
        {
        Establish context = () =>
            {
            var fakeFilesystem = new UnitTestImageStore(RootPath, new List<string>());
            controller = new ValidationImageController(fakeFilesystem);
            };
        Because of = () => result = controller.GetImage(null) as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldEndWith(Challenge.NoImagePlaceholder);
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_a_well_formed_filename_which_does_not_exist : with_fake_web_server
        {
        Establish context = () =>
            {
            var fakeFilesystem = new UnitTestImageStore(RootPath, validFiles);
            controller = new ValidationImageController(fakeFilesystem);
            };
        Because of = () => result = controller.GetImage("unittest.png") as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldEndWith(Challenge.NoImagePlaceholder);
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_a_well_formed_filename_which_exists : with_fake_web_server
        {
        Establish context = () =>
            {
            validFiles.Add(UnittestPng);
            var fakeFilesystem = new UnitTestImageStore(RootPath, validFiles);
            controller = new ValidationImageController(fakeFilesystem);
            };
        Because of = () => result = controller.GetImage(UnittestPng) as FilePathResult;
        It should_return_the_image = () => result.FileName.ShouldEndWith(UnittestPng);
        It should_set_the_correct_mime_type_for_the_image_file = () => result.ContentType.ShouldEqual("image/png");
        const string UnittestPng = "unittest.png";
        }

    [Subject(typeof(ValidationImageController), "GetImage")]
    class when_called_with_a_malformed_filename : with_fake_web_server
        {
        Establish context = () =>
            {
            var fakeFilesystem = new UnitTestImageStore(RootPath, validFiles);
            controller = new ValidationImageController(fakeFilesystem);
            };
        Because of = () => result = controller.GetImage(string.Empty) as FilePathResult;
        It should_return_the_placeholder_image = () => result.FileName.ShouldEndWith(Challenge.NoImagePlaceholder);
        }
    }