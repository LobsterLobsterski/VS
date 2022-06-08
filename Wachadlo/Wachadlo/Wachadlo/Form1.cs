using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// θ'' = − g⁄R sin θ 
/// θ(t) = θ0cos(sqrt(g/R)*t)
/// </summary>
namespace Wachadlo
{

    public partial class Form1 : Form
    {
        bool click = false;
        double angle = 90, acc = 0, vel = 0;
        double damp=0.995;//arbitralna wartość która sprawia że wahadało przestanie wachać po pewnym czasie
        int length = 100, waga=0;
        Bitmap buffer;

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //musi sprawdzać bo inaczej program będzie próbował
            //rysować nie zainiciowaną bitmape co prowadzi
            //do błędu
            if (timer1.Enabled == true)
            {
                //'nakłada' bitmape na panel
                e.Graphics.DrawImageUnscaled(buffer, 0, 0);
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            ///może poruszać kulką może????
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label4.Text = hScrollBar1.Value.ToString()+ "°";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //punkt z którego wychodzi linka
            int originX = panel2.Width / 2;
            int originY = panel2.Height / 10;
            //punkt w którym znajduję się odważnik
            int bobX = originX + (int)(Math.Sin(angle) * length);
            int bobY = originY + (int)(Math.Cos(angle) * length);

            //liczenie zmiany w kącie odważnika
            acc = (-9.81 / length) * Math.Sin(angle);
            vel += acc;
            vel *= damp;
            angle += vel;

            //bitmapa która będzie 'nakładana' na panel
            buffer = new Bitmap(panel2.Width, panel2.Height);
            Graphics g = Graphics.FromImage(buffer);

            //obiekty na bitmapie
            g.DrawLine(Pens.Black, originX, originY, bobX, bobY);
            g.DrawLine(Pens.Black, panel2.Width / 3, 0, originX, originY);
            g.DrawLine(Pens.Black, 2*panel2.Width / 3, 0, originX, originY);
            g.FillEllipse(Brushes.Black, originX - 5, originY - 5, 10, 10);
            //waga odważnika nie wpływu na jego wachania ale chciałem żeby
            //było to widoczne więc zwiększam jego wielkość
            g.FillEllipse(Brushes.Blue, bobX - (15+waga/2), bobY - (15+waga/2), 30+waga, 30+waga);

            panel2.Invalidate();

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //zmiana tekstu guzika
            if (click)
            {
                button1.Text = "Wachaj";
            }
            else
            {
                button1.Text = "Zatrzymaj";
            }
            click = !click;

            //inicializacja wartości
            angle = hScrollBar1.Value * Math.PI / 180;
            length = (int)numericUpDown1.Value;
            waga = (int)numericUpDown2.Value;
           
            timer1.Enabled = !timer1.Enabled;

        }
    }
}
