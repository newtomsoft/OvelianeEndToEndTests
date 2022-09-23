namespace OvelianeEndToEndTests.Browser;

public class EndToEndTestsApplication
{
    private readonly ILogger<EndToEndTestsApplication> _logger;
    private readonly IWebScraper _webScraper;
    private readonly ApplicationConfiguration _configuration;

    public EndToEndTestsApplication(IWebScraper webScraper, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _logger = logger;
        _webScraper = webScraper;
        _configuration = configuration;
    }

    public void Run()
    {
        var scraper = new OvelianeScraper.OvelianeScraper(_webScraper, _configuration, _logger);
        PlayScenario1(scraper);
    }

    private static void PlayScenario1(IOvelianeScraper scraper)
    {
        scraper.NavigateHomePage();
        scraper.WaitId("username").WriteInId("username", "a.goude");
        scraper.WaitId("kc-form-buttons").Click("kc-form-buttons");

        Thread.Sleep(1000);
        scraper.WaitId("password").WriteInId("password", "admin");

        Thread.Sleep(1000);
        scraper.WaitId("kc-login").Click("kc-login");

        Thread.Sleep(1000);
        scraper.ClickKeyValue("data-mat-icon-name", "assets");

        Thread.Sleep(1000);
        scraper.ClickKeyValue("data-mat-icon-name", "compliance");

        //find element by text
        scraper.ClickFirstXPathWithText("//mat-icon", "add");

    }
}