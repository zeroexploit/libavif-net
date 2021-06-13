using System;
using System.Runtime.InteropServices;

namespace LibAvif.Net.Native
{
    [StructLayout(LayoutKind.Sequential)]
    struct AvifIO
    {
        // todo: checken ob typ stimmt 
        public IntPtr destroy; 
        public IntPtr read;

        // This is reserved for future use - but currently ignored. Set it to a null pointer.
        public IntPtr write;

        // If non-zero, this is a hint to internal structures of the max size offered by the content
        // this avifIO structure is reading. If it is a static memory source, it should be the size of
        // the memory buffer; if it is a file, it should be the file's size. If this information cannot
        // be known (as it is streamed-in), set a reasonable upper boundary here (larger than the file
        // can possibly be for your environment, but within your environment's memory constraints). This
        // is used for sanity checks when allocating internal buffers to protect against
        // malformed/malicious files.
        public UInt64 sizeHint;

        // If true, *all* memory regions returned from *all* calls to read are guaranteed to be
        // persistent and exist for the lifetime of the avifIO object. If false, libavif will make
        // in-memory copies of samples and metadata content, and a memory region returned from read must
        // only persist until the next call to read.
        public int persistent;

        // The contents of this are defined by the avifIO implementation, and should be fully destroyed
        // by the implementation of the associated destroy function, unless it isn't owned by the avifIO
        // struct. It is not necessary to use this pointer in your implementation.
        public IntPtr data;
    }
}
