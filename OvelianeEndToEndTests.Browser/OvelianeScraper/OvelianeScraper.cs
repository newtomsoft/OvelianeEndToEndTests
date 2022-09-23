namespace OvelianeEndToEndTests.Browser.OvelianeScraper;

public sealed class OvelianeScraper : IOvelianeScraper
{
    private readonly ApplicationConfiguration _configuration;
    private readonly IWebScraper _webScraper;
    private readonly ILogger<EndToEndTestsApplication> _logger;


    public OvelianeScraper(IWebScraper webScraper, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _webScraper = webScraper;
        _configuration = configuration;
        _logger = logger;
    }

    public void NavigateHomePage()
    {
        _webScraper.SetWindowSize(new Size(1600, 1000));

        if (!string.IsNullOrEmpty(_configuration.ElementClassToFindInHomePage))
            _webScraper.NavigateToPage(_configuration.WebsiteUrl, _configuration.ElementClassToFindInHomePage, "ClassName");
        else
            _webScraper.NavigateToPage(_configuration.WebsiteUrl, _configuration.ElementIdToFindInHomePage);
    }

    public IOvelianeScraper Click(string id)
    {
        if (string.IsNullOrEmpty(id) is not true) _webScraper.ClickId(id);
        return this;
    }

    public void ClickKeyValue(string key, string value) => _webScraper.ClickKeyValue(key, value);

    public IOvelianeScraper WriteInId(string id, string word)
    {
        _webScraper.SendKeysToId(id, word);
        return this;
    }

    public IOvelianeScraper WaitId(string id)
    {
        _webScraper.WaitId(id);
        return this;
    }

    public void NavigateGamePage() => Click(_configuration.ElementIdClickToNavigateGamePage);

    public int Play()
    {
        return 1;
    }

    public void Toto()
    {
        _webScraper.ClickFirstXPathWithText("span", "Checkpoints");
    }
}