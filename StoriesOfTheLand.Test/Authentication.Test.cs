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

    public class AuthenticationTest
    {
        const string globalURL = "https://localhost:7202/";
        const string validEmail = "sotltesting@outlook.com";
        const string validPswd = "testing123456";
        const string invalidEmail = "whoaskedyoy@outlook.com";
        const string invalidPswd = "passwordsecure1";
        private IWebDriver driver;


        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--headless=new");
            driver = new ChromeDriver(options);
        }

        [Test]
        public void TestThatClickLogInOpensAForm()
        {
            driver.Navigate().GoToUrl(globalURL);

            // Find the Login button by its link text and click it
            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));
            loginButton.Click();

            // Use WebDriverWait to wait for the login form to appear
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Perform your assertions or actions after the login form appears
            string currentURL = driver.Url;
            Assert.IsTrue(currentURL.Contains("login"), "The URL does not include 'login'");
            Assert.IsTrue(currentURL.Contains("microsoft"), "The URL does not include 'microsoft'");

            // Quit the driver
            driver.Quit();
        }
        [Test]
        public void TestThatInvalidCredentialsIsDeniedAuthentication()
        {
            driver.Navigate().GoToUrl(globalURL);

            // Find the Login button by its link text and click it
            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));
            loginButton.Click();

            // Wait for the email input field to be visible and interactable
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("loginfmt")));

            // Enter the invalid email
            emailInput.SendKeys(invalidEmail);

            // Find the "Next" button by its text and click it
            IWebElement nextButton = driver.FindElement(By.XPath("//input[@type='submit' and @value='Next']"));
            nextButton.Click();

            // Wait for the password input field to be visible and interactable
            IWebElement passwordInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("passwd")));

            // Enter the invalid password
            passwordInput.SendKeys(invalidPswd);

            Thread.Sleep(1500);

            // Find the sign-in button by its ID and click it
            IWebElement signInButton = driver.FindElement(By.Id("idSIButton9"));
            signInButton.Click();

            bool buttonClicked = false;
            int attempts = 0;
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

            while (!buttonClicked && attempts < 20)
            {
                try
                {
                    // Use WebDriverWait to wait for the "No" button to be clickable
                    IWebElement noButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("idSIButton9")));

                    // Click the "No" button
                    noButton.Click();
                    buttonClicked = true;
                }
                catch (WebDriverTimeoutException)
                {
                    try
                    {
                        // If clicking "No" failed, try clicking it by the text on the button
                        IWebElement noButtonByText = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='No']")));
                        noButtonByText.Click();
                        buttonClicked = true;
                    }
                    catch (WebDriverTimeoutException)
                    {
                        try
                        {
                            // If clicking by text also failed, click the "Yes" button
                            IWebElement yesButton = driver.FindElement(By.XPath("//button[text()='Yes']"));
                            yesButton.Click();
                            buttonClicked = true;
                        }
                        catch (WebDriverTimeoutException)
                        {
                            // Handle any additional exceptions or actions if needed
                        }
                    }
                }
                Thread.Sleep(700);
                attempts++;
            }

            Assert.IsTrue(buttonClicked, "Failed to click any button after multiple attempts.");

            // Add any additional assertions or actions after clicking the buttons
            string currentURL = driver.Url;
            Assert.IsTrue(currentURL.Contains("login"), "The URL does not include 'login'");
            driver.Quit();
            Thread.Sleep(5000);
        }

        [Test]
        public void TestThatValidCredentialsIsAuthenticatedAndRedirectedAfterLogout()
        {
            driver.Navigate().GoToUrl(globalURL);

            // Find the Login button by its link text and click it
            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));
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
            string currentURL = driver.Url;
            Assert.AreEqual(globalURL, currentURL, "The URL is not the homepage");

            IWebElement userWelcome = driver.FindElement(By.Id("userWelcome"));

            Assert.AreEqual(userWelcome.Text, "Welcome storiesoftheland","The user is not displayed correctly");

            

            IWebElement outButton = driver.FindElement(By.LinkText("Signout"));

            outButton.Click();

            driver.Navigate().GoToUrl(globalURL);


            // Assert that trying to find the user welcome is not available after logged out
            Assert.Throws<NoSuchElementException>(
                () => driver.FindElement(By.Id("userWelcome")),
                "Element with id 'userWelcome' was found but should not be present."
            );
            driver.Quit();

        }

        [Test]
        public void TestThatValidCredentialsIsAuthenticatedAndRedirected()
        {
            driver.Navigate().GoToUrl(globalURL);

            // Find the Login button by its link text and click it
            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));
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
            while(!buttonClicked)
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
            string currentURL = driver.Url;
            Assert.AreEqual(globalURL, currentURL, "The URL is not the homepage");
            driver.Quit();
        }


    }

}