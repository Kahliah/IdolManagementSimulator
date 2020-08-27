using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;

namespace FKPOP01
{
    public partial class settingsForm : Form
    {
        public string[] imageSelection;
        public int imageSelectionIndex = 0;

        public settingsForm()
        {
            InitializeComponent();

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uploadButton_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Idol Name and Group cannot be blank");
                return;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap img = new Bitmap(openFileDialog1.FileName);

                string filePath = openFileDialog1.FileName.ToString();
                string dest = Program.resourcesPath;
                string idolName = textBox1.Text.Trim(' ').Replace(" ", "_") + "!" +
                                    textBox2.Text.Trim(' ').Replace(" ", "_");

                dest = Path.Combine(dest, idolName + Path.GetExtension(filePath));

                int size = Math.Min(img.Width, img.Height);
                var sourceRect = new Rectangle(0, 0, size, size);
                var cropped = new Bitmap(size, size);
                var g = Graphics.FromImage(cropped);
                g.DrawImage(img, 0, 0, sourceRect, GraphicsUnit.Pixel);

                label1.Text = "Current Idol Image: " + idolName + Path.GetExtension(filePath);

                try
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        pictureBox1.Image = null;
                    }

                    if (System.IO.File.Exists(dest))
                        System.IO.File.Delete(dest);

                    //Debug.WriteLine("dest is: " + dest);
                    cropped.Save(dest);
                    cropped.Dispose();
                    img.Dispose();
                    pictureBox1.Load(dest);
                    imageSelection = Directory.GetFiles(Program.resourcesPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }        

        private void settingsForm_Load(object sender, EventArgs e)
        {            
            string dest = Program.resourcesPath;
            var dir = new DirectoryInfo(dest);
            string firstFileName = "";

            if (Directory.Exists(dest))
            {
                imageSelection = Directory.GetFiles(Program.resourcesPath);
                if (imageSelection.Length != 0)
                { 
                    firstFileName = dir.GetFiles().Select(fi => fi.Name).FirstOrDefault();
                    dest = Path.Combine(dest, firstFileName);
                }
            }
            else
            {
                Directory.CreateDirectory("Resources");
            }
            
            if (File.Exists(dest))
            {
                Bitmap bmp = new Bitmap(dest);
                pictureBox1.Image = bmp;
                label1.Text = "First Idol Image: " + firstFileName;
            }
            else if (File.Exists(Path.Combine(Program.resourcesPath, "noimage.jpg")))
            {
                pictureBox1.Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));
                label1.Text = "No Idol Images Available";
            }
            else
            {
                pictureBox1.Hide();
                label1.Text = "No Images Available";
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (imageSelection.Length != 0)
            {
                if (imageSelectionIndex < imageSelection.Length - 1)
                    imageSelectionIndex++;
                pictureBox1.Load(imageSelection[imageSelectionIndex]);
                string imageName = Path.GetFileName(imageSelection[imageSelectionIndex]);
                label1.Text = "#" + (imageSelectionIndex + 1) + "  Idol Image: " + imageName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (imageSelection.Length != 0)
            {
                if (imageSelectionIndex > 0)
                    imageSelectionIndex--;
                pictureBox1.Load(imageSelection[imageSelectionIndex]);
                string imageName = Path.GetFileName(imageSelection[imageSelectionIndex]);
                label1.Text = "#" + (imageSelectionIndex + 1) + "  Idol Image: " + imageName;
            }
        }
    }

}
