using LibAvif.Net.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace LibAvif.Net
{
    public static class AvifImageDecoder
    {
        public static Image FromFile(string path)
        {
            var rgbImage = new AvifRgbImage();

            var decoderPtr = LibAvifNative.AvifDecoderCreate();
            var decoder = Marshal.PtrToStructure<AvifDecoder>(decoderPtr);

            var result = LibAvifNative.AvifDecoderSetIOFile(decoderPtr, Marshal.StringToHGlobalAnsi(path));

            return null;
        }
    }
}
