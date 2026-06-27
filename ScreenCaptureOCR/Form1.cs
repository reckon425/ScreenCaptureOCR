using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using UglyToad.PdfPig;

namespace ScreenCaptureOCR
{
    public partial class Form1 : Form
    {
        private Bitmap _capturedImage;
        private readonly OcrService _ocrService;
        private readonly TranslateService _translateService;
        private readonly HistoryManager _historyManager;
        private string _lastInputSource = "文本";  // "文本" / "文件"

        // 全局热键
        private const int HotkeyId = 1;
        private Keys _hotkey = Keys.F2;

        public Form1()
        {
            InitializeComponent();
            ApplyMetroStyle();
            btnOpenImage.Text = "上传图片";
            button1.Text = "上传文本";
            // 添加"来源"列
            if (lvHistory.Columns.Count < 3)
            {
                lvHistory.Columns.Add("来源", 80);
            }
            cbLanguage.SelectedIndex = Properties.Settings.Default.TargetLangIndex;
            _ocrService = new OcrService();
            _translateService = new TranslateService();
            _historyManager = new HistoryManager();
            RefreshHistoryList();
            RegisterHotkey();
            UpdateTrayIcon();
        }

        // ======================== Metro 风格美化 ========================

        private void ApplyMetroStyle()
        {
            var primary = System.Drawing.Color.FromArgb(91, 163, 230);      // 浅蓝 #5BA3E6
            var primaryHover = System.Drawing.Color.FromArgb(74, 144, 226); // 原主色 #4A90E2
            var formBg = System.Drawing.Color.FromArgb(245, 249, 255);      // #F5F9FF
            var textDark = System.Drawing.Color.FromArgb(44, 62, 80);       // #2C3E50
            var textSecondary = System.Drawing.Color.FromArgb(122, 140, 165); // #7A8CA5
            var borderLight = System.Drawing.Color.FromArgb(191, 215, 255); // #BFD7FF

            this.BackColor = formBg;
            this.ForeColor = textDark;

            foreach (var ctl in this.Controls)
            {
                if (ctl is System.Windows.Forms.Button btn)
                {
                    btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = primary;
                    btn.ForeColor = System.Drawing.Color.White;
                    btn.FlatAppearance.MouseOverBackColor = primaryHover;
                }
                else if (ctl is System.Windows.Forms.ComboBox cb)
                {
                    cb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    cb.BackColor = System.Drawing.Color.White;
                    cb.ForeColor = textDark;
                }
                else if (ctl is System.Windows.Forms.RichTextBox rtb)
                {
                    rtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    rtb.BackColor = System.Drawing.Color.White;
                    rtb.ForeColor = textDark;
                }
                else if (ctl is System.Windows.Forms.PictureBox pb)
                {
                    pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    pb.BackColor = System.Drawing.Color.White;
                }
                else if (ctl is System.Windows.Forms.ListView lv)
                {
                    lv.BackColor = System.Drawing.Color.White;
                    lv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    lv.ForeColor = textDark;
                    lv.FullRowSelect = true;
                    lv.GridLines = false;
                }
                else if (ctl is System.Windows.Forms.TextBox tb)
                {
                    tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    tb.BackColor = System.Drawing.Color.White;
                    tb.ForeColor = textDark;
                }
                else if (ctl is System.Windows.Forms.Label lbl)
                {
                    lbl.ForeColor = textDark;
                }
            }

            // 动态添加"历史记录"标签
            System.Windows.Forms.ListView historyView = null;
            foreach (var ctl in this.Controls)
            {
                if (ctl is System.Windows.Forms.ListView lv)
                {
                    historyView = lv;
                    break;
                }
            }
            if (historyView != null)
            {
                bool hasLabel = false;
                foreach (var ctl in this.Controls)
                {
                    if (ctl is System.Windows.Forms.Label lbl && lbl.Name == "lblHistoryHeader")
                    {
                        hasLabel = true;
                        break;
                    }
                }
                if (!hasLabel)
                {
                    var lblHistory = new System.Windows.Forms.Label();
                    lblHistory.Name = "lblHistoryHeader";
                    lblHistory.Text = "历史记录";
                    lblHistory.Font = new System.Drawing.Font("微软雅黑", 9F);
                    lblHistory.ForeColor = textDark;
                    lblHistory.AutoSize = true;
                    lblHistory.Location = new System.Drawing.Point(
                        historyView.Location.X, historyView.Location.Y - 28);
                    this.Controls.Add(lblHistory);
                }
            }
        }

