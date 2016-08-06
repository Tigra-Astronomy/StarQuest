// This file is part of the MS.Gamification project
// 
// File: XmlDocumentAttribute.cs  Created: 2016-07-21@12:10
// Last modified: 2016-08-06@00:45

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
            XDocument xmlDocument;
            try
                {
                xmlDocument = XDocument.Parse(xmlString);
                Log.Debug("Loaded valid XML document", xmlDocument);
                }
            catch (InvalidOperationException e)
                {
                Log.Warn(e, "Failing XML validation due to invalid XML markup", xmlString);
                return FailureResult(validationContext, "Invalid XML markup");
                }
            catch (Exception e)
                {
                Log.Warn(e, "Failing XML validation due to an unexpected error parsing the XML markup", xmlString);
                return FailureResult(validationContext, "Unable to parse the XML document");
                }
            try // to load the XML schema from a resource
                {
                var maybeSchema = GetSchema();
                if (maybeSchema.None)
                    return ValidationResult.Success; // No schema validation
                var schemaSet = new XmlSchemaSet();
                schemaSet.Add(maybeSchema.Single());
                try // to validate the XML document against the schema
                    {
                    xmlDocument.Validate(schemaSet, null); // rely on exceptions rather than event handlers
                    return ValidationResult.Success;
                    }
                catch (XmlSchemaException e)
                    {
                    Log.Warn(e, "Failing XML schema validation", xmlDocument);
                    return FailureResult(validationContext, "XML does not conform to the required schema");
                    }
                }
            catch (Exception e)
                {
                Log.Error(e, "Failing XML validation due to a schema resource error");
                return FailureResult(validationContext, "Error validating schema resource");
                }
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
            try
                {
                if (maybeXsdResourceName.None || maybeXsdResourceType.None)
                    return Maybe<XmlSchema>.Empty;
                var manager = new ResourceManager(maybeXsdResourceType.Single());
                var xsdString = manager.GetString(maybeXsdResourceName.Single());
                using (var textReader = new StringReader(xsdString))
                using (var xmlReader = XmlReader.Create(textReader))
                    {
                    var xsd = XmlSchema.Read(xmlReader,
                        (o, e) => { throw new ArgumentException("Unable to load the schema document"); });
                    return new Maybe<XmlSchema>(xsd);
                    }
                }
            catch (Exception)
                {
                return Maybe<XmlSchema>.Empty;
                }
            }
        }
    }