using Microsoft.Playwright;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class TestCase9
    {
        [Test]
        public async Task SearchProduct()
        {
            //1. Launch Browser
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads

            //2. Navigate to url
            await page.GotoAsync("https://automationexercise.com/");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //3. Verify homepage is visible
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/");

            //4. Click on 'Contact Us' button
            await page.ClickAsync("a[href='/products']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //5. Verify user is navigated to ALL PRODUCTS page successfully
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/products");

            //6. Enter product name in search input and click search button
            await page.FillAsync("#search_product", "Sleeveless Dress");
            await page.ClickAsync("#submit_search");

            //7. Verify 'SEARCHED PRODUCTS' is visible
            await Assertions.Expect(page.Locator("h2:has-text('SEARCHED PRODUCTS')")).ToBeVisibleAsync();

            //8. Verify all the products related to search are visible
            var productsFound = page.Locator(".product-image-wrapper");
            await Assertions.Expect(productsFound.First).ToBeVisibleAsync();
        }
    }
}