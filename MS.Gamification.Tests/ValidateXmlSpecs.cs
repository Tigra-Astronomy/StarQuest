// This file is part of the MS.Gamification project
// 
// File: ValidateXmlSpecs.cs  Created: 2016-07-21@08:17
// Last modified: 2016-07-21@11:08

using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Machine.Specifications;
using MS.Gamification.Tests.TestHelpers;

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

    class XmlDocumentAttribute : ValidationAttribute
        {
        readonly string xsd;
        bool error;
        ValidationResult result;

        public XmlDocumentAttribute(string xsd)
            {
            this.xsd = xsd;
            }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
            error = false;
            result = ValidationResult.Success;
            if (!(value is string))
                {
                return new ValidationResult("The value must be a string");
                }
            var xmlString = (string)value;
            var xmlDocument = XDocument.Parse(xmlString);
            var schema = XmlSchema.Read(XmlReader.Create(new StringReader(xsd)), HandleValidationErrors);
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);
            xmlDocument.Validate(schemaSet, HandleValidationErrors);
            return result;
            }

        void HandleValidationErrors(object sender, ValidationEventArgs e)
            {
            result = new ValidationResult("The xml is invalid");
            error = true;
            }
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
