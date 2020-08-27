using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FKPOP01
{
    public partial class idolPicksForm : Form
    {
        public mainForm CallingForm { get; set; }
        public Groups Group { get; set; }
        ResultsForm f3 = new ResultsForm();
        public bool LoadedGame { get; set; }
        public int currentPictureBox = 0;

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
            settingsForm frm = new settingsForm();
            frm.Show();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4,
                                   pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                                   pictureBox9, pictureBox10, pictureBox11, pictureBox12,
                                   pictureBox13, pictureBox14};

            if (boxes[currentPictureBox].Visible == true && CallingForm.Group.Size > currentPictureBox+1)
            {
                boxes[currentPictureBox].Visible = false;
                boxes[currentPictureBox+1].Visible = true;
                label2.Text = CallingForm.Group.AllIdols[currentPictureBox+1].PrintInfo();
                currentPictureBox++;
            }
        }

        private void IdolsPickForm_Load(object sender, EventArgs e)
        {            
            f3.CallingForm = this;
            this.LoadedGame = false;

            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4,
                                   pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                                   pictureBox9, pictureBox10, pictureBox11, pictureBox12,
                                   pictureBox13, pictureBox14};

            textBox1.Text = CallingForm.Group.Name;
            conceptBox.SelectedItem = CallingForm.Group.Concept;
            listBox1.SelectedIndex = 0;

            int idolCount = Program.GetTableCount("Idols");

            Idols idolGet;

            int id = 0, age = 0;

            string gender = "", 
                   idolgroup = "",
                   idolname = "", 
                   specialname = "";

            decimal height = 0, 
                    weight = 0;

            var client = new HttpClient();

            try
            {
                Random rnd = new Random();
                List<int> IdolIds = new List<int>();
                int newNumber = 0;

                for (int i = 0; i < CallingForm.Group.Size; i++)
                {                                 
                    newNumber = rnd.Next(1, idolCount+1);
                    while (IdolIds.Contains(newNumber))
                        newNumber = rnd.Next(1, idolCount+1);
                    IdolIds.Add(newNumber);

                    //Debug.WriteLine("New Number/Idol Id Is:" + newNumber);
                    
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync(Program.apiPath + "/idols/" + newNumber).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Connection Error: " + response.StatusCode);
                        Environment.Exit(0);
                    }

                    dynamic context = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    id = context.Items[0].Id.N;
                    age = context.Items[0].Age.N;
                    gender = context.Items[0].Gender.S;
                    height = context.Items[0].Height.N;
                    weight = context.Items[0].Weight.N;
                    idolgroup = context.Items[0].IdolGroup.S;
                    idolname = context.Items[0].IdolName.S;
                    specialname= context.Items[0].SpecialName.S;

                    idolGet = new Idols(id, age, gender, height, weight, idolgroup, idolname, specialname);
                
                    CallingForm.Group.AllIdols[i] = idolGet;
                        
                    CallingForm.Group.AllIdols[i].Age += CallingForm.Group.currentDate.Year - 2020;

                    //Debug.WriteLine("Idol is:" + CallingForm.Group.AllIdols[i].IdolName);
                        
                    string dest = "";
                    string dest2 = "";
                    string imageName = idolGet.IdolName.Replace(" ", "_") + "!" + idolGet.IdolGroup.Replace(" ", "_");
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
                    else if (File.Exists(Path.Combine(Program.resourcesPath, "noimage.jpg")))
                        boxes[i].Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));
                       
                    if (i == CallingForm.Group.Size - 1)
                        CallingForm.Group.AllIdols[i].Leader = true;                    
                }
            }
            catch (Exception ex) { Debug.WriteLine("Error: " + ex.Message); }

            pictureboxFalse();
            pictureBox1.Visible = true;
            label2.Text = CallingForm.Group.AllIdols[0].PrintInfo();
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
                    
            f3.Show();
            this.Hide();
        }

        public void formRefresh()
        {

        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4,
                                   pictureBox5, pictureBox6, pictureBox7, pictureBox8,
                                   pictureBox9, pictureBox10, pictureBox11, pictureBox12,
                                   pictureBox13, pictureBox14};

            if (boxes[currentPictureBox].Visible == true && currentPictureBox > 0)
            {
                boxes[currentPictureBox].Visible = false;
                boxes[currentPictureBox - 1].Visible = true;
                label2.Text = CallingForm.Group.AllIdols[currentPictureBox - 1].PrintInfo();
                currentPictureBox--;
            }
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Enter group name between 1-16 characters", textBox1);
        }
    }
    
}
