using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FKPOP01
{
    [Serializable()]
    public class Idols
    {
        public string IdolName { get; set; }
        public string SpecialName { get; set; }
        public string IdolGroup { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }

        public int Id { get; set; }
        public int GroupId;
        public int Vocal { get; set; }
        public int Dance { get; set; }
        public int Visual { get; set; }
        public int Charisma { get; set; }
        public int Comedy { get; set; }
        public int Acting { get; set; }
        public int SpecialBonus { get; set; }
        public int Variety { get; set; }
        public int Musical { get; set; }
        public int Rap { get; set; }

        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int Age { get; set; }

        public bool Leader = false;
        public uint Fatigue = 0;
        public double Modifier = 0;
        public string Status = "Healthy";

        public Idols(string name, int groupId, int vocal, int dance, int visual, int charisma, int comedy, 
                     int acting, decimal height, decimal weight, int age, int specialBonus, int variety, 
                     int musical, string language, string specialName, int rap, int id, string gender)
        {
            IdolName = name;
            GroupId = groupId;
            Vocal = vocal;
            Dance = dance;
            Visual = visual;
            Charisma = charisma;
            Comedy = comedy;
            Acting = acting;
            Height = height;
            Weight = weight;
            Age = age;
            SpecialBonus = specialBonus;
            Variety = variety;
            Musical = musical;
            Language = language;
            SpecialName = specialName;
            Rap = rap;
            Id = id;
            Gender = gender;
        }

        public Idols(int id, int age, string gender, decimal height, decimal weight, string idolgroup, string idolname, string specialname)
        {
            Random rnd = new Random();
            Vocal = rnd.Next(11);
            Dance = rnd.Next(11);
            Visual = rnd.Next(11);
            Charisma = rnd.Next(11);
            Comedy = rnd.Next(11);
            Acting = rnd.Next(11);
            SpecialBonus = rnd.Next(11);
            Variety = rnd.Next(11);
            Musical = rnd.Next(11);
            Rap = rnd.Next(11);

            IdolName = idolname;            
            Height = height;
            Weight = weight;
            Age = age;            
            SpecialName = specialname;            
            Id = id;
            Gender = gender;
            IdolGroup = idolgroup;
        }

        public string PrintInfo()
        {
            string infoList = "";
            infoList += "Name: " + this.IdolName + "\n";
            foreach (PropertyInfo info in typeof(Idols).GetProperties())
            {
                if(info.Name != "Id" && info.Name != "GroupId" && info.Name != "IdolName" && info.Name != "Gender")
                    infoList += String.Format("\n" + info.Name + ": " + info.GetValue(this));
            }
            infoList += "\n\nStatus: " + this.Status;
            return infoList;
        }        

        public string PrintFatigueAndStatus()
        {
            return this.IdolName + "'s Fatigue: " + this.Fatigue + ", Status: " + this.Status + "\n";
        }
        
        public void UpdateModifier()
        {
            int tempMod;
            tempMod = Vocal + Dance + Visual + Charisma + Comedy + Acting + SpecialBonus + Variety + Musical + Rap;
            tempMod = tempMod - (Age / 2);
            if (Height > 170)
                tempMod += 1;
            if (Age > 27)
                tempMod -= 1;
            this.Modifier = Convert.ToDouble(tempMod)/100;
        }

        public void UpdateFatigue(Random rnd)
        {            
            decimal rndNumber;
            rndNumber = (rnd.Next(101)) / 100;
            this.Fatigue += Convert.ToUInt32(((Age/2)+(Weight/3)*rndNumber));
        }

        public void IncreaseStat(string stat, Random rnd)
        {
            this.Fatigue += 1;
            if (this.Fatigue >= 75)
                this.Status = "Injured";
            else
                this.UpdateStatus(rnd);

            if(this.Status == "Healthy")
            {                
                switch (stat)
                {
                    case "Vocal":
                        this.Vocal += 1;
                        break;
                    case "Dance":
                        this.Dance += 1;
                        break;
                    case "Visual":
                        this.Visual += 1;
                        break;
                    case "Charisma":
                        this.Charisma += 1;
                        break;
                    case "Comedy":
                        this.Comedy += 1;
                        break;
                    case "Acting":
                        this.Acting += 1;
                        break;
                    case "Variety":
                        this.Variety += 1;
                        break;
                    case "Musical":
                        this.Musical += 1;
                        break;
                    case "Rap":
                        this.Rap += 1;
                        break;
                }
            }
            
        }

        public void UpdateStatus(Random rnd)
        {            
            int x = 0;
            string status = "";           
            
            if (this.Fatigue >= 50 && this.Fatigue < 75)
            {
                x = rnd.Next(3);
            }
            else if (this.Fatigue >= 75 && this.Fatigue < 90)
            {
                x = rnd.Next(1,4);
            }
            else if (this.Fatigue >= 90)
            {
                x = rnd.Next(2,5);
            }
            else
                status = "Healthy";

            switch (x)
            {
                case 0:
                    status = "Healthy";
                    break;
                case 1:
                case 2:
                    status = "Injured";
                    break;
                case 3:
                    status = "Fainted";
                    break;
                case 4:
                    status = "Cannot work";
                    break;
            }

            this.Status = status;
        }
    }
}
