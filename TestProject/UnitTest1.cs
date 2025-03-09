using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace TestProject
{
    public class EmployeeTests
    {
        private ChromeDriver driver;
        private string baseUrl = "https://localhost:44348";
        [SetUp]
        public void Setup()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Test]
        public void CreateEmployeeTest()
        {
            // Navigate to the Employees page
            driver.Navigate().GoToUrl(baseUrl + "/Employees");

            // Fill out the form
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Burns Montgomery");
            driver.FindElement(By.Id("Address")).SendKeys("Springfield");
            driver.FindElement(By.Id("EmergencyContact")).SendKeys("777-777-7777");
            driver.FindElement(By.Id("JobTitle")).SendKeys("Nuclear Power Plant Owner");
            driver.FindElement(By.Id("PayRate")).SendKeys("50000");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Burns Montgomery"));
        }

        [Test]
        public void CreateEmployeeWithManagerTest()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Employees");
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Homer Simpson");
            driver.FindElement(By.Id("Address")).SendKeys("742 Evergreen Terrace");
            driver.FindElement(By.Id("EmergencyContact")).SendKeys("555-555-5555");
            driver.FindElement(By.Id("JobTitle")).SendKeys("Nuclear Safety Inspector");
            driver.FindElement(By.Id("PayRate")).SendKeys("5000");

            // For manager, select the first option
            var managerDropdown = driver.FindElement(By.Id("ManagerId"));
            var select = new OpenQA.Selenium.Support.UI.SelectElement(managerDropdown);
            select.SelectByText("Burns Montgomery");

            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Homer Simpson"));
        }

        [Test]
        public void EditEmployeeTest()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Employees");
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Burns C. Montgomery");
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Burns C. Montgomery"));
        }

        //[Test]
        //public void GeneratePayrollTest()
        //{
        //    driver.Navigate().GoToUrl(baseUrl + "/Payroll");
        //    driver.FindElement(By.LinkText("Generate Payroll")).Click();
        //    driver.Navigate().Refresh();
        //    Assert.IsTrue(driver.PageSource.Contains("Payroll Generated"));
        //}

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
        }

    }
}