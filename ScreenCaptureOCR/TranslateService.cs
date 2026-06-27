using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ScreenCaptureOCR
{
    public class TranslateException : Exception
    {
        public bool IsNetworkError { get; }

        public TranslateException(string message, bool isNetworkError = false) : base(message)
        {
            IsNetworkError = isNetworkError;
        }
    }

    public class TranslateService
    {
        private readonly HttpClient _http;
        private const int MaxChunkLength = 1500;

        public TranslateService()
        {
            _http = new HttpClient();
            _http.Timeout = TimeSpan.FromSeconds(15);
        }

        public async Task<string> Translate(string text, string targetLang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            // 长文本分块翻译，避免 GET 请求 URL 超长
            if (text.Length > MaxChunkLength)
            {
                return await TranslateLongText(text, targetLang);
            }

            return await TranslateChunk(text, targetLang);
        }

        private async Task<string> TranslateLongText(string text, string targetLang)
        {
            var sb = new StringBuilder();
            int pos = 0;

            while (pos < text.Length)
            {
                int end = Math.Min(pos + MaxChunkLength, text.Length);
                if (end < text.Length)
                {
                    int breakAt = FindBreakPosition(text, end);
                    if (breakAt > pos)
                        end = breakAt;
                }

                string chunk = text.Substring(pos, end - pos);
                string result = await TranslateChunk(chunk, targetLang);
                sb.Append(result);
                pos = end;
            }

            return sb.ToString();
        }

        private int FindBreakPosition(string text, int nearPos)
        {
            // 先向前找句子边界
            for (int i = nearPos; i < text.Length && i < nearPos + 200; i++)
            {
                if ("。！？.!?\n\r".IndexOf(text[i]) >= 0)
                    return i + 1;
            }
            // 再向后找
            for (int i = nearPos - 1; i >= 0 && i > nearPos - 200; i--)
            {
                if ("。！？.!?\n\r".IndexOf(text[i]) >= 0)
                    return i + 1;
            }
            return nearPos;
        }

        private async Task<string> TranslateChunk(string text, string targetLang)
        {
            string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={targetLang}&dt=t&q={Uri.EscapeDataString(text)}";

            string response;
            try
            {
                response = await _http.GetStringAsync(url);
            }
            catch (HttpRequestException)
            {
                throw new TranslateException("网络连接失败，请检查网络", true);
            }
            catch (TaskCanceledException)
            {
                throw new TranslateException("请求超时，请检查网络", true);
            }

            var sb = new StringBuilder();
            try
            {
                var arr = JArray.Parse(response);
                var sentences = arr[0] as JArray;
                if (sentences != null)
                {
                    foreach (var item in sentences)
                    {
                        var sentenceArr = item as JArray;
                        if (sentenceArr != null && sentenceArr.Count > 0 && sentenceArr[0].Type == JTokenType.String)
                        {
                            sb.Append(sentenceArr[0].ToString());
                        }
                    }
                }
            }
            catch
            {
                return text;
            }

            return sb.ToString();
        }
    }
}
