using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace TestProject
{
    public class ClientInvoicesTests
    {
        private ChromeDriver driver;
        private string baseUrl;
        [SetUp]
        public void Setup()
        {
            baseUrl = Environment.GetEnvironmentVariable("BASEURL") ?? "http://localhost:5000";
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [Test, Order(1)]
        public void CreateClients()
        {
            // Navigate to the Clients page
            driver.Navigate().GoToUrl(baseUrl + "/Clients");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            driver.FindElement(By.Id("Name")).SendKeys("John Doe");
            driver.FindElement(By.Id("ContactInfo")).SendKeys("123-456-7890");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("John Doe"));
        }

        [Test, Order(2)]
        public void EditClients()
        {
            // Navigate to the Clients page
            driver.Navigate().GoToUrl(baseUrl + "/Clients");

            // Find the client to edit
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Jane Doe");


            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Jane Doe"));
        }

        [Test, Order(3)]
        public void CreateInvoice()
        {
            // Navigate to the Invoices page
            driver.Navigate().GoToUrl(baseUrl + "/Invoices");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            var clientDropdown = driver.FindElement(By.Id("ClientId"));
            var select = new OpenQA.Selenium.Support.UI.SelectElement(clientDropdown);
            select.SelectByText("Jane Doe");
            driver.FindElement(By.Id("TotalAmount")).SendKeys("1000");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();

           Assert.IsTrue(driver.PageSource.Contains("1000"));
        }

        [Test, Order(4)]
        public void EditInvoice()
        {
            // Navigate to the Invoices page
            driver.Navigate().GoToUrl(baseUrl + "/Invoices");
            // Find the invoice to edit

            driver.FindElement(By.LinkText("Edit")).Click();
            driver.FindElement(By.Id("TotalAmount")).Clear();
            driver.FindElement(By.Id("TotalAmount")).SendKeys("2000");

            driver.FindElement(By.Id("isPaidCheckbox")).Click(); 

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();

            Assert.IsTrue(driver.PageSource.Contains("2000"));

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
