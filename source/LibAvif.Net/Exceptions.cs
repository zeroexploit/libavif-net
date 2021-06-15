using System;

namespace LibAvif.Net
{
    public class AvifInputNullOrEmptyException : Exception
    {
        public AvifInputNullOrEmptyException() : base("the given input was null or empty") { }
    }

    public class AvifFileNotFoundException : Exception
    {
        public AvifFileNotFoundException(string path) : base($"file {path} was not found") { }
    }

    public class AvifDecoderReadFileException : Exception
    {
        public AvifDecoderReadFileException(string file, string libMessage) : base($"unable to decode file {file}. reason: {libMessage}") { }
    }

    public class AvifImageYuvToRgbException : Exception
    {
        public AvifImageYuvToRgbException(string file, string libMessage) : base($"unable to convert {file} from yuv to rgb. reason: {libMessage}") { }
    }

    public class AvifBitDepthNotSupportedException : Exception
    {
        public AvifBitDepthNotSupportedException(uint bitDepth) : base($"bit depth of {bitDepth} currently not supported. use 8 bits as for now.") { }
    }
}
