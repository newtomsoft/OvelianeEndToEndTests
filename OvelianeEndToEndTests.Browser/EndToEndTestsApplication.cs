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
        var scraper = new OvelianeScraper(new SeleniumScraper(_webDriverFactory, _logger), _configuration, _logger);
        PlayScenario1(scraper);
    }

    private static void PlayScenario1(OvelianeScraper scraper)
    {
        //Home page + login
        scraper.NavigateHomePage();
        scraper.WaitId("username").WriteInId("username", "a.goude");
        scraper.WaitId("kc-form-buttons").ClickId("kc-form-buttons");
        scraper.Delay(1000).WaitId("password").WriteInId("password", "admin");
        scraper.WaitId("kc-login").ClickId("kc-login");

        // ****************************
        //compliance create checkpoint
        // ****************************
        scraper.ClickTagText("span", " Compliance ");
        scraper.ClickTagText("span", "Checkpoints");
        scraper.ClickTagText("span", "New Checkpoint");
        scraper.WriteAttributeValue("formcontrolname", "name", "aaa-checkpoint");
        scraper.WriteAttributeValue("formcontrolname", "description", "This is the description");
        scraper.ClickTagText("span", " High ");
        scraper.WriteAttributeValue("formcontrolname", "source", "CIS");
        scraper.WriteAttributeValue("formcontrolname", "guide", "CIS RHEL 8");
        scraper.WriteAttributeValue("formcontrolname", "item", "1.6.2");
        scraper.WriteAttributeValue("formcontrolname", "command", "this is the command");
        scraper.ClickTagText("span", "Save as");
        scraper.ClickTagText("button", "Approved");
    }
}
