using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace AvilaShellApp.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        /*
        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }
        */

        [Test]
        public void PhoneNumberIsDisplayed()
        {
 
            AppResult[] results = app.WaitForElement(c => c.Marked("Téléphone"));
            app.Screenshot("HomePage");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void Home_Tab_IsDisplayed()
        {

            AppResult[] results = app.WaitForElement(c => c.Marked("Accueil"));
            app.Screenshot("HomePage");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void News_Tab_IsDisplayed()
        {

            AppResult[] results = app.WaitForElement(c => c.Marked("Actualités"));
            app.Screenshot("HomePage");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void Schedule_Tab_IsDisplayed()
        {

            AppResult[] results = app.WaitForElement(c => c.Marked("Rendez-vous"));
            app.Screenshot("HomePage");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void About_Tab_IsDisplayed()
        {

            AppResult[] results = app.WaitForElement(c => c.Marked("A propos"));
            app.Screenshot("HomePage");

            Assert.IsTrue(results.Any());
        }
    }
}
