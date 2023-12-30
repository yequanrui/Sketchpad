using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace Sketchpad
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
        }
        private int penwidth = 1;
        private Bitmap bitmap = null;
        private bool dragMode = false;
        private int drawIndex = 1;
        private int curX, curY, x, y, diffX, diffY, xx, yy;
        private Graphics curGraphics;
        private Pen curPen;
        public Cursor curpen { get; set; }
        private SolidBrush curBrush;
        private bool ereaseMode = false;
        public bool quxianMode = false;
        private bool duobianMode = false;
        private Point start;
        private Point end, aaa;
        private string filename;
        public Image newimage;
        private Stack<Bitmap> history, now;
        private Color pencolor = Color.Black;
        public Point[] point = new Point[10];
        public Point[] point1 = new Point[10];
        public int m = 1;//画曲线时用到
        public int n = 1;//输入文字的位置
        public bool writeMode = false;
        public Point write;//写入点的位置
        public bool endmode = true;
        public int a = 0, e, f;
        public int b = 0;
        public int c = 0;
        public int d = 0;
        private bool button = false;//判断橡皮是否开始工作
        private void Form1_Load(object sender, EventArgs e)
        {
            curpen = new Cursor("./pen.cur");
            Cursor = curpen;
            curPen = new Pen(pencolor, penwidth);
            文本框.Visible = false;
            bitmap = new Bitmap(780, 640);
            curGraphics = Graphics.FromImage(bitmap);//budong
            curGraphics.Clear(this.BackColor);
            橡皮擦.Visible = false;
            history = new Stack<Bitmap>();
            now = new Stack<Bitmap>();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Bitmap mjl = new Bitmap(bitmap);
            history.Push(mjl);
            if (drawIndex == 11)
            {
                if (n > 2)
                {
                    n = 1;
                }
                point1[n] = new Point(e.X, e.Y);
                n++;
            }
            文本框.Visible = false;
            if (endmode == true)
            {
                end = new Point(e.X, e.Y);
                endmode = false;
            }
            if (m >= 6)
            {
                m = 1;
            }
            if (drawIndex == 10)
            {
                point[m] = new Point(e.X, e.Y);
                m++;
            }
            curX = e.X;
            curY = e.Y;
            dragMode = true;
            start = new Point(curX, curY);
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics gra = this.CreateGraphics();
            x = e.X;
            y = e.Y;
            a = x;
            aaa = new Point(x, y);
            b = y;
            diffX = x - curX;
            diffY = y - curY;
            if (ereaseMode == true)
            {
                橡皮擦.Visible = true;
                Color clr = this.BackColor;
                this.橡皮擦.Location = new Point(x, y);
                if (e.Button == MouseButtons.Left)
                {
                    button = true;
                }
                else
                {
                    button = false;
                }
            }
            if (dragMode)
            {
                this.Refresh();
            }
            c = x;
            d = y;
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawIndex == 10 && m >= 5)
            {
                this.BackgroundImage = bitmap;
            }
            Graphics g = this.CreateGraphics();
            xx = e.X;
            yy = e.Y;
            if (drawIndex == 10)
            {
                point[m] = new Point(xx, yy);
                m++;
            }
            if ((xx < (end.X + 5) && xx > (end.X - 5)) && (yy < (end.Y + 5) && yy > (end.Y - 5)))
            {
                endmode = true;
                start = new Point(0, 0);
                xx = yy = 0;
            }
            curBrush = new SolidBrush(picColor.BackColor);
            switch (drawIndex)
            {
                case 1:
                    {
                        curGraphics.DrawLine(curPen, curX, curY, x, y);
                        break;
                    }
                case 2:
                    {
                        curGraphics.DrawEllipse(curPen, curX, curY, diffX, diffY);
                        break;
                    }
                case 3:
                    {
                        curGraphics.DrawRectangle(curPen, curX, curY, diffX, diffY);
                        break;
                    }
                case 4:
                    {
                        curGraphics.FillRectangle(curBrush, curX, curY, diffX, diffY);
                        break;
                    }
                case 5:
                    {
                        curGraphics.FillEllipse(curBrush, curX, curY, diffX, diffY);
                        break;
                    }
                case 7:
                    {
                        if ((xx < (end.X + 5) && xx > (end.X - 5)) && (yy < (end.Y + 5) && yy > (end.Y - 5)))
                        {
                            endmode = true;
                            start = new Point(0, 0);
                            xx = yy = 0;
                        }
                        if (start.X == 0 && start.Y == 0)
                        {
                            start = new Point(curX, curY);
                        }
                        curGraphics.DrawLine(curPen, start, aaa);
                        start = new Point(xx, yy);
                        break;
                    }
                case 10:
                    {
                        if (m == 3)
                        {
                            g.DrawLine(curPen, point[1], aaa);
                            break;
                        }
                        else if (m >= 6)
                        {
                            float q = 0.5f;
                            Point[] p = new Point[4];
                            p[0] = point[1];
                            p[1] = point[4];
                            p[2] = point[6];
                            p[3] = point[2];
                            curGraphics.DrawCurve(curPen, p, q);
                        }
                        break;
                    }
            }
            this.BackgroundImage = bitmap;//刷新画面
            dragMode = false;
            xx = e.X;
            yy = e.Y;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            curPen.Color = picColor.BackColor;
            curBrush = new SolidBrush(picColor.BackColor);
            if (dragMode)
            {
                switch (drawIndex)
                {
                    case 1:
                        {
                            g.DrawLine(curPen, curX, curY, x, y);
                            break;
                        }
                    case 2:
                        {
                            g.DrawEllipse(curPen, curX, curY, diffX, diffY);
                            break;
                        }
                    case 3:
                        {
                            g.DrawRectangle(curPen, curX, curY, diffX, diffY);
                            break;
                        }
                    case 4:
                        {
                            g.FillRectangle(curBrush, curX, curY, diffX, diffY);
                            break;
                        }
                    case 5:
                        {
                            g.FillEllipse(curBrush, curX, curY, diffX, diffY);
                            break;
                        }
                    case 6:
                        {
                            Color clr = picColor.BackColor;
                            SolidBrush brush = new SolidBrush(clr);
                            Pen pen1 = new Pen(brush, 5);
                            curGraphics.DrawLine(pen1, c, d, a, b);
                            this.BackgroundImage = bitmap;
                            break;
                        }
                    case 7:
                        {
                            if ((xx < (end.X + 5) && xx > (end.X - 5)) && (yy < (end.Y + 5) && yy > (end.Y - 5)))
                            {
                                endmode = true;
                                start = new Point(0, 0);
                                xx = yy = 0;
                            }
                            if (start.X == 0 && start.Y == 0)
                            {
                                start = new Point(curX, curY);
                            }
                            g.DrawLine(curPen, start, aaa);
                            start = new Point(xx, yy);
                            break;
                        }
                    case 8:
                        {
                            curGraphics.DrawLine(curPen, c, d, a, b);
                            this.BackgroundImage = bitmap;
                            break;
                        }
                    case 9:
                        {
                            Random rdm = new Random();
                            for (int i = 0; i < 50; i++)
                            {
                                int m = rdm.Next(a - 5, a + 5);
                                int n = rdm.Next(b - 5, b + 5);
                                curGraphics.DrawLine(curPen, m, n, m + 1, n + 1);
                                this.BackgroundImage = bitmap;
                            }
                            break;
                        }
                    case 10:
                        {
                            if (m == 2)
                            {
                                g.DrawLine(curPen, point[1], aaa);
                            }
                            else if (m == 3 || m == 4)
                            {
                                float q = 0.5f;
                                Point[] p = new Point[3];
                                p[0] = point[1];
                                p[1] = aaa;
                                p[2] = point[2];
                                g.DrawCurve(curPen, p, q);
                            }
                            else if (m >= 5)
                            {
                                float q = 0.5f;
                                Point[] p = new Point[4];
                                p[0] = point[1];
                                p[1] = point[4];
                                p[2] = aaa;
                                p[3] = point[2];
                                g.DrawCurve(curPen, p, q);
                            }
                            break;
                        }
                    case 11:
                        {
                            curPen.DashStyle = DashStyle.DashDot;
                            g.DrawRectangle(curPen, curX, curY, diffX, diffY);
                            文本框.Visible = true;
                            文本框.Location = new Point(curX, curY);
                            文本框.Width = diffX;
                            文本框.Height = diffY;
                            curGraphics.DrawString(文本框.Text.Trim(), new Font("Verdana", 16), curBrush, point1[1]);
                            文本框.Text = "";
                            break;
                        }
                }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            curBrush.Dispose();
            curGraphics.Dispose();
        }
        private void btnErease_Click(object sender, EventArgs e)
        {
            if (ereaseMode == false)
            {
                ereaseMode = true;
            }
            else
            {
                ereaseMode = false;
                橡皮擦.Visible = false;
                //System.Windows.Forms.Cursor.Show();
            }
        }
        #region 颜色选择栏
        private void pictureBox16_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox16.BackColor;
        }
        private void pictureBox17_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox17.BackColor;
        }
        private void pictureBox12_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox12.BackColor;
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox4.BackColor;
        }
        private void pictureBox14_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox14.BackColor;
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox10.BackColor;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox2.BackColor;
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox7.BackColor;
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox8.BackColor;
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox6.BackColor;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox1.BackColor;
        }
        private void pictureBox15_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox15.BackColor;
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox11.BackColor;
        }
        private void pictureBox20_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox20.BackColor;
        }
        private void pictureBox27_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox27.BackColor;
        }
        private void pictureBox26_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox26.BackColor;
        }
        private void pictureBox22_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox22.BackColor;
        }
        private void pictureBox19_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox19.BackColor;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox3.BackColor;
        }
        private void pictureBox24_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox24.BackColor;
        }
        private void pictureBox25_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox25.BackColor;
        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox13.BackColor;
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox5.BackColor;
        }
        private void pictureBox23_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox23.BackColor;
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox9.BackColor;
        }
        private void pictureBox21_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox21.BackColor;
        }
        private void pictureBox28_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox28.BackColor;
        }
        private void pictureBox18_Click(object sender, EventArgs e)
        {
            picColor.BackColor = pictureBox18.BackColor;
        }
        #endregion

        #region 菜单栏
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aaa = new Point(0, 0);
            start = new Point(0, 0);
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
            Graphics g = this.CreateGraphics();
            g.Clear(this.BackColor);
            curGraphics.Clear(this.BackColor);
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filename = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图像文件(*.jpg,*.png,*.gif,*.bmp)|*.jpg;*.png;*.gif;*.bmp|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filename = dlg.FileName;
            }
            try
            {
                newimage = Image.FromFile(filename);
                curGraphics.DrawImage(newimage, new Point(0, 0));
                newimage.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("选择文件错误，请查看。");
            }
        }
        private void 颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                picColor.BackColor = dlg.Color;
            }
        }
        private void 撤销ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Bitmap mjl = new Bitmap(bitmap);
            now.Push(mjl);
            bitmap = history.Pop();
            //pictureBox1.Image = bitmap;
            curGraphics = Graphics.FromImage(bitmap);
            this.BackgroundImage = bitmap;//刷新画面 
            //pictureBox1.Refresh();
        }
        private void 重做ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Bitmap mjl = new Bitmap(bitmap);
            history.Push(mjl);
            bitmap = now.Pop();
            curGraphics = Graphics.FromImage(bitmap);
            this.BackgroundImage = bitmap;//刷新画面 
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ereaseMode == true || duobianMode == true)
            {
                duobianMode = false;
                橡皮擦.Visible = false;
            }
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Image files (*.bmp)|*.bmp|All files(*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(save.FileName);
                MessageBox.Show("保存成功！");
            }
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab = new About();
            ab.Show();
        }
        #endregion

        #region 工具栏
        private void 喷枪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 9;
        }
        private void 刷子ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 6;
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        private void 钢笔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 8;
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        private void 画笔大小_SelectedIndexChanged(object sender, EventArgs e)
        {
            curPen.Width = (int)Convert.ToInt16(画笔大小.SelectedItem);
        }
        private void 橡皮ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ereaseMode == false)
            {
                ereaseMode = true;
                this.橡皮擦.Visible = true;
            }
            else
            {
                ereaseMode = false;
                this.橡皮擦.Visible = false;
            }
        }
        private void 橡皮擦_Move(object sender, EventArgs e)
        {
            if (ereaseMode == true)
            {
                橡皮擦.Visible = true;
                Color clr = this.BackColor;
                this.橡皮擦.Location = new Point(a, b);
                SolidBrush brush = new SolidBrush(clr);
                if (button == true)
                {
                    Pen pen1 = new Pen(brush, 20);
                    curGraphics.DrawLine(pen1, c, d, a, b);
                }
            }
        }
        private void 文字ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 11;
        }
        private void 直线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 1;
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        private void 曲线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
            drawIndex = 10;
        }
        private void 矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 3;
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        private void 椭圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 2;

            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        private void 多边形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xx = 0;
            yy = 0;
            drawIndex = 7;
        }
        private void 填充的矩形ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            drawIndex = 4;
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        private void 填充的椭圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawIndex = 5;
            if (ereaseMode == true || duobianMode == true)
            {
                ereaseMode = false;
                duobianMode = false;
                橡皮擦.Visible = false;
            }
        }
        #endregion
    }
}