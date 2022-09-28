namespace OvelianeEndToEndTests.Browser;

public class EndToEndTestsApplication
{
    private readonly SeleniumScraper _scraper;

    public EndToEndTestsApplication(IWebDriverFactory webDriverFactory, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _scraper = new SeleniumScraper(webDriverFactory, configuration, logger);
    }

    public void Run()
    {
        LogOn("a.goude", "admin");

        CreateCheckpointShouldSucceed("aaa-checkpoint");
        CheckpointDisplayedShouldSucceed("aaa-checkpoint");
        DeleteCheckpointShouldSucceed("aaa-checkpoint");

        CreateCheckpointShouldFailed("DisableIPv6"); // because still exists
        CloseScraper();
    }

    private void LogOn(string userName, string password)
    {
        _scraper.NavigateHomePage();
        _scraper.WriteId("username", userName);
        _scraper.ClickId("kc-form-buttons");
        _scraper.WriteId("password", password);
        _scraper.ClickId("kc-login");
    }

    private void CloseScraper() => _scraper.CloseScraper();

    private void CreateCheckpointShouldSucceed(string checkpointName)
    {
        const string action = "Create";
        const string element = "Checkpoint";
        _scraper.StartTest(action, element, checkpointName);
        NavigateCheckpoints(); //todo click logo oveliane
        if (_scraper.IsTagTextExist("span", "Checkpoints") is false) _scraper.ClickTagText("span", "Compliance");
        _scraper.ClickTagText("span", "Checkpoints");
        _scraper.ClickTagText("span", "New Checkpoint");
        _scraper.WriteAttributeValue("formcontrolname", "name", checkpointName);
        _scraper.WriteAttributeValue("formcontrolname", "description", "This is the description");
        _scraper.ClickTagText("span", " High ");
        _scraper.WriteAttributeValue("formcontrolname", "source", "CIS");
        _scraper.WriteAttributeValue("formcontrolname", "guide", "CIS RHEL 8");
        _scraper.WriteAttributeValue("formcontrolname", "item", "1.6.2");
        _scraper.WriteAttributeValue("formcontrolname", "command", "this is the command");
        _scraper.ClickTagText("span", "Save as");
        _scraper.ClickTagText("button", "Approved");
        _scraper.Delay(2000).TakeScreenShot();
        _scraper.EndTest(action, element, checkpointName);
    }

    private void CreateCheckpointShouldFailed(string checkpointName)
    {
        const string action = "CreateFail";
        const string element = "Checkpoint";
        NavigateCheckpoints();
        _scraper.StartTest(action, element, checkpointName);
        NavigateCheckpoints();
        _scraper.ClickTagText("span", "New Checkpoint");
        _scraper.WriteAttributeValue("formcontrolname", "name", checkpointName);
        _scraper.WriteAttributeValue("formcontrolname", "description", "This is the description");
        _scraper.ClickTagText("span", " High ");
        _scraper.WriteAttributeValue("formcontrolname", "source", "CIS");
        _scraper.WriteAttributeValue("formcontrolname", "guide", "CIS RHEL 8");
        _scraper.WriteAttributeValue("formcontrolname", "item", "1.6.2");
        _scraper.WriteAttributeValue("formcontrolname", "command", "this is the command");
        _scraper.ClickTagText("span", "Save as");
        _scraper.ClickTagText("button", "Approved", true);
        _scraper.CheckTagTextExist("strong", "available");
        _scraper.Delay(2000).TakeScreenShot();
        _scraper.EndTest(action, element, checkpointName);
    }

    private void CheckpointDisplayedShouldSucceed(string checkpointName)
    {
        const string action = "Display";
        const string element = "Checkpoint";
        _scraper.StartTest(action, element, checkpointName);
        NavigateCheckpoints();
        _scraper.CheckTagTextExist("span", checkpointName);
        _scraper.EndTest(action, element, checkpointName);
    }

    private void DeleteCheckpointShouldSucceed(string checkpointName)
    {
        const string action = "Delete";
        const string element = "Checkpoint";
        _scraper.StartTest(action, element, checkpointName);
        NavigateCheckpoints();

        if (_scraper.IsTagTextExist("span", checkpointName) is false)
        {

        }
        //todo : delete checkpoint

        _scraper.Delay(2000).TakeScreenShot();
        _scraper.EndTest(action, element, checkpointName);
    }
    private void NavigateCheckpoints()
    {
        if (_scraper.IsTagTextExist("span", "Checkpoints") is false)
            _scraper.ClickTagText("span", "Compliance");

        _scraper.ClickTagText("span", "Checkpoints");
    }
}
