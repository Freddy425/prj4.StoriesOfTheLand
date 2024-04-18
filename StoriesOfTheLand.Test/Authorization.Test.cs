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
using StoriesOfTheLand.Controllers;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using System.Net.Http;

namespace StoriesOfTheLand.Test
{

    public class AuthorizationTest
    {
        const string globalURL = "https://localhost:7202/";
        const string detailsURL = globalURL + "Specimen/Details/1";
        const string validEmail = "sotltesting@outlook.com";
        const string validPswd = "testing123456";
        private IWebDriver driver;


        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--headless=new");
            driver = new ChromeDriver(options);
        }

        /**
         * This method was created so that we can log in the user for the two tests rather than 
         * copying and pasting the code
         */
        public void loginUser()
        {
            // Navigate to the home page
            driver.Navigate().GoToUrl(globalURL);

            // Click the login button
            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            // Authenticate the user
            loginButton.Click();

            // Pause execution for a fixed time
            Thread.Sleep(1000);
            driver.FindElement(By.Name("loginfmt")).SendKeys(validEmail);
            Thread.Sleep(1000);

            // Find the "Next" button by its text and click it
            IWebElement nextButton = driver.FindElement(By.XPath("//input[@type='submit' and @value='Next']"));
            nextButton.Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Name("passwd")).SendKeys(validPswd);

            Thread.Sleep(3000);
            IWebElement signInButton = driver.FindElement(By.Id("idSIButton9"));
            signInButton.Click();
            Thread.Sleep(3000);

            bool buttonClicked = false;
            int attempts = 0;
            while (!buttonClicked)
            {
                try
                {
                    // Create an instance of the Actions class
                    Actions actions = new Actions(driver);

                    // Simulate pressing the Enter key
                    actions.SendKeys(Keys.Enter).Perform();
                    buttonClicked = true;
                }
                catch (Exception ex)
                {
                    buttonClicked = false;
                }
            }


            while (!buttonClicked && attempts < 150)
            {
                try
                {
                    // Find the "No" button by its ID and attempt to click it
                    IWebElement noButton = driver.FindElement(By.Id("declineButton"));
                    noButton.Click();
                    buttonClicked = true; // Set to true if click is successful
                }
                catch (ElementClickInterceptedException)
                {
                    Thread.Sleep(500); // Wait for a second before trying again
                    attempts++; // Increment the attempt counter
                }
                catch (NoSuchElementException)
                {
                    Thread.Sleep(500); // Wait for a second before trying again
                    attempts++; // Increment the attempt counter
                }
                // Add other exceptions if needed
            }

            if (!buttonClicked)
            {
                Assert.Fail("Failed to click the 'No' button after multiple attempts.");
            }
            // Here you can add any assertions or further actions following the click


