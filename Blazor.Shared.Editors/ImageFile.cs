using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blazor.Shared.Editors
{
    public class ImageFile
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("filename")]
        public string? FileName { get; set; }
        [JsonPropertyName("size")]
        public long Size { get; set; } // Grootte in bytes
        [JsonPropertyName("lastmodified")]
        public DateTime LastModified { get; set; }
    }
}
