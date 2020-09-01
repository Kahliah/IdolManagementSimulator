using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FKPOP01
{
    public partial class ResultsForm : Form
    {
        public idolPicksForm CallingForm { get; set; }
        public Groups Group { get; private set; }
        TrainingForm f4 = new TrainingForm();

        public String[] Cfs = new string[8] { "Lipstick", "Video Game", "Contact Lenses",
                                              "Sports Drink", "Fried Chicken", "Car",
                                              "Phone", "Soju" };
        public double GroupCFchance = 0;

        public ResultsForm()
        {
            InitializeComponent();
        }

        private void ResultsForm_Load(object sender, EventArgs e)
        {
            this.Group = CallingForm.Group;
            if (CallingForm.LoadedGame == false)
            {
                formRefresh();
                f4.CallingForm = this;
            }
            else
            {
                CallingForm.LoadedGame = true;
                this.Hide();
                f4.CallingForm = this;
                f4.formRefresh();
                f4.Show();
            }
            //f4.CallingForm = this;
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
            //this.Group = CallingForm.Group;
            f4.formRefresh();     
            f4.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            settingsForm frm = new settingsForm();
            frm.Show();
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

            Random rnd = new Random();
            decimal GroupAge = (CallingForm.Group.currentDate - CallingForm.Group.Debut).Days / 365.25m;
            double AllModifiers = 0;
            int newNumber = 0;

            label10.Text = "Last Comeback:\n" + CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].PrintInfo();
            label1.Text = CallingForm.Group.Name;
            label2.Text = String.Format("Comeback #{0} Results", CallingForm.Group.Comebacks);

            newNumber = rnd.Next(0, CallingForm.Group.Size);
            if (CallingForm.Group.playVideo)
                playComebackVideo(CallingForm.Group.AllIdols[newNumber].IdolGroup, CallingForm.Group.AllIdols[newNumber].IdolName);

            string Fatigues = "";
            for (int i = 0; i < CallingForm.Group.Size; i++)
            {               
                CallingForm.Group.AllIdols[i].UpdateStatus(rnd);
                CallingForm.Group.AllIdols[i].UpdateFatigue(rnd);
                CallingForm.Group.AllIdols[i].UpdateModifier();                

                if (CallingForm.Group.AllIdols[i].Status == "Injured")
                    AllModifiers += CallingForm.Group.AllIdols[i].Modifier / 3;
                else if (CallingForm.Group.AllIdols[i].Status == "Fainted")
                    AllModifiers += CallingForm.Group.AllIdols[i].Modifier / 4;
                else if (CallingForm.Group.AllIdols[i].Status == "Cannot work")
                    AllModifiers -= CallingForm.Group.AllIdols[i].Modifier;
                else
                    AllModifiers += CallingForm.Group.AllIdols[i].Modifier;
                                
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
                else if(CallingForm.Group.Size <= 4)
                {
                    boxes[i].Size = new Size(180, 180);
                }
                else if(CallingForm.Group.Size <= 8)
                {
                    boxes[i].Size = new Size(150, 150);
                }
                

            }
            //Debug.WriteLine(AllModifiers);

            //Modifier Bonuses
            if (CallingForm.Group.Concept == "Sexy" && CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Genre == "EDM")
            {
                if (CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Date.Month >= 4 &&
                    CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Date.Month < 9)                
                    AllModifiers = AllModifiers + AllModifiers * 0.40;                
                else
                    AllModifiers = AllModifiers + AllModifiers * 0.25;
            }
            if (CallingForm.Group.Concept == "Cute" && CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Genre == "Bubblegum Pop")
            {
                AllModifiers = AllModifiers + AllModifiers * 0.25;
            }
            if (CallingForm.Group.Concept == "Girl Crush" && CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Genre == "Rock")
            {
                AllModifiers = AllModifiers + AllModifiers * 0.15;
            }
            if (Program.GetWordCount(CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].TitleTrack) == 1)
            {
                AllModifiers = AllModifiers + AllModifiers * 0.25;
            }

            //Debug.WriteLine(AllModifiers);

            //Remove fans for long comeback time
            //Debug.WriteLine("before: " + CallingForm.Group.HardcoreFans);
            CallingForm.Group.FansComebackDate();
            //Debug.WriteLine("after: " + CallingForm.Group.HardcoreFans);

            //Risk of changing comeback
            if (CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Concept != CallingForm.Group.Concept)
            {                
                if (Convert.ToDouble(rnd.Next(101)) / 100 < 0.75)
                {                    
                    CallingForm.Group.HardcoreFans -= CallingForm.Group.HardcoreFans / 4;
                    CallingForm.Group.CasualFans -= CallingForm.Group.CasualFans / 2;
                }
                else
                {
                    CallingForm.Group.HardcoreFans += CallingForm.Group.HardcoreFans / 4;
                    CallingForm.Group.CasualFans += CallingForm.Group.CasualFans / 2;
                }
            }

            //Calculate new fans
            newNumber = rnd.Next(10, 30);
            CallingForm.Group.CasualFans += Convert.ToInt32(newNumber * AllModifiers);
            newNumber = rnd.Next(2, 15);
            CallingForm.Group.HardcoreFans += Convert.ToInt32(newNumber * AllModifiers);

            if(CallingForm.Group.Comebacks == 1)
            {
                newNumber = rnd.Next(1, 10);                
                CallingForm.Group.CasualFans += newNumber * (CallingForm.Group.Loan/1000*-1);
                newNumber = rnd.Next(1, 5);
                CallingForm.Group.HardcoreFans += newNumber * (CallingForm.Group.Loan/1000*-1);
            }

            //Calculate expenses
            int expenses = 0;
            //$150 Per Idol Each Comeback day
            expenses += CallingForm.Group.Size * 150 * -1;
            //$20 Per Idol each day between comebacks
            if(CallingForm.Group.Comebacks > 1)
                expenses += ((CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Date - 
                                            CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 2].Date).Days - 29) * 
                                            CallingForm.Group.Size * 20 * -1;
            CallingForm.Group.Loan = CallingForm.Group.Loan + expenses;

            //$ Per Track Each Comeback
            int tracks = CallingForm.Group.ComebackInfos[CallingForm.Group.Comebacks - 1].Tracks;
            if (tracks == 1)
            {
                CallingForm.Group.Loan -= 50;
                expenses += 50*-1;
            }
            else if(tracks > 3 && tracks < 8)
            {
                CallingForm.Group.Loan -= 40 * tracks;
                expenses += 40 * tracks * -1;
            }
            else
            {
                CallingForm.Group.Loan -= 35 * tracks;
                expenses += 35 * tracks * -1;
            }

            //Calculate profit
            int profit = 0;
            CallingForm.Group.Loan += Convert.ToInt32(AllModifiers * 100);            
            CallingForm.Group.Loan += CallingForm.Group.HardcoreFans * 15;
            CallingForm.Group.Loan += CallingForm.Group.CasualFans;
            profit = Convert.ToInt32(AllModifiers * 100) + CallingForm.Group.HardcoreFans * 15 + CallingForm.Group.CasualFans;

            //Special Event Chances
            string specialEvents = "";
                        
            //Chance idol leaves group
            if(CallingForm.Group.Loan < 0 && GroupAge > 2 && Convert.ToDouble(rnd.Next(101)) / 100 >= 0.65)
            {
                newNumber = rnd.Next(0, CallingForm.Group.Size);
                if (CallingForm.Group.Size > 1 && !CallingForm.Group.hardMode)
                {
                    specialEvents += CallingForm.Group.AllIdols[newNumber].IdolName + " has left the group.\n";
                    CallingForm.Group.IdolLeaves(newNumber);
                }
                else if (CallingForm.Group.Size > 2 && CallingForm.Group.hardMode)
                {
                    specialEvents += CallingForm.Group.AllIdols[newNumber].IdolName + " and ";
                    CallingForm.Group.IdolLeaves(newNumber);

                    newNumber = rnd.Next(0, Group.Size - 1);
                    specialEvents += CallingForm.Group.AllIdols[newNumber].IdolName + " have left the group.\n";
                    CallingForm.Group.IdolLeaves(newNumber);
                }                
                else if((CallingForm.Group.Size == 1 && !CallingForm.Group.hardMode) || 
                       (CallingForm.Group.Size == 2 && CallingForm.Group.hardMode))
                {
                    gameOverForm f5 = new gameOverForm();
                    f5.CallingForm = this;
                    f5.Reason = "All members of your group have left because they're poor.";
                    f5.Show();
                    this.Hide();
                }
            }
            else if(CallingForm.Group.Loan < 0 && GroupAge > 1 && Convert.ToDouble(rnd.Next(101)) / 100 >= 0.85)
            {
                newNumber = rnd.Next(0, Group.Size);
                if (CallingForm.Group.Size > 1 && !CallingForm.Group.hardMode)
                {
                    specialEvents += CallingForm.Group.AllIdols[newNumber].IdolName + " has left the group.\n";
                    CallingForm.Group.IdolLeaves(newNumber);
                }
                else if (CallingForm.Group.Size > 2 && CallingForm.Group.hardMode)
                {
                    specialEvents += CallingForm.Group.AllIdols[newNumber].IdolName + " and ";
                    CallingForm.Group.IdolLeaves(newNumber);

                    newNumber = rnd.Next(0, Group.Size - 1);
                    specialEvents += CallingForm.Group.AllIdols[newNumber].IdolName + " have left the group.\n";
                    CallingForm.Group.IdolLeaves(newNumber);
                }
                else if ((CallingForm.Group.Size == 1 && !CallingForm.Group.hardMode) || 
                        (CallingForm.Group.Size == 2 && CallingForm.Group.hardMode))
                {
                    gameOverForm f5 = new gameOverForm();
                    f5.CallingForm = this;
                    f5.Reason = "All members of your group have left because they're poor.";
                    f5.Show();
                    this.Hide();
                }
            }

            //Chance winning music program
            if(CallingForm.Group.HardcoreFans + CallingForm.Group.CasualFans > 15000 && 
               CallingForm.Group.HardcoreFans + CallingForm.Group.CasualFans < 40000)
            {
                if (Convert.ToDouble(rnd.Next(101)) / 100 < 0.30)
                {
                    CallingForm.Group.Loan += 2500;
                    profit += 2500;
                    specialEvents += "Your Group has won at a music program thanks to the fans!\n";
                }
            }
            else if (CallingForm.Group.HardcoreFans + CallingForm.Group.CasualFans > 40000 && 
                     CallingForm.Group.HardcoreFans + CallingForm.Group.CasualFans < 70000)
            {
                if (Convert.ToDouble(rnd.Next(101)) / 100 < 0.75)
                {
                    CallingForm.Group.Loan += 2500;
                    profit += 2500;
                    specialEvents += "Your Group has won at a music program thanks to the fans!\n";
                }
            }
            else if (CallingForm.Group.HardcoreFans + CallingForm.Group.CasualFans > 100000)
            {
                if (Convert.ToDouble(rnd.Next(101)) / 100 < 0.98)
                {
                    CallingForm.Group.Loan += 2500;
                    profit += 2500;
                    specialEvents += "Your Group has won at a music program thanks to the fans!\n";
                }
            }
            else
            {
                if (Convert.ToDouble(rnd.Next(101)) / 100 < 0.04)
                {
                    CallingForm.Group.Loan += 2500;
                    profit += 2500;
                    specialEvents += "Your Group has won at a music program thanks to the fans!\n";
                }
            }

            //CF Chance
            for(int i = 0; i < CallingForm.Group.Size; i++)
            {
                if(CallingForm.Group.AllIdols[i].Charisma > 15 && CallingForm.Group.AllIdols[i].Visual > 5 || 
                   CallingForm.Group.AllIdols[i].Visual > 20)
                {
                    if(Convert.ToDouble(rnd.Next(101))/100 >= 0.75)
                    {
                        CallingForm.Group.Loan += 500;
                        profit += 500;
                        specialEvents += CallingForm.Group.AllIdols[i].IdolName + " Has Landed A " + Cfs[rnd.Next(0, 8)] + " CF!\n";
                    }
                }
                else if(CallingForm.Group.AllIdols[i].Charisma > 25 && CallingForm.Group.AllIdols[i].Visual > 5 || 
                        CallingForm.Group.AllIdols[i].Visual > 30)
                {
                    if (Convert.ToDouble(rnd.Next(101)) / 100 >= 0.50)
                    {
                        CallingForm.Group.Loan += 1000;
                        profit += 1000;
                        specialEvents += CallingForm.Group.AllIdols[i].IdolName + " Has Landed A " + Cfs[rnd.Next(0, 8)] + " CF!\n";
                    }
                }
                else if (CallingForm.Group.AllIdols[i].Charisma > 30 && CallingForm.Group.AllIdols[i].Visual > 10 || 
                         CallingForm.Group.AllIdols[i].Visual > 45)
                {
                    if (Convert.ToDouble(rnd.Next(101)) / 100 >= 0.25)
                    {
                        CallingForm.Group.Loan += 2500;
                        profit += 2500;
                        specialEvents += CallingForm.Group.AllIdols[i].IdolName + " Has Landed A " + Cfs[rnd.Next(0, 8)] + " CF!\n";
                    }
                }
            }
            this.GroupCFchance = CallingForm.Group.GroupCFchance();
            
            if(this.GroupCFchance >= 0.80)
            {
                CallingForm.Group.Loan += 2000*CallingForm.Group.Size;
                profit += 2000 * CallingForm.Group.Size;
                specialEvents += CallingForm.Group.Name + " Has Landed A " + Cfs[rnd.Next(0, 7)] + " CF!\n";
            }
            else if (this.GroupCFchance >= 0.60)
            {
                CallingForm.Group.Loan += 1000 * CallingForm.Group.Size;
                profit += 1000 * CallingForm.Group.Size;
                specialEvents += CallingForm.Group.Name + " Has Landed A " + Cfs[rnd.Next(0, 7)] + " CF!\n";
            }
            else if (this.GroupCFchance >= 0.25)
            {
                CallingForm.Group.Loan += 300 * CallingForm.Group.Size;
                profit += 300 * CallingForm.Group.Size;
                specialEvents += CallingForm.Group.Name + " Has Landed A " + Cfs[rnd.Next(0, 7)] + " CF!\n";
            }            

            //Chance group disbands after 7 years
            if (GroupAge > 7)
            {
                if (Convert.ToDouble(rnd.Next(101)) / 100 < 0.50)
                {
                    gameOverForm f5 = new gameOverForm();
                    f5.CallingForm = this;
                    f5.Reason = "Your group has spent a long time together and decided to disband";
                    f5.Show();
                    this.Hide();
                }
            }

            //Add days for promotion time
            if (CallingForm.Group.currentDate.Year != CallingForm.Group.currentDate.AddDays(28).Year)
                CallingForm.Group.IdolsBirthday();
            CallingForm.Group.currentDate = CallingForm.Group.currentDate.AddDays(28);

            //Set Labels (Display Info)
            label3.Text = Fatigues;

            label5.Text = "Expenses: $" + expenses;

            label6.Text = "Profit: $" + profit;

            label4.Text = "Balance: $" + CallingForm.Group.Loan.ToString();

            label7.Text = "Casual Fans: " + CallingForm.Group.CasualFans;
            label8.Text = "Hardcore Fans: " + CallingForm.Group.HardcoreFans;

            label9.Text = "Current Date: " + CallingForm.Group.currentDate.Date.ToShortDateString();

            label11.Text = specialEvents;
        }
        
        //Show all comebacks
        private void button2_Click(object sender, EventArgs e)
        {
            string allComebacks = "";

            for (int i = 0; i < CallingForm.Group.Comebacks; i++)
            {
                allComebacks += "Comeback #" + (i + 1) + "\n" + CallingForm.Group.ComebackInfos[i].PrintInfo() + "\n";
            }

            Form f1 = new Form();
            f1.Size = new Size(250, 500);
            f1.BackColor = this.BackColor;
            FlowLayoutPanel p1 = new FlowLayoutPanel();
            Label l1 = new Label();
            l1.Text = allComebacks;
            l1.Font = new Font(l1.Font.Name, 10, l1.Font.Style, l1.Font.Unit);
            l1.AutoSize = true;
            p1.Controls.Add(l1);
            p1.AutoScroll = true;
            p1.Dock = DockStyle.Fill;
            f1.Controls.Add(p1);
            f1.StartPosition = FormStartPosition.CenterScreen;
            f1.ShowIcon = false;
            f1.Show();
        }

        //bring up browser to play video if enabled by user
        public void playComebackVideo(string groupName, string memberName)
        {
            string url = "";
            string htmlCode = "";
            
            url = "https://youtube.com/results?search_query=" + groupName + "+" + memberName + "+fancam";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if(response.StatusCode == HttpStatusCode.OK)
            {
                Stream recieveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(recieveStream);
                else
                    readStream = new StreamReader(recieveStream, Encoding.GetEncoding(response.CharacterSet));

               htmlCode = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            int Start;
            if (htmlCode.Contains("href=\"/watch?v=") && htmlCode.Contains("\" class="))
            {
                Start = htmlCode.IndexOf("href=\"/watch?v=", 0) + 15;                
                url = "https://youtube.com/watch?v=" + htmlCode.Substring(Start, 11);
                //Debug.WriteLine(url);
            }
            
            System.Diagnostics.Process.Start(url);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {            
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = CallingForm.Group.Name + "_IMS";
            saveFileDialog1.DefaultExt = "sav";
            saveFileDialog1.InitialDirectory = Program.resourcesPath;
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.WriteToBinaryFile(saveFileDialog1.FileName, CallingForm.Group);
            }
        }
    }
}
