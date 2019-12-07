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



         int WIDTH = 958, HEIGHT = 1008, HAND = 958; 
 
 
        int u;  //in degree 
        int cx, cy;     //center of the circle 
        int x, y;       //HAND coordinate 

 
        int tx, ty, lim = 20; 

 
        //Bitmap bmp; 
        Pen p; 
         Graphics g; 

 
         public Form1()
         {
            InitializeComponent();
            com = new SerialPort("COM3", 9600);
            com.Open();
            p = new Pen(Color.Green, 1f);
        }


        private void Form1_Load(object sender, EventArgs e)
        { 
            //background color 
            this.BackColor = Color.Black; 

 
            //center 
             cx = WIDTH ; 
             cy = HEIGHT; 
 
 
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
            string[] dati=s.Split(',');
            string gradi, distanza;
            try
            {
                gradi = dati[0];//gradi
                distanza = dati[1];//dis
                v[int.Parse(gradi) - 15] = int.Parse(distanza);
            }
            catch(Exception ex)
            {
                return;
            }
           

            u = int.Parse(gradi)-90;


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
                 tx = cx + (int) (HAND* Math.Sin(Math.PI* tu / 180)); 
                 ty = cy - (int) (HAND* Math.Cos(Math.PI* tu / 180)); 
             } 
             else 
             { 
                 tx = cx - (int) (HAND* -Math.Sin(Math.PI* tu / 180)); 
                 ty = cy - (int) (HAND* Math.Cos(Math.PI* tu / 180)); 
             }

            //graphics 
            g = this.CreateGraphics();
            g.Clear(Color.Black);
            
            //draw circle 
            g.DrawEllipse(p, 0, 50, WIDTH * 2, HEIGHT * 2 - 100);//bigger circle 
            g.DrawEllipse(p, 250, 300, WIDTH * 2 - 500, HEIGHT * 2 - 600);//smaller circle 
            g.DrawEllipse(p, 800, 850, WIDTH * 2 - 1600, HEIGHT * 2 - 1700);  
            g.DrawEllipse(p, 1400, 1450, WIDTH * 2 - 2800, HEIGHT * 2 - 2900);     

            //draw perpendicular line 
            g.DrawLine(p, new Point(WIDTH, 50), new Point(WIDTH, HEIGHT)); // UP-DOWN 
             g.DrawLine(p, new Point(0, HEIGHT), new Point(WIDTH * 2, HEIGHT)); //LEFT-RIGHT 
 
 
             //draw HAND 
             g.DrawLine(p, new Point(cx, cy), new Point(x, y)); 
 
           
         } 
     } 
 } 
