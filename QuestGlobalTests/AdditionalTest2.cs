using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class AdditionalTest2
    {
        [Test]
        public async Task VerifyLinksInMenu()
        {
            //1. Launch Browser & go to website
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 });
            var page = await browser.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads
            await page.GotoAsync("https://automationexercise.com/");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            await page.ClickAsync("a[href='/login']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears
            await Assertions.Expect(page.Locator("h2:has-text('Login to your account')")).ToBeVisibleAsync();
            await page.FillAsync("input[data-qa='login-email']", "dariatutunel26@gmail.com");
            await page.FillAsync("input[data-qa='login-password']", "1234");
            await page.ClickAsync("button[data-qa='login-button']");

            //2. Extract all the links from the menu
            var menuLinks = page.Locator(".shop-menu.pull-right ul li a");
            int linkCount = await menuLinks.CountAsync();  
            List<string> urllist = new List<string>();
            for(int i=0;  i<linkCount; i++)
            {
                string? href = await menuLinks.Nth(i).GetAttributeAsync("href");
                if (!string.IsNullOrEmpty(href) )
                {
                    if(href.StartsWith("/"))
                    {
                        href = "https://automationexercise.com" + href;
                    }
                    urllist.Add(href);
                }
            }

            //3. Test each link 
            var apiContext = await playwright.APIRequest.NewContextAsync();
            Assert.Multiple(async () =>
            {
                foreach (string url in urllist)
                {
                    var response = await apiContext.GetAsync(url);
                    Assert.That(response.Ok, Is.True, $"[Broken link detected!]: {url} has returned error code: {response.Status}");
                }
            });

        }
    }
}