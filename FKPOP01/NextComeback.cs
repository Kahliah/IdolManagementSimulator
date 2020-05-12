using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FKPOP01
{
    public partial class NextComebackForm : Form
    {
        public TrainingForm CallingForm { get; set; }
        public Groups Group { get; private set; }

        public NextComebackForm()
        {
            InitializeComponent();
        }

        private void NextComebackForm_Load(object sender, EventArgs e)
        {
            this.Group = CallingForm.Group;
            formRefresh();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            var confrimationBox = MessageBox.Show("Are you sure you want to exit? (Remember to save first.)", "Exit", MessageBoxButtons.YesNo);
            if (confrimationBox == DialogResult.Yes)
            {
                this.Close();
                Environment.Exit(0);
            }
        }

        private void nextButton1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Fields cannot be blank.");
                return;
            }

            this.Group = CallingForm.Group;                     
           
            this.Group.ComebackInfos.Add(new Comeback(textBox3.Text, 
                                                      textBox2.Text, 
                                                      conceptBox.Text, 
                                                      Convert.ToInt32(numericUpDown1.Value), 
                                                      listBox1.Text, 
                                                      dateTimePicker1.Value));

            Group.Comebacks += 1;

            if (CallingForm.Group.currentDate.Year != dateTimePicker1.Value.Year)
                CallingForm.Group.IdolsBirthday(dateTimePicker1.Value.Year - CallingForm.Group.currentDate.Year);

            CallingForm.Group.currentDate = dateTimePicker1.Value;
            CallingForm.Group.Concept = conceptBox.Text;

            this.Hide();

            CallingForm.CallingForm.Show();
            CallingForm.CallingForm.formRefresh();
                        
        }

        public void formRefresh()
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14};
            for (int i = 0; i < 14; i++){
                boxes[i].Image = null;
            }

            label1.Text = CallingForm.Group.Name;
            conceptBox.SelectedItem = CallingForm.Group.Concept;
            listBox1.SelectedItem = CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Genre;

            textBox2.Text = "";
            textBox3.Text = "";

            dateTimePicker1.MinDate = CallingForm.Group.currentDate;
            dateTimePicker1.Value = CallingForm.Group.currentDate;

            string Fatigues = "";

            for (int i = 0; i < CallingForm.Group.Size; i++)
            {
                Fatigues += CallingForm.Group.AllIdols[i].PrintFatigueAndStatus();

                //Set up picture boxes                
                string dest = "";
                string dest2 = "";
                dest = Path.Combine(Program.resourcesPath, CallingForm.Group.AllIdols[i].IdolName + CallingForm.Group.AllIdols[i].GroupId + ".jpg");
                dest2 = Path.Combine(Program.resourcesPath, CallingForm.Group.AllIdols[i].IdolName + CallingForm.Group.AllIdols[i].GroupId + ".jpeg");
                if (File.Exists(dest))
                {
                    boxes[i].Load(dest);
                }
                else if (File.Exists(dest2))
                {
                    boxes[i].Load(dest2);
                }
                else
                    boxes[i].Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));

                if (CallingForm.Group.Size == 1)
                {
                    boxes[i].Size = new Size(300, 300);
                }
                else if (CallingForm.Group.Size <= 4)
                {
                    boxes[i].Size = new Size(180, 180);
                }
                else if (CallingForm.Group.Size <= 8)
                {
                    boxes[i].Size = new Size(150, 150);
                }
                
            }
            label3.Text = Fatigues;
            label4.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();
            label11.Text = CallingForm.Group.PrintGroupInfo();
            label12.Text = "Last Comeback:\n" + CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].PrintInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            settingsForm frm = new settingsForm();
            frm.Show();
        }
    }
}
