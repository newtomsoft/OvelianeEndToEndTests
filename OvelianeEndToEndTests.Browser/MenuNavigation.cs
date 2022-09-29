namespace OvelianeEndToEndTests.Browser;

public class MenuNavigation
{
    private readonly SeleniumScraper _scraper;

    public MenuNavigation(SeleniumScraper scraper)
    {
        _scraper = scraper;
    }

    public void ClickCheckpointsMenu()
    {
        if (_scraper.IsTagTextExist("span", "Checkpoints") is false)
            _scraper.ClickTagText("span", "Compliance");

        _scraper.ClickTagText("span", "Checkpoints");
    }
}
