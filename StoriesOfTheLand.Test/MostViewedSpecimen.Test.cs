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
using Moq;
using Microsoft.Identity.Client;

namespace StoriesOfTheLand.Test
    {
        public class mostviewedspecimen
        {
            private Mock<IAnalyticsDataClient> mockAnalyticsDataClient;
            [SetUp]
            public void SetUp()
            {
                mockAnalyticsDataClient = new Mock<IAnalyticsDataClient>();
            }
            [Test]
            public void testOnlySpecimensAreReturned()
            {
                // Setup the mock to return a list of tuples with specimen details including a non-specimen entry
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/specimen/Details/1", "1"),
            ("/specimen/Details/2", "10"),
            ("/specimen/Details/3", "3"),
            ("/specimen/Details/4", "4"),
            ("/specimen/Details/5", "5"),
            ("/specimen/Details/6", "6"),
            ("/specimen/Details/7", "7"),
            ("/specimen/Details/8", "8"),
            ("/specimen/Details/9", "9"),
            ("/specimen/Details/10", "10"),
            ("/Specimen/16", "15"), // This should not be included
            ("/Home/Details/15", "100") // This should not be included
                    });
                // Call the method that processes the API response
                var resultAPI = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var mostPopularSpecimen = HomeController.organizedMostViewed(resultAPI); // Assuming organizeMostViewed is adapted for list handling
                                                                                         // Assert to ensure no entries from "Home" or incorrect specimen paths are included
                foreach (var specimen in mostPopularSpecimen)
                {
                    Assert.AreNotEqual(specimen, 15, "The result includes an entry for the Home page, which is not expected.");
                    Assert.AreNotEqual(specimen, 16, "The result includes an entry for the Home page, which is not expected.");
                }
            }
            [Test]
            public void testTopTenSpecimensAreReturned()
            {
                // Setup the mock to return a list of tuples with specimen details, including extra entries
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/Specimen/Details/1", "1"),
            ("/Specimen/Details/2", "1"),
            ("/Specimen/Details/3", "3"),
            ("/Specimen/Details/4", "4"),
            ("/Specimen/Details/5", "5"),
            ("/Specimen/Details/6", "6"),
            ("/Specimen/Details/7", "7"),
            ("/Specimen/Details/8", "8"),
            ("/Specimen/Details/9", "9"),
            ("/Specimen/Details/10", "10"),
            ("/Specimen/Details/11", "11"),
            ("/Specimen/Details/12", "12"),
            ("/Specimen/Details/13", "13"),
            ("/Specimen/Details/14", "14"),
            ("/Specimen/Details/100", "100"), // Highest metric
            ("/Home/Details", "16") // Should not be included
                    });
                // Call the method that is under test
                var resultAPI = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var topSpecimenNumbers = HomeController.organizedMostViewed(resultAPI);
                // Expected specimen numbers in descending order of metric, top 10 only
                int[] expectedSpecimenNumbers = new int[] { 100, 14, 13, 12, 11, 10, 9, 8, 7, 6 };
                // Assert to check if the most popular specimens are returned in the correct order
                Assert.AreEqual(expectedSpecimenNumbers.Length, topSpecimenNumbers.Count, "The number of returned specimens does not match the expected count.");
                for (int i = 0; i < expectedSpecimenNumbers.Length; i++)
                {
                    Assert.AreEqual(expectedSpecimenNumbers[i], topSpecimenNumbers[i], $"The specimen at rank {i + 1} should have ID {expectedSpecimenNumbers[i]}.");
                }
            }
            [Test]
            public void testReturnsExactly10SpecimensID()
            {
                // Setup the mock to return string with specimen details including a non-specimen entry
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/Specimen/Details/1", "1"),
            ("/Specimen/Details/2", "1"),
            ("/Specimen/Details/3", "3"),
            ("/Specimen/Details/4", "4"),
            ("/Specimen/Details/5", "5"),
            ("/Specimen/Details/6", "6"),
            ("/Specimen/Details/7", "7"),
            ("/Specimen/Details/8", "8"),
            ("/Specimen/Details/9", "9"),
            ("/Specimen/Details/10", "10"),
            ("/Specimen/Details/11", "11"),
            ("/Specimen/Details/12", "12"),
            ("/Specimen/Details/13", "13"),
            ("/Specimen/Details/14", "14"),
            ("/Specimen/Details/100", "100"), // Highest metric
            ("/Home/Details", "16") // Should not be included
                    });
                // Call the method that is under test
                var resultAPI = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var mostPopularSpecimen = HomeController.organizedMostViewed(resultAPI);
                foreach (var item in mostPopularSpecimen)
                {
                    Console.WriteLine(item.ToString());
                }
                // Assert that exactly 10 specimen IDs are returned
                Assert.AreEqual(10, mostPopularSpecimen.Count, "The number of returned specimen IDs should be exactly 10.");
            }
            [Test]
            public void TestOneMovesUpOneSpot()
            {
                // Initial setup with lower visit count for specimen 14
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/Specimen/Details/1", "1"),
            ("/Specimen/Details/2", "1"),
            ("/Specimen/Details/3", "3"),
            ("/Specimen/Details/4", "4"),
            ("/Specimen/Details/5", "5"),
            ("/Specimen/Details/6", "6"),
            ("/Specimen/Details/7", "7"),
            ("/Specimen/Details/8", "8"),
            ("/Specimen/Details/9", "9"),
            ("/Specimen/Details/10", "10"),
            ("/Specimen/Details/11", "11"),
            ("/Specimen/Details/12", "12"),
            ("/Specimen/Details/13", "13"),
            ("/Specimen/Details/14", "14"),
            ("/Specimen/Details/15", "15"),
            ("/Home/Details", "16")  // Not included in results
                    });
                var resultAPI = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var mostPopularSpecimen1 = HomeController.organizedMostViewed(resultAPI);
                // Assert initial order before the change
                Assert.AreEqual(15, mostPopularSpecimen1[0], "Initially, specimen 15 should be the most visited.");
                Assert.AreEqual(14, mostPopularSpecimen1[1], "Initially, specimen 14 should be the second most visited.");
                // Change the number of visits for specimen 14 to make it the most visited
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/Specimen/Details/1", "1"),
            ("/Specimen/Details/2", "1"),
            ("/Specimen/Details/3", "3"),
            ("/Specimen/Details/4", "4"),
            ("/Specimen/Details/5", "5"),
            ("/Specimen/Details/6", "6"),
            ("/Specimen/Details/7", "7"),
            ("/Specimen/Details/8", "8"),
            ("/Specimen/Details/9", "9"),
            ("/Specimen/Details/10", "10"),
            ("/Specimen/Details/11", "11"),
            ("/Specimen/Details/12", "12"),
            ("/Specimen/Details/13", "13"),
            ("/Specimen/Details/14", "17"),  // Increased visits
            ("/Specimen/Details/15", "15"),
            ("/Home/Details", "16")  // Not included in results
                    });
                var resultAPI2 = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var mostPopularSpecimen2 = HomeController.organizedMostViewed(resultAPI2);
                // Assert order after the change
                Assert.AreEqual(14, mostPopularSpecimen2[0], "After the change, specimen 14 should be the most visited.");
                Assert.AreEqual(15, mostPopularSpecimen2[1], "After the change, specimen 15 should be the second most visited.");
            }
            [Test]
            public void TestIfThereAreLessThan9specimensShows9()
            {
                // Setup the mock to return string with 9 specimen details
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/Specimen/Details/1", "1"),
            ("/Specimen/Details/2", "1"),
            ("/Specimen/Details/3", "3"),
            ("/Specimen/Details/4", "4"),
            ("/Specimen/Details/5", "5"),
            ("/Specimen/Details/6", "6"),
            ("/Specimen/Details/7", "7"),
            ("/Specimen/Details/8", "8"),
            ("/Specimen/Details/9", "9")
                       // Not included in results
                    });
                // Call the method under test
                var resultAPI = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var mostPopularSpecimen = HomeController.organizedMostViewed(resultAPI);
                // Assert that all 9 specimen IDs are returned
                Assert.AreEqual(9, mostPopularSpecimen.Count, "The number of returned specimen IDs should be exactly 9.");
                foreach (var item in mostPopularSpecimen)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            /// <summary>
            /// This function is going to simulate a api response without any specimen details page. then it will check the return int array of the filtered result is empty
            /// </summary>
            [Test]
            public void TestThereAreNoSpecimens()
            {
                // Mock setup to return a string with no specimen details
                mockAnalyticsDataClient.Setup(x => x.callAPI(It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(new List<(string Dimension, string Metric)>
                    {
            ("/Home/Details", "16")  // Not included in results
                    });
                // Call the method that processes the API response
                var resultAPI = mockAnalyticsDataClient.Object.callAPI("dimensionName", "metricName");
                var mostPopularSpecimen = HomeController.organizedMostViewed(resultAPI);
                // Assert that the returned integer array is empty
                Assert.IsNotNull(mostPopularSpecimen, "The returned array should not be null.");
                Assert.AreEqual(0, mostPopularSpecimen.Count, "The returned array should be empty.");
            }
        }
    }

