using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LibAvif.Net.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct AvifDecoder
    {
        // Defaults to AVIF_CODEC_CHOICE_AUTO: Preference determined by order in availableCodecs table (avif.c)
        public AvifCodecChoice codecChoice;

        // Defaults to 1.
        public int maxThreads;

        // avifs can have multiple sets of images in them. This specifies which to decode.
        // Set this via avifDecoderSetSource().
        public AvifDecoderSource requestedSource;

        // All decoded image data; owned by the decoder. All information in this image is incrementally
        // added and updated as avifDecoder*() functions are called. After a successful call to
        // avifDecoderParse(), all values in decoder->image (other than the planes/rowBytes themselves)
        // will be pre-populated with all information found in the outer AVIF container, prior to any
        // AV1 decoding. If the contents of the inner AV1 payload disagree with the outer container,
        // these values may change after calls to avifDecoderRead*(),avifDecoderNextImage(), or
        // avifDecoderNthImage().
        //
        // The YUV and A contents of this image are likely owned by the decoder, so be sure to copy any
        // data inside of this image before advancing to the next image or reusing the decoder. It is
        // legal to call avifImageYUVToRGB() on this in between calls to avifDecoderNextImage(), but use
        // avifImageCopy() if you want to make a complete, permanent copy of this image's YUV content or
        // metadata.
        public IntPtr image; // AvifImage*

        // Counts and timing for the current image in an image sequence. Uninteresting for single image files.
        public int imageIndex;                // 0-based
        public int imageCount;                // Always 1 for non-sequences
        public AvifImageTiming imageTiming;   //
        public UInt64 timescale;            // timescale of the media (Hz)
        double duration;               // in seconds (durationInTimescales / timescale)
        public UInt64 durationInTimescales; // duration in "timescales"

        // This is true when avifDecoderParse() detects an alpha plane. Use this to find out if alpha is
        // present after a successful call to avifDecoderParse(), but prior to any call to
        // avifDecoderNextImage() or avifDecoderNthImage(), as decoder->image->alphaPlane won't exist yet.
        public int alphaPresent;

        // Enable any of these to avoid reading and surfacing specific data to the decoded avifImage.
        // These can be useful if your avifIO implementation heavily uses AVIF_RESULT_WAITING_ON_IO for
        // streaming data, as some of these payloads are (unfortunately) packed at the end of the file,
        // which will cause avifDecoderParse() to return AVIF_RESULT_WAITING_ON_IO until it finds them.
        // If you don't actually leverage this data, it is best to ignore it here.
        public int ignoreExif;
        public int ignoreXMP;

        // stats from the most recent read, possibly 0s if reading an image sequence
        public int ioStats;

        // Use one of the avifDecoderSetIO*() functions to set this
        public IntPtr io; // AvifIO*

        // Internals used by the decoder
        public IntPtr data;
    }
    enum AvifCodecChoice
    {
        AVIF_CODEC_CHOICE_AUTO = 0,
        AVIF_CODEC_CHOICE_AOM,
        AVIF_CODEC_CHOICE_DAV1D,   // Decode only
        AVIF_CODEC_CHOICE_LIBGAV1, // Decode only
        AVIF_CODEC_CHOICE_RAV1E,   // Encode only
        AVIF_CODEC_CHOICE_SVT      // Encode only
    }

    enum AvifDecoderSource
    {
        // If a moov box is present in the .avif(s), use the tracks in it, otherwise decode the primary item.
        AVIF_DECODER_SOURCE_AUTO = 0,

        // Use the primary item and the aux (alpha) item in the avif(s).
        // This is where single-image avifs store their image.
        AVIF_DECODER_SOURCE_PRIMARY_ITEM,

        // Use the chunks inside primary/aux tracks in the moov block.
        // This is where avifs image sequences store their images.
        AVIF_DECODER_SOURCE_TRACKS,

        // Decode the thumbnail item. Currently unimplemented.
        // AVIF_DECODER_SOURCE_THUMBNAIL_ITEM
    }

    [StructLayout(LayoutKind.Sequential)]
    struct AvifImageTiming
    {
        public UInt64 timescale;            // timescale of the media (Hz)
        public double pts;                    // presentation timestamp in seconds (ptsInTimescales / timescale)
        public UInt64 ptsInTimescales;      // presentation timestamp in "timescales"
        public double duration;               // in seconds (durationInTimescales / timescale)
        public UInt64 durationInTimescales; // duration in "timescales"
    }
}
