using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace FKPOP01
{
    public partial class idolPicksForm : Form
    {
        public mainForm CallingForm { get; set; }
        public Groups Group { get; set; }
        ResultsForm f3 = new ResultsForm();
        public bool LoadedGame { get; set; }

        public idolPicksForm()
        {
            InitializeComponent();
                      
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            var confrimationBox = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo);
            if (confrimationBox == DialogResult.Yes)
            {
                this.Close();
                Environment.Exit(0);
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {

        }

        private void nextButton_Click(object sender, EventArgs e)
        {           

            if (pictureBox1.Visible == true && CallingForm.Group.Size > 1)
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[1].PrintInfo();
            }
            else if (pictureBox2.Visible == true && CallingForm.Group.Size > 2)
            {
                pictureBox2.Visible = false;
                pictureBox3.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[2].PrintInfo();
            }

            else if (pictureBox3.Visible == true && CallingForm.Group.Size > 3)
            {
                pictureBox3.Visible = false;
                pictureBox4.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[3].PrintInfo();
            }

            else if (pictureBox4.Visible == true && CallingForm.Group.Size > 4)
            {
                pictureBox4.Visible = false;
                pictureBox5.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[4].PrintInfo();
            }

            else if (pictureBox5.Visible == true && CallingForm.Group.Size > 5)
            {
                pictureBox5.Visible = false;
                pictureBox6.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[5].PrintInfo();
            }

            else if (pictureBox6.Visible == true && CallingForm.Group.Size > 6)
            {
                pictureBox6.Visible = false;
                pictureBox7.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[6].PrintInfo();
            }

            else if (pictureBox7.Visible == true && CallingForm.Group.Size > 7)
            {
                pictureBox7.Visible = false;
                pictureBox8.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[7].PrintInfo();
            }

            else if (pictureBox8.Visible == true && CallingForm.Group.Size > 8)
            {
                pictureBox8.Visible = false;
                pictureBox9.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[8].PrintInfo();
            }

            else if (pictureBox9.Visible == true && CallingForm.Group.Size > 9)
            {
                pictureBox9.Visible = false;
                pictureBox10.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[9].PrintInfo();
            }

            else if (pictureBox10.Visible == true && CallingForm.Group.Size > 10)
            {
                pictureBox10.Visible = false;
                pictureBox11.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[10].PrintInfo();
            }

            else if (pictureBox11.Visible == true && CallingForm.Group.Size > 11)
            {
                pictureBox11.Visible = false;
                pictureBox12.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[11].PrintInfo();
            }

            else if (pictureBox12.Visible == true && CallingForm.Group.Size > 12)
            {
                pictureBox12.Visible = false;
                pictureBox13.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[12].PrintInfo();
            }

            else if (pictureBox13.Visible == true && CallingForm.Group.Size > 13)
            {
                pictureBox13.Visible = false;
                pictureBox14.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[13].PrintInfo();
            }

        }

        private void IdolsPickForm_Load(object sender, EventArgs e)
        {            
            f3.CallingForm = this;
            this.LoadedGame = false;

            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14};

            textBox1.Text = CallingForm.Group.Name;
            conceptBox.SelectedItem = CallingForm.Group.Concept;
            listBox1.SelectedIndex = 0;

            int idolCount = Program.GetTableCount("Idols");

            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {

                    string query = "Select * From Idols";
                    SqlDataAdapter da = new SqlDataAdapter(query, connection);
                    connection.Open();

                    DataSet ds = new DataSet();
                    da.Fill(ds, "Idols");
                    DataTable dt = new DataTable();
                    dt = ds.Tables["Idols"];

                    Random rnd = new Random();
                    List<int> IdolIds = new List<int>();
                    int newNumber = 0;

                    for (int i = 0; i < CallingForm.Group.Size; i++)
                    {
                        newNumber = rnd.Next(idolCount);
                        while (IdolIds.Contains(newNumber))
                            newNumber = rnd.Next(idolCount);
                        IdolIds.Add(newNumber);
                        
                        CallingForm.Group.AllIdols[i] = new Idols();                       

                        foreach (PropertyInfo info in typeof(Idols).GetProperties())
                        {
                            object value = dt.Rows[newNumber][info.Name];
                            
                            if (info.PropertyType == typeof(int) && value.GetType() != typeof(System.DBNull))
                            {
                                info.SetValue(CallingForm.Group.AllIdols[i], dt.Rows[newNumber][info.Name]);
                            }
                            else if (info.PropertyType == typeof(int) && value.GetType() == typeof(System.DBNull))
                            {
                                info.SetValue(CallingForm.Group.AllIdols[i], rnd.Next(11));
                            }
                            else if (info.PropertyType == typeof(string) && value.GetType() != typeof(System.DBNull))
                            {
                                info.SetValue(CallingForm.Group.AllIdols[i], dt.Rows[newNumber][info.Name].ToString().Trim(' '));
                            }
                            else if (info.PropertyType == typeof(string) && value.GetType() == typeof(System.DBNull))
                            {
                                info.SetValue(CallingForm.Group.AllIdols[i], "");
                            }
                        }                        
                        CallingForm.Group.AllIdols[i].GroupId = Convert.ToInt32(dt.Rows[newNumber]["IdolGroup"].ToString());                        
                        CallingForm.Group.AllIdols[i].Height = Convert.ToDecimal(dt.Rows[newNumber]["Height"].ToString());
                        CallingForm.Group.AllIdols[i].Weight = Convert.ToDecimal(dt.Rows[newNumber]["Weight"].ToString());

                        CallingForm.Group.AllIdols[i].Age += CallingForm.Group.currentDate.Year - 2020;
                        
                        //Debug.WriteLine(CallingForm.Group.AllIdols[i].Id + " real ID. Id: " + newNumber +
                        //  " name: " + CallingForm.Group.AllIdols[i].Name + " vocal: " + CallingForm.Group.AllIdols[i].Vocal);

                        //CallingForm.Group.AllIdols[i].GenerateStats();

                        string dest = "";
                        string dest2 = "";
                        dest = Path.Combine(Program.resourcesPath, CallingForm.Group.AllIdols[i].IdolName + CallingForm.Group.AllIdols[i].GroupId + ".jpg");
                        dest2 = Path.Combine(Program.resourcesPath, CallingForm.Group.AllIdols[i].IdolName + CallingForm.Group.AllIdols[i].GroupId + ".jpeg");
                        if (File.Exists(dest))
                        {
                            boxes[i].Load(dest);
                        }
                        else if (File.Exists(dest2)){
                            boxes[i].Load(dest2);
                        }
                        else
                            boxes[i].Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));

                        if (i == CallingForm.Group.Size - 1)
                            CallingForm.Group.AllIdols[i].Leader = true;
                    }                                                           
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message + "\nLine: " + ex.StackTrace);
                }
                pictureboxFalse();
                pictureBox1.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[0].PrintInfo();
            }
        }

        public void pictureboxFalse()
        {
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;
            pictureBox13.Visible = false;
            pictureBox14.Visible = false;
            pictureBox10.Visible = false;
            pictureBox9.Visible = false;
            pictureBox8.Visible = false;
            pictureBox7.Visible = false;
            pictureBox6.Visible = false;
            pictureBox5.Visible = false;
            pictureBox4.Visible = false;
            pictureBox3.Visible = false;
            pictureBox2.Visible = false;
            pictureBox1.Visible = false;
        }
                
        private void flowLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void finishButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Group Name cannot be blank");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Album Name cannot be blank");
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Title Track Name cannot be blank");
                return;
            }

            this.Group = CallingForm.Group;
            this.Group.Name = textBox1.Text.ToString();
            this.Group.Concept = conceptBox.Text;
            this.Group.ComebackInfos.Add(new Comeback(textBox3.Text, 
                                                      textBox2.Text, 
                                                      this.Group.Concept, 
                                                      Convert.ToInt32(numericUpDown1.Value), 
                                                      listBox1.Text,
                                                      this.Group.Debut));

            Group.Comebacks += 1;

            this.Hide();            
            f3.Show();
        }

        public void formRefresh()
        {

        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            //if (pictureBox1.Visible == true && CallingForm.Group.Size > 1)
            //{
            //    pictureBox1.Visible = false;
            //    pictureBox2.Visible = true;
            //    label2.Text = CallingForm.Group.AllIdols[1].PrintInfo();
            //}
            if (pictureBox2.Visible == true && CallingForm.Group.Size > 1)
            {
                pictureBox2.Visible = false;
                pictureBox1.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[0].PrintInfo();
            }

            else if (pictureBox3.Visible == true && CallingForm.Group.Size > 2)
            {
                pictureBox3.Visible = false;
                pictureBox2.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[1].PrintInfo();
            }

            else if (pictureBox4.Visible == true && CallingForm.Group.Size > 3)
            {
                pictureBox4.Visible = false;
                pictureBox3.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[2].PrintInfo();
            }

            else if (pictureBox5.Visible == true && CallingForm.Group.Size > 4)
            {
                pictureBox5.Visible = false;
                pictureBox4.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[3].PrintInfo();
            }

            else if (pictureBox6.Visible == true && CallingForm.Group.Size > 5)
            {
                pictureBox6.Visible = false;
                pictureBox5.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[4].PrintInfo();
            }

            else if (pictureBox7.Visible == true && CallingForm.Group.Size > 6)
            {
                pictureBox7.Visible = false;
                pictureBox6.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[5].PrintInfo();
            }

            else if (pictureBox8.Visible == true && CallingForm.Group.Size > 7)
            {
                pictureBox8.Visible = false;
                pictureBox7.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[6].PrintInfo();
            }

            else if (pictureBox9.Visible == true && CallingForm.Group.Size > 8)
            {
                pictureBox9.Visible = false;
                pictureBox8.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[7].PrintInfo();
            }

            else if (pictureBox10.Visible == true && CallingForm.Group.Size > 9)
            {
                pictureBox10.Visible = false;
                pictureBox9.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[8].PrintInfo();
            }

            else if (pictureBox11.Visible == true && CallingForm.Group.Size > 10)
            {
                pictureBox11.Visible = false;
                pictureBox10.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[9].PrintInfo();
            }

            else if (pictureBox12.Visible == true && CallingForm.Group.Size > 11)
            {
                pictureBox12.Visible = false;
                pictureBox11.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[10].PrintInfo();
            }

            else if (pictureBox13.Visible == true && CallingForm.Group.Size > 12)
            {
                pictureBox13.Visible = false;
                pictureBox12.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[11].PrintInfo();
            }
            else if (pictureBox14.Visible == true && CallingForm.Group.Size > 13)
            {
                pictureBox14.Visible = false;
                pictureBox13.Visible = true;
                label2.Text = CallingForm.Group.AllIdols[12].PrintInfo();
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Enter group name between 1-16 characters", textBox1);
        }
    }
    
}
