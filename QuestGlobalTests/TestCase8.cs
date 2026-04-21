using Microsoft.Playwright;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class TestCase8
    {
        [Test]
        public async Task Products()
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

            //6. The products list is visible
            await Assertions.Expect(page.Locator(".features_items")).ToBeVisibleAsync();

            //7. Click on 'View Product' of first product
            await page.ClickAsync("a[href='/product_details/1']");

            //8. User is landed to product detail page
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/product_details/1");

            //9. Verify that detail detail is visible: product name, category, price, availability, condition, brand
            var productInfo = page.Locator(".product-information");
            await Assertions.Expect(productInfo.Locator("h2")).ToBeVisibleAsync();
            await Assertions.Expect(productInfo.Locator("p:has-text('Category:')")).ToBeVisibleAsync();
            await Assertions.Expect(productInfo.Locator("span span").First).ToBeVisibleAsync();
            await Assertions.Expect(productInfo.Locator("p:has-text('Availability:')")).ToBeVisibleAsync();
            await Assertions.Expect(productInfo.Locator("p:has-text('Condition:')")).ToBeVisibleAsync();
            await Assertions.Expect(productInfo.Locator("p:has-text('Brand:')")).ToBeVisibleAsync();
        }
    }
}