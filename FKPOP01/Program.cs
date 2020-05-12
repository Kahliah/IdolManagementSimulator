using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.Common;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace FKPOP01
{
    static class Program
    {
        public static string provider = ConfigurationManager.AppSettings["provider"];
        public static string connectionString = ConfigurationManager.AppSettings["connectionString"];
        public static DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
        //public static string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public static string projectPath = Directory.GetCurrentDirectory();
        public static string resourcesPath = Path.Combine(projectPath, "Resources");
    
        //Console.ReadLine();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainForm());
        }

        public static int GetTableCount(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))
            {
                try
                {
                    string query = String.Format("SELECT COUNT(*) from {0}", tableName);                    
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    int count = (int)cmd.ExecuteScalar();
                    connection.Close();
                    return count;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return 0;
                }
            }
        }

        public static int GetWordCount(string sentence)
        {
            int wordCount = 0;
            int idx = 0;

            while (idx < sentence.Length && char.IsWhiteSpace(sentence[idx]))
                idx++;

            while(idx < sentence.Length)
            {
                while (idx < sentence.Length && !char.IsWhiteSpace(sentence[idx]))
                    idx++;

                wordCount++;

                while (idx < sentence.Length && char.IsWhiteSpace(sentence[idx]))
                    idx++;
            }

            return wordCount;
        }

        public static void WriteToBinaryFile<Groups>(string filePath, Groups objectToWrite, bool append = false)
        {
            using(Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static Groups ReadFromBinaryFile<Groups>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (Groups)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
