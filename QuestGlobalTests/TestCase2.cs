using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class TestCase2
    {
        [Test]
        public async Task LoginUser_CorrectDetails()
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

            //4. Click on 'Signup / Login' button
            await page.ClickAsync("a[href='/login']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //5. Verify 'Login to your account' is visible
            await Assertions.Expect(page.Locator("h2:has-text('Login to your account')")).ToBeVisibleAsync();

            //6. Enter correct email and password
            await page.FillAsync("input[data-qa='login-email']", "dariatutunel26@gmail.com");
            await page.FillAsync("input[data-qa='login-password']", "1234");

            //7. Click 'login' button
            await page.ClickAsync("button[data-qa='login-button']");

            //8. Verify that 'Logged in as username' is visible
            await Assertions.Expect(page.Locator("text=Logged in as")).ToBeVisibleAsync();

            //9. Click 'Delete Account' button
            await page.ClickAsync("a[href='/delete_account']");

            //10. Verify that 'ACCOUNT DELETED!' is visible
            await Assertions.Expect(page.Locator("b:has-text('Account Deleted!')")).ToBeVisibleAsync();
        }
    }
}