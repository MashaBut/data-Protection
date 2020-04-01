using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PI__3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            cd.SetKey(Key);
            cd.SetReplaceTable(bRepTab);
        }
        Coder cd = new Coder();
        byte[] prvst;
        byte[] Key = {
         0x33,  0x20,  0x6d,  0x54,
         0x32,  0x6c,  0x65,  0x68,
         0x20,  0x65,  0x73,  0x69,
         0x62,  0x6e,  0x73,  0x73,
         0x79,  0x67,  0x61,  0x20,
         0x74,  0x74,  0x67,  0x69,
         0x65,  0x68,  0x65,  0x73,
         0x73,  0x3d,  0x2C,  0x20
                          };
        byte[] bRepTab = {
                                 0x4A,0x92,0xD8,0x0E,0x6B,0x1C,0x7F,0x53,
                                 0xEB,0x4C,0x6D,0xFA,0x23,0x81,0x07,0x59,
                                 0x58,0x1D,0xA3,0x42,0xEF,0xC7,0x60,0x9B,
                                 0x7D,0xA1,0x08,0x9F,0xE4,0x6C,0xB2,0x53,
                                 0x6C,0x71,0x5F,0xD8,0x4A,0x9E,0x03,0xB2,
                                 0x4B,0xA0,0x72,0x1D,0x36,0x85,0x9C,0xFE,
                                 0xDB,0x41,0x3F,0x59,0x0A,0xE7,0x68,0x2C,
                                 0x1F,0xD0,0x57,0xA4,0x92,0x3E,0x6B,0x8C
                             };

        private void button1_Click(object sender, EventArgs e)
        {
            string text = "";
            text = textBox1.Text;
            if (text!="")
            {

                if (text.Length % 8 == 0)
                {
                    byte[] tex1 = new byte[text.Length];
                    for (int i = 0; i < text.Length; i++)
                    {
                        tex1[i] += Convert.ToByte(text[i]);
                    }
                    prvst = cd.SimpleEncoding(tex1);
                    textBox2.Text = System.Text.Encoding.ASCII.GetString(prvst);
                }
                else
                {
                    MessageBox.Show("Введите текст кратный 8");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = System.Text.Encoding.ASCII.GetString(cd.SimpleDecoding(prvst));
        }
    }
}
