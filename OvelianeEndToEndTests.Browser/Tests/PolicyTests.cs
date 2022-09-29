namespace OvelianeEndToEndTests.Browser.Tests;

public class PolicyTests : BaseTests, IElementTests
{
    public PolicyTests(SeleniumScraper scraper, MenuNavigation menuNavigation) : base(scraper, menuNavigation, "policy") { }

    public void CreateShouldSucceed(string elementName)
    {
        throw new NotImplementedException();
    }

    public void CreateShouldFail(string elementName)
    {
        throw new NotImplementedException();
    }

    public void DisplayShouldSucceed(string elementName)
    {
        throw new NotImplementedException();
    }

    public void DisplayShouldFail(string elementName)
    {
        throw new NotImplementedException();
    }

    public void DeleteShouldSucceed(string elementName, string textToConfirm)
    {
        throw new NotImplementedException();
    }

    public void DeleteShouldFail(string checkpointName, string textToConfirm)
    {
        throw new NotImplementedException();
    }
}