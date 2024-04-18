using StoriesOfTheLand.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.DependencyInjection;
using StoriesOfTheLand.Data;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NuGet.Packaging;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StoriesOfTheLand.Controllers;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using Microsoft.AspNetCore.Components.Infrastructure;
using System.Net.Http;
using System.Net;
using static System.Net.WebRequestMethods;

namespace StoriesOfTheLand.Test
{

    public class MediaTests
    {
        private Media MediaObject;

        [SetUp]
        public void SetUp()
        {
            MediaObject = new Media()
            {
                MediaPath = "abc.png",
                MediaType = "Image"
            };
        }

        #region FunctionalTests
        [TestFixture]
    public class HtmlTest
    {
            private static readonly HttpClient httpClient = new HttpClient();
            const string globalURL = "https://localhost:7202";

            [Test]
            public void TestCarouselHTMLIfThereIsMoreThanOneImage()
            {
                string url = globalURL + "/Specimen/Details/1";

                // Make a request to the URL
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Ensure the request was successful
                Assert.IsTrue(response.IsSuccessStatusCode, $"Failed to retrieve content from {url}. Status code: {response.StatusCode}");

                // Read the HTML content from the response
                string htmlContent = response.Content.ReadAsStringAsync().Result;

                // Perform assertions or checks on the HTML content
                Assert.IsTrue(htmlContent.Contains("<div id=\"carouselExampleControls\" class=\"carousel slide\" data-bs-ride=\"carousel\">"), "Expected content not found in HTML");

            }

            [Test]
            public void TestNoCarouselHTMLIfThereIsOneImage()
            {
                string url = globalURL + "/Specimen/Details/2";

                // Make a request to the URL
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Ensure the request was successful
                Assert.IsTrue(response.IsSuccessStatusCode, $"Failed to retrieve content from {url}. Status code: {response.StatusCode}");

                // Read the HTML content from the response
                string htmlContent = response.Content.ReadAsStringAsync().Result;

                // Perform assertions or checks on the HTML content
                Assert.IsTrue(htmlContent.Contains("div class=\"d-print-none\" id=\"oneImage\">"), "Expected content not found in HTML");

            }
            [Test]
            public void TestNoImageHTMLIfThereIsNoImage()
            {
                string url = globalURL + "/Specimen/Details/7";

                // Make a request to the URL
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Ensure the request was successful
                Assert.IsTrue(response.IsSuccessStatusCode, $"Failed to retrieve content from {url}. Status code: {response.StatusCode}");

                // Read the HTML content from the response
                string htmlContent = response.Content.ReadAsStringAsync().Result;

                // Perform assertions or checks on the HTML content
                Assert.IsFalse(htmlContent.Contains("<div id=\"carouselExampleControls\" class=\"carousel slide\" data-bs-ride=\"carousel\">"), "Expected content not found in HTML");
                Assert.IsFalse(htmlContent.Contains("<div id=\"oneImage\">"), "Expected content not found in HTML");

            }

            [Test]
            public void TestAudioPlayerIfThereIsAnAudioFile()
            {
                string url = globalURL + "/Specimen/Details/1";

                // Make a request to the URL
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Ensure the request was successful
                Assert.IsTrue(response.IsSuccessStatusCode, $"Failed to retrieve content from {url}. Status code: {response.StatusCode}");

                // Read the HTML content from the response
                string htmlContent = response.Content.ReadAsStringAsync().Result;

                // Perform assertions or checks on the HTML content
                Assert.IsTrue(htmlContent.Contains("<audio controls>"), "Expected content not found in HTML");

            }

            [Test]
            public void TestNoAudioPlayerIfThereIsNoAudioFile()
            {
                string url = globalURL + "/Specimen/Details/2";

                // Make a request to the URL
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Ensure the request was successful
                Assert.IsTrue(response.IsSuccessStatusCode, $"Failed to retrieve content from {url}. Status code: {response.StatusCode}");

                // Read the HTML content from the response
                string htmlContent = response.Content.ReadAsStringAsync().Result;

                // Perform assertions or checks on the HTML content
                Assert.IsFalse(htmlContent.Contains("<audio controls>"), "Expected content not found in HTML");

            }
        }
        
        #endregion

        #region SpecimenAudioTests
        //test is deprecated
        [Test]
        public void testInvalidFileType()
        {
            MediaObject.MediaPath = "Blueberry.txt";

            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Media file must be of type m4a, mp3, jpeg, or png", errors[0].ErrorMessage);
        }

        [Test]
        public void testInvalidAudioFileLengthTooShort()
        {
            MediaObject.MediaType = "Audio";
            MediaObject.MediaPath = "a.mp3";
            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Media path must be between 6 and 254 characters", errors[0].ErrorMessage);
        }

        [Test]
        public void testInvalidAudioFileLengthTooLong()
        {
            MediaObject.MediaPath = new string('a', 251);
            MediaObject.MediaPath += ".mp3";
            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Media path must be between 6 and 254 characters", errors[0].ErrorMessage);
        }

        [Test]
        public void testInvalidAudioFileHasNoType()
        {
            MediaObject.MediaPath = "Blueberrym4a";
            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Media file must be of type m4a, mp3, jpeg, or png", errors[0].ErrorMessage);
        }

        [Test]
        public void testValidAudioFileTypeMp3()
        {
            MediaObject.MediaPath = "Blueberry.mp3";

            var errors = ValidationHelper.Validate(MediaObject);

            Assert.IsEmpty(errors);
        }

