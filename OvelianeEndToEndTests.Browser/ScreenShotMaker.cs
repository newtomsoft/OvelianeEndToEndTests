using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;

namespace OvelianeEndToEndTests.Browser;

public class ScreenShotMaker
{
    private readonly ILogger<EndToEndTestsApplication> _logger;
    private readonly string _pathToScreenShots;

    public ScreenShotMaker(ILogger<EndToEndTestsApplication> logger)
    {
        _logger = logger;
        _pathToScreenShots = DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
        Directory.CreateDirectory(_pathToScreenShots);
    }

    public void SaveScreenShot(Func<byte[]> takeScreenShot, bool isError)
    {
        using var screenShotStream = new MemoryStream(takeScreenShot());
        using var screenShotImage = Image.Load(screenShotStream);
        SaveImage(screenShotImage, isError);
    }

    public void SaveScreenShotOnElement(string caller, Func<byte[]> takeScreenShot, List<IWebElement> webElements, bool isError)
    {
        var cropRectangle = CropRectangle(webElements);
        using var screenShotStream = new MemoryStream(takeScreenShot());
        using var screenShotImage = Image.Load(screenShotStream);
        if (cropRectangle.X + cropRectangle.Width > screenShotImage.Width) cropRectangle.Width = screenShotImage.Width - cropRectangle.X;
        if (cropRectangle.Y + cropRectangle.Height > screenShotImage.Height) cropRectangle.Height = screenShotImage.Height - cropRectangle.Y;
        screenShotImage.Mutate(i => i.Crop(cropRectangle));
        SaveImage(screenShotImage, isError);
    }

    private void SaveImage(Image screenShotImage, bool isError)
    {
        var imagePrefixName = $"{DateTime.Now:HH-mm-ss}_{ImagePrefixName()}";
        if (isError) imagePrefixName = $"ERROR_{imagePrefixName}";
        var screenShotFilename = Path.Combine(_pathToScreenShots, imagePrefixName + ".png");
        screenShotImage.Save(screenShotFilename);
    }

    private static Rectangle CropRectangle(IReadOnlyList<IWebElement> webElements)
    {
        const int margin = 5;
        var minX = webElements.Min(e => e.Location.X); if (minX >= margin) minX -= margin;
        var minY = webElements.Min(e => e.Location.Y); if (minY >= margin) minY -= margin;
        var maxX = webElements.Max(e => e.Location.X + e.Size.Width) + margin;
        var maxElementY = webElements.MaxBy(e => e.Location.Y);
        var maxY = maxElementY!.Location.Y + maxElementY.Size.Height + margin;
        var totalHeight = maxY - minY;
        var totalWidth = maxX - minX;
        var originPoint = new Point(minX, minY);
        var cropSize = new Size(totalWidth, totalHeight);
        return new Rectangle(originPoint, cropSize);
    }

    private static string ImagePrefixName()
    {
        var stackTrace = new StackTrace();
        var frames = stackTrace.GetFrames();
        var taker = frames[4].GetMethod()!.Name;
        var testName = frames[5].GetMethod()!.Name;
        var testGroup = frames[5].GetMethod()!.DeclaringType!.Name;
        return $"{testGroup}_{testName}_{taker}";
    }

    public void CompareScreenShots()
    {
        var directoryScreenShots = new DirectoryInfo(_pathToScreenShots);
        var directoryGoldenMaster = new DirectoryInfo("GoldenMaster");
        var goldenMasterFiles = directoryGoldenMaster.GetFiles();
        var noteSameCount = 0;
        var md5 = MD5.Create();
        foreach (var file in directoryScreenShots.GetFiles("*.png").Where(name => name.Name.Contains("ERROR") is false))
        {
            using var streamFile = file.OpenRead();
            var fileMd5 = md5.ComputeHash(streamFile);
            var masterFileName = file.Name[9..];
            var masterFile = goldenMasterFiles.FirstOrDefault(x => x.Name == masterFileName);
            if (masterFile is null)
            {
                _logger.LogWarning("The file {fileName} has no golden master", file.Name);
                continue;
            }
            using var streamGoldenMaster = masterFile.OpenRead();
            var masterFileMd5 = md5.ComputeHash(streamGoldenMaster);
            if (fileMd5.SequenceEqual(masterFileMd5)) continue;
            noteSameCount++;
            _logger.LogWarning("The screenShot {fileName} is different from the golden master", file.Name);
        }
        if (noteSameCount == 0)
            _logger.LogInformation("All screenShots are the same as their golden master");
    }
}
