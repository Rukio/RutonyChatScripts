using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			string filename = ProgramProps.dir_scripts + @"\dragon.txt";
			string dragonusers = ProgramProps.dir_scripts + @"\dragonusers.txt";
			
			if (File.Exists(filename))
			{
                RutonyBot.BotSay(site, "Дракон уже создан!");
				return;
			}

			string[] arrString = param.Split(' ');
			
            
            if (arrString.Length != 3) {
                RutonyBot.BotSay(site, "Не хватает параметров!");
                return;
            }
			
			Random rnd = new Random();
            int randomDragon = rnd.Next(1, 4);

			int hp = int.Parse(arrString[randomDragon-1]);

			RutonyBotFunctions.FileAddString(filename, string.Format("{0}", hp));
			
			string[] dragons = {"Визариона", "Рейгаля", "Дрогона"};


			RutonyBot.BotSay(site, string.Format("Вы нашли логово {0}! Бейте его всей толпой!", dragons[randomDragon-1]));
			
			new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    Thread.Sleep(3600000); //Время дракона в милисекундах

					try {
                        File.Delete(dragonusers);
					} catch { }

					try {
                        File.Delete(filename);
                	} catch { }

					RutonyBot.BotSay(site, "Вы не смогли убить дракона и он улетел!");
			}).Start();
        	return;

			
		}	
    }
}