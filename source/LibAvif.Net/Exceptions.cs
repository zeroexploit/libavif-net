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

    public class AvifDecoderReadMemoryException : Exception
    {
        public AvifDecoderReadMemoryException(string libMessage) : base($"unable to decode buffer. reason: {libMessage}") { }
    }

    public class AvifImageYuvToRgbException : Exception
    {
        public AvifImageYuvToRgbException(string libMessage) : base($"unable to convert from yuv to rgb. reason: {libMessage}") { }
    }

    public class AvifBitDepthNotSupportedException : Exception
    {
        public AvifBitDepthNotSupportedException(uint bitDepth) : base($"bit depth of {bitDepth} currently not supported. use 8 bits as for now.") { }
    }

    public class AvifStreamNotSupportedException : Exception
    {
        public AvifStreamNotSupportedException() : base("stream must be readable and either seekable or read position should be at the beginning") { }
    }

    public class AvifStreamReadException : Exception
    {
        public AvifStreamReadException(Exception exception) : base("unable to read stream. see inner exception for details.", exception) { }
    }
}
