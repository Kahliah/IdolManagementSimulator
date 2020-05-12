using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FKPOP01
{
    [Serializable()]
    public class Comeback
    {
        public string TitleTrack { get; set; }
        public string AlbumName { get; set; }
        public string Concept { get; set; }
        public int Tracks { get; set; }
        public string Genre { get; set; }
        public DateTime Date;

        public Comeback(string titleTrack, string albumName, string concept, int tracks, string genre, DateTime date)
        {
            TitleTrack = titleTrack;
            AlbumName = albumName;
            Concept = concept;
            Tracks = tracks;
            Genre = genre;
            Date = date;
        }

        public string PrintInfo()
        {
            string infoList = "";
            foreach (PropertyInfo info in typeof(Comeback).GetProperties())
            {
                infoList += String.Format(info.Name + ": " + info.GetValue(this) + "\n");
            }
            infoList += "Date: " + Date.ToShortDateString() + "\n";
            return infoList;
        }
    }
}
