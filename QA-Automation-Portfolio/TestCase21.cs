using Microsoft.Playwright;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace AutomationPortofolio
{
    public class TestCase21
    {
        [Test]
        public async Task Review()
        {
            //1. Launch Browser
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads

            //2. Navigate to url
            await page.GotoAsync("https://automationexercise.com/");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //3. Click on 'Products' button
            await page.ClickAsync("a[href='/products']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            // 4.Verify user is navigated to ALL PRODUCTS page successfully
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/products");

            //5. Click on 'View Product' button
            await page.Locator(".choose a").First.ClickAsync();

            //6. Verify 'Write Your Review' is visible
            await Assertions.Expect(page.Locator("a[href='#reviews']")).ToBeVisibleAsync();

            //7. Enter name, email and review
            await page.FillAsync("#name", "Daria");
            await page.FillAsync("#email", "daria1234@gmail.com");
            await page.FillAsync("#review", "This review was generated using Playwright.");

            //8. Click 'Submit' button
            await page.ClickAsync("#button-review");

            //9. Verify success message 'Thank you for your review.'
            await Assertions.Expect(page.Locator("div.alert-success:has-text('Thank you for your review.')")).ToBeVisibleAsync();
        }
    }
}