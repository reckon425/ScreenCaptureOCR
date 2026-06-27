using System;
using System.Windows.Forms;

namespace ScreenCaptureOCR
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            ApplyMetroStyle();
            LoadSettings();
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
                else if (ctl is System.Windows.Forms.TextBox tb)
                {
                    tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    tb.BackColor = System.Drawing.Color.White;
                    tb.ForeColor = textDark;
                }
                else if (ctl is System.Windows.Forms.ComboBox cb)
                {
                    cb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    cb.BackColor = System.Drawing.Color.White;
                    cb.ForeColor = textDark;
                }
                else if (ctl is System.Windows.Forms.Label lbl)
                {
                    lbl.ForeColor = textDark;
                }
            }
        }

        private void LoadSettings()
        {
            txtOcrApiKey.Text = Properties.Settings.Default.OcrApiKey;
            txtOcrSecretKey.Text = Properties.Settings.Default.OcrSecretKey;
            cbDefaultLang.SelectedIndex = Properties.Settings.Default.TargetLangIndex;
            txtHotkey.Text = ((Keys)Properties.Settings.Default.Hotkey).ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.OcrApiKey = txtOcrApiKey.Text.Trim();
            Properties.Settings.Default.OcrSecretKey = txtOcrSecretKey.Text.Trim();
            Properties.Settings.Default.TargetLangIndex = cbDefaultLang.SelectedIndex;

            // 解析快捷键
            try
            {
                var key = (Keys)Enum.Parse(typeof(Keys), txtHotkey.Text.Trim(), true);
                Properties.Settings.Default.Hotkey = (int)key;
            }
            catch
            {
                MessageBox.Show("快捷键格式无效，请重新输入", "提示");
                return;
            }

            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            // 只允许功能键 + 修饰键组合
            Keys key = e.KeyCode;
            if (key >= Keys.F1 && key <= Keys.F12)
            {
                Keys final = e.Modifiers | key;
                if (final == key) final = Keys.F1; // 至少需要修饰键
                txtHotkey.Text = final.ToString();
                e.SuppressKeyPress = true;
            }
            else if (key == Keys.Back || key == Keys.Delete)
            {
                txtHotkey.Clear();
                e.SuppressKeyPress = true;
            }
        }
    }
}
