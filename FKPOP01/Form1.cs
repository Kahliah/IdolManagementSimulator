using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace FKPOP01
{
    public partial class mainForm : Form
    {        
        public Groups Group { get; private set; }

        public mainForm()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void nextButton_Click(object sender, EventArgs e)
        {            
            if (groupName.Text == "")
            {
                MessageBox.Show("Group Name cannot be blank");
                return;
            }

            this.Group = new Groups(groupName.Text, conceptBox.Text, loanAmt.Value, groupSize.Value, hardBox.Checked, dateTimePicker1.Value.Date, checkBox1.Checked);
            this.Group.currentDate = dateTimePicker1.Value.Date;
            Group.GroupInfo();
            this.Hide();
            idolPicksForm f2 = new idolPicksForm();
            f2.CallingForm = this;
            f2.Show();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            settingsForm frm = new settingsForm();                                    
            frm.Show();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            conceptBox.SelectedIndex = 0;
            groupName.Text = "";            
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Program.resourcesPath;
            openFileDialog1.DefaultExt = "sav";
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Groups group = Program.ReadFromBinaryFile<Groups>(openFileDialog1.FileName);
                this.Group = group;
                //this.Group.AllIdols = group.AllIdols;
                //this.Group.ComebackInfos = group.ComebackInfos;

                this.Hide();

                idolPicksForm f2 = new idolPicksForm();
                f2.CallingForm = this;
                f2.Group = this.Group;
                f2.LoadedGame = true;
                
                ResultsForm f3 = new ResultsForm();
                f3.CallingForm = f2;
                f3.Show();
                f3.Hide();
            }
        }

        private void groupName_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Enter group name between 1-16 characters", groupName);
        }

        private void hardBox_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Hard mode enables things such as members leave quicker", hardBox);
        }

        private void checkBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Opens your browser to random members fancam on YouTube", checkBox1);
        }

        private void loadButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Load game from save file", loadButton);
        }
    }
}
