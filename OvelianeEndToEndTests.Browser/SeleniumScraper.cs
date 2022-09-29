namespace OvelianeEndToEndTests.Browser;

public class SeleniumScraper
{
    private readonly ApplicationConfiguration _configuration;
    private readonly ILogger<EndToEndTestsApplication> _logger;
    private readonly IWebDriver _webDriver;
    private readonly ScreenShotMaker _screenShotMaker;
    private int _errorCount;
    private IWebElement? _currentWebElement;

    public SeleniumScraper(IWebDriverFactory webDriverFactory, ApplicationConfiguration configuration, ILogger<EndToEndTestsApplication> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _webDriver = webDriverFactory.CreateDriver();
        _screenShotMaker = new ScreenShotMaker(_logger);
    }

    public void NavigateHomePage()
    {
        SetWindowSize(new System.Drawing.Size(1600, 1000));
        NavigateToPage(_configuration.WebsiteUrl, _configuration.ElementIdToFindInHomePage);
    }

    public void ClickFirstXPathWithText(string xPath, string text)
    {
        _webDriver.FindElements(By.XPath(xPath)).First(e => e.Text == text).Click();
    }

    public void ClickId(string id)
    {
        try
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementToBeClickable(By.Id(id))).Click();
        }
        catch
        {
            _errorCount++;
            _logger.LogError("Unable to click elementType with id {id}", id);
            TakeScreenShot(true);
        }
    }

    public void ClickClass(string className)
    {
        try
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementToBeClickable(By.ClassName(className))).Click();
        }
        catch
        {
            _errorCount++;
            _logger.LogError("Unable to click elementType with class {className}", className);
            TakeScreenShot(true);
        }
    }

    public void ClickAttributeValue(string attributeName, string attributeValue)
    {
        var element = FindElement(_webDriver, By.CssSelector($"[{attributeName}='{attributeValue}']"), TimeSpan.FromSeconds(10));
        ClickElement(element);
    }

    public void ClickTagText(string tag, string text, bool shouldFail = false)
    {
        var element = FindElement(_webDriver, By.XPath($"//{tag}[text()='{text}']"), TimeSpan.FromSeconds(1)) ??
                      FindElement(_webDriver, By.XPath($"//{tag}[contains(text(),'{text}')]"), TimeSpan.FromSeconds(3))!;
        try
        {
            ClickElement(element);
            if (!shouldFail) return;
            _errorCount++;
            _logger.LogError("Click elementType with tag {tag} and text {text} {status}", tag, text, "should have failed");
        }
        catch when (shouldFail is false)
        {
            _errorCount++;
            _logger.LogError("Click elementType with tag {tag} and text {text} {status}", tag, text, "failed");
            TakeScreenShot(true);
        }
        catch when (shouldFail)
        {
            // do nothing
        }
    }

    public bool IsTagTextExist(string tag, string text)
    {
        var element = FindElement(_webDriver, By.XPath($"//{tag}[text()='{text}']"), TimeSpan.FromSeconds(1)) ??
                      FindElement(_webDriver, By.XPath($"//{tag}[contains(text(),'{text}')]"), TimeSpan.FromSeconds(1));

        return element is not null;
    }

    private static void ClickElement(IWebElement? element)
    {
        if (element is null || IsDisable(element)) throw new Exception(nameof(element));
        try
        {
            element.Click();
        }
        catch
        {
            ClickElement(element.FindElement(By.XPath("./..")));
        }
    }

    private static bool IsDisable(IWebElement? element)
    {
        while (true)
        {
            if (element is null || element.TagName == "html") return false;
            if (element.Enabled is false) return true;
            element = element.FindElement(By.XPath("./.."));
        }
    }

    public SeleniumScraper WriteClass(string className, string text)
    {
        FindElement(_webDriver, By.ClassName(className), TimeSpan.FromSeconds(2))!.SendKeys(text);
        return this;
    }

    public SeleniumScraper WriteId(string id, string text)
    {
        new WebDriverWait(_webDriver, TimeSpan.FromSeconds(4)).Until(ExpectedConditions.ElementIsVisible(By.Id(id))).SendKeys(text); ;
        return this;
    }

    public void WriteAttributeValue(string attributeName, string attributeValue, string text)
    {
        try
        {
            FindElement(_webDriver, By.CssSelector($"[{attributeName}='{attributeValue}']"), TimeSpan.FromSeconds(2))!
                .SendKeys(text);
        }
        catch
        {
            _errorCount++;
            _logger.LogError("unable to write text {text} in elementType with attribute {attributeName} and value {attributeValue}", text, attributeName, attributeValue);
            TakeScreenShot(true);
        }
    }

    public void WaitClass(string className)
    {
        FindElement(_webDriver, By.ClassName(className), TimeSpan.FromSeconds(20));
    }

    public SeleniumScraper WaitId(string id)
    {
        FindElement(_webDriver, By.Id(id), TimeSpan.FromSeconds(20));
        return this;
    }

    private void SetWindowSize(System.Drawing.Size size)
    {
        _webDriver.Manage().Window.Size = size;
    }

    private void NavigateToPage(string pageUrl, string elementToFindInPage, string elementType = "Id")
    {
        try
        {
            _webDriver.Navigate().GoToUrl(pageUrl);
        }
        catch
        {
            ClickId("advancedButton");
            ClickId("exceptionDialogButton");
        }
        var wait = FluentWait.Create(_webDriver);
        wait.WithTimeout(TimeSpan.FromMilliseconds(30000));
        wait.PollingInterval = TimeSpan.FromMilliseconds(25);
        wait.Until(IsPageLoaded);
        _logger.LogInformation("page {pageUrl} loaded", pageUrl);

        bool IsPageLoaded(IWebDriver webDriver) => elementType == "Id"
                ? FindElement(webDriver, By.Id(elementToFindInPage), TimeSpan.FromSeconds(5)) is not null
                : FindElement(webDriver, By.ClassName(elementToFindInPage), TimeSpan.FromSeconds(5)) is not null;
    }

    private void TakeScreenShot(bool isError) => _screenShotMaker.SaveScreenShot(() => ((ITakesScreenshot)_webDriver).GetScreenshot().AsByteArray, isError);

    private static IWebElement? FindElement(ISearchContext element, By by, TimeSpan timeOut = default)
    {
        if (timeOut == new TimeSpan()) timeOut = TimeSpan.FromSeconds(3);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        while (stopWatch.Elapsed < timeOut)
        {
            var elements = element.FindElements(by);
            if (elements.Any()) return elements.First();
        }
        return null;
    }

    private static ReadOnlyCollection<IWebElement>? FindElements(ISearchContext element, By by, TimeSpan timeOut = default)
    {
        if (timeOut == new TimeSpan()) timeOut = TimeSpan.FromSeconds(3);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        while (stopWatch.Elapsed < timeOut)
        {
            try
            {
                var elements = element.FindElements(by);
                if (elements.Any()) return elements;
                Task.Delay(50).Wait();
            }
            catch
            {
                Task.Delay(50).Wait();
            }
        }
        return null;
    }

    public void StartTest(string test, string elementType, string elementName)
    {
        _errorCount = 0;
        _logger.LogInformation("{test} {elementType} \"{elementName}\" {status}", test, elementType, elementName, "started");
    }

    public void EndTest(string test, string elementType, string elementName)
    {
        Thread.Sleep(800);
        TakeScreenShot(_errorCount > 0);
        if (_errorCount == 0)
            _logger.LogInformation("{test} {elementType} \"{elementName}\" {status}", test, elementType, elementName, "success");
        else
            _logger.LogError("{test} {elementType} \"{elementName}\" {status}", test, elementType, elementName, "failed");
    }

    public SeleniumScraper CheckTagTextExist(string tag, string text, bool shouldFail = false)
    {
        var element = FindElement(_webDriver, By.XPath($"//{tag}[text()='{text}']"), TimeSpan.FromSeconds(1)) ??
                      FindElement(_webDriver, By.XPath($"//{tag}[contains(text(),'{text}')]"), TimeSpan.FromSeconds(1));

        switch (shouldFail)
        {
            case false when element is null:
                _errorCount++;
                _logger.LogError("elementType with tag {tag} and text {text} not found", tag, text);
                break;
            case true when element is null:
            case false:
                break;
            case true:
                _errorCount++;
                _logger.LogError("elementType with tag {tag} and text {text} found", tag, text);
                break;
        }

        _currentWebElement = element;
        return this;
    }

    public void CloseScraper()
    {
        _webDriver.Close();
        _webDriver.Quit();
    }

    public SeleniumScraper ThenSelectRow()
    {
        while (true)
        {
            if (_currentWebElement is null || _currentWebElement.TagName == "mat-row") return this;
            _currentWebElement = _currentWebElement!.FindElement(By.XPath("./.."));
        }
    }

    public SeleniumScraper ThenClickClass(string className)
    {
        if (_currentWebElement is null) return this;
        _currentWebElement!.FindElement(By.ClassName(className)).Click();
        return this;
    }

    public void CompareScreenShots() => _screenShotMaker.CompareScreenShots();
}