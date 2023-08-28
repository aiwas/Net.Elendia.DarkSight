using System;
using System.Drawing;

namespace Net.Elendia.DarkSight;

internal class ProcessorTest {

    public static async void OcrTest() {
        var image = Image.FromFile(@"../../../test.png");
        var bmp = new Bitmap(image);
        var crop = bmp.Clone(new Rectangle(2840, 0, 1000, 500), bmp.PixelFormat);
        var sbmp = await Processor.ConvertSoftwareBitmap(crop);
        var result = await Processor.RunOcr(sbmp);

        Console.WriteLine(result);
    }
}
