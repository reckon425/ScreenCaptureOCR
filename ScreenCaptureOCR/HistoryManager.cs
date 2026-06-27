using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace ScreenCaptureOCR
{
    public class HistoryManager
    {
        private readonly string _historyDir;
        private readonly List<HistoryItem> _items;

        public IReadOnlyList<HistoryItem> Items => _items.AsReadOnly();

        public HistoryManager()
        {
            _historyDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ScreenCaptureOCR", "History");
            Directory.CreateDirectory(_historyDir);
            _items = new List<HistoryItem>();
            Load();
        }

        public HistoryItem Add(Bitmap image, string ocrText, string translationText, string sourceType)
        {
            string preview = ocrText != null && ocrText.Length > 25
                ? ocrText.Substring(0, 25) + "..."
                : (ocrText ?? "(空)");

            var item = new HistoryItem
            {
                Id = Guid.NewGuid().ToString("N"),
                Time = DateTime.Now,
                PreviewText = preview,
                OcrText = ocrText ?? "",
                TranslationText = translationText ?? "",
                SourceType = sourceType,
                ImagePath = ""
            };

            if (image != null)
            {
                string filePath = Path.Combine(_historyDir, item.Id + ".jpg");
                try
                {
                    using (var thumb = new Bitmap(image, 200, 150))
                    {
                        thumb.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    item.ImagePath = filePath;
                }
                catch { }
            }

            _items.Insert(0, item);
            Save();
            return item;
        }

        public void Save()
        {
            while (_items.Count > 50)
            {
                var old = _items[_items.Count - 1];
                try { File.Delete(old.ImagePath); } catch { }
                _items.RemoveAt(_items.Count - 1);
            }

            string json = JsonConvert.SerializeObject(_items);
            File.WriteAllText(Path.Combine(_historyDir, "index.json"), json);
        }

        public void Load()
        {
            string idxFile = Path.Combine(_historyDir, "index.json");
            if (!File.Exists(idxFile)) return;

            try
            {
                string json = File.ReadAllText(idxFile);
                var list = JsonConvert.DeserializeObject<List<HistoryItem>>(json);
                if (list == null) return;

                _items.Clear();
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.ImagePath) || !File.Exists(item.ImagePath))
                        continue;
                    _items.Add(item);
                }
            }
            catch
            {
                _items.Clear();
            }
        }

        public static string GetSourceLabel(string sourceType)
        {
            switch (sourceType)
            {
                case "截图": return "[截图]";
                case "文本": return "[手动]";
                case "文件": return "[文件]";
                default: return sourceType ?? "";
            }
        }
    }
}
