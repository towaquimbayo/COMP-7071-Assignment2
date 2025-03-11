using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace TestProject
{
    public class HousingTests
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
        public void CreateAssetTest()
        {
            // Navigate to the Assets page
            driver.Navigate().GoToUrl(baseUrl + "/Assets");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            driver.FindElement(By.Id("Type")).SendKeys("House");
            driver.FindElement(By.Id("RentAmount")).SendKeys("1000");
            driver.FindElement(By.Id("IsOccupied")).SendKeys("True");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();

            // Create a new asset
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Type")).SendKeys("Complex");
            driver.FindElement(By.Id("RentAmount")).SendKeys("2000");
            driver.FindElement(By.Id("IsOccupied")).SendKeys("False");
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(driver.PageSource.Contains("House"));
                Assert.IsTrue(driver.PageSource.Contains("Complex"));
            });
        }

        [Test, Order(2)]
        public void EditAssetTest()
        {
            // Navigate to the Assets page
            driver.Navigate().GoToUrl(baseUrl + "/Assets");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the form
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys("Apartment");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Apartment"));
        }

        [Test, Order(3)]
        public void CreateRenterTest()
        {
            // Navigate to the Renters page
            driver.Navigate().GoToUrl(baseUrl + "/Renters");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            driver.FindElement(By.Id("Name")).SendKeys("Oswald");
            driver.FindElement(By.Id("ContactInfo")).SendKeys("123-456-7890");
            driver.FindElement(By.Id("EmergencyContact")).SendKeys("098-765-4321");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Oswald"));
        }

        [Test, Order(4)]
        public void EditRenterTest()
        {
            // Navigate to the Renters page
            driver.Navigate().GoToUrl(baseUrl + "/Renters");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the form
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Oswald Jenkins");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Oswald Jenkins"));
        }

        [Test, Order(5)]
        public void CreateRentHistoryTest()
        {
            //Navigate to the RentHistory Page
            driver.Navigate().GoToUrl(baseUrl + "/RentHistory");
            driver.FindElement(By.LinkText("Create New Rent History")).Click();

            // Fill out the form
            var assetDropdown = driver.FindElement(By.Id("AssetId"));
            var selectAsset = new OpenQA.Selenium.Support.UI.SelectElement(assetDropdown);
            selectAsset.SelectByText("Apartment");

            var renterDropdown = driver.FindElement(By.Id("RenterId"));
            var selectRenter = new OpenQA.Selenium.Support.UI.SelectElement(renterDropdown);
            selectRenter.SelectByText("Oswald Jenkins");

            driver.FindElement(By.Id("OldRentAmount")).SendKeys("1000");
            driver.FindElement(By.Id("NewRentAmount")).SendKeys("1200");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("1,200.00"));

        }

        [Test, Order(6)]
        public void EditRentHistoryTest()
        {
            // Navigate to the RentHistory page
            driver.Navigate().GoToUrl(baseUrl + "/RentHistory");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the form
            driver.FindElement(By.Id("NewRentAmount")).Clear();
            driver.FindElement(By.Id("NewRentAmount")).SendKeys("1300");

            // Submit the form
            driver.FindElement(By.XPath("//button[contains(text(), 'Save')]")).Click();
            Assert.IsTrue(driver.PageSource.Contains("1,300.00"));
        }

        [Test, Order(7)]
        public void CreateRentInvoiceTest()
        {
            // Navigate to the RentInvoices page
            driver.Navigate().GoToUrl(baseUrl + "/RentInvoice");
            driver.FindElement(By.LinkText("Generate New Invoice")).Click();

            // Fill out the form
            var assetDropdown = driver.FindElement(By.Id("AssetId"));
            var selectAsset = new OpenQA.Selenium.Support.UI.SelectElement(assetDropdown);
            selectAsset.SelectByText("Apartment");

            var renterDropdown = driver.FindElement(By.Id("RenterId"));
            var selectRenter = new OpenQA.Selenium.Support.UI.SelectElement(renterDropdown);
            selectRenter.SelectByText("Oswald Jenkins");

            driver.FindElement(By.Id("AmountDue")).SendKeys("1300");

            // Submit the form
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("1,300.00"));
        }

        [Test, Order(8)]
        public void EditRentInvoiceTest()
        {
            // Navigate to the RentInvoices page
            driver.Navigate().GoToUrl(baseUrl + "/RentInvoice");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit the form
            driver.FindElement(By.Id("AmountDue")).Clear();
            driver.FindElement(By.Id("AmountDue")).SendKeys("1400");

            // Submit the form
            driver.FindElement(By.XPath("//button[contains(text(), 'Save')]")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("1,400.00"));
        }

        [Test, Order(9)]
        public void CreateOccupancyHistoryTest()
        {
            // Navigate to the OccupancyHistory page
            driver.Navigate().GoToUrl(baseUrl + "/OccupancyHistory");
            driver.FindElement(By.LinkText("Create New")).Click();

            // Fill out the form
            var assetDropdown = driver.FindElement(By.Id("AssetId"));
            var selectAsset = new OpenQA.Selenium.Support.UI.SelectElement(assetDropdown);
            selectAsset.SelectByText("Apartment");

            var renterDropdown = driver.FindElement(By.Id("RenterId"));
            var selectRenter = new OpenQA.Selenium.Support.UI.SelectElement(renterDropdown);
            selectRenter.SelectByText("Oswald Jenkins");

            // Submit the form
            driver.FindElement(By.XPath("//button[contains(text(), 'Save')]")).Click();
            driver.Navigate().Refresh();

            Assert.IsTrue(driver.PageSource.Contains("Oswald Jenkins"));

        }

        [Test, Order(10)]
        public void EditOccupancyHistoryTest()
        {
            // Navigate to the OccupancyHistory page
            driver.Navigate().GoToUrl(baseUrl + "/OccupancyHistory");
            driver.FindElement(By.LinkText("Edit")).Click();

            // Edit Apartment to Complex
            var assetDropdown = driver.FindElement(By.Id("AssetId"));
            var selectAsset = new OpenQA.Selenium.Support.UI.SelectElement(assetDropdown);
            selectAsset.SelectByText("Complex");

            // Submit the form
            driver.FindElement(By.XPath("//input[@type='submit']")).Click();
            driver.Navigate().Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Complex"));
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
