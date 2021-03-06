using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			string filename = ProgramProps.dir_scripts + @"\dragon.txt";

			if (File.Exists(filename))
			{
                RutonyBot.BotSay(site, "Дракон уже создан!");
				return;
			}

			RutonyBotFunctions.FileAddString(filename, string.Format("{0}", int.Parse(param)));
			
			RutonyBot.BotSay(site, "Вы нашли логово дракона с его сокровищинцей! Бейте его всей толпой!");
			
			
		}	
    }
}