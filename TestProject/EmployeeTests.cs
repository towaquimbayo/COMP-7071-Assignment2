using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject
{
    public class EmployeeTests
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
        public void CreateEmployeeTest()
        {
            // Navigate to the Employees page
            driver.Navigate().GoToUrl(baseUrl + "/Employees");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
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


        [Test, Order(2)]
        public void EditEmployeeTest()
        {
            // Navigate to the Employees page
            driver.Navigate().GoToUrl(baseUrl + "/Employees");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the form
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Burns C. Montgomery");
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Burns C. Montgomery"));
        }

        [Test, Order(3)]
        public void CreateEmployeeWithManagerTest()
        {
            // Navigate to the Employees page
            driver.Navigate().GoToUrl(baseUrl + "/Employees");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            driver.FindElement(By.Id("Name")).SendKeys("Homer Simpson");
            driver.FindElement(By.Id("Address")).SendKeys("742 Evergreen Terrace");
            driver.FindElement(By.Id("EmergencyContact")).SendKeys("555-555-5555");
            driver.FindElement(By.Id("JobTitle")).SendKeys("Nuclear Safety Inspector");
            driver.FindElement(By.Id("PayRate")).SendKeys("5000");

            // For manager, select the first option
            var managerDropdown = driver.FindElement(By.Id("ManagerId"));
            var select = new OpenQA.Selenium.Support.UI.SelectElement(managerDropdown);
            select.SelectByText("Burns C. Montgomery");

            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Homer Simpson"));
        }

        //[Test, Order(4)]
        //public void CreatePayrollTests()
        //{
        //    // Navigate to the Payroll page
        //    driver.Navigate().GoToUrl(baseUrl + "/Payroll");
        //    driver.FindElement(By.LinkText("Create New")).Click();

        //    // Fill out the form
        //    var employeeDropdown = driver.FindElement(By.Id("EmployeeId"));
        //    var select = new OpenQA.Selenium.Support.UI.SelectElement(employeeDropdown);
        //    select.SelectByText("Homer Simpson");

        //    driver.FindElement(By.Id("BasePay")).SendKeys("5000");
        //    driver.FindElement(By.Id("Deductions")).SendKeys("200");
        //    driver.FindElement(By.Id("OvertimePay")).SendKeys("500");
        //    driver.FindElement(By.Id("TaxRate")).SendKeys("0.1");
        //    driver.FindElement(By.Id("NetPay")).SendKeys("4770");

        //    // Submit the form
        //    // Scroll + wait before clicking submit
        //    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //    var submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//input[@type='submit']")));
        //    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", submitButton);
        //    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(submitButton)).Click();

        //    driver.Navigate().Refresh();
        //    Assert.IsTrue(driver.PageSource.Contains("4770"));
        //}

        //[Test, Order(5)]
        //public void EditPayrollTests()
        //{
        //    // Navigate to the Payroll page
        //    driver.Navigate().GoToUrl(baseUrl + "/Payroll");
        //    driver.FindElement(By.LinkText("Edit")).Click();

        //    // Edit the form
        //    driver.FindElement(By.Id("BasePay")).Clear();
        //    driver.FindElement(By.Id("BasePay")).SendKeys("2000");

        //    driver.FindElement(By.Id("NetPay")).Clear();
        //    driver.FindElement(By.Id("NetPay")).SendKeys("4950");
        //    // Scroll + wait before clicking submit
        //    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //    var submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//input[@type='submit']")));
        //    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", submitButton);
        //    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(submitButton)).Click();


        //    driver.Navigate().Refresh();
        //    Assert.IsTrue(driver.PageSource.Contains("4950"));
        //}

        [Test, Order(6)]
        public void CreateShiftsTests()
        {
            // Navigate to the Shifts page
            driver.Navigate().GoToUrl(baseUrl + "/Shifts");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            var employeeDropdown = driver.FindElement(By.Id("EmployeeId"));
            var select = new OpenQA.Selenium.Support.UI.SelectElement(employeeDropdown);
            select.SelectByText("Homer Simpson");

            driver.FindElement(By.Id("isRecurringCheckbox")).Click();

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Homer Simpson"));
        }

        [Test, Order(7)]
        public void CreateVacationRequestsTest()
        {
            // Navigate to the VacationRequests page
            driver.Navigate().GoToUrl(baseUrl + "/VacationRequests");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            var employeeDropdown = driver.FindElement(By.Id("EmployeeId"));
            var select = new OpenQA.Selenium.Support.UI.SelectElement(employeeDropdown);
            select.SelectByText("Homer Simpson");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Homer Simpson"));

        }

        //[Test, Order(8)]
        //public void EditVacationRequestsTest()
        //{
        //    // Navigate to the VacationRequests page
        //    driver.Navigate().GoToUrl(baseUrl + "/VacationRequests");
        //    driver.FindElement(By.LinkText("Edit")).Click();
        //    // Edit the form
        //    driver.FindElement(By.Id("ApprovedByManager")).Click();
        //    var approvalDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        //    driver.FindElement(By.Id("ApprovalDate")).SendKeys(approvalDate);

        //    // Submit the form
        //    driver.FindElement(By.XPath("//input[@type='submit']")).Click();
        //    driver.Navigate().Refresh();
        //    Assert.IsTrue(driver.PageSource.Contains(approvalDate));

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
