using System.IO;
using System;
using System.Collections.Generic;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {

            string filename = ProgramProps.dir_scripts + @"\question.txt";
            string ascers = ProgramProps.dir_scripts + @"\questionascers.txt";

           
			
            string[] arrString = text.Split(':');

            if (arrString.Length != 2) {
                RutonyBot.BotSay(site, username + " а ответ где? Kappa");
                return;
            }
            string[] question = File.ReadAllLines(filename); 
			RutonyBot.BotSay(site, "Вопрос стримера: " + question[0]);
            RutonyBot.BotSay(site, "Ответ: " + arrString[1]);

            string[] listUsers = File.ReadAllLines(ascers);
            int countPlayers = RutonyBotFunctions.FileLength(ascers) - 1;
			for (int j = 1; j <= countPlayers; j++)
			{
					
				string[] arrStringFile = listUsers[j].Split(' ');
                if (arrStringFile[1] == arrString[1])
                {
                     //RutonyBot.BotSay(site, arrStringFile[0] + " совпадение!");
                    RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == arrStringFile[0].Trim().ToLower());
                    if (cr_win == null) {
                        RutonyBot.BotSay(site,
                            username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                        return;
                    }
                    else
                    {
                        cr_win.CreditsQty += Convert.ToInt32(int.Parse(param));
                    }
				    
                }

                RutonyBot.BotSay(site, arrStringFile[0] + " ответил: " + arrStringFile[1]);


			}

            try {
                    File.Delete(ascers);
			} catch { }

			try {
                    File.Delete(filename);
            } catch { }
            


        }
    }
}