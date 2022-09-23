namespace OvelianeEndToEndTests.Browser.OvelianeScraper;

public interface IOvelianeScraper
{
    void NavigateHomePage();
    void NavigateGamePage();
    int Play();
    IOvelianeScraper Click(string id);
    IOvelianeScraper WriteInId(string id, string word);
    IOvelianeScraper WaitId(string id);
    void ClickKeyValue(string key, string value);
}