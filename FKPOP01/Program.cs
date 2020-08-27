using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.Common;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace FKPOP01
{
    static class Program
    {
        public static string projectPath = Directory.GetCurrentDirectory();
        public static string resourcesPath = Path.Combine(projectPath, "Resources");
        public static string apiPath = "https://utw3a4a6u5.execute-api.us-west-2.amazonaws.com/api_deploy_1";

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
            int count = 0;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(apiPath + "/table-count/" + tableName).Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Connection Error: " + response.StatusCode);
                    Environment.Exit(0);
                }

                dynamic result = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                count = result.Count;
            }
            return count;
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
