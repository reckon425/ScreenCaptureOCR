using System;

namespace ScreenCaptureOCR
{
    public class HistoryItem
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string ImagePath { get; set; }
        public string PreviewText { get; set; }
        public string OcrText { get; set; }
        public string TranslationText { get; set; }
        public string SourceType { get; set; }
    }
}
