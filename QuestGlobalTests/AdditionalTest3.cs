using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class AdditionalTest3
    {
        [Test]
        public async Task MobileViewportResponsiveness()
        {
            //1. Create a virtual phone
            using var playwright = await Playwright.CreateAsync();
            var iphone = playwright.Devices["iPhone 13"];
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads
            await page.GotoAsync("https://automationexercise.com/");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //2. Standard Desktop Menu should be replaced by a "Hamburger" Menu button
            var desktopMenu = page.Locator(".shop-menu ul");
            await Assertions.Expect(desktopMenu).Not.ToBeVisibleAsync();
            var hamburgerMenuIcon = page.Locator(".navbar-toggle");
            await Assertions.Expect(hamburgerMenuIcon).ToBeVisibleAsync();

            //3. Verify "Hamburger" Menu responsiveness
            await hamburgerMenuIcon.ClickAsync();
            await page.WaitForTimeoutAsync(1000);
            var mobileHomeLink = page.Locator(".navbar-collapse ul li a:has-text('Home')").First;
            await Assertions.Expect(mobileHomeLink).ToBeVisibleAsync();
            await page.WaitForTimeoutAsync(1000);
        }
    }
}
