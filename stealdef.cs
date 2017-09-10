using System.IO;
using System;
using System.Collections.Generic;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			string filename = ProgramProps.dir_scripts + @"\def_" + username + ".txt";

			if (File.Exists(filename))
			{
                RutonyBot.BotSay(site, username+ " вы уже защищаетесь!!");
				return;
			}

			RutonyBotFunctions.FileAddString(filename, string.Format("{0}", username));
			
			RutonyBot.BotSay(site, username + " вы теперь защищены от воров(почти) Kappa");
			
			
		}	
    }
}