namespace OvelianeEndToEndTests.Browser.Tests;

public interface IElementTests
{
    void LogOn(string userName, string password);
    void CreateShouldSucceed(string elementName);
    void CreateShouldFail(string elementName);
    void DisplayShouldSucceed(string elementName);
    void DisplayShouldFail(string elementName);
    void DeleteShouldSucceed(string elementName, string textToConfirm);
    void DeleteShouldFail(string checkpointName, string textToConfirm);
}