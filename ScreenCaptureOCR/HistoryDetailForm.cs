using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenCaptureOCR
{
    public partial class HistoryDetailForm : Form
    {
        private readonly HistoryItem _item;

        public HistoryDetailForm(HistoryItem item)
        {
            _item = item;
            InitializeComponent();
            ApplyMetroStyle();
            LoadDetail();
        }

        private void InitializeComponent()
        {
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lblOcr = new System.Windows.Forms.Label();
            this.rtbOcr = new System.Windows.Forms.RichTextBox();
            this.lblTranslation = new System.Windows.Forms.Label();
            this.rtbTranslation = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            //
            // pbImage
            //
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(20, 50);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(400, 300);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            //
            // lblOcr
            //
            this.lblOcr.AutoSize = true;
            this.lblOcr.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblOcr.Location = new System.Drawing.Point(440, 50);
            this.lblOcr.Name = "lblOcr";
            this.lblOcr.Size = new System.Drawing.Size(110, 31);
            this.lblOcr.TabIndex = 1;
            this.lblOcr.Text = "识别结果";
            //
            // rtbOcr
            //
            this.rtbOcr.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rtbOcr.Location = new System.Drawing.Point(440, 90);
            this.rtbOcr.Name = "rtbOcr";
            this.rtbOcr.ReadOnly = true;
            this.rtbOcr.Size = new System.Drawing.Size(450, 120);
            this.rtbOcr.TabIndex = 2;
            //
            // lblTranslation
            //
            this.lblTranslation.AutoSize = true;
            this.lblTranslation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblTranslation.Location = new System.Drawing.Point(440, 230);
            this.lblTranslation.Name = "lblTranslation";
            this.lblTranslation.Size = new System.Drawing.Size(110, 31);
            this.lblTranslation.TabIndex = 3;
            this.lblTranslation.Text = "翻译结果";
            //
            // rtbTranslation
            //
            this.rtbTranslation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.rtbTranslation.Location = new System.Drawing.Point(440, 270);
            this.rtbTranslation.Name = "rtbTranslation";
            this.rtbTranslation.ReadOnly = true;
            this.rtbTranslation.Size = new System.Drawing.Size(450, 120);
            this.rtbTranslation.TabIndex = 4;
            //
            // btnClose
            //
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnClose.Location = new System.Drawing.Point(400, 410);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // lblTime
            //
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblTime.Location = new System.Drawing.Point(20, 15);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(110, 31);
            this.lblTime.TabIndex = 6;
            this.lblTime.Text = "历史详情";
            //
            // HistoryDetailForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 470);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.rtbTranslation);
            this.Controls.Add(this.lblTranslation);
            this.Controls.Add(this.rtbOcr);
            this.Controls.Add(this.lblOcr);
            this.Controls.Add(this.pbImage);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "历史记录详情";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void ApplyMetroStyle()
        {
            var primary = System.Drawing.Color.FromArgb(74, 144, 226);
            var primaryHover = System.Drawing.Color.FromArgb(59, 125, 216);
            var formBg = System.Drawing.Color.FromArgb(245, 249, 255);
            var textDark = System.Drawing.Color.FromArgb(44, 62, 80);

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
                else if (ctl is System.Windows.Forms.Label lbl)
                {
                    lbl.ForeColor = textDark;
                }
            }
        }

        private void LoadDetail()
        {
            string sourceLabel = "";
            switch (_item.SourceType)
            {
                case "截图": sourceLabel = "来源：截图（OCR识别）"; break;
                case "文本": sourceLabel = "来源：手动输入"; break;
                case "文件": sourceLabel = "来源：文件上传"; break;
                default: sourceLabel = "来源：" + (_item.SourceType ?? "未知"); break;
            }
            lblTime.Text = _item.Time.ToString("yyyy-MM-dd HH:mm:ss") + "    " + sourceLabel;

            if (!string.IsNullOrEmpty(_item.ImagePath))
            {
                try
                {
                    pbImage.Image = Image.FromFile(_item.ImagePath);
                }
                catch { }
            }

            rtbOcr.Text = _item.OcrText ?? "";
            rtbTranslation.Text = _item.TranslationText ?? "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Label lblOcr;
        private System.Windows.Forms.RichTextBox rtbOcr;
        private System.Windows.Forms.Label lblTranslation;
        private System.Windows.Forms.RichTextBox rtbTranslation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTime;
    }
}
