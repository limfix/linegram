using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineGram
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

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            Console.WriteLine("{0} {1}", Execute.IsLogged(webBrowser1), Execute.IsOnProfilePage(webBrowser1));
            if (Execute.IsLogged(webBrowser1) && Execute.IsOnProfilePage(webBrowser1))
            {
                Execute.ProcessPage(webBrowser1);
            }
            else
            {
                label1.Visible = true;
                label2.Visible = true;
            }
            
        }

        public Button GetButton()
        {
            return button1;
        }


    }
}