        // ======================== 打开文本文件 ========================

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "支持的文件|*.txt;*.log;*.ini;*.csv;*.docx;*.pdf|文本文件|*.txt;*.log;*.ini;*.csv|Word文档|*.docx|PDF文档|*.pdf|所有文件|*.*";
                ofd.Title = "选择要翻译的文件";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string text = ReadFileText(ofd.FileName);
                        txtManualInput.Text = text;
                        _lastInputSource = "文件";
                        string fileName = Path.GetFileName(ofd.FileName);
                        MessageBox.Show("已加载文件：" + fileName + "\n共 " + text.Length + " 个字符", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("读取文件失败：" + ex.Message, "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string ReadFileText(string path)
        {
            string ext = Path.GetExtension(path).ToLower();

            switch (ext)
            {
                case ".txt":
                case ".log":
                case ".ini":
                case ".csv":
                    string utf8Text = File.ReadAllText(path, Encoding.UTF8);
                    if (!string.IsNullOrWhiteSpace(utf8Text))
                        return utf8Text;
                    return File.ReadAllText(path, Encoding.Default);

                case ".docx":
                    return ReadDocxText(path);

                case ".pdf":
                    return ReadPdfText(path);

                default:
                    return File.ReadAllText(path, Encoding.UTF8);
            }
        }

        private string ReadDocxText(string path)
        {
            var sb = new StringBuilder();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry("word/document.xml");
                if (entry == null) return "无法解析此文档：找不到 word/document.xml";

                using (var reader = new StreamReader(entry.Open()))
                {
                    string xml = reader.ReadToEnd();
                    XDocument doc = XDocument.Parse(xml);
                    XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

                    foreach (var t in doc.Descendants(w + "t"))
                    {
                        sb.Append(t.Value);
                    }
                }
            }
            return sb.ToString();
        }

        private string ReadPdfText(string path)
        {
            var sb = new StringBuilder();
            using (var pdf = PdfDocument.Open(path))
            {
                for (int i = 1; i <= pdf.NumberOfPages; i++)
                {
                    var page = pdf.GetPage(i);
                    sb.AppendLine(page.Text);
                }
            }
            return sb.ToString();
        }

        // ======================== 热键注册 ========================

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void RegisterHotkey()
        {
            uint modifiers = 0;
            if ((_hotkey & Keys.Control) == Keys.Control) modifiers |= 0x0002;
            if ((_hotkey & Keys.Alt) == Keys.Alt) modifiers |= 0x0001;
            if ((_hotkey & Keys.Shift) == Keys.Shift) modifiers |= 0x0004;

            Keys keyCode = _hotkey & ~Keys.Control & ~Keys.Alt & ~Keys.Shift;
            RegisterHotKey(this.Handle, HotkeyId, modifiers, (uint)keyCode);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == HotkeyId)
            {
                StartCapture();
            }
            base.WndProc(ref m);
        }

        // ======================== 截图 ========================

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置 NotifyIcon 图标（从系统图标获取）
            notifyIcon.Icon = SystemIcons.Application;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                StartCapture();
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            StartCapture();
        }

