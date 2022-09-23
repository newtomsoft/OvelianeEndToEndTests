namespace OvelianeEndToEndTests.Browser.Scraper;

public interface IWebScraper
{
    void SetWindowSize(Size size);
    void NavigateToPage(string pageUrl, string elementToFindInPage, string elementType = "Id");
    XmlDocument? GetXmlDocumentFromClassName(string className, string excludedClassName);
    XmlDocument? GetXmlDocumentFromId(string id);
    XmlDocument? GetXmlDocumentFromTable();
    XmlDocument GetPage();

    void ClickFirstXPathWithText(string xPath, string text);
    void ClickId(string id);
    void ClickClass(string className);
    void ClickKeyValue(string key, string value);
    XmlDocument? GetClass(string className);
    void SendKeysToClass(string className, string keys);
    void SendKeysToId(string id, string keys);

    void WaitClass(string className);
    void WaitId(string id);
}