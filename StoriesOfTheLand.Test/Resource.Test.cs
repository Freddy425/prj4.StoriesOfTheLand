using StoriesOfTheLand.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System.Numerics;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.DependencyInjection;
using StoriesOfTheLand.Data;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using StoriesOfTheLand.Controllers;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.DevTools.V121.Network;

namespace StoriesOfTheLand.Test
{

    public class ResourceTests
    {
        private Resource ResourceObject;
        private FR_Resource FR_ResourceObject;

        [SetUp]
        public void SetUp()
        {
            //Sets up the specimen object for use in the test
            ResourceObject = new Resource()
            {
                ResourceID = 1,
                ResourceTitle = "Title",
                ResourceDescription = "Description",
                ResourceImage = "image.png",
                ResourceURL = "https://saskpolytech.ca/",

            };

            FR_ResourceObject = new FR_Resource()
            {
                FR_ResourceTitle = "titre",
                FR_ResourceDescription = "Description",

            };
        }

        #region Title


        [Test]
        public void TestResourcesTitleCantBe101Characters()
        {
            string InvalidTitle = new string('A', 101);
            ResourceObject.ResourceTitle = InvalidTitle;
            var Errors = ValidationHelper.Validate(ResourceObject);


            Assert.AreEqual(1, Errors.Count);
            Assert.AreEqual("Title must be between 4 and 100 characters.", Errors[0].ErrorMessage);

        }


        [Test]
        public void TestResourcesTitleCanBe100Characters()
        {
            string validTitle = new string('A', 100);
            ResourceObject.ResourceTitle = validTitle;
            var Errors = ValidationHelper.Validate(ResourceObject);
            Assert.AreEqual(0, Errors.Count);
        }

        [Test]
        public void TestResourcesTitleCantBe3Chars()
        {
            // Arrange
            string invalidTitle = new string('B', 3);
            ResourceObject.ResourceTitle = invalidTitle;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error.");
            Assert.AreEqual("Title must be between 4 and 100 characters.", errors[0].ErrorMessage, "The validation message for a title with 3 characters did not match expectations.");
        }

        [Test]
        public void TestResourcesTitleCanBe4Chars()
        {
            // Arrange
            string validTitle = new string('B', 4);
            ResourceObject.ResourceTitle = validTitle;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for a title with 4 characters.");
        }

        [Test]
        public void TestResourcesTitleIsMandatory()
        {
            // Arrange
            ResourceObject.ResourceTitle = string.Empty;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error for a mandatory title.");
            Assert.AreEqual("Title is mandatory", errors[0].ErrorMessage, "The validation message for a missing title did not match expectations.");
        }

        #endregion


        #region description
        [Test]
        public void TestResourcesDescriptionCantBe1001Characters()
        {
            // Arrange
            string invalidDescription = new string('C', 1001);
            ResourceObject.ResourceDescription = invalidDescription;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error.");
            Assert.AreEqual("Description must be between 4 and 1000 characters.", errors[0].ErrorMessage, "The validation message for a description with 801 characters did not match expectations.");
        }

        [Test]
        public void TestResourcesDescriptionCanBe800Chars()
        {
            // Arrange
            string validDescription = new string('C', 800);
            ResourceObject.ResourceDescription = validDescription;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for a description with 800 characters.");
        }



        [Test]
        public void TestResourcesDescriptionCantBe3Chars()
        {
            // Arrange
            string invalidDescription = new string('D', 3);
            ResourceObject.ResourceDescription = invalidDescription;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error.");
            Assert.AreEqual("Description must be between 4 and 1000 characters.", errors[0].ErrorMessage, "The validation message for a description with 3 characters did not match expectations.");
        }

        [Test]
        public void TestResourcesDescriptionCanBe4Chars()
        {
            // Arrange
            string validDescription = new string('D', 4);
            ResourceObject.ResourceDescription = validDescription;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for a description with 4 characters.");
        }
        #endregion


        #region URL

