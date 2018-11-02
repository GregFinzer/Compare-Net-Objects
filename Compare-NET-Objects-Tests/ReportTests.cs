#if !NETSTANDARD

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
        public void HtmlReportTest()
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movie.html");

            if (File.Exists(expected))
                File.Delete(expected);

            Movie movie1 = new Movie();
            movie1.Name = "Oblivion";
            movie1.PaymentForTomCruise = 2000000M;

            Movie movie2 = new Movie();
            movie2.Name = "Edge of Tomorrow";
            movie2.PaymentForTomCruise = 3000000M;

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = Int32.MaxValue;
            ComparisonResult result = compareLogic.Compare(movie1, movie2);

            HtmlReport htmlReport = new HtmlReport();

            // The config object is to overwrite the defaults
            htmlReport.Config.GenerateFullHtml = true; // if false, it will only generate an html table
            htmlReport.Config.HtmlTitle = "Comparison Report";
            htmlReport.Config.BreadCrumbColumName = "Bread Crumb"; 
            htmlReport.Config.ExpectedColumnName = "Expected";
            htmlReport.Config.ActualColumnName = "Actual";
            //htmlReport.Config.IncludeCustomCSS(".diff-crumb {background: gray;}"); // add some custom css
            htmlReport.OutputFile(result.Differences, expected);

            Assert.IsTrue(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expected)));

            htmlReport.LaunchApplication(expected);
        }

        [Test]
        public void BeyondCompareReportTest()
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "beyondExpected.txt");
            string actual = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "beyondActual.txt");

            if (File.Exists(expected))
                File.Delete(expected);

            if (File.Exists(actual))
                File.Delete(actual);

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
            beyondCompare.OutputFiles(result.Differences, expected, actual);

            Assert.IsTrue(File.Exists(expected));
            Assert.IsTrue(File.Exists(actual));

            if (!string.IsNullOrEmpty(beyondCompare.FindBeyondCompare()))
                beyondCompare.LaunchApplication(expected, actual);
        }

        [Test]
        public void WinMergeReportTest()
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "winMergeExpected.txt");
            string actual = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "winMergeActual.txt");

            if (File.Exists(expected))
                File.Delete(expected);

            if (File.Exists(actual))
                File.Delete(actual);

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
            winMergeReport.OutputFiles(result.Differences, expected, actual);

            Assert.IsTrue(File.Exists(expected));
            Assert.IsTrue(File.Exists(actual));

            if (!string.IsNullOrEmpty(winMergeReport.FindWinMerge()))
                winMergeReport.LaunchApplication(expected,actual);
        }

        [Test]
        public void CsvReportTest()
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movie.csv");

            if (File.Exists(expected))
                File.Delete(expected);

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
            csvReport.OutputFile(result.Differences, expected);

            Assert.IsTrue(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expected)));

            csvReport.LaunchApplication(expected);
        }

        [Test]
        public void UserFriendlyReportTest()
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "movie.txt");

            if (File.Exists(expected))
                File.Delete(expected);

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
            friendlyReport.OutputFile(result.Differences, expected);

            Assert.IsTrue(File.Exists(expected));

            Console.WriteLine(friendlyReport.OutputString(result.Differences));

            friendlyReport.LaunchApplication(expected);
        }

        [Test]
        public void UserFriendlyReportTreeTest()
        {
            string expected = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "entityTree.txt");

            if (File.Exists(expected))
                File.Delete(expected);

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
            friendlyReport.OutputFile(result.Differences, expected);

            Assert.IsTrue(File.Exists(expected));

            Console.WriteLine(friendlyReport.OutputString(result.Differences));
            friendlyReport.LaunchApplication(expected);
        }
    }
}

#endif
