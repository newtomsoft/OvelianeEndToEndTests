namespace OvelianeEndToEndTests.Browser.OvelianeScraper;

public sealed class OvelianeScraper : IOvelianeScraper
{
    private readonly ApplicationConfiguration _configuration;
    private readonly SeleniumScraper _scraper;
    private readonly ILogger<EndToEndTestsApplication> _logger;


    public OvelianeScraper(SeleniumScraper scraper, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _scraper = scraper;
        _configuration = configuration;
        _logger = logger;
    }

    public void NavigateHomePage()
    {
        _scraper.SetWindowSize(new Size(1600, 1000));

        if (!string.IsNullOrEmpty(_configuration.ElementClassToFindInHomePage))
            _scraper.NavigateToPage(_configuration.WebsiteUrl, _configuration.ElementClassToFindInHomePage, "ClassName");
        else
            _scraper.NavigateToPage(_configuration.WebsiteUrl, _configuration.ElementIdToFindInHomePage);
    }

    public IOvelianeScraper Click(string id)
    {
        if (string.IsNullOrEmpty(id) is not true) _scraper.ClickId(id);
        return this;
    }

    public void ClickKeyValue(string key, string value) => _scraper.ClickKeyValue(key, value);

    public IOvelianeScraper WriteInId(string id, string word)
    {
        _scraper.SendKeysToId(id, word);
        return this;
    }

    public IOvelianeScraper WaitId(string id)
    {
        _scraper.WaitId(id);
        return this;
    }

    public void NavigateGamePage() => Click(_configuration.ElementIdClickToNavigateGamePage);

    public int Play()
    {
        return 1;
    }

    public void Toto()
    {
        _scraper.ClickFirstXPathWithText("span", "Checkpoints");
    }
}