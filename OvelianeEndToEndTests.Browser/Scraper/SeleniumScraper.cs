namespace OvelianeEndToEndTests.Browser.Scraper;

public class SeleniumScraper : IWebScraper
{
    private readonly ILogger<EndToEndTestsApplication> _logger;
    private readonly IWebDriver _webDriver;

    public SeleniumScraper(IWebDriverFactory webDriverFactory, ILogger<EndToEndTestsApplication> logger)
    {
        _logger = logger;
        _webDriver = webDriverFactory.CreateDriver();
    }


    public XmlDocument GetPage()
    {
        var webDriverPageSource = _webDriver.PageSource;
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(webDriverPageSource);
        return xmlDocument;
    }

    public void ClickFirstXPathWithText(string xPath, string text) => _webDriver.FindElements(By.XPath(xPath)).First(e => e.Text == text).Click();
    public void ClickId(string id) => FindElement(_webDriver, By.Id(id), TimeSpan.FromSeconds(2))!.Click();
    public void ClickClass(string className) => FindElement(_webDriver, By.ClassName(className), TimeSpan.FromSeconds(4))!.Click();
    public void ClickKeyValue(string key, string value) => FindElement(_webDriver, By.CssSelector($"[{key}='{value}']"), TimeSpan.FromSeconds(4))!.Click();
    public XmlDocument? GetClass(string className)
    {
        var element = FindElement(_webDriver, By.ClassName(className), TimeSpan.FromSeconds(4));
        return ConvertToXmlDocument(element);
    }

    public void SendKeysToClass(string className, string keys) => FindElement(_webDriver, By.ClassName(className), TimeSpan.FromSeconds(2))!.SendKeys(keys);
    public void SendKeysToId(string id, string keys) => FindElement(_webDriver, By.Id(id), TimeSpan.FromSeconds(2))!.SendKeys(keys);

    public void WaitClass(string className) => FindElement(_webDriver, By.ClassName(className), TimeSpan.FromMinutes(2));
    public void WaitId(string id) => FindElement(_webDriver, By.Id(id), TimeSpan.FromMinutes(2));
    public void SetWindowSize(Size size) => _webDriver.Manage().Window.Size = size;
    public void NavigateToPage(string pageUrl, string elementToFindInPage, string elementType = "Id")
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

        bool IsPageLoaded(IWebDriver webDriver) =>
            elementType == "Id"
                ? FindElement(webDriver, By.Id(elementToFindInPage), TimeSpan.FromSeconds(5)) is not null
                : FindElement(webDriver, By.ClassName(elementToFindInPage), TimeSpan.FromSeconds(5)) is not null;
    }

    public XmlDocument? GetXmlDocumentFromClassName(string className, string excludedClassName)
    {
        while (true)
        {
            try
            {
                var element = FindElement(_webDriver, By.ClassName(className), TimeSpan.FromSeconds(5));
                var containsExcludedClassName = element?.GetAttribute("class").Contains(excludedClassName);
                return containsExcludedClassName is null or true ? null : ConvertToXmlDocument(element);
            }
            catch
            {
                Task.Delay(10).Wait();
            }
        }
    }

    public XmlDocument? GetXmlDocumentFromId(string id)
    {
        var element = FindElement(_webDriver, By.Id(id), TimeSpan.FromSeconds(2));
        return ConvertToXmlDocument(element);
    }

    public XmlDocument? GetXmlDocumentFromTable()
    {
        var element = FindElement(_webDriver, By.CssSelector("table"), TimeSpan.FromSeconds(2));
        return ConvertToXmlDocument(element);
    }


    private static XmlDocument? ConvertToXmlDocument(IWebElement? element)
    {
        if (element is null) return null;
        var html = element.GetAttribute("innerHTML");
        var doc = new XmlDocument();
        doc.LoadXml($"<root>{html}</root>");
        return doc;
    }

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
}