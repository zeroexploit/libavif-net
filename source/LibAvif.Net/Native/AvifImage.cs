using System;
using System.Runtime.InteropServices;

namespace LibAvif.Net.Native
{
    [StructLayout(LayoutKind.Sequential)]
    class AvifImage
    {
        // Image information
        public UInt32 width;
        public UInt32 height;
        public UInt32 depth; // all planes must share this depth; if depth>8, all planes are uint16_t internally

        public AvifPixelFormat yuvFormat;
        public AvifRange yuvRange;
        public AvifChromaSamplePosition yuvChromaSamplePosition;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public IntPtr[] yuvPlanes; // uint8_t * [3]

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public UInt32[] yuvRowBytes;// uint32_t  [3] 

        public int imageOwnsYUVPlanes;

        public AvifRange alphaRange;
        public IntPtr alphaPlane;
        public UInt32 alphaRowBytes;
        public int imageOwnsAlphaPlane;
        public int alphaPremultiplied;

        // ICC Profile
        public AvifRWData icc;

        // CICP information:
        // These are stored in the AV1 payload and used to signal YUV conversion. Additionally, if an
        // ICC profile is not specified, these will be stored in the AVIF container's `colr` box with
        // a type of `nclx`. If your system supports ICC profiles, be sure to check for the existence
        // of one (avifImage.icc) before relying on the values listed here!
        public UInt16 colorPrimaries;
        public UInt16 transferCharacteristics;
        public UInt16 matrixCoefficients;

        // Transformations - These metadata values are encoded/decoded when transformFlags are set
        // appropriately, but do not impact/adjust the actual pixel buffers used (images won't be
        // pre-cropped or mirrored upon decode). Basic explanations from the standards are offered in
        // comments above, but for detailed explanations, please refer to the HEIF standard (ISO/IEC
        // 23008-12:2017) and the BMFF standard (ISO/IEC 14496-12:2015).
        //
        // To encode any of these boxes, set the values in the associated box, then enable the flag in
        // transformFlags. On decode, only honor the values in boxes with the associated transform flag set.
        public UInt32 transformFlags;
        public AvifPixelAspectRatioBox pasp;
        public AvifCleanApertureBox clap;
        public AvifImageRotation irot;
        public AvifImageMirror imir;

        // Metadata - set with avifImageSetMetadata*() before write, check .size>0 for existence after read
        public AvifRWData exif;
        public AvifRWData xmp;
    }

    enum AvifPixelFormat
    {
        // No pixels are present
        AVIF_PIXEL_FORMAT_NONE = 0,

        AVIF_PIXEL_FORMAT_YUV444,
        AVIF_PIXEL_FORMAT_YUV422,
        AVIF_PIXEL_FORMAT_YUV420,
        AVIF_PIXEL_FORMAT_YUV400
    }

    enum AvifRange
    {
        AVIF_RANGE_LIMITED = 0,
        AVIF_RANGE_FULL = 1
    }

    enum AvifChromaSamplePosition
    {
        AVIF_CHROMA_SAMPLE_POSITION_UNKNOWN = 0,
        AVIF_CHROMA_SAMPLE_POSITION_VERTICAL = 1,
        AVIF_CHROMA_SAMPLE_POSITION_COLOCATED = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    struct AvifPixelAspectRatioBox
    {
        // 'pasp' from ISO/IEC 14496-12:2015 12.1.4.3

        // define the relative width and height of a pixel
        public UInt32 hSpacing;
        public UInt32 vSpacing;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct AvifCleanApertureBox
    {
        // 'clap' from ISO/IEC 14496-12:2015 12.1.4.3

        // a fractional number which defines the exact clean aperture width, in counted pixels, of the video image
        public UInt32 widthN;
        public UInt32 widthD;

        // a fractional number which defines the exact clean aperture height, in counted pixels, of the video image
        public UInt32 heightN;
        public UInt32 heightD;

        // a fractional number which defines the horizontal offset of clean aperture centre minus (width-1)/2. Typically 0.
        public UInt32 horizOffN;
        public UInt32 horizOffD;

        // a fractional number which defines the vertical offset of clean aperture centre minus (height-1)/2. Typically 0.
        public UInt32 vertOffN;
        public UInt32 vertOffD;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct AvifImageRotation
    {
        // 'irot' from ISO/IEC 23008-12:2017 6.5.10

        // angle * 90 specifies the angle (in anti-clockwise direction) in units of degrees.
        byte angle; // legal values: [0-3]
    }

    [StructLayout(LayoutKind.Sequential)]
    struct AvifImageMirror
    {
        // 'imir' from ISO/IEC 23008-12:2017 6.5.12:
        // "axis specifies a vertical (axis = 0) or horizontal (axis = 1) axis for the mirroring operation."
        //
        // Legal values: [0, 1]
        //
        // 0: Mirror about a vertical axis ("left-to-right")
        // 1: Mirror about a horizontal axis ("top-to-bottom")
        byte axis;
    }
}
