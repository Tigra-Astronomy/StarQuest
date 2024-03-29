// This file is part of the MS.Gamification project
// 
// File: XmlDocumentAttribute.cs  Created: 2016-07-21@12:10
// Last modified: 2016-08-06@03:02

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using JetBrains.Annotations;
using MS.Gamification.DataAccess;
using NLog;

namespace MS.Gamification.ViewModels.CustomValidation
    {
    [AttributeUsage(AttributeTargets.Property)]
    internal class XmlDocumentAttribute : ValidationAttribute
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly Maybe<string> maybeXsdResourceName = Maybe<string>.Empty;
        private readonly Maybe<Type> maybeXsdResourceType = Maybe<Type>.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlDocumentAttribute" /> class with no schema. No schema
        ///     validation will take place, but the validated object must still consist of well-formed XML.
        /// </summary>
        public XmlDocumentAttribute() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmlDocumentAttribute" /> class with a resource identifier
        ///     that can be used to load an XML Schema Document (XSD). The validated object must be well-formed XML and
        ///     must also conform to the supplied schema. Note that failure to load the schema will result in the
        ///     validated object being considered invalid.
        /// </summary>
        /// <param name="schemaResourceName">Name of the schema resource.</param>
        /// <param name="schemaResourceType"><see cref="Type" /> of the schema resource.</param>
        public XmlDocumentAttribute([NotNull] string schemaResourceName, [NotNull] Type schemaResourceType)
            {
            maybeXsdResourceName = new Maybe<string>(schemaResourceName);
            maybeXsdResourceType = new Maybe<Type>(schemaResourceType);
            }

        /// <summary>
        ///     Validates the specified <paramref name="value" /> with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        ///     An instance of the <see cref="ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
            // An empty or null object should pass validation. 
            // [Required] attribute can be used to ensure a non-empty item.
            if (string.IsNullOrEmpty(value as string))
                {
                return ValidationResult.Success;
                }
            var xmlString = (string) value;
            try
                {
                var xmlDocument = XDocument.Parse(xmlString);
                }
            catch (XmlException e)
                {
                Log.Warn(e, "Failing XML validation due to invalid markup", xmlString);
                return FailureResult(validationContext, "Invalid XML markup");
                }
            var schemaSet = new XmlSchemaSet();
            try
                {
                var maybeSchema = GetSchema();
                if (maybeSchema.None)
                    return ValidationResult.Success; // No schema validation
                schemaSet.Add(maybeSchema.Single());
                }
            catch (XmlException e)
                {
                Log.Error(e, "Failing validation due to an error loading XML schema");
                return FailureResult(validationContext,
                    "Unable to load the XML schema for validation (please report this as a bug)");
                }
            var xmlReaderSettings = new XmlReaderSettings
                {
                DtdProcessing = DtdProcessing.Prohibit,
                CheckCharacters = true,
                ValidationType = ValidationType.Schema,
                Schemas = schemaSet,
                ConformanceLevel = ConformanceLevel.Document,
                ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings,
                CloseInput = true
                };
            xmlReaderSettings.ValidationEventHandler += ValidationEventHandler;

            using (var stream = GenerateStreamFromString(xmlString))
            using (var xmlReader = XmlReader.Create(stream, xmlReaderSettings))
                try
                    {
                    while (xmlReader.Read()) {}
                    return ValidationResult.Success;
                    }
                catch (XmlSchemaException e)
                    {
                    Log.Warn(e, "Failing XML validation due to schema validation error", xmlString);
                    return FailureResult(validationContext, e.Message);
                    }
            }

        private void ValidationEventHandler(object sender, ValidationEventArgs validationEventArgs)
            {
            throw validationEventArgs.Exception ?? new XmlSchemaException(validationEventArgs.Message);
            }

        private ValidationResult FailureResult(ValidationContext validationContext, string message = null)
            {
            // If the user specified an error message in the attribute use, always use that.
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
                return new ValidationResult(ErrorMessage, new List<string> {validationContext.MemberName});
            // Otherwise, use an internally generated message
            var resultMessage = string.IsNullOrWhiteSpace(message) ? "Invalid XML" : message;
            return new ValidationResult(resultMessage, new List<string> {validationContext.MemberName});
            }

        private Maybe<XmlSchema> GetSchema()
            {
            if (maybeXsdResourceName.None || maybeXsdResourceType.None)
                return Maybe<XmlSchema>.Empty;
            var manager = new ResourceManager(maybeXsdResourceType.Single());
            var xsdString = manager.GetString(maybeXsdResourceName.Single());
            using (var textReader = new StringReader(xsdString))
            using (var xmlReader = XmlReader.Create(textReader))
                {
                var xsd = XmlSchema.Read(xmlReader,
                    (o, e) => { throw new XmlException("Unable to load the schema"); });
                return new Maybe<XmlSchema>(xsd);
                }
            }

        private Stream GenerateStreamFromString(string s)
            {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
            }
        }
    }