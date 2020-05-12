using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        Int32 groupId = 0;

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

            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    string query = "INSERT INTO Groups ([GroupName], [MemberCount]) values(@GroupName, @MemberCount)";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.Add("@GroupName", SqlDbType.NChar).Value = textBox1.Text;
                    cmd.Parameters.Add("@MemberCount", SqlDbType.Int).Value = Convert.ToInt32(numericUpDown1.Value);
                    connection.Open();
                    int q = cmd.ExecuteNonQuery();

                    query = "Select Id From Groups Where GroupName = '" + textBox1.Text + "'";

                    cmd = new SqlCommand(query, connection);
                    
                    groupId = (Int32)cmd.ExecuteScalar();

                    if (q > 0)
                        MessageBox.Show("Group Created, now add members info");

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
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

            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    string[] querys = new string[members];
                    SqlCommand[] cmd = new SqlCommand[members];
                    int i = 0;

                    connection.Open();

                    for (int j = 0; j < members; j++)
                    {                        
                        querys[j] = "INSERT INTO Idols ([IdolName], [IdolGroup], [Weight], [Height], [Age]) values(@IdolName, @IdolGroup, @Weight, @Height, @Age)";
                        cmd[j] = new SqlCommand(querys[j], connection);

                        cmd[j].Parameters.Add("@IdolName", SqlDbType.NChar).Value = names[j].Text;
                        cmd[j].Parameters.Add("@IdolGroup", SqlDbType.Int).Value = groupId;
                        cmd[j].Parameters.Add("@Weight", SqlDbType.Int).Value = Convert.ToInt32(weights[j].Text);
                        cmd[j].Parameters.Add("@Height", SqlDbType.Int).Value = Convert.ToInt32(heights[j].Text);
                        cmd[j].Parameters.Add("@Age", SqlDbType.Int).Value = Convert.ToInt32(ages[j].Text);

                        i = cmd[j].ExecuteNonQuery();
                    }

                    connection.Close();

                    if (i > 0)
                    {
                        MessageBox.Show("Members added");
                    }
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message + "\n (Please no decimals in Weight/Height");
                }
            }
        }
    }
}
