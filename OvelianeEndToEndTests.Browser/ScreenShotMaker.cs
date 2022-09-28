using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace OvelianeEndToEndTests.Browser;

public class ScreenShotMaker
{
    private readonly string _pathToScreenShots;

    public ScreenShotMaker()
    {
        _pathToScreenShots = DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
        Directory.CreateDirectory(_pathToScreenShots);
    }

    public void SaveScreenShot(Func<byte[]> takeScreenShot)
    {
        using var screenShotStream = new MemoryStream(takeScreenShot());
        using var screenShotImage = Image.Load(screenShotStream);
        SaveImage(screenShotImage);
    }
    public void SaveScreenShotOnElement(Func<byte[]> takeScreenShot, List<IWebElement> webElements)
    {
        var cropRectangle = CropRectangle(webElements);
        using var screenShotStream = new MemoryStream(takeScreenShot());
        using var screenShotImage = Image.Load(screenShotStream);
        if (cropRectangle.X + cropRectangle.Width > screenShotImage.Width) cropRectangle.Width = screenShotImage.Width - cropRectangle.X;
        if (cropRectangle.Y + cropRectangle.Height > screenShotImage.Height) cropRectangle.Height = screenShotImage.Height - cropRectangle.Y;
        screenShotImage.Mutate(i => i.Crop(cropRectangle));
        SaveImage(screenShotImage);
    }

    private void SaveImage(Image screenShotImage)
    {
        var screenShotFilename = Path.Combine(_pathToScreenShots, DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + ".png");
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
}
