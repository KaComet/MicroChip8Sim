using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroChip8Sim;

namespace IDE
{
    public partial class Form2 : Form
    {
        private MC8Sim sim;
        private Bitmap bitmap;
        private List<char> program;
        private int c;
        public Form2(List<char> setProgram)
        {
            program = setProgram;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            sim = new MC8Sim();
            sim.reset();

            for (var i = 0; i < program.Count; i++)
                sim.setMem((char)i, program[i]);


            bitmap = new Bitmap(256, 256);
            c = 0;
            InitTimer();
        }

        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 30; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double dt = 0.00006;
            for (int i = 0; i < 5000; i++)
                sim.tick(0.00006 * 100.0);

            for (var i = 0; i < 0x100; i++)
            {
                for (var j = 0; j < 0x100; j++)
                {
                    if (sim.pixel(i, j))
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, Color.White);
                    }
                }
            }
            //Console.WriteLine(c++);
            this.Refresh();
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
    }
}
