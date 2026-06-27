using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ScreenCaptureOCR
{
    public class OcrException : Exception
    {
        public bool IsNetworkError { get; }

        public OcrException(string message, bool isNetworkError = false) : base(message)
        {
            IsNetworkError = isNetworkError;
        }
    }

    public class OcrService
    {
        private readonly HttpClient _http;
        private string _accessToken;
        private DateTime _tokenExpiry;
        private readonly string _apiKey;
        private readonly string _secretKey;

        public OcrService()
        {
            _http = new HttpClient();
            _http.Timeout = TimeSpan.FromSeconds(30);
            _apiKey = Properties.Settings.Default.OcrApiKey;
            _secretKey = Properties.Settings.Default.OcrSecretKey;
        }

        private async Task<string> GetAccessToken()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.Now < _tokenExpiry)
                return _accessToken;

            string url = $"https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id={_apiKey}&client_secret={_secretKey}";
            var response = await _http.PostAsync(url, null);
            string json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);

            if (obj["error"] != null)
                throw new Exception($"获取Token失败: {obj["error_description"]}");

            _accessToken = obj["access_token"].ToString();
            int expiresIn = obj["expires_in"].Value<int>();
            _tokenExpiry = DateTime.Now.AddSeconds(expiresIn - 300);
            return _accessToken;
        }

        public async Task<string> RecognizeText(Bitmap image)
        {
            string token;
            try
            {
                token = await GetAccessToken();
            }
            catch (HttpRequestException)
            {
                throw new OcrException("网络连接失败，请检查网络", true);
            }
            catch (TaskCanceledException)
            {
                throw new OcrException("请求超时，请检查网络", true);
            }

            string base64 = ImageToBase64(image);
            string url = $"https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token={token}";

            var content = new StringContent($"image={Uri.EscapeDataString(base64)}", Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response;
            string json;
            try
            {
                response = await _http.PostAsync(url, content);
                json = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                throw new OcrException("网络连接失败，请检查网络", true);
            }
            catch (TaskCanceledException)
            {
                throw new OcrException("请求超时，请检查网络", true);
            }
            var obj = JObject.Parse(json);

            if (obj["error_code"] != null)
                throw new Exception($"OCR识别失败: {obj["error_msg"]}");

            var wordsResult = obj["words_result"] as JArray;
            if (wordsResult == null || wordsResult.Count == 0)
                return "";

            var sb = new StringBuilder();
            foreach (var item in wordsResult)
            {
                sb.AppendLine(item["words"].ToString());
            }
            return sb.ToString().TrimEnd();
        }

        private string ImageToBase64(Bitmap image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                // 压缩图片，限制大小在4MB以内（百度API限制）
                var qualityImage = CompressImage(image, 1024);
                qualityImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private Bitmap CompressImage(Bitmap source, int maxDimension)
        {
            if (source.Width <= maxDimension && source.Height <= maxDimension)
                return new Bitmap(source);

            float ratio = Math.Min((float)maxDimension / source.Width, (float)maxDimension / source.Height);
            int newWidth = (int)(source.Width * ratio);
            int newHeight = (int)(source.Height * ratio);

            var result = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(source, 0, 0, newWidth, newHeight);
            }
            return result;
        }
    }
}