        private void StartCapture()
        {
            this.WindowState = FormWindowState.Minimized;
            System.Threading.Thread.Sleep(400);

            Bitmap screenShot = null;
            try
            {
                var bounds = SystemInformation.VirtualScreen;
                screenShot = new Bitmap(bounds.Width, bounds.Height);
                using (var g = Graphics.FromImage(screenShot))
                {
                    g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
                }

                using (var fs = new FormScreenshot(screenShot, this.DeviceDpi / 96f))
                {
                    screenShot = null;
                    if (fs.ShowDialog() == DialogResult.OK && fs.CapturedRegion != null)
                    {
                        _capturedImage?.Dispose();
                        _capturedImage = fs.CapturedRegion;
                        pbPreview.Image = _capturedImage;
                        rtbOcrResult.Text = "";
                        rtbTranslation.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                screenShot?.Dispose();
                MessageBox.Show("截图失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            }
        }

        // ======================== 打开本地图片 ========================

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.tiff";
                ofd.Title = "选择要识别的图片";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var img = Image.FromFile(ofd.FileName);
                        _capturedImage?.Dispose();
                        _capturedImage = new Bitmap(img);
                        img.Dispose();
                        pbPreview.Image = _capturedImage;
                        rtbOcrResult.Text = "";
                        rtbTranslation.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开图片失败：" + ex.Message, "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ======================== OCR + 翻译 ========================

        private string GetTargetLang()
        {
            switch (cbLanguage.SelectedItem.ToString())
            {
                case "中文": return "zh";
                case "英语": return "en";
                case "日语": return "ja";
                case "韩语": return "ko";
                case "法语": return "fr";
                case "德语": return "de";
                case "俄语": return "ru";
                default: return "en";
            }
        }

        private async void btnTranslate_Click(object sender, EventArgs e)
        {
            string manualText = txtManualInput.Text.Trim();
            string targetLang = GetTargetLang();

            // 如果有手动输入的文字，直接翻译（跳过OCR）
            if (!string.IsNullOrEmpty(manualText))
            {
                btnTranslate.Enabled = false;
                btnTranslate.Text = "翻译中...";
                try
                {
                    string translated = await Task.Run(() => _translateService.Translate(manualText, targetLang));
                    rtbOcrResult.Text = manualText;
                    rtbTranslation.Text = translated;

                    _historyManager.Add(null, manualText, translated, _lastInputSource);
                    _lastInputSource = "文本";  // 用完重置
                    RefreshHistoryList();
                }
                catch (TranslateException tex)
                {
                    rtbOcrResult.Text = "";
                    rtbTranslation.Text = "";
                    MessageBox.Show("翻译失败：" + tex.Message, "网络错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    rtbOcrResult.Text = "";
                    rtbTranslation.Text = "";
                    MessageBox.Show("翻译失败：" + ex.Message, "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnTranslate.Enabled = true;
                    btnTranslate.Text = "翻译";
                }
                return;
            }

            // 没有手动输入 → OCR + 翻译
            if (_capturedImage == null)
            {
                MessageBox.Show("请截图或打开图片，或输入要翻译的文字", "提示");
                return;
            }

            btnTranslate.Enabled = false;
            btnTranslate.Text = "识别中...";

            try
            {
                string ocrText = await Task.Run(() => _ocrService.RecognizeText(_capturedImage));
                rtbOcrResult.Text = ocrText;

                if (!string.IsNullOrWhiteSpace(ocrText))
                {
                    btnTranslate.Text = "翻译中...";
                    string translated = await Task.Run(() => _translateService.Translate(ocrText, targetLang));
                    rtbTranslation.Text = translated;

                    // OCR+翻译成功后加入历史记录
                    if (_capturedImage != null)
                    {
                        _historyManager.Add(_capturedImage, ocrText, translated, "截图");
                        RefreshHistoryList();
                    }
                }
                else
                {
                    rtbTranslation.Text = "未识别到文字";
                }
            }
            catch (OcrException oex)
            {
                rtbOcrResult.Text = "";
                rtbTranslation.Text = "";
                MessageBox.Show(oex.Message, "识别失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (TranslateException tex)
            {
                rtbOcrResult.Text = "";
                rtbTranslation.Text = "";
                MessageBox.Show(tex.Message, "翻译失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("请求失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                rtbOcrResult.Text = "";
                rtbTranslation.Text = "";
            }
            finally
            {
                btnTranslate.Enabled = true;
                btnTranslate.Text = "翻译";
            }
        }

        // ======================== 导出结果 ========================

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbOcrResult.Text) && string.IsNullOrWhiteSpace(rtbTranslation.Text))
            {
                MessageBox.Show("没有可导出的内容", "提示");
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "文本文件|*.txt";
                sfd.FileName = $"OCR结果_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var sw = new StreamWriter(sfd.FileName))
                        {
                            sw.WriteLine("=== OCR 识别结果 ===");
                            sw.WriteLine(rtbOcrResult.Text);
                            sw.WriteLine();
                            sw.WriteLine("=== 翻译结果 ===");
                            sw.WriteLine(rtbTranslation.Text);
                        }
                        MessageBox.Show("导出成功：" + sfd.FileName, "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("导出失败：" + ex.Message, "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ======================== 复制 ========================

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rtbTranslation.Text))
            {
                Clipboard.SetText(rtbTranslation.Text);
                MessageBox.Show("翻译结果已复制到剪贴板", "提示");
            }
        }

        // ======================== 历史记录 ========================

        private void RefreshHistoryList()
        {
            lvHistory.BeginUpdate();
            lvHistory.Items.Clear();
            foreach (var item in _historyManager.Items)
            {
                var lvItem = new ListViewItem(item.Time.ToString("HH:mm:ss"));
                lvItem.SubItems.Add(HistoryManager.GetSourceLabel(item.SourceType));
                lvItem.SubItems.Add(item.PreviewText);
                lvItem.Tag = item;
                lvHistory.Items.Add(lvItem);
            }
            lvHistory.EndUpdate();
        }

        private void lvHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvHistory.SelectedItems.Count == 0) return;

            var item = lvHistory.SelectedItems[0].Tag as HistoryItem;
            if (item == null) return;

            rtbOcrResult.Text = item.OcrText ?? "";
            rtbTranslation.Text = item.TranslationText ?? "";

            if (!string.IsNullOrEmpty(item.ImagePath))
            {
                try
                {
                    var img = Image.FromFile(item.ImagePath);
                    _capturedImage?.Dispose();
                    _capturedImage = new Bitmap(img);
                    img.Dispose();
                    pbPreview.Image = _capturedImage;
                }
                catch { }
            }
        }

        private void lvHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lvHistory.SelectedItems.Count == 0) return;

            var item = lvHistory.SelectedItems[0].Tag as HistoryItem;
            if (item == null) return;

            using (var detail = new HistoryDetailForm(item))
            {
                detail.ShowDialog();
            }
        }

        // ======================== 系统托盘 ========================

        private void UpdateTrayIcon()
        {
            var programIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (programIcon != null)
                notifyIcon.Icon = programIcon;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(1000, "截图OCR翻译工具",
                    "程序已最小化到托盘，按快捷键随时截图", ToolTipIcon.Info);
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void miShow_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowWindow()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.BringToFront();
            notifyIcon.Visible = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HotkeyId);
            notifyIcon.Visible = false;
            base.OnFormClosing(e);
        }

        // ======================== 设置窗口 ========================

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var form = new SettingsForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    cbLanguage.SelectedIndex = Properties.Settings.Default.TargetLangIndex;
                    // 重新注册热键
                    UnregisterHotKey(this.Handle, HotkeyId);
                    _hotkey = (Keys)Properties.Settings.Default.Hotkey;
                    RegisterHotkey();
                }
            }
        }

        private void lblManualInput_Click(object sender, EventArgs e)
        {

        }

    }
}
