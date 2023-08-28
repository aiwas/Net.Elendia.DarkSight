using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;

namespace Net.Elendia.DarkSight;

public class Processor {

    public Point UpperLeftScreen { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }

    public Processor(Point upperLeftScreen, int width, int height) {
        UpperLeftScreen = upperLeftScreen;
        Width = width;
        Height = height;
    }

    public async Task<OcrResult> Execute() {
        var bmp = Capture(UpperLeftScreen, Width, Height);
        var sbmp = await ConvertSoftwareBitmap(bmp);
        var result = await RunOcr(sbmp);
        return result;
    }

    public static Bitmap Capture(Point upperLeftScreen, int width, int height) {
        var b = new Bitmap(width, height);
        using var g = Graphics.FromImage(b);
        g.CopyFromScreen(upperLeftScreen, new Point(0, 0), b.Size);

        return b;
    }

    public static async Task<SoftwareBitmap> ConvertSoftwareBitmap(Bitmap bmp) {
        using var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Bmp);

        using var ras = ms.AsRandomAccessStream();
        var decoder = await BitmapDecoder.CreateAsync(ras);
        var sbmp = await decoder.GetSoftwareBitmapAsync();

        return sbmp;
    }

    public static async Task<OcrResult> RunOcr(SoftwareBitmap sbmp) {
        //OCRを実行する
        var engine = OcrEngine.TryCreateFromLanguage(new Windows.Globalization.Language("ja-JP"));
        var result = await engine.RecognizeAsync(sbmp);
        return result;
    }

}
