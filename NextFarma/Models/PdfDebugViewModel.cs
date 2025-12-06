using System.Collections.Generic;

namespace NextFarma.Models
{
    public class PdfWordInfo
    {
        public string Text { get; set; } = string.Empty;
        public double Left { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
    }

    public class PdfDebugViewModel
    {
        public List<PdfWordInfo> Words { get; set; } = new List<PdfWordInfo>();
    }
}
