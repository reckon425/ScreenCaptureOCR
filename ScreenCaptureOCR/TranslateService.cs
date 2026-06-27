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

        public TranslateService()
        {
            _http = new HttpClient();
            _http.Timeout = TimeSpan.FromSeconds(15);
        }

        public async Task<string> Translate(string text, string targetLang)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

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
