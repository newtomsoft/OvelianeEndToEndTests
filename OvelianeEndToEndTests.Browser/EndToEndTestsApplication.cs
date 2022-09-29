using OvelianeEndToEndTests.Browser.Tests;

namespace OvelianeEndToEndTests.Browser;

public class EndToEndTestsApplication
{
    private readonly SeleniumScraper _scraper;
    private readonly IElementTests _checkpointTests;
    private readonly IElementTests _assetTests;
    private readonly IElementTests _policyTests;
    private readonly IElementTests _deploymentTests;

    public EndToEndTestsApplication(IWebDriverFactory webDriverFactory, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _scraper = new SeleniumScraper(webDriverFactory, configuration, logger);
        var menuNavigation = new MenuNavigation(_scraper);
        _assetTests = new AssetTests(_scraper, menuNavigation);
        _checkpointTests = new CheckpointTests(_scraper, menuNavigation);
        _policyTests = new PolicyTests(_scraper, menuNavigation);
        _deploymentTests = new DeploymentTests(_scraper, menuNavigation);
    }

    public void Run()
    {
        _checkpointTests.LogOn("a.goude", "admin");

        _checkpointTests.CreateShouldSucceed("aaa-checkpoint");
        _checkpointTests.DisplayShouldSucceed("aaa-checkpoint");
        _checkpointTests.DeleteShouldSucceed("aaa-checkpoint", "DELETE");

        _checkpointTests.CreateShouldFail("DisableIPv6"); // still exists
        _checkpointTests.DisplayShouldFail("checkpoint dont exist");
        _checkpointTests.DeleteShouldFail("Audit SGID executables", "BadWord");

        CloseScraper();
        CompareScreenShots();
    }

    private void CloseScraper() => _scraper.CloseScraper();

    private void CompareScreenShots() => _scraper.CompareScreenShots();
}