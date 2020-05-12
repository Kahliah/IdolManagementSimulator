using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FKPOP01
{
    public partial class gameOverForm : Form
    {
        public ResultsForm CallingForm { get; set; }
        public Groups Group { get; private set; }
        public string Reason { get; set; }

        public gameOverForm()
        {
            InitializeComponent();
        }

        private void gameOverForm_Load(object sender, EventArgs e)
        {
            label1.Text = "You have lost because: " + Reason;
            label2.Text = "Your groups stats:\n" + CallingForm.Group.PrintGroupInfo();
            label3.Text = "Current Date: " + CallingForm.Group.currentDate.ToShortDateString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string allComebacks = "";

            for (int i = 0; i < CallingForm.Group.Comebacks; i++)
            {
                allComebacks += "Comeback #" + (i + 1) + "\n" + CallingForm.Group.ComebackInfos[i].PrintInfo() + "\n";
            }

            Form f1 = new Form();
            f1.Size = new Size(250, 500);
            FlowLayoutPanel p1 = new FlowLayoutPanel();
            Label l1 = new Label();
            l1.Text = allComebacks;
            l1.AutoSize = true;
            p1.Controls.Add(l1);
            p1.AutoScroll = true;
            p1.Dock = DockStyle.Fill;
            f1.Controls.Add(p1);
            f1.StartPosition = FormStartPosition.CenterScreen;
            f1.Show();
        }
    }
}
