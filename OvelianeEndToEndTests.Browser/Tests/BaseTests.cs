namespace OvelianeEndToEndTests.Browser.Tests;

public class BaseTests
{
    protected readonly SeleniumScraper Scraper;
    protected readonly MenuNavigation MenuNavigation;
    protected const bool ShouldFail = true;
    protected readonly string ElementType;

    public void LogOn(string userName, string password)
    {
        Scraper.NavigateHomePage();
        Scraper.WriteId("username", userName);
        Scraper.ClickId("kc-form-buttons");
        Scraper.WriteId("password", password);
        Scraper.ClickId("kc-login");
    }

    protected BaseTests(SeleniumScraper scraper, MenuNavigation menuNavigation, string elementType)
    {
        Scraper = scraper;
        MenuNavigation = menuNavigation;
        ElementType = elementType;
    }

    protected static string GetCurrentMethodName()
    {
        var stackTrace = new StackTrace();
        var stackFrame = stackTrace.GetFrame(1);
        return stackFrame!.GetMethod()!.Name;
    }
}