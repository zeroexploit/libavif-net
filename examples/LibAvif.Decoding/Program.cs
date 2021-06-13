using LibAvif.Net;
using System.Drawing.Imaging;

namespace LibAvif.Decoding
{
    class Program
    {
        static void Main(string[] args)
        {
            using var image = AvifImageDecoder.FromFile("/home/roddel/test.avif");//AvifImageDecoder.FromFile(@"C:\Users\j.roddelkopf\Downloads\Neuer Ordner\test.avif");
            if (image == null)
                return;

            image.Save("/home/roddel/test.jpg", ImageFormat.Jpeg);
        }
    }
}