        [Test]
        public void TestResourcesURLCantBe201Characters()
        {
            // Arrange
            string invalidURL = "https://www." + new string('E', 185) + ".com";
            ResourceObject.ResourceURL = invalidURL;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error.");
            Assert.AreEqual("URL must be between 9 and 200 characters.", errors[0].ErrorMessage, "The validation message for a URL with 201 characters did not match expectations.");
        }
        [Test]
        public void TestResourcesURLValidPasses()
        {
            // Arrange
            string validURL = "https://saskpolytech.ca/";
            ResourceObject.ResourceURL = validURL;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation error.");
        }
        [Test]
        public void TestResourcesURLCanBe200Chars()
        {
            // Arrange
            string validURL = "https://www." + new string('E', 183) + ".com/";
            ResourceObject.ResourceURL = validURL;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            if (errors.Count > 0)
            {
                Console.WriteLine("Validation errors found:");
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                    Console.WriteLine(validURL);
                }
            }

            Assert.AreEqual(0, errors.Count, "Expected no validation errors for a URL with 200 characters.");
        }


        [Test]
        public void TestResourcesURLCantBe8Chars()
        {
            // Arrange
            string invalidURL = "http://a";
            ResourceObject.ResourceURL = invalidURL;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error.");
            Assert.AreEqual("URL must be between 9 and 200 characters.", errors[0].ErrorMessage, "The validation message for a URL with 3 characters did not match expectations.");
        }

        [Test]
        public void TestResourcesURLCanBe9Chars()
        {
            // Arrange
            string validURL = "http://aa";
            ResourceObject.ResourceURL = validURL;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for a URL with 9 characters.");
        }

        [Test]
        public void TestResourcesURLIsMandatory()
        {
            // Arrange
            ResourceObject.ResourceURL = string.Empty; // Empty string for mandatory check

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error for a mandatory URL.");
            Assert.AreEqual("URL is mandatory", errors[0].ErrorMessage, "The validation message for a missing URL did not match expectations.");
        }

        [Test]
        public void TestInvalidResourcesURLFails()
        {
            // Arrange
            string invalidURL = "Saskpolyt";
            ResourceObject.ResourceURL = invalidURL;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            Assert.AreEqual("Please enter a valid URL. Ensure it starts with http:// or https:// and follows the standard format, such as http://www.example.com.", errors[0].ErrorMessage, "The validation message for a missing URL did not match expectations.");

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected 1 validation error.");
        }

        #endregion


        #region Image
        [Test]
        public void TestRespurcesIMGValidMediaPasses()
        {
            // Arrange
            string validImageFile = "Nctr.png";
            ResourceObject.ResourceImage = validImageFile;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for a valid image file.");
        }

        [Test]
        public void TestResourceIMGInvalidMediaDoesntPass()
        {
            // Arrange
            string invalidImageFile = "NCTR.txt"; // An invalid image file extension
            ResourceObject.ResourceImage = invalidImageFile;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error for an invalid image file.");
            Assert.AreEqual("Media file must be of type jpg, jpeg or png", errors[0].ErrorMessage, "The validation message for an invalid image file did not match expectations.");
        }

        [Test]
        public void TestResourcesIMGPathCantBe7Chars()
        {
            // Arrange
            string shortImagePath = "aaa.png"; // Too short to be valid
            ResourceObject.ResourceImage = shortImagePath;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Print error messages before performing the checks
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error for too short image path.");
            Assert.AreEqual("Image path must be between 8 and 500 characters.", errors[0].ErrorMessage, "The validation message for a too short image path did not match expectations.");
        }

        [Test]
        public void TestResourcesIMGPathCanBe8Chars()
        {
            // Arrange
            string minimumImagePath = "aaaa.png"; // Minimum valid length
            ResourceObject.ResourceImage = minimumImagePath;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for minimum valid image path length.");
        }

        [Test]
        public void TestResourceIMGPathCantBe501Chars()
        {
            // Arrange
            string longImagePath = new string('G', 497) + ".png"; // 501 characters in total
            ResourceObject.ResourceImage = longImagePath;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(1, errors.Count, "Expected exactly one validation error for too long image path.");
            Assert.AreEqual("Image path must be between 8 and 500 characters.", errors[0].ErrorMessage, "The validation message for a too long image path did not match expectations.");
        }

