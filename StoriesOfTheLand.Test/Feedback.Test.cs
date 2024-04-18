using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoriesOfTheLand.Models;
using StoriesOfTheLand.Controllers;

namespace StoriesOfTheLand.Test;

public class FeedbackUnitTests
{
    private Feedback FeedbackObject;

    [SetUp]
    public void SetUp()
    {
        // Sets up the Feedback object for use in testing
        FeedbackObject = new Feedback()
        {
            Name = "Testy McTesterson",
            Email = "TestyM@gmail.com",
            Subject = "Feedback about Wild Mint specimen",
            SpecimenID = 1,
            Details = "This is just a test of the details field in the Feedback object.",
            Status = Status.New,
            CreateDate = DateTime.Now,
        };
    }

    [Test]
    public void testValidFeedbackCanBeSubmitted()
    {
        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.IsEmpty(errors); // Ensure no errors are thrown
    }

    #region Name
    [Test]
    // Test name lower bound
    public void testNameFieldMustHaveAtLeast2Characters()
    {
        FeedbackObject.Name = "A"; // Assign a name containing a single A

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure one error is thrown
        Assert.AreEqual("Name length must be between 2 and 25", errors[0].ErrorMessage);  // Ensure error is correct
    }

    [Test]
    // Test name upper bound
    public void testNameFieldCannotExceed25Characters()
    {
        FeedbackObject.Name = new string('A', 26); // Assign a name containing 26 A's

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure one error is thrown
        Assert.AreEqual("Name length must be between 2 and 25", errors[0].ErrorMessage);  // Ensure error is correct
    }

    [Test]
    //Test name upper bound
    public void testNameFieldCanGoUpTo25Characters()
    {
        FeedbackObject.Name = new string('A', 25); // Assign a name containing 25 A's

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.IsEmpty(errors); // Ensure no errors are thrown
    }

    #endregion


    #region Email

    [Test]
    // Test email required
    public void testThatEmailFieldIsRequired()
    {
        FeedbackObject.Email = "";

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure one error is thrown
        Assert.AreEqual("Please leave an email to contact", errors[0].ErrorMessage);  // Ensure error is correct
    }

    [Test]
    // Test email validity
    public void testEmailFieldCannotHaveInvalidEmail()
    {
        FeedbackObject.Email = "test.test";

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure one error is thrown
        Assert.AreEqual("Email must be a valid email address", errors[0].ErrorMessage);  // Ensure error is correct
    }

    #endregion


    #region Details

    [Test]
    // Test Details lower bound
    public void testThatDetailsFieldMustHaveAtLeast30Characters()
    {
        FeedbackObject.Details = new string('C', 29);

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure one error is thrown
        Assert.AreEqual("Details length must be between 30 and 2000", errors[0].ErrorMessage);  // Ensure error is correct
    }

    [Test]
    // Test Details upper bound
    public void testThatDetailsFieldCannotHaveMoreThan2000Characters()
    {
        FeedbackObject.Details = new string('D', 2001);

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure one error is thrown
        Assert.AreEqual("Details length must be between 30 and 2000", errors[0].ErrorMessage);  // Ensure error is correct
    }

    [Test]
    // Test Details lower bound
    public void testThatDetailsFieldCanHaveUpTo2000Characters()
    {
        FeedbackObject.Details = new string('E', 2000);

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.IsEmpty(errors); // Ensure no errors are thrown
    }



    #endregion


    #region Status
    [Test]
    public void TestStatusValidation()
    {
        // Define the valid status values
        Status[] validStatuses = { Status.New, Status.InProgress, Status.PendingReponse, Status.Resolved };

        // Iterate through each valid status
        foreach (var status in validStatuses)
        {
            FeedbackObject.Status = status;

            var errors = ValidationHelper.Validate(FeedbackObject);
            Assert.AreEqual(0, errors.Count, $"For status: {status}"); // Ensure there are no errors
        }

        // Test for invalid status values
        FeedbackObject.Status = (Status)4;
        var invalidErrors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, invalidErrors.Count); // Ensure there is one error
        Assert.AreEqual("Status must be between 0 and 3.", invalidErrors[0].ErrorMessage);

        FeedbackObject.Status = (Status)(-1);
        var negativeInvalidErrors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, negativeInvalidErrors.Count); // Ensure there is one error
        Assert.AreEqual("Status must be between 0 and 3.", negativeInvalidErrors[0].ErrorMessage);
    }


    #endregion

    #region Date
    [Test]
    public void testValidDateNow()
    {
        FeedbackObject.CreateDate = DateTime.Now;

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(0, errors.Count); // Ensure there are no errors
    }
    [Test]
    public void testInvalidDateFuture()
    {
        FeedbackObject.CreateDate = new DateTime(2025, 2, 15);

        var errors = ValidationHelper.Validate(FeedbackObject);
        Assert.AreEqual(1, errors.Count); // Ensure there are no errors
        Assert.AreEqual("Date must not be in the future", errors[0].ErrorMessage);  // Ensure error is correct
    }
  

    #endregion
}
