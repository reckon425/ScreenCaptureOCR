namespace ScreenCaptureOCR
{
    partial class SettingsForm
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
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtOcrApiKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.txtOcrSecretKey = new System.Windows.Forms.TextBox();
            this.lblDefaultLang = new System.Windows.Forms.Label();
            this.cbDefaultLang = new System.Windows.Forms.ComboBox();
            this.lblHotkey = new System.Windows.Forms.Label();
            this.txtHotkey = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblApiKey
            //
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblApiKey.Location = new System.Drawing.Point(30, 30);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(102, 31);
            this.lblApiKey.TabIndex = 0;
            this.lblApiKey.Text = "OCR API Key";
            //
            // txtOcrApiKey
            //
            this.txtOcrApiKey.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtOcrApiKey.Location = new System.Drawing.Point(180, 27);
            this.txtOcrApiKey.Name = "txtOcrApiKey";
            this.txtOcrApiKey.Size = new System.Drawing.Size(350, 35);
            this.txtOcrApiKey.TabIndex = 1;
            //
            // lblSecretKey
            //
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblSecretKey.Location = new System.Drawing.Point(30, 80);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(109, 31);
            this.lblSecretKey.TabIndex = 2;
            this.lblSecretKey.Text = "OCR Secret Key";
            //
            // txtOcrSecretKey
            //
            this.txtOcrSecretKey.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtOcrSecretKey.Location = new System.Drawing.Point(180, 77);
            this.txtOcrSecretKey.Name = "txtOcrSecretKey";
            this.txtOcrSecretKey.Size = new System.Drawing.Size(350, 35);
            this.txtOcrSecretKey.TabIndex = 3;
            //
            // lblDefaultLang
            //
            this.lblDefaultLang.AutoSize = true;
            this.lblDefaultLang.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDefaultLang.Location = new System.Drawing.Point(30, 130);
            this.lblDefaultLang.Name = "lblDefaultLang";
            this.lblDefaultLang.Size = new System.Drawing.Size(110, 31);
            this.lblDefaultLang.TabIndex = 4;
            this.lblDefaultLang.Text = "默认翻译语言";
            //
            // cbDefaultLang
            //
            this.cbDefaultLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultLang.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cbDefaultLang.Items.AddRange(new object[] {
            "中文", "英语", "日语", "韩语", "法语", "德语", "俄语"});
            this.cbDefaultLang.Location = new System.Drawing.Point(180, 127);
            this.cbDefaultLang.Name = "cbDefaultLang";
            this.cbDefaultLang.Size = new System.Drawing.Size(200, 35);
            this.cbDefaultLang.TabIndex = 5;
            //
            // lblHotkey
            //
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblHotkey.Location = new System.Drawing.Point(30, 180);
            this.lblHotkey.Name = "lblHotkey";
            this.lblHotkey.Size = new System.Drawing.Size(110, 31);
            this.lblHotkey.TabIndex = 6;
            this.lblHotkey.Text = "全局截图快捷键";
            //
            // txtHotkey
            //
            this.txtHotkey.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtHotkey.Location = new System.Drawing.Point(180, 177);
            this.txtHotkey.Name = "txtHotkey";
            this.txtHotkey.ReadOnly = true;
            this.txtHotkey.Size = new System.Drawing.Size(200, 35);
            this.txtHotkey.TabIndex = 7;
            this.txtHotkey.Text = "F2";
            this.txtHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHotkey_KeyDown);
            //
            // btnSave
            //
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSave.Location = new System.Drawing.Point(180, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 40);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCancel.Location = new System.Drawing.Point(300, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // SettingsForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 310);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtHotkey);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.cbDefaultLang);
            this.Controls.Add(this.lblDefaultLang);
            this.Controls.Add(this.txtOcrSecretKey);
            this.Controls.Add(this.lblSecretKey);
            this.Controls.Add(this.txtOcrApiKey);
            this.Controls.Add(this.lblApiKey);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox txtOcrApiKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.TextBox txtOcrSecretKey;
        private System.Windows.Forms.Label lblDefaultLang;
        private System.Windows.Forms.ComboBox cbDefaultLang;
        private System.Windows.Forms.Label lblHotkey;
        private System.Windows.Forms.TextBox txtHotkey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
