namespace OvelianeEndToEndTests.Browser.Configuration;

[Serializable]
public class ApplicationConfiguration
{
    public Language Language { get; set; } = Language.French;
    public string WebsiteUrl { get; set; } = default!;
    public string ElementIdToFindInHomePage { get; set; } = default!;
    public string ElementClassToFindInHomePage { get; set; } = default!;
    public string ElementIdClickToNavigateGamePage { get; set; } = default!;
}