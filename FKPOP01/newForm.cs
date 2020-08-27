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

namespace FKPOP01
{
    public partial class newForm : Form
    {
        Label[] labels = new Label[14];
        TextBox[] names = new TextBox[14];
        TextBox[] weights = new TextBox[14];
        TextBox[] heights = new TextBox[14];
        TextBox[] ages = new TextBox[14];

        public newForm()
        {
            InitializeComponent();
        }

        private void newForm_Load(object sender, EventArgs e)
        {
            flowLayoutPanel2.Hide();            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int members = Convert.ToInt32(numericUpDown1.Value);

            if(textBox1.Text == "")
            {
                MessageBox.Show("Group Name cannot be blank");
                return;
            }

            flowLayoutPanel1.Hide();
            flowLayoutPanel2.Controls.Remove(button1);
            flowLayoutPanel2.Show();

            int idolCount = Program.GetTableCount("Idols");
            int groupsCount = Program.GetTableCount("Groups");

            for (int i = 0; i < members; i++)
            {
                labels[i] = new Label();
                labels[i].Text = String.Format("Idol {0} Name, Weight, Height, Age", i + 1);
                labels[i].Size = new Size(200, 15);

                names[i] = new TextBox();
                names[i].Text = "Name";
                names[i].Size = new Size(208, 20);

                weights[i] = new TextBox();
                weights[i].Text = "Weight (lb)";
                weights[i].Size = new Size(100, 20);

                heights[i] = new TextBox();
                heights[i].Text = "Height (cm)";
                heights[i].Size = new Size(100, 20);

                ages[i] = new TextBox();
                ages[i].Text = "Age";
                ages[i].Size = new Size(50, 20);

                flowLayoutPanel2.Controls.Add(labels[i]);
                flowLayoutPanel2.Controls.Add(names[i]);
                flowLayoutPanel2.Controls.Add(weights[i]);
                flowLayoutPanel2.Controls.Add(heights[i]);
                flowLayoutPanel2.Controls.Add(ages[i]);
            }
            flowLayoutPanel2.Controls.Add(button1);

            button1.Click += new EventHandler(button1_Click);

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int members = Convert.ToInt32(numericUpDown1.Value);
            int groupsCount = Program.GetTableCount("Groups");

            for (int j = 0; j < members; j++)
            {
                if (names[j].Text == "" || weights[j].Text == "" || heights[j].Text == "" || ages[j].Text == "")
                {
                    MessageBox.Show("Fields cannot be blank");
                    return;
                }
            }
        }
    }
}
