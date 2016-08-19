// This file is part of the MS.Gamification project
// 
// File: BadgesControllerSpecs.cs  Created: 2016-08-14@18:53
// Last modified: 2016-08-18@00:46

using System.Linq;
using System.Web;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.Controllers;
using MS.Gamification.GameLogic;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.Controllers
    {
    /*
     * Bahaviours for the Badges Controller
     * 
     * Concern: image identifier
     * When creating an image identifier from filename
     * + It should replace non-alphanumeric characters with hyphens
     * + It should convert to all lowercase
     * + It should remove any file extension
     * 
     * Concern: Save to Disk
     * When uploading a valid badge image
     * - It should persist the image to storage using the image identifier
     * - It should return the image identifier as the HTTP response
     */

    [Subject(typeof(BadgesController), "Upload")]
    class when_uploading_a_new_badge_image : with_standard_mission<BadgesController>
        {
        Establish context = () =>
            {
            var fakePostedFile = A.Fake<HttpPostedFileBase>();
            A.CallTo(() => fakePostedFile.FileName).Returns("Unit+Test.png");
            store = new UnitTestImageStore(@"C:\Test");
            ControllerUnderTest = ContextBuilder
                .WithImageStore(store)
                .WithPostedFile(fakePostedFile)
                .Build();
            };
        Because of = () => ControllerUnderTest.Upload() /*.Await()*/;
        It should_persist_the_image = () => store.SaveCalled.ShouldBeTrue();
        It should_use_the_expected_identifier = () => store.ImageIdentifier.ShouldEqual(ExpectedIdentifier);
        It should_create_the_badge_in_the_database =
            () => UnitOfWork.Badges.GetAll().Single(p => p.ImageIdentifier == ExpectedIdentifier);
        static UnitTestImageStore store;
        const string ExpectedIdentifier = "unit-test";
        }

    [Subject(typeof(BadgesController), "Image Identifier")]
    class when_creating_an_image_identifier_from_a_filename 
        {
        Establish context = () => FileName = "The Cat Sat On.The Mat.thingy";
        Because of = () => Identifier = FileName.ToImageIdentifier();
        It should_produce_the_expected_identifier = () => Identifier.ShouldEqual(ExpectedIdentifier);
        static string FileName;
        static string Identifier;
        const string ExpectedIdentifier = "the-cat-sat-on-the-mat";
        }
    }