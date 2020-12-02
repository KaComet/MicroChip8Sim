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
            if (lexerOutput.Item2.Count > 0)
            {
                string errors = "";
                foreach (var error in lexerOutput.Item2)
                {
                    errors += error + '\n';
                }
                MessageBox.Show(errors, "Error while lexing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var assmblerOutput = Assembler.Assemble(lexerOutput.Item1);

            if (assmblerOutput.Item2.Count > 0)
            {
                string errors = "";
                foreach (var error in assmblerOutput.Item2)
                {
                    errors += error + '\n';
                }
                MessageBox.Show(errors, "Error while assmbling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Form2 emulatorView = new Form2(assmblerOutput.Item1);
            emulatorView.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String text = richTextBox1.Text;
            var lexerOutput = Lexer.lex(text.Split('\n'));
            if (lexerOutput.Item2.Count > 0)
            {
                string errors = "";
                foreach (var error in lexerOutput.Item2)
                {
                    errors += error + '\n';
                }
                MessageBox.Show(errors, "Error while lexing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var assmblerOutput = Assembler.Assemble(lexerOutput.Item1);

            if (assmblerOutput.Item2.Count > 0)
            {
                string errors = "";
                foreach (var error in assmblerOutput.Item2)
                {
                    errors += error + '\n';
                }
                MessageBox.Show(errors, "Error while assmbling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "MIF File|*.mif";
            saveFileDialog1.Title = "Save an MIF File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var program = assmblerOutput.Item1;

                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

                string mifHeader = "WIDTH=16;\n" +
                    "DEPTH=4096;\n\n" +
                    "ADDRESS_RADIX=HEX;\n" +
                    "DATA_RADIX=HEX;\n\n" +
                    "CONTENT BEGIN\n";

                byte[] headerBytes = new UTF8Encoding(true).GetBytes(mifHeader);
                fs.Write(headerBytes, 0, headerBytes.Length);

                for (var i = 0; i < program.Count; i++)
                {
                    string line = $"\t{i.ToString("X4")}\t:\t{((int)program[i]).ToString("X4")};\n";
                    byte[] lineBytes = new UTF8Encoding(true).GetBytes(line);
                    fs.Write(lineBytes, 0, lineBytes.Length);
                }

                string fill = $"\t[{program.Count.ToString("X4")}..0FFF]\t:\t1000;\n";
                byte[] fillBytes = new UTF8Encoding(true).GetBytes(fill);
                fs.Write(fillBytes, 0, fillBytes.Length);

                string footer = "END;\n";
                byte[] footerBytes = new UTF8Encoding(true).GetBytes(footer);
                fs.Write(footerBytes, 0, footerBytes.Length);

                fs.Close();
            }
        }
    }
}
