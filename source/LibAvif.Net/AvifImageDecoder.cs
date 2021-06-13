using LibAvif.Net.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LibAvif.Net
{
    public static class AvifImageDecoder
    {
        public static Image FromFile(string path)
        {
            var decoderPtr = LibAvifNative.AvifDecoderCreate();
            var imagePtr = LibAvifNative.AvifImageCreateEmpty();

            var result = LibAvifNative.AvifDecoderReadFile(decoderPtr, imagePtr, Marshal.StringToHGlobalAnsi(path));
            if (result != AvifResult.AVIF_RESULT_OK)
                throw new Exception("Unable to open image: " + Marshal.PtrToStringAnsi(LibAvifNative.AvifResultToString(result))); // todo: cleanup and use explicit exception

            var rgbImagePtr = Marshal.AllocHGlobal(Marshal.SizeOf<AvifRgbImage>());
            LibAvifNative.AvifRGBImageSetDefaults(rgbImagePtr, imagePtr);
            LibAvifNative.AvifRGBImageAllocatePixels(rgbImagePtr);

            if (LibAvifNative.AvifImageYUVToRGB(imagePtr, rgbImagePtr) != AvifResult.AVIF_RESULT_OK)
                throw new Exception("Unable to convert yuv to rgb: " + Marshal.PtrToStringAnsi(LibAvifNative.AvifResultToString(result))); // todo: cleanup and use explicit exception

            var rgbImage = Marshal.PtrToStructure<AvifRgbImage>(rgbImagePtr);

            var elements = rgbImage.width * rgbImage.height * 4;
            var buffer = new byte[elements];

            // rgbImage.depth > 8 ??
            Marshal.Copy(rgbImage.pixels, buffer, 0, (int)elements);

            for (var i = 0; i < buffer.Length; i += 4)
            {
                var R = buffer[i];
                var G = buffer[i + 1];
                var B = buffer[i + 2];
                var A = buffer[i + 3];

                buffer[i] = B;
                buffer[i + 1] = G;
                buffer[i + 2] = R;
                buffer[i + 3] = A;
            }

            var bitmap = new Bitmap((int)rgbImage.width, (int)rgbImage.height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, (int)rgbImage.width, (int)rgbImage.height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var ptr = bmpData.Scan0;
            Marshal.Copy(buffer, 0, ptr, buffer.Length);
            bitmap.UnlockBits(bmpData);

            LibAvifNative.AvifRGBImageFreePixels(rgbImagePtr);
            Marshal.FreeHGlobal(rgbImagePtr);
            LibAvifNative.AvifImageDestroy(imagePtr);
            LibAvifNative.AvifDecoderDestroy(decoderPtr);

            return bitmap;
        }
    }
}
