using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SimpleRadar
{
    public partial class Form1 : Form
    {
        Timer t = new Timer();

        SerialPort com = null;
        int[] v = new int[150];

        int HAND;

        int u;  //in degree 
        int cx, cy;     //center of the circle 
        int x, y;       //HAND coordinate
        int xp, yp;

        int tx, ty, lim = 20;

        //Bitmap bmp; 
        Pen p;
        Graphics g;
        Pen pRed = new Pen(Color.Red, 1f);

        public Form1()
        {
            InitializeComponent();
            Width = 1000;
            Height = 500;
            com = new SerialPort("COM3", 9600);
            com.Open();
            p = new Pen(Color.Green, 1f);
            HAND = Width/2;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            p = new Pen(Color.Green, 1f);
            for (int h = 0; h <= 350; h += 70)
            {
                e.Graphics.DrawEllipse(p, 0 + h, 0 + h, Height * 2 - h * 2, Width - h * 2);
            }

            e.Graphics.DrawLine(p, new Point(Width / 2, 0), new Point(Width / 2, Height)); // UP-DOWN 
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            Refresh();
            Width = 1000;
            Height = 500;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //background color 
            this.BackColor = Color.Black;

            //center 
            cx = Width/2;
            cy = Height;

            //initial degree of HAND 
            u = 0;

            //timer 
            t.Interval = 30; //in millisecond 
            t.Tick += new EventHandler(this.t_Tick);
            t.Start(); 
        }


        private void t_Tick(object sender, EventArgs e)
        {
            float pixDistance = 0.0f;
            string s = com.ReadLine();
            string[] dati = s.Split(',');
            string gradi, distanza;
            try
            {
                gradi = dati[0];//gradi
                distanza = dati[1];//dis
                v[int.Parse(gradi) - 15] = int.Parse(distanza);
            }
            catch (Exception ex)
            {
                return;
            }

            u = int.Parse(gradi) - 90;

            //calcolo dei punti
            pixDistance = float.Parse(distanza) * 22.5f;

            //calculate x, y coordinate of HAND 
            int tu = (u - lim) % 360;

            x = cx + (int)(HAND * Math.Sin(Math.PI * u / 180));
            y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));

            if (tu >= 0 && tu <= 180)
            {
                //right half 
                //tu in degree is converted into radian.
                tx = cx + (int)(HAND * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int)(HAND * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }

            xp = pixDistance * Math.Cos(u);
            yp = pixDistance * Math.Sin(u);

            g.DrawLine(pRed, xp, yp, xp + 1, yp + 1);

            //graphics 
            g = this.CreateGraphics();
            Refresh();

            //draw HAND 
            g.DrawLine(p, new Point(cx, cy), new Point(x, y));

            
        }
    }
}
