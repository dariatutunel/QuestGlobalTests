using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class AdditionalTest1
    {
        [Test]
        public async Task RecommendedItemsCarousel()
        {
            //1. Launch Browser & go to website
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads
            await page.GotoAsync("https://automationexercise.com/");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //2. Scroll to Recommended Items Carousel
            var recommendedItems = page.Locator(".recommended_items");
            await recommendedItems.ScrollIntoViewIfNeededAsync();
            await Assertions.Expect(page.Locator("h2:has-text('Recommended Items')").First).ToBeVisibleAsync();

            //3. Test carousel functionality (arrows working and products changing)
            var visibleProduct = page.Locator("#recommended-item-carousel .item.active .productinfo p").First;
            string nameBefore = await visibleProduct.InnerTextAsync();

            var rightArrow = page.Locator(".right.recommended-item-control");
            await rightArrow.ClickAsync();
            await Assertions.Expect(visibleProduct).Not.ToHaveTextAsync(nameBefore);
            string nameAfterRight = await visibleProduct.InnerTextAsync();
            Assert.That(nameBefore, Is.Not.EqualTo(nameAfterRight), "Right arrow is working!");

            var leftArrow = page.Locator(".left.recommended-item-control");
            await leftArrow.ClickAsync();
            await Assertions.Expect(visibleProduct).ToHaveTextAsync(nameBefore);
            string nameAfterLeft = await visibleProduct.InnerTextAsync();
            Assert.That(nameBefore, Is.EqualTo(nameAfterLeft), "Left arrow is working!");

        }
    }
}
