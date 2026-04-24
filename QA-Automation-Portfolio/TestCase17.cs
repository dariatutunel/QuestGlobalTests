using Microsoft.Playwright;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationPortofolio
{
    public class TestCase17
    {
        [Test]
        public async Task RemoveProducts()
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

            //4. Add products to cart
            var firstProduct = page.Locator(".product-image-wrapper").Nth(0);
            await firstProduct.HoverAsync();
            await firstProduct.Locator(".add-to-cart").First.ClickAsync();
            await page.ClickAsync("button:has-text('Continue Shopping')");
            var secondProduct = page.Locator(".product-image-wrapper").Nth(1);
            await secondProduct.HoverAsync();
            await secondProduct.Locator(".add-to-cart").First.ClickAsync();
            await page.ClickAsync("button:has-text('Continue Shopping')");

            //5. Click 'Cart' button
            await page.ClickAsync("a[href='/view_cart']");

            //6. Verify that cart page is displayed
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/view_cart");

            //7. Click 'X' button corresponding to particular product
            await page.Locator(".cart_quantity_delete").First.ClickAsync();

            //8. Verify that product is removed from the cart
            var cartRows = page.Locator("#cart_info_table tbody tr");
            await Assertions.Expect(cartRows).ToHaveCountAsync(1);
        }
    }
}