        [Test]
        public void TestResourceIMGPathCanBe50Chars()
        {
            // Arrange
            string validImagePath = new string('G', 46) + ".png"; // Exactly 50 characters
            ResourceObject.ResourceImage = validImagePath;

            // Act
            var errors = ValidationHelper.Validate(ResourceObject);

            // Assert
            Assert.AreEqual(0, errors.Count, "Expected no validation errors for maximum valid image path length.");
        }

        #endregion


        #region FrenchTitle
        [Test]
        public void TestFrenchResourcesTitleCantBe201Characters()
        {
            string invalidTitle = new string('A', 201);
            FR_ResourceObject.FR_ResourceTitle = invalidTitle;
            var errors = ValidationHelper.Validate(FR_ResourceObject);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Title must be between 3 and 200 characters.", errors[0].ErrorMessage);
        }

        [Test]
        public void TestFrenchResourcesTitleCanBe200Chars()
        {
            string validTitle = new string('A', 200);
            FR_ResourceObject.FR_ResourceTitle = validTitle;
            var errors = ValidationHelper.Validate(FR_ResourceObject);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void TestFrenchResourcesTitleCantBe2Chars()
        {
            string invalidTitle = new string('B', 2);
            FR_ResourceObject.FR_ResourceTitle = invalidTitle;
            var errors = ValidationHelper.Validate(FR_ResourceObject);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Title must be between 3 and 200 characters.", errors[0].ErrorMessage);
        }

        [Test]
        public void TestFrenchResourcesTitleCanBe3Chars()
        {
            string validTitle = new string('B', 3);
            FR_ResourceObject.FR_ResourceTitle = validTitle;
            var errors = ValidationHelper.Validate(FR_ResourceObject);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void TestFrenchResourcesTitleIsMandatory()
        {
            FR_ResourceObject.FR_ResourceTitle = string.Empty;
            var errors = ValidationHelper.Validate(FR_ResourceObject);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Title is mandatory", errors[0].ErrorMessage);
        }
        #endregion

        #region FrenchDescription
        [Test]
        public void TestFrenchResourcesDescriptionCantBe2001Characters()
        {
            string invalidDescription = new string('C', 2001);
            FR_ResourceObject.FR_ResourceDescription = invalidDescription;
            var errors = ValidationHelper.Validate(FR_ResourceObject);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Description must be between 3 and 2000 characters.", errors[0].ErrorMessage);
        }

        [Test]
        public void TestFrenchResourcesDescriptionCanBe2000Chars()
        {
            string validDescription = new string('C', 2000);
            FR_ResourceObject.FR_ResourceDescription = validDescription;
            var errors = ValidationHelper.Validate(FR_ResourceObject);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void TestFrenchResourcesDescriptionCantBe2Chars()
        {
            string invalidDescription = new string('D', 2);
            FR_ResourceObject.FR_ResourceDescription = invalidDescription;
            var errors = ValidationHelper.Validate(FR_ResourceObject);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Description must be between 3 and 2000 characters.", errors[0].ErrorMessage);
        }

        [Test]
        public void TestFrenchResourcesDescriptionCanBe3Chars()
        {
            string validDescription = new string('D', 3);
            FR_ResourceObject.FR_ResourceDescription = validDescription;
            var errors = ValidationHelper.Validate(FR_ResourceObject);
            Assert.IsEmpty(errors);
        }

        #endregion

        #region FrenchObject
        [Test]
        public void TestFrenchResourceIsOptional()
        {
            ResourceObject.FR_Resource = null;

            var errors = ValidationHelper.Validate(ResourceObject);
            Assert.IsEmpty(errors);

        }

        [Test]
        public void TestValidFrenchResource()
        {
            ResourceObject.FR_Resource = FR_ResourceObject;

            var errors = ValidationHelper.Validate(ResourceObject);
            Assert.IsEmpty(errors);

        }


        #endregion

    }

}