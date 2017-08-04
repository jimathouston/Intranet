using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Intranet.Services.Models
{
    public class StreamWithMetadata
    {
        public StreamWithMetadata(Stream stream, string contentType, string eTag)
        {
            this.Stream = stream;
            this.ContentType = contentType;
            this.ETag = eTag;
        }

        public Stream Stream { get; private set; }
        public string ContentType { get; private set; }
        public string ETag { get; private set; }
    }
}
