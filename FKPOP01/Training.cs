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
    public partial class TrainingForm : Form
    {
        public ResultsForm CallingForm { get; set; }
        public Groups Group { get; private set; }
        NextComebackForm f5 = new NextComebackForm();

        public TrainingForm()
        {
            InitializeComponent();
        }

        private void TrainingForm_Load(object sender, EventArgs e)
        {
            this.Group = CallingForm.Group;
            formRefresh();
            f5.CallingForm = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settingsForm frm = new settingsForm();
            frm.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label8.Text = CallingForm.Group.AllIdols[comboBox1.SelectedIndex].PrintInfo();
        }

        //Train single idol in single stat
        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            CallingForm.Group.AllIdols[comboBox1.SelectedIndex].IncreaseStat(comboBox2.Text, rnd);            
            label8.Text = CallingForm.Group.AllIdols[comboBox1.SelectedIndex].PrintInfo();

            if (CallingForm.Group.currentDate.Year != CallingForm.Group.currentDate.AddDays(1).Year)
                CallingForm.Group.IdolsBirthday();
            CallingForm.Group.currentDate = CallingForm.Group.currentDate.AddDays(1);
            

            label4.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();

            String Fatigues = "";

            for (int i = 0; i < CallingForm.Group.Size; i++)
            {                
                Fatigues += CallingForm.Group.AllIdols[i].PrintFatigueAndStatus();
            }
            label3.Text = Fatigues;

            CallingForm.Group.Loan -= 100;
            label9.Text = "Money: $" + CallingForm.Group.Loan.ToString();

        }

        //Rest single idol
        private void button3_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            if (CallingForm.Group.AllIdols[comboBox1.SelectedIndex].Fatigue != 0)
            {
                CallingForm.Group.AllIdols[comboBox1.SelectedIndex].Fatigue -= 1;
                if (CallingForm.Group.AllIdols[comboBox1.SelectedIndex].Fatigue <= 75)
                    CallingForm.Group.AllIdols[comboBox1.SelectedIndex].Status = "Healthy";
                else
                    CallingForm.Group.AllIdols[comboBox1.SelectedIndex].UpdateStatus(rnd);
            }
            else
            {
                MessageBox.Show("This idol is fully rested!");
                return;
            }
            
            label8.Text = CallingForm.Group.AllIdols[comboBox1.SelectedIndex].PrintInfo();

            if (CallingForm.Group.currentDate.Year != CallingForm.Group.currentDate.AddDays(1).Year)
                CallingForm.Group.IdolsBirthday();
            CallingForm.Group.currentDate = CallingForm.Group.currentDate.AddDays(1);


            label4.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();

            String Fatigues = "";

            for (int i = 0; i < CallingForm.Group.Size; i++)
            {
                Fatigues += CallingForm.Group.AllIdols[i].PrintFatigueAndStatus();
            }
            label3.Text = Fatigues;

            CallingForm.Group.Loan -= 20*CallingForm.Group.Size;
            label9.Text = "Money: $" + CallingForm.Group.Loan.ToString();
        }

        private void nextButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Group = CallingForm.Group;
            f5.formRefresh();
            f5.Show();
        }

        public void formRefresh()
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4,
                                   pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                                   pictureBox9, pictureBox10, pictureBox11, pictureBox12,
                                   pictureBox13, pictureBox14};
            for (int i = 0; i < 14; i++)
            {
                boxes[i].Image = null;
            }

            BindingSource box1Binding = new BindingSource();
            box1Binding.DataSource = CallingForm.Group.AllIdols;
            comboBox1.DataSource = box1Binding.DataSource;
            comboBox1.DisplayMember = "IdolName";
            comboBox1.ValueMember = "Id";
            comboBox2.SelectedIndex = 0;

            label8.Text = CallingForm.Group.AllIdols[0].PrintInfo();

            label1.Text = CallingForm.Group.Name;
            string Fatigues = "";

            for (int i = 0; i < CallingForm.Group.Size; i++)
            {                
                Fatigues += CallingForm.Group.AllIdols[i].PrintFatigueAndStatus();

                //Set up picture boxes                
                string dest = "";
                string dest2 = "";
                string imageName = CallingForm.Group.AllIdols[i].IdolName.Replace(" ", "_") + "!"
                                    + CallingForm.Group.AllIdols[i].IdolGroup.Replace(" ", "_");

                dest = Path.Combine(Program.resourcesPath, imageName + ".jpg");
                dest2 = Path.Combine(Program.resourcesPath, imageName + ".jpeg");

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

            if (CallingForm.Group.currentDate.Year != CallingForm.Group.currentDate.AddDays(1).Year)
                CallingForm.Group.IdolsBirthday();
            CallingForm.Group.currentDate = CallingForm.Group.currentDate.AddDays(1);

            label4.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();
            label9.Text = "Money: $" + CallingForm.Group.Loan.ToString();
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

        //Rest all idols
        private void button4_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            for(int i=0; i < CallingForm.Group.Size; i++)
            {
                if (CallingForm.Group.AllIdols[i].Fatigue != 0)
                {
                    CallingForm.Group.AllIdols[i].Fatigue -= 1;
                    if (CallingForm.Group.AllIdols[comboBox1.SelectedIndex].Fatigue <= 75)
                        CallingForm.Group.AllIdols[i].Status = "Healthy";
                    else
                        CallingForm.Group.AllIdols[i].UpdateStatus(rnd);
                }
            }

            if (CallingForm.Group.currentDate.Year != CallingForm.Group.currentDate.AddDays(1).Year)
                CallingForm.Group.IdolsBirthday();
            CallingForm.Group.currentDate = CallingForm.Group.currentDate.AddDays(1);
            
            label4.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();

            label8.Text = CallingForm.Group.AllIdols[comboBox1.SelectedIndex].PrintInfo();

            String Fatigues = "";

            for (int i = 0; i < CallingForm.Group.Size; i++)
            {                
                Fatigues += CallingForm.Group.AllIdols[i].PrintFatigueAndStatus();
            }
            label3.Text = Fatigues;

            CallingForm.Group.Loan -= 50*CallingForm.Group.Size;
            label9.Text = "Money: $" + CallingForm.Group.Loan.ToString();
        }

        //Train all idols in single stat
        private void button5_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            for (int i=0; i<CallingForm.Group.Size; i++)
            {
                CallingForm.Group.AllIdols[i].IncreaseStat(comboBox2.Text, rnd);
            }
            label8.Text = CallingForm.Group.AllIdols[comboBox1.SelectedIndex].PrintInfo();

            if (CallingForm.Group.currentDate.Year != CallingForm.Group.currentDate.AddDays(1).Year)
                CallingForm.Group.IdolsBirthday();
            CallingForm.Group.currentDate = CallingForm.Group.currentDate.AddDays(1);


            label4.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();

            String Fatigues = "";

            for (int i = 0; i < CallingForm.Group.Size; i++)
            {
                Fatigues += CallingForm.Group.AllIdols[i].PrintFatigueAndStatus();
            }
            label3.Text = Fatigues;

            CallingForm.Group.Loan -= 75*CallingForm.Group.Size;
            label9.Text = "Money: $" + CallingForm.Group.Loan.ToString();
        }
    }
}
