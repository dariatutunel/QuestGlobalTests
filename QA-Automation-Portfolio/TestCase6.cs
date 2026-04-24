using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AutomationPortofolio
{
    public class TestCase6
    {
        [Test]
        public async Task ContactUsForm()
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
            await page.ClickAsync("a[href='/contact_us']");
            await RemovePopUp.PopUp(page); //Consents if PopUp appears

            //5. Verify 'GET IN TOUCH' is visible
            await Assertions.Expect(page.Locator("h2:has-text('GET IN TOUCH')")).ToBeVisibleAsync();

            //6. Enter name, email, subject and message 
            await page.FillAsync("input[data-qa='name']", "Daria");
            await page.FillAsync("input[data-qa='email']", "daria1234@gmail.com");
            await page.FillAsync("input[data-qa='subject']", "test");
            await page.FillAsync("textarea[data-qa='message']", "playwright is fun!");

            //7. Upload file
            string filePath = "test_file.txt";
            System.IO.File.WriteAllText(filePath, "This is a test message for upload sent by Playwright.");
            await page.SetInputFilesAsync("input[name='upload_file']", filePath);

            //8 & 9. Click 'Submit' and OK button
            page.Dialog += async (_, dialog) => { await dialog.AcceptAsync(); };
            await page.ClickAsync("input[data-qa='submit-button']");

            //10. Verify success message 'Success! Your details have been submitted successfully.' is visible
            await Assertions.Expect(page.Locator(".status.alert-success:has-text('Success! Your details have been submitted successfully.')")).ToBeVisibleAsync();

            //11. Click 'Home' button and verify that landed to home page successfully
            await page.ClickAsync("a[href='/']");
            await Assertions.Expect(page).ToHaveURLAsync("https://automationexercise.com/");
        }
    }
}
