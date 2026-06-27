using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenCaptureOCR
{
    public class FormScreenshot : Form
    {
        private enum CaptureState { Idle, Selecting, Selected }

        private Point _startPoint;
        private Rectangle _selectedRegion;
        private CaptureState _state = CaptureState.Idle;
        private readonly Bitmap _screenShot;
        private Bitmap _capturedRegion;
        private readonly float _dpiScale;

        // 拖拽选区移动
        private bool _isDraggingSelection;
        private Point _dragStartPoint;
        private Rectangle _dragStartRegion;

        // 浮动工具栏
        private Panel _toolbar;
        private Timer _safetyTimer;

        public Bitmap CapturedRegion => _capturedRegion;

        /// <summary>
        /// 接收外部传入的截图，不再自行调用 CopyFromScreen
        /// </summary>
        public FormScreenshot(Bitmap screenShot, float dpiScale)
        {
            _screenShot = screenShot ?? throw new ArgumentNullException(nameof(screenShot));
            _dpiScale = dpiScale;

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Cross;
            this.BackColor = Color.Gray;
            this.ShowInTaskbar = false;

            BuildToolbar();
            BuildCloseButton();
            StartSafetyTimer();
        }

        private void BuildToolbar()
        {
            var btnConfirm = new Button
            {
                Text = "确认",
                Font = new Font("微软雅黑", 10F, FontStyle.Bold),
                Size = new Size(90, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += (sender, args) => ConfirmSelection();

            var btnCancel = new Button
            {
                Text = "取消",
                Font = new Font("微软雅黑", 10F),
                Size = new Size(90, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(80, 80, 80),
                ForeColor = Color.White,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (sender, args) => CancelSelection();

            _toolbar = new Panel
            {
                Size = new Size(192, 52),
                BackColor = Color.FromArgb(45, 45, 45),
                Visible = false,
            };
            btnConfirm.Location = new Point(5, 6);
            btnCancel.Location = new Point(97, 6);
            _toolbar.Controls.Add(btnConfirm);
            _toolbar.Controls.Add(btnCancel);
            this.Controls.Add(_toolbar);
        }

        /// <summary>
        /// 右上角固定关闭按钮，任何时候都能退出
        /// </summary>
        private void BuildCloseButton()
        {
            var btnExit = new Button
            {
                Text = "关闭",
                Font = new Font("微软雅黑", 10F, FontStyle.Bold),
                Size = new Size(70, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(200, 60, 60),
                ForeColor = Color.White,
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (sender, args) => CancelSelection();
            // OnLoad 中设置位置，此时 ClientSize 才正确
            this.Load += (sender, args) =>
            {
                btnExit.Left = this.ClientSize.Width - btnExit.Width - 8;
                btnExit.Top = 8;
            };
            this.Controls.Add(btnExit);
        }

        /// <summary>
        /// 安全计时器：30 秒无操作自动退出，防止意外卡死
        /// </summary>
        private void StartSafetyTimer()
        {
            _safetyTimer = new Timer { Interval = 30000 };
            _safetyTimer.Tick += (sender, args) =>
            {
                _safetyTimer.Stop();
                CancelSelection();
            };
            _safetyTimer.Start();
        }

        private void ResetSafetyTimer()
        {
            _safetyTimer.Stop();
            _safetyTimer.Start();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            ResetSafetyTimer();

            if (_state == CaptureState.Selected && _selectedRegion.Contains(e.Location))
            {
                // 在选区内按下 → 进入拖拽移动模式
                _isDraggingSelection = true;
                _dragStartPoint = e.Location;
                _dragStartRegion = _selectedRegion;
                return;
            }

            // 在选区外按下（或第一次）→ 开始新选择
            _state = CaptureState.Selecting;
            _startPoint = e.Location;
            _selectedRegion = Rectangle.Empty;
            _toolbar.Visible = false;
            _isDraggingSelection = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDraggingSelection)
            {
                int dx = e.X - _dragStartPoint.X;
                int dy = e.Y - _dragStartPoint.Y;
                _selectedRegion = new Rectangle(
                    _dragStartRegion.X + dx,
                    _dragStartRegion.Y + dy,
                    _dragStartRegion.Width,
                    _dragStartRegion.Height);
                UpdateToolbarPosition();
                this.Invalidate();
                return;
            }

            if (_state == CaptureState.Selecting)
            {
                _selectedRegion = new Rectangle(
                    Math.Min(_startPoint.X, e.X),
                    Math.Min(_startPoint.Y, e.Y),
                    Math.Abs(_startPoint.X - e.X),
                    Math.Abs(_startPoint.Y - e.Y));
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isDraggingSelection)
            {
                _isDraggingSelection = false;
                return;
            }

            if (e.Button == MouseButtons.Left && _state == CaptureState.Selecting)
            {
                if (_selectedRegion.Width > 10 && _selectedRegion.Height > 10)
                {
                    _state = CaptureState.Selected;
                    this.Cursor = Cursors.SizeAll;
                    UpdateToolbarPosition();
                    _toolbar.Visible = true;
                    this.Invalidate();
                }
                else
                {
                    _state = CaptureState.Idle;
                    _selectedRegion = Rectangle.Empty;
                    this.Invalidate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                CancelSelection();
            }
        }

        private void UpdateToolbarPosition()
        {
            int x = _selectedRegion.Right - _toolbar.Width / 2;
            int y = _selectedRegion.Bottom + 8;
            x = Math.Max(10, Math.Min(x, this.ClientSize.Width - _toolbar.Width - 10));
            y = Math.Max(10, Math.Min(y, this.ClientSize.Height - _toolbar.Height - 10));
            _toolbar.Location = new Point(x, y);
        }

        private void ConfirmSelection()
        {
            _safetyTimer.Stop();

            int x = (int)(_selectedRegion.X * _dpiScale);
            int y = (int)(_selectedRegion.Y * _dpiScale);
            int w = (int)(_selectedRegion.Width * _dpiScale);
            int h = (int)(_selectedRegion.Height * _dpiScale);

            int bmpW = _screenShot.Width, bmpH = _screenShot.Height;
            x = Math.Max(0, Math.Min(x, bmpW - 1));
            y = Math.Max(0, Math.Min(y, bmpH - 1));
            w = Math.Min(w, bmpW - x);
            h = Math.Min(h, bmpH - y);

            if (w > 10 && h > 10)
            {
                try
                {
                    _capturedRegion = _screenShot.Clone(new Rectangle(x, y, w, h), _screenShot.PixelFormat);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("选区处理失败：" + ex.Message, "错误");
                    return;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void CancelSelection()
        {
            _safetyTimer.Stop();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ResetSafetyTimer();
                if (_state == CaptureState.Selected)
                {
                    _state = CaptureState.Idle;
                    _selectedRegion = Rectangle.Empty;
                    _toolbar.Visible = false;
                    this.Cursor = Cursors.Cross;
                    this.Invalidate();
                    return;
                }
                CancelSelection();
            }
            else if (e.KeyCode == Keys.Enter && _state == CaptureState.Selected)
            {
                ConfirmSelection();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // 背景：截图
            if (_screenShot != null)
                e.Graphics.DrawImage(_screenShot, this.ClientRectangle);

            // 半透明遮罩层
            using (var dimBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                e.Graphics.FillRectangle(dimBrush, this.ClientRectangle);

            if (_state == CaptureState.Selecting || _state == CaptureState.Selected)
            {
                if (_selectedRegion.Width > 0 && _selectedRegion.Height > 0)
                {
                    // 挖空选区，露出原图
                    using (var reveal = new SolidBrush(Color.FromArgb(1, 0, 0, 0)))
                        e.Graphics.FillRectangle(reveal, _selectedRegion);

                    // 边框
                    using (var pen = new Pen(
                        _state == CaptureState.Selected ? Color.DeepSkyBlue : Color.Red, 3))
                        e.Graphics.DrawRectangle(pen, _selectedRegion);

                    // 尺寸提示
                    string info = $"{_selectedRegion.Width} × {_selectedRegion.Height}";
                    using (var font = new Font("微软雅黑", 10F, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.White))
                        e.Graphics.DrawString(info, font, brush,
                            _selectedRegion.Right + 6, _selectedRegion.Top - 2);

                    if (_state == CaptureState.Selected)
                    {
                        string hint = "拖动选区移动位置  |  Enter确认  |  Esc取消";
                        using (var font = new Font("微软雅黑", 9F))
                        using (var bg = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                        using (var fg = new SolidBrush(Color.White))
                        {
                            var sz = e.Graphics.MeasureString(hint, font);
                            float hx = Math.Max(0, _selectedRegion.Left);
                            float hy = Math.Max(0, _selectedRegion.Top - sz.Height - 6);
                            e.Graphics.FillRectangle(bg, hx, hy, sz.Width + 12, sz.Height + 6);
                            e.Graphics.DrawString(hint, font, fg, hx + 6, hy + 2);
                        }
                    }
                }
            }
            else
            {
                string hint = "按住鼠标左键拖动选择区域  |  右键/Esc取消";
                using (var font = new Font("微软雅黑", 12F))
                using (var brush = new SolidBrush(Color.White))
                {
                    var sz = e.Graphics.MeasureString(hint, font);
                    e.Graphics.DrawString(hint, font, brush,
                        (this.ClientSize.Width - sz.Width) / 2,
                        this.ClientSize.Height - 60);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _safetyTimer?.Dispose();
            _screenShot?.Dispose();
        }
    }
}
