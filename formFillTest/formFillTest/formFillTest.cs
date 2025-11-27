using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Net.Mail;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.DevTools.V140.Browser;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.DevTools.V140.Input;



namespace formFillTest;
public class formFillTest
{
    IWebDriver driver = new ChromeDriver();
    [SetUp]
    public void SetUp()
    {
        driver.Navigate().GoToUrl("https://hangers-crisbusa.web.app/");
        Assert.That(driver.Title, Is.EqualTo("Jangapp"));
        driver.Manage().Window.Maximize();
    }

    [TearDown]
    public void TearDown()
    {
        if (driver != null)
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    public void ScrollToElement(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center', behavior: 'fast'});", element);
        }
public void WaitForElementInViewport(IWebDriver driver, IWebElement element, int timeoutInSeconds)
{
    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
    
    wait.Until(d =>
    {
        try
        {
            return (bool)((IJavaScriptExecutor)d).ExecuteScript(
                "var elem = arguments[0],                 " +
                "  box = elem.getBoundingClientRect(),    " +
                "  cx = box.left + box.width / 2,         " +
                "  cy = box.top + box.height / 2,         " +
                "  e = document.elementFromPoint(cx, cy); " +
                "for (; e; e = e.parentElement) {         " +
                "  if (e === elem)                        " +
                "    return true;                          " +
                "}                                        " +
                "return false;", element);
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    });
}
        
    [Test]
    [TestCase("Emilio","Flores","emiliofc00@gmail.com","Necesito informes y precios")]
    public void fillForm(string name,string lastName,string eMail,string message)
    {

        driver.FindElement(By.Name("firstname")).SendKeys(name);
        driver.FindElement(By.Name("lastname")).SendKeys(lastName);
        driver.FindElement(By.Name("email")).SendKeys(eMail);
        driver.FindElement(By.Name("message")).SendKeys(message);
        IWebElement button = driver.FindElement(By.XPath("//button[contains(., 'Enviar')]"));
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        ScrollToElement(button);
        WaitForElementInViewport(driver, button, 10);
        button.Click();
    }
}
