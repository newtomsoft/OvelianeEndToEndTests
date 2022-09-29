namespace OvelianeEndToEndTests.Browser.Tests;

public class CheckpointTests : BaseTests, IElementTests
{
    public CheckpointTests(SeleniumScraper scraper, MenuNavigation menuNavigation) : base(scraper, menuNavigation, "checkpoint") { }

    public void CreateShouldSucceed(string elementName)
    {
        var test = GetCurrentMethodName();
        Scraper.StartTest(test, ElementType, elementName);
        MenuNavigation.ClickCheckpointsMenu();
        Scraper.ClickTagText("span", "New Checkpoint");
        Scraper.WriteAttributeValue("formcontrolname", "name", elementName);
        Scraper.WriteAttributeValue("formcontrolname", "description", "This is the description");
        Scraper.ClickTagText("span", " High ");
        Scraper.WriteAttributeValue("formcontrolname", "source", "CIS");
        Scraper.WriteAttributeValue("formcontrolname", "guide", "CIS RHEL 8");
        Scraper.WriteAttributeValue("formcontrolname", "item", "1.6.2");
        Scraper.WriteAttributeValue("formcontrolname", "command", "this is the command");
        Scraper.ClickTagText("span", "Save as");
        Scraper.ClickTagText("button", "Approved");
        Scraper.EndTest(test, ElementType, elementName);
    }

    public void CreateShouldFail(string elementName)
    {
        var test = GetCurrentMethodName();
        Scraper.StartTest(test, ElementType, elementName);
        MenuNavigation.ClickCheckpointsMenu();
        Scraper.ClickTagText("span", "New Checkpoint");
        Scraper.WriteAttributeValue("formcontrolname", "name", elementName);
        Scraper.WriteAttributeValue("formcontrolname", "description", "This is the description");
        Scraper.ClickTagText("span", " High ");
        Scraper.WriteAttributeValue("formcontrolname", "source", "CIS");
        Scraper.WriteAttributeValue("formcontrolname", "guide", "CIS RHEL 8");
        Scraper.WriteAttributeValue("formcontrolname", "item", "1.6.2");
        Scraper.WriteAttributeValue("formcontrolname", "command", "this is the command");
        Scraper.ClickTagText("span", "Save as", ShouldFail);
        Scraper.CheckTagTextExist("strong", "available");
        Scraper.EndTest(test, ElementType, elementName);
    }

    public void DisplayShouldSucceed(string elementName)
    {
        var test = GetCurrentMethodName();
        Scraper.StartTest(test, ElementType, elementName);
        MenuNavigation.ClickCheckpointsMenu();
        Scraper.CheckTagTextExist("span", elementName);
        Scraper.EndTest(test, ElementType, elementName);
    }
    public void DisplayShouldFail(string elementName)
    {
        var test = GetCurrentMethodName();
        Scraper.StartTest(test, ElementType, elementName);
        MenuNavigation.ClickCheckpointsMenu();
        Scraper.CheckTagTextExist("span", elementName, ShouldFail);
        Scraper.EndTest(test, ElementType, elementName);
    }

    public void DeleteShouldSucceed(string elementName, string textToConfirm)
    {
        var test = GetCurrentMethodName();
        Scraper.StartTest(test, ElementType, elementName);
        MenuNavigation.ClickCheckpointsMenu();
        Scraper.CheckTagTextExist("span", elementName).ThenSelectRow().ThenClickClass("la-trash");
        Scraper.WriteAttributeValue("formcontrolname", "confirm", textToConfirm);
        Scraper.ClickTagText("span", "Delete");
        Scraper.EndTest(test, ElementType, elementName);
    }

    public void DeleteShouldFail(string checkpointName, string textToConfirm)
    {
        var test = GetCurrentMethodName();
        Scraper.StartTest(test, ElementType, checkpointName);
        MenuNavigation.ClickCheckpointsMenu();
        Scraper.CheckTagTextExist("span", checkpointName).ThenSelectRow().ThenClickClass("la-trash");
        Scraper.WriteAttributeValue("formcontrolname", "confirm", textToConfirm);
        Scraper.ClickTagText("span", "Delete", ShouldFail);
        Scraper.EndTest(test, ElementType, checkpointName);
    }
}