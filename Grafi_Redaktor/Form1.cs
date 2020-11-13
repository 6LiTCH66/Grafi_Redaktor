﻿using System;
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
        Graphics g, newGraph;
        Bitmap bmp;
        Pen mainPen;
        private bool undoCliked = false;
        List<Point> Lines = new List<Point>();

        public Form1()
        {
            InitializeComponent();
            

        }
        void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
                pictureBox1.Image.Save(sfd.FileName, format);
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;

            isMouseDown = true;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown == true)
            {
                if(lastPoint != null)
                {
                    if(pictureBox1.Image == null)
                    {
                        bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

                        pictureBox1.Image = bmp;
                    }
                    using (g = Graphics.FromImage(pictureBox1.Image))
                    {

                        if (dotToolStripMenuItem.Checked == true)
                        {
                            mainPen = new Pen(Color.Black, trackBar1.Value);
                            mainPen.DashStyle = DashStyle.Dot;

                        }
                        else if(dashToolStripMenuItem.Checked == true)
                        {
                            mainPen = new Pen(Color.Black, trackBar1.Value);
                            mainPen.DashStyle = DashStyle.DashDotDot;
                            
                        }
                        else if(solidToolStripMenuItem.Checked == true)
                        {
                            mainPen = new Pen(Color.Black, trackBar1.Value);
                            mainPen.DashStyle = DashStyle.Solid;
                        }

                        else
                        {
                            mainPen = new Pen(Color.Black, trackBar1.Value);
                        }
                        
                        g.DrawLine(mainPen, lastPoint, e.Location);

                        

                    }

                    
                    pictureBox1.Invalidate();

                    lastPoint = e.Location;

                    Lines.Add(lastPoint);

                    if (undoCliked == true)
                    {
                        Lines.RemoveAt(Lines.Count - 1);
                    }






                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;

            lastPoint = Point.Empty;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Do you want save your Image?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(dr == DialogResult.No)
            {
                if (pictureBox1.Image != null)
                {

                    pictureBox1.Image = null;

                    Invalidate();
                }
            }
            else if (dr == DialogResult.Yes)
            {
                SaveFile();
            }
           
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
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();
            pictureBox1.ImageLocation = op.FileName;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
            undoCliked = true;
            
        }
    }
}