        [Test]
        public void testValidAudioFileTypeM4a()
        {
            MediaObject.MediaPath = "Blueberry.m4a";

            var errors = ValidationHelper.Validate(MediaObject);

            Assert.IsEmpty(errors);
        }

        [Test]
        public void testValidAudioFileLenghtLowerBoundary()
        {
            MediaObject.MediaPath = "ab.mp3";

            var errors = ValidationHelper.Validate(MediaObject);

            Assert.IsEmpty(errors);

        }

        [Test]
        public void testValidAudioFileLengthUpperBoundary()
        {
            MediaObject.MediaPath = new string('a', 250);
            MediaObject.MediaPath += ".mp3";
            var errors = ValidationHelper.Validate(MediaObject);

            Assert.IsEmpty(errors);
        }

        #endregion


        #region SpecimenImagePath

        /* "abc.png" is passed in which is valid
        */
        [Test]
        public void specimenImagePngIsValidtype()
        {
            MediaObject.MediaPath = "Blueberry.png";
            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);

        }

        /* "abc.jpeg" is passed in which is valid
         */
        [Test]
        public void specimenImageJpegIsValidtype()
        {

            MediaObject.MediaPath = "Blueberry.jpeg";

            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);
        }

        /* "abc.jpg" is passed in which is valid
         */
        [Test]
        public void specimenImageJpgIsValidtype()
        {

            MediaObject.MediaPath = "Blueberry.jpg";

            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);
        }

        /* "abcgfjdjfdpng" is passed in which is invalid
         * and an exception is thrown
         */
        [Test]
        public void specimenImageHasNoType()
        {
            MediaObject.MediaPath = "Blueberrypng";


            var errors = ValidationHelper.Validate(MediaObject);

            Assert.AreEqual(errors.Count, 1);
            Assert.AreEqual("Media file must be of type m4a, mp3, jpeg, or png", errors[0].ErrorMessage);


        }

        /* "abc.pn" is passed in which is invalid
         * and an exception is thrown
         */
        [Test]
        public void specimenImageIsNotValidtype()
        {
            //.abc .webp .pn .jp abcabc should fail
            MediaObject.MediaPath = "Blueberry.pn";

            var errors = ValidationHelper.Validate(MediaObject);

            Assert.AreEqual(errors.Count, 1);
            Assert.AreEqual("Media file must be of type m4a, mp3, jpeg, or png", errors[0].ErrorMessage);
        }

        /* 255 ending/including ".png" is passed in which is too large
         * and an exception is thrown
         */
        [Test]
        public void specimenImageSourceNameIsTooBig()
        {
            /*
             abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde
             abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde
             abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde */
            //255char

            MediaObject.MediaPath = new string('a', 251);
            MediaObject.MediaPath += ".png";


            var errors = ValidationHelper.Validate(MediaObject);

            Assert.AreEqual(errors.Count, 1);
            Assert.AreEqual("Media path must be between 6 and 254 characters", errors[0].ErrorMessage);
        }

        /* 254 ending/including ".png" is passed in which is just almost too big
        */
        [Test]
        public void specimenImageSourceNameIsOnMaxBoundaryCaseValid()
        {
            MediaObject.MediaPath = new string('a', 250);
            MediaObject.MediaPath += ".png";

            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);
        }

        /*".png" is passed in which is invalid
         * and an exception is thrown.
         */
        [Test]
        public void specimenImageSourceNameIsTooSmall()
        {
            MediaObject.MediaPath = "a.png";

            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(errors.Count, 1);
            Assert.AreEqual("Media path must be between 6 and 254 characters", errors[0].ErrorMessage);
        }

        //check
        [Test]
        public void specimenImagePathContainsMoreThanOneImageValid()
        {
            MediaObject.MediaPath = "abc.png,abc.jpg";
            var errors = ValidationHelper.Validate(MediaObject);

            Assert.IsEmpty(errors);
        }
        #endregion


        #region MediaTypeTests

        [Test]
        public void TestValidMediaTypeImage()
        {
            MediaObject.MediaType = "Image";
            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);

        }
        [Test]
        public void TestValidMediaTypeAudio()
        {
            MediaObject.MediaType = "Audio";
            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);

        }
        [Test]
        public void TestValidMediaTypeVideo()
        {
            MediaObject.MediaType = "Video";
            var errors = ValidationHelper.Validate(MediaObject);


            Assert.IsEmpty(errors);

        }
        [Test]
        public void TestInvalidMediaType()
        {
            MediaObject.MediaType = "Movie";

            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Media type must be Audio, Image, or Video", errors[0].ErrorMessage);
        }

        [Test]
        public void TestInvalidMediaLengthTooShort()
        {
            MediaObject.MediaType = "Gif";

            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("Media type must be Audio, Image, or Video", errors[0].ErrorMessage);
            Assert.AreEqual("Media Type must be between 5 and 20 characters", errors[1].ErrorMessage);
        }

        [Test]
        public void TestInvalidMediaLengthTooLong()
        {
            MediaObject.MediaType = new string('a', 21);

            var errors = ValidationHelper.Validate(MediaObject);
            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("Media type must be Audio, Image, or Video", errors[0].ErrorMessage);
            Assert.AreEqual("Media Type must be between 5 and 20 characters", errors[1].ErrorMessage);
        }


        #endregion

    }
}
