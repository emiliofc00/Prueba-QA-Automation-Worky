using System;
using Allure.NUnit.Attributes;
using NUnit.Allure.Core;
using Allure.Net.Commons;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualBasic;

namespace formFillTest
{
    [TestFixture]
    [Allure.NUnit.AllureNUnit] 
    [AllureSuite("Form Fill Tests")]
    public class formFillTest
    {
        IWebDriver driver = new ChromeDriver();
        [SetUp]
        [AllureStep("Configuración inicial del navegador y navegación a la URL")]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://hangers-crisbusa.web.app/");
            Assert.That(driver.Title, Is.EqualTo("Jangapp"));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        [AllureStep("Cerrar el navegador")]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        [AllureStep("Desplazar la vista hacia el elemento")]
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
        [AllureOwner("QA Team")]
        [AllureDescription("Verifica que el formulario de contacto se pueda completar y enviar con datos válidos.")]
        [TestCase("Emilio", "Flores", "emiliofc00@gmail.com", "Necesito informes y precios")]
        public void fillForm(string name, string lastName, string eMail, string message)
        {
            driver.FindElement(By.Name("firstname")).SendKeys(name);
            driver.FindElement(By.Name("lastname")).SendKeys(lastName);
            driver.FindElement(By.Name("email")).SendKeys(eMail);
            driver.FindElement(By.Name("message")).SendKeys(message);

            IWebElement button = driver.FindElement(By.XPath("//button[contains(., 'Enviar')]"));
            ScrollToElement(button);
            WaitForElementInViewport(driver, button, 10);
            button.Click();
        }
    }
}
