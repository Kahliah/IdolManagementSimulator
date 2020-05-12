using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FKPOP01
{
    [Serializable()]
    public class Groups
    {
        public string Name { get; set; }
        public string Concept { get; set; }
        public int Loan { get; set; }
        public int Size { get; set; }
        public bool hardMode;
        public bool playVideo;
        public DateTime Debut { get; set; }
        public Idols[] AllIdols;
        public int Comebacks = 0;
        public int HardcoreFans = 0;
        public int CasualFans = 0;
        public DateTime currentDate { get; set; }
        public List<Comeback> ComebackInfos = new List<Comeback>();

        public Groups(string name, string concept, decimal loan, decimal size, bool hard, DateTime debut, bool video)
        {
            Name = name;
            Concept = concept;
            Loan = -1*(Convert.ToInt32(loan));
            Size = Convert.ToInt32(size);
            hardMode = hard;
            Debut = debut;           
            AllIdols = new Idols[Size];
            playVideo = video;
        }

        public void GroupInfo()
        {
            Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", Name, Concept, Loan, Size, hardMode, Debut);
        }

        public string PrintGroupInfo()
        {
            string info = String.Format("Name: {0}\nLast Concept: {1}\nCurrent Money: ${2}\nHard Mode: {3}\nDebut Date: {4}", 
                                        Name, Concept, Loan, hardMode, Debut.ToShortDateString());
            return info;
        }

        public void IdolsBirthday()
        {
            for(int i = 0; i < this.Size; i++)
            {
                this.AllIdols[i].Age += 1;
            }
        }

        public void IdolsBirthday(int years)
        {
            for (int i = 0; i < this.Size; i++)
            {
                this.AllIdols[i].Age += years;
            }
        }

        public void FansComebackDate()
        {
            decimal Years = 0;

            if (this.Comebacks > 1)
            {
                Years = (this.ComebackInfos[this.Comebacks - 1].Date - this.ComebackInfos[this.Comebacks - 2].Date).Days / 365.25m;
                Debug.WriteLine("days btween: " + (this.ComebackInfos[this.Comebacks - 1].Date - this.ComebackInfos[this.Comebacks - 2].Date).Days);
            }

            if (Years >= 0.45m && Years < 0.90m)
            {
                this.CasualFans -= this.CasualFans / 4;
                this.HardcoreFans -= this.HardcoreFans / 8;
            }
            else if (Years >= 0.90m && Years < 1.5m)
            {
                this.CasualFans -= this.CasualFans / 3;
                this.HardcoreFans -= this.HardcoreFans / 6;
            }
            else if (Years >= 1.5m)
            {
                this.CasualFans -= this.CasualFans / 2;
                this.HardcoreFans -= this.HardcoreFans / 4;
            }
            
        }

        public double GroupCFchance()
        {
            double totalCharisma = 0;
            double totalVisual = 0;

            for (int i = 0; i < this.Size; i++)
            {
                totalCharisma += this.AllIdols[i].Charisma;
                totalVisual += this.AllIdols[i].Visual;
            }

            totalCharisma = totalCharisma / this.Size;
            totalVisual = totalCharisma / this.Size;

            return ((totalCharisma + totalVisual) / 2) / 100;
        }

        public void IdolLeaves(int IdolIdx)
        {
            Idols[] reformedGroup = new Idols[this.Size - 1];
            int j = 0;

            for(int i = 0; i < this.Size; i++)
            {
                if(i != IdolIdx)
                {
                    reformedGroup[j] = this.AllIdols[i];
                    j++;
                }
            }

            this.Size -= 1;
            this.AllIdols = null;
            this.AllIdols = new Idols[this.Size];
            this.AllIdols = reformedGroup;
        }
    }
}
