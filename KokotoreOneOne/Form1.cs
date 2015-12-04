using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KokotoreOneOne
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Bitmap screenBmp;
        Pen pen;
        Font font;
        Rect drawRect;

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
            Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bmp.Size);
            g.Dispose();
            screenBmp = new Bitmap(bmp.Width, bmp.Height);
            pen = new Pen(Color.Red);
            font = new Font("メイリオ", 10);
            drawRect = new Rect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;

            //表示
            pictureBox1.Image = bmp;
        }

        private int startX = -1;
        private int startY = -1;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            startX = e.X;
            startY = e.Y;
        }

        /// <summary>
        /// 四隅の座標から高さと幅を求める
        /// 
        /// </summary>
        public class Rect
        {
            public int top { get; set;}
            public int left { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public void set(int x1, int y1, int x2,int y2)
            {
                if (x1 < x2)
                {
                    left = x1;
                    width = x2 - x1;
                }
                else
                {
                    left = x2;
                    width = x1 - x2;
                }
                if(y1 < y2)
                {
                    top = y1;
                    height = y2 - y1;
                }
                else
                {
                    top = y2;
                    height = y1 - y2;
                }
            }
        }

        /// <summary>
        /// マウス移動時は矩形を描画
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = Graphics.FromImage(screenBmp);
            g.DrawImage(bmp, 0, 0);
            this.drawRect.set(startX, startY, e.X, e.Y);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            if (startX > 0)
            {
                g.DrawRectangle(pen, this.drawRect.left, this.drawRect.top, this.drawRect.width, this.drawRect.height);

                g.DrawString((e.X - startX) + "," + (e.Y - startY), font,Brushes.Gray, e.X, e.Y);

            }
            else
            {
                g.DrawString((e.X) + "," + (e.Y ), font, Brushes.Gray, e.X, e.Y);

            }
            pictureBox1.Image = screenBmp;
        }

        /// <summary>
        /// 保存して終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.drawRect.set(startX, startY, e.X, e.Y);

            Rectangle rect = new Rectangle(this.drawRect.left, this.drawRect.top, this.drawRect.width, this.drawRect.height);
            Bitmap trimBmp = bmp.Clone(rect, bmp.PixelFormat);
            DateTime now = DateTime.Now;
            string fileName = now.ToString("スクリーンショット yyyy-MM-dd HH.mm.ss");
            trimBmp.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\"+fileName+".png", System.Drawing.Imaging.ImageFormat.Png);
            try
            {
                trimBmp.Dispose();
            }
            catch { };
            try
            {
                bmp.Dispose();
            }
            catch { }
            try
            {
                screenBmp.Dispose();
            }
            catch { }
            Close();
        }
    }
}
