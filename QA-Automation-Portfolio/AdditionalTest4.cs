using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationPortofolio
{
    public class AdditionalTest4
    {
        [Test]
        public async Task VerifySessionPersistenceOnReload()
        {
            //1. Launch Browser & go to website
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads
            await page.GotoAsync("https://automationexercise.com/products");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //2. Add product to cart
            var firstAddToCart = page.Locator(".add-to-cart").First;
            await firstAddToCart.ClickAsync();
            var viewCartLink = page.Locator("u:has-text('View Cart')");
            await viewCartLink.ClickAsync();

            //3. Verify product is in cart
            var cartRows = page.Locator("#cart_info_table tbody tr");
            await Assertions.Expect(cartRows).ToHaveCountAsync(1);
            string productNameBefore = await page.Locator(".cart_description h4 a").First.InnerTextAsync();

            //4. Reload page
            await page.ReloadAsync();
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            //5. Verify that the product is still in cart
            await Assertions.Expect(cartRows).ToHaveCountAsync(1);
            string productNameAfter = await page.Locator(".cart_description h4 a").First.InnerTextAsync();
            Assert.That(productNameBefore, Is.EqualTo(productNameAfter), "Cart is empty after reload!");

        }
    }
}