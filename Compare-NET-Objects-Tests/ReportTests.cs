using System;
using System.Collections.Generic;
using System.IO;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.Reports;
using KellermanSoftware.CompareNetObjectsTests.TestClasses;
using NUnit.Framework;

namespace KellermanSoftware.CompareNetObjectsTests
{
    [TestFixture]
    public class ReportTests
    {
        [Test]
        public void BeyondCompareReportTest()
        {
            if (File.Exists("beyondExpected.txt"))
                File.Delete("beyondExpected.txt");

            if (File.Exists("beyondActual.txt"))
                File.Delete("beyondActual.txt");

            Movie beyondExpected = new Movie();
            beyondExpected.Name = "Oblivion";
            beyondExpected.PaymentForTomCruise = 2000000M;

            Movie beyondActual = new Movie();
            beyondActual.Name = "Edge of Tomorrow";
            beyondActual.PaymentForTomCruise = 3000000M;

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = Int32.MaxValue;
            ComparisonResult result = compareLogic.Compare(beyondExpected, beyondActual);

            BeyondCompareReport beyondCompare = new BeyondCompareReport();
            beyondCompare.OutputFiles(result.Differences, "beyondExpected.txt", "beyondActual.txt");

            Assert.IsTrue(File.Exists("beyondExpected.txt"));
            Assert.IsTrue(File.Exists("beyondActual.txt"));

            if (!string.IsNullOrEmpty(beyondCompare.FindBeyondCompare()))
                beyondCompare.LaunchApplication("beyondExpected.txt", "beyondActual.txt");
        }

        [Test]
        public void WinMergeReportTest()
        {
            if (File.Exists("winMergeExpected.txt"))
                File.Delete("winMergeExpected.txt");

            if (File.Exists("winMergeActual.txt"))
                File.Delete("winMergeActual.txt");

            Movie winMergeExpected = new Movie();
            winMergeExpected.Name = "Oblivion";
            winMergeExpected.PaymentForTomCruise = 2000000M;

            Movie winMergeActual = new Movie();
            winMergeActual.Name = "Edge of Tomorrow";
            winMergeActual.PaymentForTomCruise = 3000000M;

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = Int32.MaxValue;
            ComparisonResult result = compareLogic.Compare(winMergeExpected, winMergeActual);

            WinMergeReport winMergeReport = new WinMergeReport();
            winMergeReport.OutputFiles(result.Differences, "winMergeExpected.txt", "winMergeActual.txt");

            Assert.IsTrue(File.Exists("winMergeExpected.txt"));
            Assert.IsTrue(File.Exists("winMergeActual.txt"));

            if (!string.IsNullOrEmpty(winMergeReport.FindWinMerge()))
                winMergeReport.LaunchApplication("winMergeExpected.txt", "winMergeActual.txt");
        }

        [Test]
        public void CsvReportTest()
        {
            if (File.Exists("movie.csv"))
                File.Delete("movie.csv");

            Movie movie1 = new Movie();
            movie1.Name = "Oblivion";
            movie1.PaymentForTomCruise = 2000000M;

            Movie movie2 = new Movie();
            movie2.Name = "Edge of Tomorrow";
            movie2.PaymentForTomCruise = 3000000M;

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = Int32.MaxValue;
            ComparisonResult result = compareLogic.Compare(movie1, movie2);

            CsvReport csvReport = new CsvReport();
            csvReport.OutputFile(result.Differences, "movie.csv");

            Assert.IsTrue(File.Exists("movie.csv"));

            csvReport.LaunchApplication("movie.csv");
        }

        [Test]
        public void UserFriendlyReportTest()
        {
            if (File.Exists("movie.txt"))
                File.Delete("movie.txt");

            Movie movie1 = new Movie();
            movie1.Name = "Oblivion";
            movie1.PaymentForTomCruise = 2000000M;

            Movie movie2 = new Movie();
            movie2.Name = "Edge of Tomorrow";
            movie2.PaymentForTomCruise = 3000000M;

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = Int32.MaxValue;
            ComparisonResult result = compareLogic.Compare(movie1, movie2);

            UserFriendlyReport friendlyReport = new UserFriendlyReport();
            friendlyReport.OutputFile(result.Differences, "movie.txt");

            Assert.IsTrue(File.Exists("movie.txt"));

            Console.WriteLine(friendlyReport.OutputString(result.Differences));

            friendlyReport.LaunchApplication("movie.txt");
        }

        [Test]
        public void UserFriendlyReportTreeTest()
        {
            List<Entity> entityTree = new List<Entity>();

            //Brave Sir Robin Security Company
            Entity top1 = new Entity();
            top1.Description = "Brave Sir Robin Security Company";
            top1.Parent = null;
            top1.EntityLevel = Level.Company;
            entityTree.Add(top1);

            Entity div1 = new Entity();
            div1.Description = "Minstrils";
            div1.EntityLevel = Level.Division;
            div1.Parent = top1;
            top1.Children.Add(div1);

            Entity div2 = new Entity();
            div2.Description = "Sub Contracted Fighting";
            div2.EntityLevel = Level.Division;
            div2.Parent = top1;
            top1.Children.Add(div2);

            Entity dep2 = new Entity();
            dep2.Description = "Trojan Rabbit Department";
            dep2.EntityLevel = Level.Department;
            dep2.Parent = div2;
            div2.Children.Add(dep2);

            //Roger the Shrubber's Fine Shrubberies
            Entity top1b = new Entity();
            top1b.Description = "Roger the Shrubber's Fine Shrubberies";
            top1b.Parent = null;
            top1b.EntityLevel = Level.Company;
            entityTree.Add(top1b);

            Entity div1b = new Entity();
            div1b.Description = "Manufacturing";
            div1b.EntityLevel = Level.Division;
            div1b.Parent = top1;
            top1b.Children.Add(div1);

            Entity dep1b = new Entity();
            dep1b.Description = "Design Department";
            dep1b.EntityLevel = Level.Department;
            dep1b.Parent = div1b;
            div1b.Children.Add(dep1b);

            Entity dep2b = new Entity();
            dep2b.Description = "Arranging Department";
            dep2b.EntityLevel = Level.Department;
            dep2b.Parent = div1b;
            div1b.Children.Add(dep2b);

            Entity div2b = new Entity();
            div2b.Description = "Sales";
            div2b.EntityLevel = Level.Division;
            div2b.Parent = top1;
            top1b.Children.Add(div2b);

            List<Entity> entityTreeCopy = Common.CloneWithSerialization(entityTree);

            entityTreeCopy[1].Children[1].Description = "Retail";

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = Int32.MaxValue;
            ComparisonResult result = compareLogic.Compare(entityTree, entityTreeCopy);

            UserFriendlyReport friendlyReport = new UserFriendlyReport();
            friendlyReport.OutputFile(result.Differences, "entityTree.txt");

            Assert.IsTrue(File.Exists("entityTree.txt"));

            Console.WriteLine(friendlyReport.OutputString(result.Differences));
            friendlyReport.LaunchApplication("entityTree.txt");
        }
    }
}
