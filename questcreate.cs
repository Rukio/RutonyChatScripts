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
                RutonyBot.BotSay(site, username + " а вопрос где? Kappa");
                return;
            }

            RutonyBotFunctions.FileAddString(filename, arrString[1]);
            RutonyBotFunctions.FileAddString(ascers, "");

			RutonyBot.BotSay(site, "Вопрос стримера: " + arrString[1]);
            RutonyBot.BotSay(site, "Отвечайте на него командой !ответ ");
            


        }
    }
}