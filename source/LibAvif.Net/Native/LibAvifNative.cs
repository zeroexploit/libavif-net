using System;
using System.Runtime.InteropServices;

namespace LibAvif.Net.Native
{
    static class LibAvifNative
    {
        [DllImport("libavif", EntryPoint = "avifDecoderCreate", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr AvifDecoderCreate();

        [DllImport("libavif", EntryPoint = "avifDecoderSetIOFile", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifDecoderSetIOFile(IntPtr decoder, IntPtr filename);

        [DllImport("libavif", EntryPoint = "avifDecoderParse", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifDecoderParse(ref AvifDecoder decoder);

        [DllImport("libavif", EntryPoint = "avifDecoderNextImage", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifDecoderNextImage(ref AvifDecoder decoder);

        [DllImport("libavif", EntryPoint = "avifRGBImageSetDefaults", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifRGBImageSetDefaults(ref AvifRgbImage rgb, ref AvifImage image);

        [DllImport("libavif", EntryPoint = "avifRGBImageAllocatePixels", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifRGBImageAllocatePixels(ref AvifRgbImage rgb);

        [DllImport("libavif", EntryPoint = "avifImageYUVToRGB", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifImageYUVToRGB(ref AvifImage image, ref AvifRgbImage rgb);

        [DllImport("libavif", EntryPoint = "avifRGBImageFreePixels", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifRGBImageFreePixels(ref AvifRgbImage rgb);
        
        [DllImport("libavif", EntryPoint = "avifDecoderDestroy", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifDecoderDestroy(ref AvifDecoder decoder);

        [DllImport("libavif", EntryPoint = "avifResultToString", CallingConvention = CallingConvention.StdCall)]
        public static extern string avifResultToString(AvifResult result);
    }
}
