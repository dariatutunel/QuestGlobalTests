using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class TestCase1
    {
        [Test]
        public async Task RegisterUser()
        {
            //1. Launch browser
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 200 }); // Am pus 200 ca să se miște mai repede formularul
            var page = await browser.NewPageAsync();
            await RemovePopUp.AdBlock(page); //Blocks ads

            //2. Navigate to url
            await page.GotoAsync("https://automationexercise.com/");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //3. Verify home page
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/");

            //4. Click on 'Signup / Login' button
            await page.ClickAsync("a[href='/login']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //5. Verify 'New User Signup!' is visible
            await Assertions.Expect(page.Locator("h2:has-text('New User Signup!')")).ToBeVisibleAsync();

            //6. Enter name and email address 
            await page.FillAsync("input[data-qa='signup-name']", "Daria1234");
            await page.FillAsync("input[data-qa='signup-email']", "daria1234@gmail.com");

            //7. Click 'Signup' button
            await page.ClickAsync("button[data-qa='signup-button']");

            //8. Verify that 'ENTER ACCOUNT INFORMATION' is visible
            await Assertions.Expect(page.Locator("b:has-text('Enter Account Information')")).ToBeVisibleAsync();

            //9. Fill details: Title, Name, Email, Password, Date of birth
            await page.ClickAsync("#id_gender2"); 
            await page.FillAsync("input[data-qa='password']", "ParolaMeaSecreta123");
            await page.SelectOptionAsync("select[data-qa='days']", new[] { "26" });
            await page.SelectOptionAsync("select[data-qa='months']", new[] { "5" }); 
            await page.SelectOptionAsync("select[data-qa='years']", new[] { "2000" });

            //10 & 11. Select checkboxes
            await page.CheckAsync("#newsletter");
            await page.CheckAsync("#optin");

            //12. Fill details 
            await page.FillAsync("input[data-qa='first_name']", "Daria");
            await page.FillAsync("input[data-qa='last_name']", "Andreea");
            await page.FillAsync("input[data-qa='company']", "Quest Global");
            await page.FillAsync("input[data-qa='address']", "Strada Testarii, Nr. 1");
            await page.SelectOptionAsync("select[data-qa='country']", new[] { "United States" });
            await page.FillAsync("input[data-qa='state']", "California");
            await page.FillAsync("input[data-qa='city']", "Los Angeles");
            await page.FillAsync("input[data-qa='zipcode']", "90001");
            await page.FillAsync("input[data-qa='mobile_number']", "0722112233");

            //13. Click 'Create Account button'
            await page.ClickAsync("button[data-qa='create-account']");

            //14. Verify that 'ACCOUNT CREATED!' is visible
            await Assertions.Expect(page.Locator("b:has-text('Account Created!')")).ToBeVisibleAsync();

            //15. Click 'Continue' button
            await page.ClickAsync("a[data-qa='continue-button']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //16. Verify that 'Logged in as username' is visible
            await Assertions.Expect(page.Locator("text=Logged in as")).ToBeVisibleAsync();

            //17. Click 'Delete Account' button
            await page.ClickAsync("a[href='/delete_account']");

            //18. Verify 'ACCOUNT DELETED!' is visible and click 'Continue' button
            await Assertions.Expect(page.Locator("b:has-text('Account Deleted!')")).ToBeVisibleAsync();
            await page.ClickAsync("a[data-qa='continue-button']");
        }
    }
}