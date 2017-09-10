using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;


//by pasvitas twitch.tv/pasvitas

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			
            string filename = ProgramProps.dir_scripts + @"deposit_" + username + ".txt";
            string textsfile = ProgramProps.dir_scripts + @"\texts.json";

            string[] filetexts = File.ReadAllLines(textsfile);

			CreditName names = JsonConvert.DeserializeObject<CreditName>(filetexts[0]);

			int credit;

            RankControl.ChatterRank cr = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());

			if (cr == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                return;
            }

            string[] arrStringParam = param.Split(' ');
			

            if (File.Exists(filename))
			{
                RutonyBot.BotSay(site, username + " вы уже сделали вклад!");
				return;
			}

            
            if (arrStringParam.Length != 2) {
                RutonyBot.BotSay(site, "Не хватает параметров!");
                return;
            }
			
			if (!Int32.TryParse(text.Substring(text.IndexOf(" ") + 1), out credit)) {
                RutonyBot.BotSay(site, "Количество " + names.mnpads[1] + " должно быть больше 0!");
                return;
            }
            if (credit <= 0) {
                RutonyBot.BotSay(site, names.mnpads[1] + " должно быть больше 0!");
                return;
            }
            if (credit > int.Parse(arrStringParam[1])) {
                RutonyBot.BotSay(site, names.mnpads[1] + " должно быть не больше " + int.Parse(arrStringParam[1]) + "!");
                return;
            }
            if (cr.CreditsQty < credit) {
                RutonyBot.BotSay(site, string.Format("У вас всего {0} {1}!", cr.CreditsQty, names.mnpads[1]));
                return;
            }
			
			cr.CreditsQty -= credit;
	


                RutonyBot.BotSay(site, username + ", спасибо за вклад, жди процентов через " + int.Parse(arrStringParam[0]) + " секунд!");
                RutonyBotFunctions.FileAddString(filename, username);

                new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    Thread.Sleep(int.Parse(arrStringParam[0])*1000);
                    
                       
						
						/*Random winrnd = new Random();
						int winrandom = winrnd.Next(1, 5);
						
						double cofint = winrandom*0.2;
						
						cr.CreditsQty += Convert.ToInt32(credit*(1+cofint));*/

                        credit = Convert.ToInt32(credit*1.1);

                        cr.CreditsQty += credit;
						
						RutonyBot.BotSay(site, username + ", вы получаете " + credit + " " + names.mnpads[1] + " за вклад!");

                        try {
							File.Delete(filename);
						} catch { }
					
					}).Start();
                return;
			
        }

           

    }

    class CreditName
	{

		public string[] edpads {get; set;}
		public string[] mnpads {get; set;}

		public CreditName()
		{
			edpads = new string[6];
			mnpads = new string[6];
		}




	}
}