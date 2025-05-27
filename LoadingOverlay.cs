using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Home_assistant
{
    public class LoadingOverlay : UserControl
    {
        private PictureBox spinner;
        private Label lblLoading;
        private Bitmap blurredBackground;

        public LoadingOverlay()
        {
            InitializeOverlay();
        }

        private void InitializeOverlay()
        {
            // Fill the parent, no simple BackColor
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Transparent;

            // Spinner setup
            spinner = new PictureBox
            {
                Size = new Size(64, 64),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.spinner,
                Anchor = AnchorStyles.None,
                BackColor = Color.Transparent
            };

            // Loading text
            lblLoading = new Label
            {
                AutoSize = true,
                Text = "Loading...",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.None
            };

            this.Controls.Add(spinner);
            this.Controls.Add(lblLoading);
            this.Visible = false;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            spinner.Location = new Point((this.Width - spinner.Width) / 2, (this.Height - spinner.Height) / 2);
            lblLoading.Location = new Point((this.Width - lblLoading.Width) / 2, spinner.Bottom + 10);
        }

        public void ShowOverlay()
        {
            // Capture and blur the parent background
            CaptureAndBlurBackground();
            this.BringToFront();
            this.Visible = true;
        }

        public void HideOverlay()
        {
            this.Visible = false;
            blurredBackground?.Dispose();
            blurredBackground = null;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (blurredBackground != null)
            {
                e.Graphics.DrawImage(blurredBackground, 0, 0);
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        private void CaptureAndBlurBackground()
        {
            var parent = this.Parent as Control;
            if (parent == null) return;

            // Capture
            var bmp = new Bitmap(parent.ClientSize.Width, parent.ClientSize.Height);
            parent.DrawToBitmap(bmp, new Rectangle(Point.Empty, parent.ClientSize));

            // Apply blur
            blurredBackground = GaussianBlur(bmp, 10);
            bmp.Dispose();
        }

        private Bitmap GaussianBlur(Bitmap image, int radius)
        {
            // Simple box blur approximation
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var blurred = new Bitmap(image.Width, image.Height);

            using (var sourceData = new BitmapDataWrapper(image))
            using (var targetData = new BitmapDataWrapper(blurred))
            {
                var w = image.Width;
                var h = image.Height;
                var r = radius;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int rSum = 0, gSum = 0, bSum = 0;
                        int count = 0;

                        for (int yy = y - r; yy <= y + r; yy++)
                        {
                            for (int xx = x - r; xx <= x + r; xx++)
                            {
                                if (xx >= 0 && xx < w && yy >= 0 && yy < h)
                                {
                                    var idx = (yy * w + xx) * 4;
                                    bSum += sourceData.Pixels[idx + 0];
                                    gSum += sourceData.Pixels[idx + 1];
                                    rSum += sourceData.Pixels[idx + 2];
                                    count++;
                                }
                            }
                        }

                        var destIdx = (y * w + x) * 4;
                        targetData.Pixels[destIdx + 0] = (byte)(bSum / count);
                        targetData.Pixels[destIdx + 1] = (byte)(gSum / count);
                        targetData.Pixels[destIdx + 2] = (byte)(rSum / count);
                        targetData.Pixels[destIdx + 3] = sourceData.Pixels[destIdx + 3];
                    }
                }
            }
            return blurred;
        }

        // Helper class for fast pixel access
        private class BitmapDataWrapper : IDisposable
        {
            public byte[] Pixels;
            private BitmapData bmpData;
            private Bitmap bitmap;

            public BitmapDataWrapper(Bitmap bmp)
            {
                bitmap = bmp;
                bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                Pixels = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, Pixels, 0, bytes);
            }

            public void Dispose()
            {
                System.Runtime.InteropServices.Marshal.Copy(Pixels, 0, bmpData.Scan0, Pixels.Length);
                bitmap.UnlockBits(bmpData);
            }
        }
    }
}
