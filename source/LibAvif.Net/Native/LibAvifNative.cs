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
        public static extern AvifResult AvifDecoderParse(IntPtr decoder);

        [DllImport("libavif", EntryPoint = "avifDecoderNextImage", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifDecoderNextImage(IntPtr decoder);

        [DllImport("libavif", EntryPoint = "avifRGBImageSetDefaults", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifRGBImageSetDefaults(IntPtr rgb, IntPtr image);

        [DllImport("libavif", EntryPoint = "avifRGBImageAllocatePixels", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifRGBImageAllocatePixels(IntPtr rgb);

        [DllImport("libavif", EntryPoint = "avifImageYUVToRGB", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifImageYUVToRGB(IntPtr image, IntPtr rgb);

        [DllImport("libavif", EntryPoint = "avifRGBImageFreePixels", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifRGBImageFreePixels(IntPtr rgb);
        
        [DllImport("libavif", EntryPoint = "avifDecoderDestroy", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifDecoderDestroy(IntPtr decoder);

        [DllImport("libavif", EntryPoint = "avifResultToString", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr AvifResultToString(AvifResult result);

        [DllImport("libavif", EntryPoint = "avifDecoderReadFile", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifDecoderReadFile(IntPtr decoder, IntPtr avifImage, IntPtr filename);

        [DllImport("libavif", EntryPoint = "avifImageCreateEmpty", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr AvifImageCreateEmpty();

        [DllImport("libavif", EntryPoint = "avifImageDestroy", CallingConvention = CallingConvention.StdCall)]
        public static extern void AvifImageDestroy(IntPtr image);

        [DllImport("libavif", EntryPoint = "avifDecoderReadMemory", CallingConvention = CallingConvention.StdCall)]
        public static extern AvifResult AvifDecoderReadMemory(IntPtr decoder, IntPtr avifImage, IntPtr buffer, int bufferSize);

        
    }
}
