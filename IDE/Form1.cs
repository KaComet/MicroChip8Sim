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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String text = richTextBox1.Text;
            var lexerOutput = Lexer.lex(text.Split('\n'));
            var assmblerOutput = Assembler.Assemble(lexerOutput.Item1);
            Form2 emulatorView = new Form2(assmblerOutput.Item1);
            emulatorView.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}
