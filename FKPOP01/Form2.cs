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
using System.IO;
using System.Diagnostics;

namespace FKPOP01
{
    public partial class settingsForm : Form
    {


        public settingsForm()
        {
            InitializeComponent();

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uploadButton_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap img = new Bitmap(openFileDialog1.FileName);

                string filePath = openFileDialog1.FileName.ToString();
                string dest = Program.resourcesPath;

                dest = Path.Combine(dest, comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + Path.GetExtension(filePath));

                int size = Math.Min(img.Width, img.Height);
                var sourceRect = new Rectangle(0, 0, size, size);
                var cropped = new Bitmap(size, size);
                var g = Graphics.FromImage(cropped);
                g.DrawImage(img, 0, 0, sourceRect, GraphicsUnit.Pixel);

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            newForm frm = new newForm();
            frm.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    string query = "Select IdolName, Id From Idols Where IdolGroup = '" + (comboBox1.SelectedValue) + "'";
                    SqlDataAdapter da = new SqlDataAdapter(query, connection);
                    connection.Open();

                    DataSet ds = new DataSet();
                    da.Fill(ds, "Idols");

                    comboBox2.DisplayMember = "IdolName";
                    comboBox2.ValueMember = "Id";
                    comboBox2.DataSource = ds.Tables["Idols"];

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            string dest = Program.resourcesPath;
            dest = Path.Combine(dest,
                                comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + ".jpg");

            if (File.Exists(dest))
            {
                Bitmap bmp = new Bitmap(dest);
                pictureBox1.Image = bmp;
            }
            else
            {
                dest = Path.Combine(Program.resourcesPath,
                                    comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + ".jpeg");

                if (File.Exists(dest))
                {
                    Bitmap bmp = new Bitmap(dest);
                    pictureBox1.Image = bmp;
                }
                else
                    pictureBox1.Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));
            }

        }

        private void settingsForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    string query = "Select GroupName, Id From Groups";
                    SqlDataAdapter da = new SqlDataAdapter(query, connection);
                    connection.Open();

                    DataSet ds = new DataSet();
                    da.Fill(ds, "Groups");

                    comboBox1.DisplayMember = "GroupName";
                    comboBox1.ValueMember = "Id";
                    comboBox1.DataSource = ds.Tables["Groups"];

                    query = "Select IdolName, Id From Idols Where IdolGroup = '" + ds.Tables["Groups"].Rows[0]["Id"] + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(query, connection);

                    DataSet ds2 = new DataSet();
                    da2.Fill(ds, "Idols");

                    comboBox2.DisplayMember = "IdolName";
                    comboBox2.ValueMember = "Id";
                    comboBox2.DataSource = ds.Tables["Idols"];

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            string dest = Program.resourcesPath;
            dest = Path.Combine(dest, comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + ".jpg");

            if (File.Exists(dest))
            {
                Bitmap bmp = new Bitmap(dest);
                pictureBox1.Image = bmp;
            }
            else
            {
                dest = Path.Combine(Program.resourcesPath, comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + ".jpeg");
                if (File.Exists(dest))
                {
                    Bitmap bmp = new Bitmap(dest);
                    pictureBox1.Image = bmp;
                }
                else
                    pictureBox1.Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    string query = "Select * From Idols Where IdolName = '" + comboBox2.GetItemText(comboBox2.SelectedItem) + 
                                    "' And IdolGroup = '" + comboBox1.SelectedValue + "'";
                    //Console.WriteLine(query);
                    SqlDataAdapter da = new SqlDataAdapter(query, connection);
                    connection.Open();

                    DataTable idolprop = new DataTable();
                    da.Fill(idolprop);

                    BindingSource bs = new BindingSource();

                    bs.DataSource = idolprop;

                    DataTable swap = new DataTable();

                    for (int i = 0; i <= idolprop.Rows.Count; i++)
                    {
                        swap.Columns.Add(Convert.ToString(i));
                    }
                    for (int col = 0; col < idolprop.Columns.Count; col++)
                    {
                        var r = swap.NewRow();
                        r[0] = idolprop.Columns[col].ToString();
                        for (int j = 1; j <= idolprop.Rows.Count; j++)
                            r[j] = idolprop.Rows[j - 1][col];

                        swap.Rows.Add(r);
                    }

                    dataGridView1.DataSource = swap;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            string dest = Program.resourcesPath;
            dest = Path.Combine(dest, comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + ".jpg");
            if (File.Exists(dest))
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
                Bitmap bmp = new Bitmap(dest);
                pictureBox1.Image = bmp;
            }
            else
            {
                dest = Path.Combine(Program.resourcesPath, comboBox2.GetItemText(comboBox2.SelectedItem).Trim(' ') + (comboBox1.SelectedValue) + ".jpeg");
                if (File.Exists(dest))
                {
                    Bitmap bmp = new Bitmap(dest);
                    pictureBox1.Image = bmp;
                }
                else
                    pictureBox1.Load(Path.Combine(Program.resourcesPath, "noimage.jpg"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var confrimationBox = MessageBox.Show("Are you sure you want to delete this group?", "Confirm Deletion", MessageBoxButtons.YesNo);

            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    if (confrimationBox == DialogResult.Yes)
                    {
                        string query = "Select Id From Groups Where GroupName = '" + comboBox1.GetItemText(comboBox1.SelectedItem) + "'";
                        
                        SqlCommand cmd = new SqlCommand(query, connection);                        
                        connection.Open();

                        Int32 GroupId = (Int32)cmd.ExecuteScalar();

                        query = "Delete From Idols Where IdolGroup = '" + GroupId + "'";

                        cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();

                        query = "Delete From Groups Where GroupName = '" + comboBox1.GetItemText(comboBox1.SelectedItem) + "'";

                        cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();

                        connection.Close();

                        MessageBox.Show("Deletion Complete");                   

                        this.Close();
                    }
                    else
                        return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }          
        }
    }

}
