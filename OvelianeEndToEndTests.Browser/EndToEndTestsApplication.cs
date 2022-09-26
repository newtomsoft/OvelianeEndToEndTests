namespace OvelianeEndToEndTests.Browser;

public class EndToEndTestsApplication
{
    private readonly IWebDriverFactory _webDriverFactory;
    private readonly ILogger<EndToEndTestsApplication> _logger;
    private readonly ApplicationConfiguration _configuration;

    public EndToEndTestsApplication(IWebDriverFactory webDriverFactory, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _webDriverFactory = webDriverFactory;
        _logger = logger;
        _configuration = configuration;
    }

    public void Run()
    {
        var scraper = new OvelianeScraper.OvelianeScraper(new SeleniumScraper(_webDriverFactory, _logger), _configuration, _logger);
        PlayScenario1(scraper);
    }

    private static void PlayScenario1(IOvelianeScraper scraper)
    {
        scraper.NavigateHomePage();
        scraper.WaitId("username").WriteInId("username", "a.goude");

        Thread.Sleep(1000);
        scraper.WaitId("kc-form-buttons").Click("kc-form-buttons");

        Thread.Sleep(1000);
        scraper.WaitId("password").WriteInId("password", "admin");

        Thread.Sleep(1000);
        scraper.WaitId("kc-login").Click("kc-login");

        Thread.Sleep(1000);
        scraper.ClickKeyValue("data-mat-icon-name", "assets");

        Thread.Sleep(1000);
        scraper.ClickKeyValue("data-mat-icon-name", "compliance");

        scraper.ClickKeyValue("href", "/webapp/en/compliance/checkpoints");
        //find element by text

    }
}
