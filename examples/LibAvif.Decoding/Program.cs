using LibAvif.Net;
using System.Drawing.Imaging;
using System.IO;

namespace LibAvif.Decoding
{
    class Program
    {
        static void Main()
        {
            // load avif from file
            using (var fromFileImage = AvifImageDecoder.FromFile("test.avif"))
                fromFileImage.Save("fromFile.jpg", ImageFormat.Jpeg);

            // load avif from in-memory byte array
            using (var fromBufferImage = AvifImageDecoder.FromBuffer(File.ReadAllBytes("test.avif")))
                fromBufferImage.Save("fromBuffer.jpg", ImageFormat.Jpeg);

            // load avif from stream
            using (var inputStream = new FileStream("test.avif", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var fromStreamImage = AvifImageDecoder.FromStream(inputStream))
                fromStreamImage.Save("fromStream.jpg", ImageFormat.Jpeg);
        }
    }
}