            buttonClicked = false;
            attempts = 0;
            while (!buttonClicked)
            {
                try
                {
                    // Create an instance of the Actions class
                    Actions actions = new Actions(driver);

                    // Simulate pressing the Enter key
                    actions.SendKeys(Keys.Enter).Perform();
                    buttonClicked = true;
                }
                catch (Exception ex)
                {
                    buttonClicked = false;
                }
            }
        }

        /*
         * Test that the learner cannot see the Add Media button
         */
        [Test]
        public void testThatAddMediaButtonDoesNotDisplayForLearners()
        {
            // Navigate to details page
            driver.Navigate().GoToUrl(detailsURL);

            // Check to see that the element with the class name of btn-primary (Add Media Button) does not appear
            var elements = driver.FindElements(By.CssSelector("html > body > div.container > main.pb-3 > button.btn.btn-primary"));


            Assert.AreEqual(0, elements.Count);
            // Quit the driver
            driver.Quit();
        }
        /*
         * Test that the learner cannot see the QR Code Reveal button
         */
        [Test]
        public void testThatRevealQRCodeButtonDoesnotAppearForLearners()
        {
            // Navigate to the 
            driver.Navigate().GoToUrl(detailsURL);

            bool isElementDisplayed;
            try
            {
                var element = driver.FindElement(By.CssSelector("button.btn.btn-primary.d-print-none[data-bs-toggle='modal'][data-bs-target='#mediaModal']"));
                isElementDisplayed = element.Displayed;
            }
            catch (NoSuchElementException)
            {
                // Element is not found, which means it's not displayed either
                isElementDisplayed = false;
            }

            // Assert that the element is not displayed
            Assert.AreEqual(false, isElementDisplayed, "The element is unexpectedly displayed.");

            driver.Quit();
        }

        /*
         * Test that Administrators can see the Add Media Button 
         */
        [Test]
        public void testThatAddMediaButtonDisplaysForAdministrators()
        {
            // Login the user
            loginUser();

            // Navigate to the URL
            driver.Navigate().GoToUrl(detailsURL);


            // Check to see that the element displays
            Assert.AreEqual(true, driver.FindElement(By.CssSelector("button.btn.btn-primary.d-print-none[data-bs-toggle='modal'][data-bs-target='#mediaModal']")).Displayed);

            // Quit the driver
            driver.Quit();
        }

        /*
         * Test that Administrators can see the Reveal QR Code Button
         */
        [Test]
        public void testThatRevealQRCodeButtonDisplaysForAdministrators()
        {
            // Login the Administrator
            loginUser();

            // Go to the details URL
            driver.Navigate().GoToUrl(detailsURL);

            // Check to see that the content is displayed
            Assert.AreEqual(true, driver.FindElement(By.Id("qrCollapseBtn")).Displayed);
            driver.Quit();
        }
        [Test]
        public void testThatAdminCanStillSeeNormalPages()
        {
            loginUser();

            // Go to the home page
            driver.Navigate().GoToUrl(globalURL);

            // Ensure that the H1 Stories of the land element displays
            Assert.AreEqual(true, driver.FindElement(By.CssSelector("h1")).Displayed);

            // Go to the Index Page
            driver.Navigate().GoToUrl(globalURL + "Specimen");

            // Ensures that the Specimen Index h1 element displays
            Assert.AreEqual(true, driver.FindElement(By.CssSelector("h1")).Displayed);

            // Go to the Details Page
            driver.Navigate().GoToUrl(globalURL + "Specimen/Details/1");

            // Ensure that the audio displays for the details
            Assert.AreEqual(true, driver.FindElement(By.ClassName("Audio")).Displayed);

            // Go to the Map Page
            driver.Navigate().GoToUrl(globalURL + "Specimen/Map");

            // Ensure that the map displays for the map page
            Assert.AreEqual(true, driver.FindElement(By.Id("map")).Displayed);


            // Exit the driver
            driver.Quit();
        }

        /*
         * This test is responsible for going through all of the normal pages that a learner can see
         * and getting content such as headers, ids, and class names from each page to verify that
         * the item exists
         */
        [Test]
        public void testThatLearnerCanStillSeeNormalPages()
        {
            // Go to the home page
            driver.Navigate().GoToUrl(globalURL);

            // Ensure that the H1 Stories of the land element displays
            Assert.AreEqual(true, driver.FindElement(By.CssSelector("h1")).Displayed);

            // Go to the Index Page
            driver.Navigate().GoToUrl(globalURL + "Specimen");

            // Ensures that the Specimen Index h1 element displays
            Assert.AreEqual(true, driver.FindElement(By.CssSelector("h1")).Displayed);

            // Go to the Details Page
            driver.Navigate().GoToUrl(globalURL + "Specimen/Details/1");

            // Ensure that the audio displays for the details
            Assert.AreEqual(true, driver.FindElement(By.ClassName("Audio")).Displayed);

            // Go to the Map Page
            driver.Navigate().GoToUrl(globalURL + "Specimen/Map");

            // Ensure that the map displays for the map page
            Assert.AreEqual(true, driver.FindElement(By.Id("map")).Displayed);


            // Exit the driver
            driver.Quit();
        }
        [Test]
        public void testThatAdminCanAddSpecimen()
        {
            loginUser();

            // Navigate after login
            driver.Navigate().GoToUrl(globalURL + "Specimen/Create");

            // Checking to see if form group exists ensures that all the form classes are displayed
            Assert.AreEqual(true, driver.FindElement(By.ClassName("form-group")).Displayed);

        }

        /*
         * This test ensures that the learner cannot go to the create page
         */
        [Test]
        public void testThatLearnerCannotMakePosts()
        {
            // Go to the create page
            driver.Navigate().GoToUrl(globalURL + "Specimen/Create");

            // Get the current URL
            var currentURL = driver.Url.ToString();

            // The user shouldn't be able to go to the create page because it redirects them to the login
            Assert.AreEqual(true, currentURL.StartsWith("https://login.microsoftonline.com/"));

            // Quit the driver
            driver.Quit();
        }

        [Test]
        public void testThatLearnerCanMakeFeedbackPost()
        {
            // Go to the Index Page
            driver.Navigate().GoToUrl(globalURL + "Specimen");

            // Go to the Details Page
            driver.Navigate().GoToUrl(globalURL + "Specimen/Details/1");

            Assert.AreEqual(true, driver.FindElement(By.LinkText("Send feedback about this page")).Displayed);

            driver.Navigate().GoToUrl(globalURL + "Feedback/Create?specid=1");

            Assert.AreEqual(true, driver.FindElement(By.LinkText("Back to Specimen")).Displayed);

            driver.Quit();

        }
    }
}