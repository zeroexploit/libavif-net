using System;
using System.Runtime.InteropServices;

namespace LibAvif.Net.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct AvifRWData
    {
        public IntPtr data;
        public UIntPtr size;
    }
}
