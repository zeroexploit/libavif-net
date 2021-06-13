using LibAvif.Net;

namespace LibAvif.Decoding
{
    class Program
    {
        static void Main(string[] args)
        {
            var image = AvifImageDecoder.FromFile("/home/roddel/test.avif");//AvifImageDecoder.FromFile(@"C:\Users\j.roddelkopf\Downloads\Neuer Ordner\test.avif");
        }
    }
}
