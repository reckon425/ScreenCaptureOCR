namespace ScreenCaptureOCR
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCapture = new System.Windows.Forms.Button();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.rtbOcrResult = new System.Windows.Forms.RichTextBox();
            this.rtbTranslation = new System.Windows.Forms.RichTextBox();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpenImage = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lvHistory = new System.Windows.Forms.ListView();
            this.chTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miShow = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblManualInput = new System.Windows.Forms.Label();
            this.txtManualInput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCapture
            // 
            this.btnCapture.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnCapture.Location = new System.Drawing.Point(23, 23);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(130, 50);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "截图 (F1)";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // cbLanguage
            // 
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Items.AddRange(new object[] {
            "中文",
            "英语",
            "日语",
            "韩语",
            "法语",
            "德语",
            "俄语"});
            this.cbLanguage.Location = new System.Drawing.Point(23, 87);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(130, 43);
            this.cbLanguage.TabIndex = 1;
            // 
            // pbPreview
            // 
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(23, 138);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(520, 340);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPreview.TabIndex = 2;
            this.pbPreview.TabStop = false;
            // 
            // rtbOcrResult
            // 
            this.rtbOcrResult.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rtbOcrResult.Location = new System.Drawing.Point(23, 518);
            this.rtbOcrResult.Name = "rtbOcrResult";
            this.rtbOcrResult.ReadOnly = true;
            this.rtbOcrResult.Size = new System.Drawing.Size(520, 271);
            this.rtbOcrResult.TabIndex = 3;
            this.rtbOcrResult.Text = "";
            // 
            // rtbTranslation
            // 
            this.rtbTranslation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rtbTranslation.Location = new System.Drawing.Point(23, 841);
            this.rtbTranslation.Name = "rtbTranslation";
            this.rtbTranslation.ReadOnly = true;
            this.rtbTranslation.Size = new System.Drawing.Size(1110, 305);
            this.rtbTranslation.TabIndex = 4;
            this.rtbTranslation.Text = "";
            // 
            // btnTranslate
            // 
            this.btnTranslate.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnTranslate.Location = new System.Drawing.Point(341, 81);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(143, 45);
            this.btnTranslate.TabIndex = 5;
            this.btnTranslate.Text = "翻译";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnCopy.Location = new System.Drawing.Point(1194, 855);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 45);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "复制结果";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(20, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 31);
            this.label1.TabIndex = 7;
            this.label1.Text = "识别结果";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label2.Location = new System.Drawing.Point(20, 822);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 31);
            this.label2.TabIndex = 8;
            this.label2.Text = "翻译结果";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label3.Location = new System.Drawing.Point(165, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 31);
            this.label3.TabIndex = 9;
            this.label3.Text = "目标翻译语言";
            // 
            // btnOpenImage
            // 
            this.btnOpenImage.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnOpenImage.Location = new System.Drawing.Point(171, 23);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(153, 50);
            this.btnOpenImage.TabIndex = 10;
            this.btnOpenImage.Text = "上传图片";
            this.btnOpenImage.UseVisualStyleBackColor = true;
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnExport.Location = new System.Drawing.Point(1194, 942);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 45);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "导出结果";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnSettings.Location = new System.Drawing.Point(1055, 25);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 50);
            this.btnSettings.TabIndex = 12;
            this.btnSettings.Text = "设置";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lvHistory
            // 
            this.lvHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTime,
            this.chText});
            this.lvHistory.FullRowSelect = true;
            this.lvHistory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvHistory.HideSelection = false;
            this.lvHistory.Location = new System.Drawing.Point(572, 518);
            this.lvHistory.MultiSelect = false;
            this.lvHistory.Name = "lvHistory";
            this.lvHistory.Size = new System.Drawing.Size(604, 271);
            this.lvHistory.TabIndex = 13;
            this.lvHistory.UseCompatibleStateImageBehavior = false;
            this.lvHistory.View = System.Windows.Forms.View.Details;
            this.lvHistory.SelectedIndexChanged += new System.EventHandler(this.lvHistory_SelectedIndexChanged);
            this.lvHistory.DoubleClick += new System.EventHandler(this.lvHistory_DoubleClick);
            // 
            // chTime
            // 
            this.chTime.Text = "时间";
            this.chTime.Width = 80;
            // 
            // chText
            // 
            this.chText.Text = "内容";
            this.chText.Width = 110;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.trayMenu;
            this.notifyIcon.Text = "截图OCR翻译工具";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShow,
            this.miExit});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(185, 80);
            // 
            // miShow
            // 
            this.miShow.Name = "miShow";
            this.miShow.Size = new System.Drawing.Size(184, 38);
            this.miShow.Text = "显示窗口";
            this.miShow.Click += new System.EventHandler(this.miShow_Click);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(184, 38);
            this.miExit.Text = "退出";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // lblManualInput
            // 
            this.lblManualInput.AutoSize = true;
            this.lblManualInput.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblManualInput.Location = new System.Drawing.Point(586, 104);
            this.lblManualInput.Name = "lblManualInput";
            this.lblManualInput.Size = new System.Drawing.Size(206, 31);
            this.lblManualInput.TabIndex = 14;
            this.lblManualInput.Text = "手动输入文字翻译";
            this.lblManualInput.Click += new System.EventHandler(this.lblManualInput_Click);
            // 
            // txtManualInput
            // 
            this.txtManualInput.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtManualInput.Location = new System.Drawing.Point(583, 138);
            this.txtManualInput.Multiline = true;
            this.txtManualInput.Name = "txtManualInput";
            this.txtManualInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtManualInput.Size = new System.Drawing.Size(621, 340);
            this.txtManualInput.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 44);
            this.button1.TabIndex = 16;
            this.button1.Text = "上传文本";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1506, 1245);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtManualInput);
            this.Controls.Add(this.lblManualInput);
            this.Controls.Add(this.lvHistory);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnOpenImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.rtbTranslation);
            this.Controls.Add(this.rtbOcrResult);
            this.Controls.Add(this.pbPreview);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.btnCapture);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "截图OCR翻译工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.RichTextBox rtbOcrResult;
        private System.Windows.Forms.RichTextBox rtbTranslation;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpenImage;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ListView lvHistory;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chText;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem miShow;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.Label lblManualInput;
        private System.Windows.Forms.TextBox txtManualInput;
        private System.Windows.Forms.Button button1;
    }
}
