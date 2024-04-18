using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StoriesOfTheLand.Controllers;
using StoriesOfTheLand.Models;
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
    public class FaqTests
    {

        Faq faq;
        [SetUp]
        public void SetUp()
        {
           faq = new Faq() { Title = "Can anyone go into the camp", Description = "Yes Everyone is free to  come in and learn the various plants" };


        }

        [Test]
        public void ValidTitle()
        {
            faq.Title = "Can anyone go into the camp?";
            var ValidationResultList = ValidationHelper.Validate(faq);
            
            Assert.That(ValidationResultList.Count, Is.EqualTo(0));

        }
        [Test]
        public void EmptyTitle()
        {
            faq.Title = "";
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(1));
            Assert.That(ValidationResultList[0].ErrorMessage, Is.EqualTo("Title Is Required"));
        }

        [Test]
        public void LowerBoundTitle()
        {
            faq.Title = new string('a', 5);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(0));

        }
        [Test]
        public void BelowLowerboundTitle()
        {
            faq.Title = new string('a', 4);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(1));
            Assert.That(ValidationResultList[0].ErrorMessage, Is.EqualTo("Title must be atleast 5 characters long"));

        }
        [Test]
        public void UpperBoundTitle()
        {
            faq.Title = new string('a', 200);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(0));

        }
        [Test]
        public void AboveUpperBoundTitle()
        {
            faq.Title = new string('a', 201);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(1));
            Assert.That(ValidationResultList[0].ErrorMessage, Is.EqualTo("Title can\'t be more than 200 characters"));

        }
        [Test]
        public void ValidDescription()
        {
            faq.Description = "Yes Anyone can go into the Camp and Learn";
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(0));
        }
        [Test]
        public void EmptyDescription()
        {
            faq.Description = "";
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(1));
            Assert.That(ValidationResultList[0].ErrorMessage, Is.EqualTo("Description Is Required"));
        }

        [Test]
        public void LowerBoundDescription()
        {
            faq.Description = new string('a', 10);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(0));

        }
        [Test]
        public void BelowLowerboundDescription()
        {
            faq.Description = new string('a', 9);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(1));
            Assert.That(ValidationResultList[0].ErrorMessage, Is.EqualTo("Description must be atleast 10 characters"));

        }
        [Test]
        public void UpperBoundDescription()
        {
            faq.Description = new string('a', 500);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(0));

        }
        [Test]
        public void AboveUpperBoundDescription()
        {
            faq.Description = new string('a', 501);
            var ValidationResultList = ValidationHelper.Validate(faq);
            Assert.That(ValidationResultList.Count, Is.EqualTo(1));
            Assert.That(ValidationResultList[0].ErrorMessage, Is.EqualTo("Description can't be more than 500 characters"));

        }


      

    }
}
