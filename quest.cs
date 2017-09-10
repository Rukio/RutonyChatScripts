using System.IO;
using System;
using System.Collections.Generic;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {

            string filename = ProgramProps.dir_scripts + @"\question.txt";
            string ascers = ProgramProps.dir_scripts + @"\questionascers.txt";

           
			
            string[] arrString = text.Split(' ');

            if (arrString.Length < 2) {
                string[] question = File.ReadAllLines(filename); 
                RutonyBot.BotSay(site, "Вопрос: " + question[0]);
                return;
            }

            string[] listUsers = File.ReadAllLines(ascers);
            int countPlayers = RutonyBotFunctions.FileLength(ascers);
            bool flag = false;
			for (int j = 0; j < countPlayers; j++)
			{
					
				string[] arrStringFile = listUsers[j].Split(' ');
							
				if (arrStringFile[0] == username)
                {
                    flag = true;
                }
					

			}

            if (flag == true)
            {
                RutonyBot.BotSay(site, username +  "вы уже отвечали!");
                return;
            }

            RutonyBotFunctions.FileAddString(ascers, username + " " + arrString[1].Trim().ToLower());
            RutonyBot.BotSay(site, username + "спасибо за ответ Kappa");

            
        }
    }
}