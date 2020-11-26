using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafi_Redaktor
{
    public partial class Form1 : Form
    {
        Point lastPoint = Point.Empty; 
        bool isMouseDown = new Boolean();
        Graphics g;
        Bitmap bmp;
        Pen mainPen;
        List<Point> Lines = new List<Point>();

        int historyCounter;

        GraphicsPath currentPath;
        List<Image> History;
        Color color1;


        public Form1()
        {
            History = new List<Image>();
            InitializeComponent();

        }
        void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }

                if(pictureBox1.Image != null)
                {
                    pictureBox1.Image.Save(sfd.FileName, format);
                }
                else
                {
                    MessageBox.Show("Can not save an empty file");
                }


            }
        }
        void OpenFile()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();
            pictureBox1.ImageLocation = op.FileName;
        }

        void NewFile()
        {
            DialogResult dr = MessageBox.Show("Do you want save your Image?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No)
            {
                if (pictureBox1.Image != null)
                {
                    History.Clear();
                    historyCounter = 0;
                    pictureBox1.Image = null;

                    Invalidate();
                }
                
            }
            else if (dr == DialogResult.Yes)
            {
                SaveFile();
                History.Clear();
                historyCounter = 0;
                Bitmap pic = new Bitmap(750, 500);
                pictureBox1.Image = pic;
                History.Add(new Bitmap(pictureBox1.Image));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            currentPath = new GraphicsPath();
            isMouseDown = true;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown == true)
            {
                if(lastPoint != null)
                {
                    switch (e.Button)
                    {
                        case MouseButtons.Left:
                            if (Date.Value.IsEmpty)
                            {
                                color1 = Color.Black;
                            }
                            else
                            {
                                color1 = Date.Value;
                            }
                            break;
                        case MouseButtons.Right:
                            color1 = Color.White;
                            break;
                    }

                    if(pictureBox1.Image == null)
                    {
                        bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        pictureBox1.Image = bmp;
                    }
                    mainPen = new Pen(color1, trackBar1.Value);
                    

                    if (dotToolStripMenuItem.Checked == true)
                    {
                        mainPen.DashStyle = DashStyle.Dot;
                    }

                    else if(dashToolStripMenuItem.Checked == true)
                    {
                        mainPen.DashStyle = DashStyle.DashDotDot;
                            
                    }

                    else if(solidToolStripMenuItem.Checked == true)
                    {
                        mainPen.DashStyle = DashStyle.Solid;
                    }
                    g = Graphics.FromImage(pictureBox1.Image);
                    currentPath.AddLine(lastPoint, e.Location);
                    g.DrawPath(mainPen, currentPath);
                    lastPoint = e.Location;
                    g.Dispose();
                    pictureBox1.Invalidate();
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
            lastPoint = Point.Empty;
            if (pictureBox1.Image != null)
            {
                History.Add(new Bitmap(pictureBox1.Image));
                History.RemoveRange(historyCounter + 1, History.Count - historyCounter - 1);

                if (historyCounter + 1 < 10) historyCounter++;
                if (History.Count - 1 == 10) History.RemoveAt(0);

                isMouseDown = false;

                try
                {
                    if(currentPath != null)
                    {
                        currentPath.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Try Again");
                    }
                }
                catch
                {
                    
                };
            }
            else
            {
                MessageBox.Show("Your image is not supported by this redactor");
                OpenFile();
            }
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 10;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                NewFile();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }


        private void dotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            solidToolStripMenuItem.Checked = false;
            dashToolStripMenuItem.Checked = false;
            dotToolStripMenuItem.Checked = true;

        }

        private void dashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            solidToolStripMenuItem.Checked = false;
            dotToolStripMenuItem.Checked = false;
            dashToolStripMenuItem.Checked = true;

        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dotToolStripMenuItem.Checked = false;
            dashToolStripMenuItem.Checked = false;
            solidToolStripMenuItem.Checked = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                if (History.Count != 0 && historyCounter != 0)
                {
                    pictureBox1.Image = new Bitmap(History[--historyCounter]);
                }
                else
                {
                    MessageBox.Show("История пуста");
                }
            }
            else
            {
                MessageBox.Show("История пуста");
            }
            
            
        }

        private void rendoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (historyCounter < History.Count - 1)
            {
                pictureBox1.Image = new Bitmap(History[++historyCounter]);
            }
            else
            {
                MessageBox.Show("История пуста");
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
            

        }

        private void newFile_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveFile1_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void exitFile_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                NewFile();
                this.Close();
            }
            else
            {
                this.Close();
            }
            
        }
    }
}
