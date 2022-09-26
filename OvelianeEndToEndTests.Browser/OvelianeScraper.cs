namespace OvelianeEndToEndTests.Browser;

public sealed class OvelianeScraper
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

    public void ClickId(string id) => _scraper.ClickId(id);
    public void ClickAttributeValue(string attributeName, string attributeValue) => _scraper.ClickAttributeValue(attributeName, attributeValue);
    public void ClickTagText(string tag, string text) => _scraper.ClickTagText(tag, text);

    public OvelianeScraper WriteInId(string id, string word)
    {
        _scraper.WriteToId(id, word);
        return this;
    }

    public OvelianeScraper WriteAttributeValue(string attributeName, string attributeValue, string text)
    {
        _scraper.WriteAttributeValue(attributeName, attributeValue, text);
        return this;
    }

    public OvelianeScraper WaitId(string id)
    {
        _scraper.WaitId(id);
        return this;
    }

    public void NavigateGamePage() => ClickId(_configuration.ElementIdClickToNavigateGamePage);

    public int Play()
    {
        return 1;
    }

    public OvelianeScraper Delay(int delayInMilliseconds)
    {
        Thread.Sleep(delayInMilliseconds);
        return this;
    }
}