using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class AdditionalTest5
    {
        [Test]
        public async Task VerifyCartSyncAcrossTabs()
        {
            //1. Setup one session and 2 tabs
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var context = await browser.NewContextAsync();
            var tab1 = await context.NewPageAsync();
            var tab2 = await context.NewPageAsync();
            await RemovePopUp.AdBlock(tab1); //Blocks ads
            await RemovePopUp.AdBlock(tab2); //Blocks ads

            //2. Actions in Tab 1
            await tab1.GotoAsync("https://automationexercise.com/products");
            await RemovePopUp.PopUp(tab1); //Consents if PopUp appears
            var firstProduct = tab1.Locator(".productinfo p").First;
            string productTab1= await firstProduct.InnerTextAsync();
            var addToCart = tab1.Locator(".add-to-cart").First;
            await addToCart.ClickAsync();
            await tab1.WaitForTimeoutAsync(1000);

            //3. Verify Tab 2 to sync
            await tab2.GotoAsync("https://automationexercise.com/view_cart");
            await RemovePopUp.PopUp(tab2); //Consents if PopUp appears
            var cartRows = tab2.Locator("#cart_info_table tbody tr");
            await Assertions.Expect(cartRows).ToHaveCountAsync(1);
            string productTab2 = await tab2.Locator(".cart_description h4 a").First.InnerTextAsync();
            Assert.That(productTab1, Is.EqualTo(productTab2), "Tabs do not sync!");

        }
    }
}
           