using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProject
{
    // Requires both client and employee tests to be ran first
    public class ServiceTests
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

        [Test, Order(1)]
        public void CreateService()
        {
            // Navigate to the Services page
            driver.Navigate().GoToUrl(baseUrl + "/Services");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            driver.FindElement(By.Id("Name")).SendKeys("Rental");
            driver.FindElement(By.Id("Description")).SendKeys("Renting out equipment");
            driver.FindElement(By.Id("Rate")).SendKeys("100");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            // Create another service
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Repair");
            driver.FindElement(By.Id("Description")).SendKeys("Renting out equipment");
            driver.FindElement(By.Id("Rate")).SendKeys("50");
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(driver.PageSource.Contains("Rental"));
                Assert.IsTrue(driver.PageSource.Contains("Repair"));
            });
        }

        [Test, Order(2)]
        public void EditService()
        {
            // Navigate to the Services page
            driver.Navigate().GoToUrl(baseUrl + "/Services");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the rate
            driver.FindElement(By.Id("Rate")).Clear();
            driver.FindElement(By.Id("Rate")).SendKeys("200");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();

            Assert.IsTrue(driver.PageSource.Contains("200"));
        }

        [Test, Order(3)]
        public void CreateServiceBooking()
        {
            // Navigate to the ServiceBookings page
            driver.Navigate().GoToUrl(baseUrl + "/ServiceBookings");
            driver.FindElement(By.LinkText("Create New")).Click();

            //Fill out the form
            var clientDropdown = driver.FindElement(By.Name("ClientId"));
            var clientSelect = new OpenQA.Selenium.Support.UI.SelectElement(clientDropdown);
            clientSelect.SelectByText("Jane Doe");

            var serviceDropdown = driver.FindElement(By.Name("ServiceId"));
            var serviceSelect = new OpenQA.Selenium.Support.UI.SelectElement(serviceDropdown);
            serviceSelect.SelectByText("Rental");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Rental"));    
        }

        [Test, Order(4)]
        public void EditServiceBooking()
        {
            // Navigate to the ServiceBookings page
            driver.Navigate().GoToUrl(baseUrl + "/ServiceBookings");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the service
            var serviceDropdown = driver.FindElement(By.Id("ServiceId"));
            var serviceSelect = new OpenQA.Selenium.Support.UI.SelectElement(serviceDropdown);
            serviceSelect.SelectByText("Repair");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Repair"));
        }

        [Test, Order(5)]
        public void CreateServiceSchedule()
        {
            // Navigate to the ServiceSchedules page
            driver.Navigate().GoToUrl(baseUrl + "/ServiceSchedule");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            var employeeDropdown = driver.FindElement(By.Name("EmployeeId"));
            var employeeSelect = new OpenQA.Selenium.Support.UI.SelectElement(employeeDropdown);
            employeeSelect.SelectByText("Homer Simpson");

            var serviceDropdown = driver.FindElement(By.Name("ServiceId"));
            var serviceSelect = new OpenQA.Selenium.Support.UI.SelectElement(serviceDropdown);
            serviceSelect.SelectByText("Rental");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Homer Simpson"));
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
