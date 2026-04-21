using Microsoft.Playwright;
using System.Threading.Tasks;

namespace QuestGlobalTests
{
    public class RemovePopUp // for GDPR consent banner
    {
        public static async Task PopUp(IPage page)
        {
            try
            {
                await page.ClickAsync("button:has-text('Consent')", new PageClickOptions { Timeout = 3000 });
            }
            catch
            {

            }
        }
        public static async Task AdBlock(IPage page) //blocks Google Ads and Vignettes
        {
            await page.RouteAsync("**/*", async route =>
            {
                string url = route.Request.Url.ToLower();
                if (url.Contains("googleads") || url.Contains("googlesyndication") || url.Contains("doubleclick"))
                {
                    await route.AbortAsync();
                }
                else
                {
                    await route.ContinueAsync();
                }
            });
        }
    }
}