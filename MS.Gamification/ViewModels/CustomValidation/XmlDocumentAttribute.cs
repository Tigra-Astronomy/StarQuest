// This file is part of the MS.Gamification project
// 
// File: XmlDocumentAttribute.cs  Created: 2016-07-21@11:30
// Last modified: 2016-07-21@11:34

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MS.Gamification.ViewModels.CustomValidation
    {
    [AttributeUsage(AttributeTargets.Property)]
    class XmlDocumentAttribute : ValidationAttribute
        {
        readonly string xsd;
        bool error;

        public XmlDocumentAttribute(string xsd)
            {
            this.xsd = xsd;
            }

        public string ErrorMessage { get; set; } = "The XML is not valid";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
            error = false;
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
            return error
                ? new ValidationResult(ErrorMessage, new List<string> {validationContext.MemberName})
                : ValidationResult.Success;
            }

        void HandleValidationErrors(object sender, ValidationEventArgs e)
            {
            error = true;
            }
        }
    }
