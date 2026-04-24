using Microsoft.Playwright;
using Microsoft.VisualBasic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationPortofolio
{
    public class TestCase12
    {
        [Test]
        public async Task AddProducts()
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

            //4. Click on 'Products' button
            await page.ClickAsync("a[href='/products']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //5. Hover over first product and click 'Add to cart'
            var firstProduct = page.Locator(".product-image-wrapper").Nth(0);
            await firstProduct.HoverAsync();
            await firstProduct.Locator(".add-to-cart").First.ClickAsync();

            //6. Click 'Continue Shopping' button
            await page.ClickAsync("button:has-text('Continue Shopping')");

            //7. Hover over second product and click 'Add to cart'
            var secondProduct = page.Locator(".product-image-wrapper").Nth(1);
            await secondProduct.HoverAsync();
            await secondProduct.Locator(".add-to-cart").First.ClickAsync();

            //8. Click 'View Cart' button
            await page.ClickAsync("u:has-text('View Cart')");

            //9. Verify both products are added to Cart
            var cartRows = page.Locator("#cart_info_table tbody tr");
            await Assertions.Expect(cartRows).ToHaveCountAsync(2);

            //10. Verify their prices, quantity and total price
            var firstProductRow = cartRows.Nth(0);
            await Assertions.Expect(firstProductRow.Locator(".cart_price")).ToBeVisibleAsync();
            await Assertions.Expect(firstProductRow.Locator(".cart_quantity")).ToBeVisibleAsync();
            await Assertions.Expect(firstProductRow.Locator(".cart_total")).ToBeVisibleAsync();
            var secondProductRow = cartRows.Nth(1);
            await Assertions.Expect(secondProductRow.Locator(".cart_price")).ToBeVisibleAsync();
            await Assertions.Expect(secondProductRow.Locator(".cart_quantity")).ToBeVisibleAsync();
            await Assertions.Expect(secondProductRow.Locator(".cart_total")).ToBeVisibleAsync();

        }
    }
}