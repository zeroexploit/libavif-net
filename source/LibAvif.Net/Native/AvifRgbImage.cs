using System;
using System.Runtime.InteropServices;

namespace LibAvif.Net.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct AvifRgbImage
    {
        public UInt32 width;       // must match associated avifImage
        public UInt32 height;      // must match associated avifImage
        public UInt32 depth;       // legal depths [8, 10, 12, 16]. if depth>8, pixels must be uint16_t internally
        public AvifRGBFormat format; // all channels are always full range
        public AvifChromaUpsampling chromaUpsampling; // Defaults to AVIF_CHROMA_UPSAMPLING_AUTOMATIC: How to upsample non-4:4:4 UV (ignored for 444) when converting to RGB.
                                                      // Unused when converting to YUV. avifRGBImageSetDefaults() prefers quality over speed.
        public int ignoreAlpha;        // Used for XRGB formats, treats formats containing alpha (such as ARGB) as if they were
                                       // RGB, treating the alpha bits as if they were all 1.
        public IntPtr pixels;
        public UInt32 rowBytes;
    }

    enum AvifRGBFormat
    {
        AVIF_RGB_FORMAT_RGB = 0,
        AVIF_RGB_FORMAT_RGBA, // This is the default format set in avifRGBImageSetDefaults().
        AVIF_RGB_FORMAT_ARGB,
        AVIF_RGB_FORMAT_BGR,
        AVIF_RGB_FORMAT_BGRA,
        AVIF_RGB_FORMAT_ABGR
    }

    enum AvifChromaUpsampling
    {
        AVIF_CHROMA_UPSAMPLING_AUTOMATIC = 0,    // Chooses best trade off of speed/quality (prefers libyuv, else uses BEST_QUALITY)
        AVIF_CHROMA_UPSAMPLING_FASTEST = 1,      // Chooses speed over quality (prefers libyuv, else uses NEAREST)
        AVIF_CHROMA_UPSAMPLING_BEST_QUALITY = 2, // Chooses the best quality upsampling, given settings (avoids libyuv)
        AVIF_CHROMA_UPSAMPLING_NEAREST = 3,      // Uses nearest-neighbor filter (built-in)
        AVIF_CHROMA_UPSAMPLING_BILINEAR = 4      // Uses bilinear filter (built-in)
    }
}
