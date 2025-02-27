using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace TestProject1
{
    public class EmployeeTests
    {
        private ChromeDriver driver;
        private string baseUrl = "http://localhost:5285";
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
            driver.Navigate().GoToUrl(baseUrl + "/Employees");
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Homer Simpson");
            driver.FindElement(By.Id("Address")).SendKeys("Springfield");
            driver.FindElement(By.Id("JobTitle")).SendKeys("Nuclear Safety Inspector"); 
            driver.FindElement(By.Id("PayRate")).SendKeys("1000");  
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
            driver.FindElement(By.Id("Name")).SendKeys("Homer J. Simpson");
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Homer J. Simpson"));
        }

        [Test]
        public void GeneratePayrollTest()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Payroll");
            driver.FindElement(By.LinkText("Generate Payroll")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Payroll Generated"));
        }

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