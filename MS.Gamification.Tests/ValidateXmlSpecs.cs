// This file is part of the MS.Gamification project
// 
// File: ValidateXmlSpecs.cs  Created: 2016-07-21@08:17
// Last modified: 2016-07-21@11:39

using System.ComponentModel.DataAnnotations;
using Machine.Specifications;
using MS.Gamification.Tests.TestHelpers;
using MS.Gamification.ViewModels.CustomValidation;

namespace Namespace
    {
    [Subject(typeof(XmlDocumentAttribute))]
    internal class When_the_xml_file_conforms_to_the_schema
        {
        Establish context = () =>
            {
            xml = TestData.FromEmbeddedResource("PreconditionsEngine.HasAll-1-2-4.xml");
            xsd = TestData.FromEmbeddedResource("PreconditionsEngine.LevelPreconditionSchema.xsd");
            Validator = new TestableXmlDocumentAttribute(xsd);
            };

        Because of = () => Result = Validator.TestIsValid(xml, new ValidationContext(xml));

        It should_validate = () => Result.ShouldBeTheSameAs(ValidationResult.Success);
        static string xsd;
        static string xml;
        static TestableXmlDocumentAttribute Validator;
        static ValidationResult Result;
        }

    class TestableXmlDocumentAttribute : XmlDocumentAttribute
        {
        public TestableXmlDocumentAttribute(string xsd) : base(xsd) {}

        public ValidationResult TestIsValid(object value, ValidationContext validationContext)
            {
            return IsValid(value, validationContext);
            }
        }
    }
