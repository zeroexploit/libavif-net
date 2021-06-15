using LibAvif.Net.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace LibAvif.Net
{
    public static class AvifImageDecoder
    {
        public static Image FromFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new AvifInputNullOrEmptyException();

            if (!File.Exists(path))
                throw new AvifFileNotFoundException(path);

            var decoderPtr = IntPtr.Zero;
            var imagePtr = IntPtr.Zero;
            var rgbImagePtr = IntPtr.Zero;

            try
            {
                decoderPtr = LibAvifNative.AvifDecoderCreate();
                imagePtr = LibAvifNative.AvifImageCreateEmpty();
                rgbImagePtr = Marshal.AllocHGlobal(Marshal.SizeOf<AvifRgbImage>());

                var result = LibAvifNative.AvifDecoderReadFile(decoderPtr, imagePtr, Marshal.StringToHGlobalAnsi(path));
                if (result != AvifResult.AVIF_RESULT_OK)
                    throw new AvifDecoderReadFileException(path, Marshal.PtrToStringAnsi(LibAvifNative.AvifResultToString(result)));

                var bitmap = ConvertToBitmap(imagePtr, rgbImagePtr);

                return bitmap;
            }
            finally
            {
                if (rgbImagePtr != IntPtr.Zero)
                {
                    LibAvifNative.AvifRGBImageFreePixels(rgbImagePtr);
                    Marshal.FreeHGlobal(rgbImagePtr);
                }

                if (imagePtr != IntPtr.Zero)
                    LibAvifNative.AvifImageDestroy(imagePtr);

                if (decoderPtr != IntPtr.Zero)
                    LibAvifNative.AvifDecoderDestroy(decoderPtr);
            }
        }

        public static Image FromBuffer(byte[] inputBytes)
        {
            if (inputBytes?.Any() != true)
                throw new AvifInputNullOrEmptyException();

            var decoderPtr = IntPtr.Zero;
            var imagePtr = IntPtr.Zero;
            var rgbImagePtr = IntPtr.Zero;
            var inputBytesPtr = IntPtr.Zero;

            try
            {
                decoderPtr = LibAvifNative.AvifDecoderCreate();
                imagePtr = LibAvifNative.AvifImageCreateEmpty();
                rgbImagePtr = Marshal.AllocHGlobal(Marshal.SizeOf<AvifRgbImage>());
                inputBytesPtr = Marshal.AllocHGlobal(inputBytes.Length);
                Marshal.Copy(inputBytes, 0, inputBytesPtr, inputBytes.Length);

                var result = LibAvifNative.AvifDecoderReadMemory(decoderPtr, imagePtr, inputBytesPtr, inputBytes.Length);
                if (result != AvifResult.AVIF_RESULT_OK)
                    throw new AvifDecoderReadMemoryException(Marshal.PtrToStringAnsi(LibAvifNative.AvifResultToString(result)));

                var bitmap = ConvertToBitmap(imagePtr, rgbImagePtr);

                return bitmap;
            }
            finally
            {
                if (inputBytesPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(inputBytesPtr);

                if (rgbImagePtr != IntPtr.Zero)
                {
                    LibAvifNative.AvifRGBImageFreePixels(rgbImagePtr);
                    Marshal.FreeHGlobal(rgbImagePtr);
                }

                if (imagePtr != IntPtr.Zero)
                    LibAvifNative.AvifImageDestroy(imagePtr);

                if (decoderPtr != IntPtr.Zero)
                    LibAvifNative.AvifDecoderDestroy(decoderPtr);
            }
        }

        public static Image FromStream(Stream inputStream)
        {
            if (!inputStream.CanRead || (inputStream.Position != 0 && !inputStream.CanSeek))
                throw new AvifStreamNotSupportedException();

            if (inputStream.Position != 0)
                inputStream.Seek(0, SeekOrigin.Begin);

            byte[] inputBytes;

            try
            {
                using var memoryStream = new MemoryStream();
                inputStream.CopyTo(memoryStream);
                inputBytes = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new AvifStreamReadException(ex);
            }

            return FromBuffer(inputBytes);
        }

        static Bitmap ConvertToBitmap(IntPtr imagePtr, IntPtr rgbImagePtr)
        {
            LibAvifNative.AvifRGBImageSetDefaults(rgbImagePtr, imagePtr);
            var rgbImage = Marshal.PtrToStructure<AvifRgbImage>(rgbImagePtr);
            rgbImage.format = AvifRGBFormat.AVIF_RGB_FORMAT_BGRA;
            Marshal.StructureToPtr(rgbImage, rgbImagePtr, true);
            LibAvifNative.AvifRGBImageAllocatePixels(rgbImagePtr);

            var result = LibAvifNative.AvifImageYUVToRGB(imagePtr, rgbImagePtr);
            if (result != AvifResult.AVIF_RESULT_OK)
                throw new AvifImageYuvToRgbException(Marshal.PtrToStringAnsi(LibAvifNative.AvifResultToString(result)));

            rgbImage = Marshal.PtrToStructure<AvifRgbImage>(rgbImagePtr);
            if (rgbImage.depth > 8)
                throw new AvifBitDepthNotSupportedException(rgbImage.depth);

            const int valuesPerPixel = 4;
            var elements = rgbImage.width * rgbImage.height * valuesPerPixel;
            var buffer = new byte[elements];
            Marshal.Copy(rgbImage.pixels, buffer, 0, (int)elements);

            var bitmap = new Bitmap((int)rgbImage.width, (int)rgbImage.height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, (int)rgbImage.width, (int)rgbImage.height);
            var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var ptr = bmpData.Scan0;
            Marshal.Copy(buffer, 0, ptr, buffer.Length);
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }
    }
}
