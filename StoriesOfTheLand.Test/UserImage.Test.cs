using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SQLitePCL;
using StoriesOfTheLand.Controllers;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
namespace StoriesOfTheLand.Test
{
    public class UserImageTest
    {


        // Create a user image object
        private UserImage UserImageObject;
        [SetUp]
        public void SetUp()
        {

        }

        // Test that the UserIMage object is created
        [Test]
        public void TestThatValidUserImageObjectCanBeCreated()
        {
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "127.0.0.1",
                FileSize = 1,

            };
            Assert.That(1, Is.EqualTo(UserImageObject.UserImageiD));
            Assert.That("127.0.0.1", Is.EqualTo(UserImageObject.IP));
            Assert.That(1, Is.EqualTo(UserImageObject.FileSize));
            Assert.That(false, Is.EqualTo(UserImageObject.status));
        }

        // Test that the IP address is required
        [Test]
        public void TestThatIPAddressIsRequired()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                FileSize = 1,
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be errors
            Assert.That(1, Is.EqualTo(errors.Count()));
            Assert.That("IP address is required", Is.EqualTo(errors.First().ErrorMessage));
        }

        [Test]
        public void TestThatIPAddressOf15CharactersCanBeCreated()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "12345678910112",
                FileSize = 1,
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be no errors
            Assert.That(0, Is.EqualTo(errors.Count()));
        }

        [Test]
        public void TestThatIPAddressOf16CharactersWontCreate()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "1111111111111111",
                FileSize = 1,
            };

            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be errors
            Assert.That(1, Is.EqualTo(errors.Count()));
            Assert.That("IP Address can be no larger than 15 characters", Is.EqualTo(errors.First().ErrorMessage));
        }

        [Test]
        public void TestThatIPAddressOf14CharactersCanBeCreated()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 1,
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be no errors
            Assert.That(0, Is.EqualTo(errors.Count()));


        }

        [Test]
        public void TestThatFileSizeIsrequired()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be errors
            Assert.That(1, Is.EqualTo(errors.Count()));
            Assert.That("File Size Must be Above 0 AND less than 1 MB", Is.EqualTo(errors.First().ErrorMessage));
        }

        [Test]
        public void TestThatFileSizeOf0CannotBeCreated()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 0,
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be errors
            Assert.That(1, Is.EqualTo(errors.Count()));
            Assert.That("File Size Must be Above 0 AND less than 1 MB", Is.EqualTo(errors.First().ErrorMessage));
        }

        [Test]
        public void TestThatFileSizeOf1CanBeCreated()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 1,
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be no errors
            Assert.That(0, Is.EqualTo(errors.Count()));
        }
        [Test]
        public void TestThatDateUploadedGetsCreatedAutomatically()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 1,
            };
            Assert.That(null, !Is.EqualTo(UserImageObject.DateUploaded));

        }

        [Test]
        public void TestThatStatusIsFalseByDefault()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 1,
            };
            Assert.That(false, Is.EqualTo(UserImageObject.status));
        }

        [Test]
        public void TestThatValidMediaPathCanBeCreated()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 1,
                MediaPath = "Wild Mint.png"
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be no errors
            Assert.That(0, Is.EqualTo(errors.Count()));
        }
        [Test]
        public void TestThatInvalidMediaPathCannotBeCreated()
        {
            // Create a new UserImage object
            UserImageObject = new UserImage()
            {
                UserImageiD = 1,
                IP = "11111111111111",
                FileSize = 1,
                MediaPath = "example.json"
            };
            var errors = ValidationHelper.Validate(UserImageObject); //use the ValidationHelper class to see if there is errors
            // There should be errors
            Assert.That(1, Is.EqualTo(errors.Count()));
            Assert.That("Media file must be of type jpeg, jpg, or png", Is.EqualTo(errors.First().ErrorMessage));
        }

    }

}
