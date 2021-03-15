using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace TestProject1
{
    public class Tests
    {

        IWebDriver driver;
        protected ExtentReports extent;
        protected ExtentKlovReporter klov;
        protected ExtentTest test;

        public string Capture(IWebDriver driver, string nome)
        {
            string caminhoImagem = @"C:\Users\marcio.nascimento\source\repos\TestProject1\TestProject1\relatorios\print" + nome + ".png";
            Screenshot imagem = ((ITakesScreenshot)driver).GetScreenshot();
            imagem.SaveAsFile(caminhoImagem , ScreenshotImageFormat.Png);
            return caminhoImagem;

        }


        public void printScreen(string status,string mensagem, string nomeTest)
        {

            DateTime hora = DateTime.Now;
            string nomeArquivo = nomeTest + hora.ToString("ddMMyy_hhmmss") + ".png";
            string caminhoPrint = Capture(driver, nomeArquivo);

            if (status == "passou")
            {
                test.Log(Status.Pass, mensagem);
            }
            else if (status == "falhou")
            {
                test.Log(Status.Fail, mensagem);
            }
            test.AddScreenCaptureFromPath(caminhoPrint);
        }

        [SetUp]
        public void Setup()
        {
            var htmlReport = new ExtentHtmlReporter(@"C:\Users\marcio.nascimento\source\repos\TestProject1\TestProject1\relatorios\relatorio.html");
            htmlReport.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent = new ExtentReports();
            klov = new ExtentKlovReporter();
            extent.AttachReporter(htmlReport);
        
            driver = new ChromeDriver();
            driver.Url = "https://opensource-demo.orangehrmlive.com/";
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            
        }

        [Test]
        public void TestLogin()
        {
            string nomeTest = "TestLogin";
            test = extent.CreateTest(nomeTest);
            printScreen("passou", "Tela inicio", nomeTest);
            driver.FindElement(By.Id("txtUsername")).SendKeys("Admin");
            driver.FindElement(By.Id("txtPassword")).SendKeys("admin123");
            driver.FindElement(By.Id("btnLogin")).Click();

            if (driver.FindElement(By.XPath("//img[@src='/webres_5fee89a90600f2.94309009/themes/default/images/logo.png']")).Displayed){
                printScreen("passou","Login Realizado com sucesso", nomeTest);
                Assert.Pass();
            }
            else
            {
                printScreen("falhou", "Falha ao tentar login", nomeTest);
                Assert.Fail();
            }
            
        }

        //public MediaEntityModelProvider PrintScreen(string nome)
        //{
        //        var screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsBase64Encodedstring;
        //        return MediaEntityBuilder.CreateScreenCaptureFromBase64string(screenshot, nome).Build();
        //}
            

        [TearDown]
        public void fechaNavegador()
        {
            driver.Close();
            extent.Flush();
        }
    }
}