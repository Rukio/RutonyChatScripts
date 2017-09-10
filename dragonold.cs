using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

//by pasvitas. twitch.tv/pasvitas

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			string filename = ProgramProps.dir_scripts + @"\dragon.txt";
			string dragonusers = ProgramProps.dir_scripts + @"\dragonusers.txt";

			string textsfile = ProgramProps.dir_scripts + @"\texts.json";

            string[] filetexts = File.ReadAllLines(textsfile);


		 	CreditName names = JsonConvert.DeserializeObject<CreditName>(filetexts[0]);
			

			//RutonyBot.BotSay(site, names.mnpads[0] + " " + names.edpads[0]);


			if (!File.Exists(filename))
			{
                RutonyBot.BotSay(site, "Дракон еще не появился! Попросите администратора об этом!");
				return;
			}

			string[] hp = File.ReadAllLines(filename);
			int currenthp = Convert.ToInt32(hp[0]);
			RankControl.ChatterRank strlist = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());

			if (strlist == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                return;
            }

			int str = strlist.RankOrder;
			
			Random rnd = new Random();
			int rndstr = rnd.Next(1, 11);
			int krit = 1;
			
			switch (rndstr) 
			{	
				case 1: 
				
					currenthp -= (str+2)*3;
					krit = 3;
					break;
					
				case 2: 
				
					RutonyBot.BotSay(site, string.Format("{0} промахивается по дракону!", username));
					break;
				
				case 3:
				
					RutonyBot.BotSay(site, string.Format("Дракон блокирует удар {0}!", username));
					//RutonyBot.BotSay(site, "/timeout " + username + " 30");
					break;
				case 4:
					currenthp += 5;
					RutonyBot.BotSay(site, string.Format("Дракон уклоняется от удара {0} и восставнавливает силы!(+5 хп)!", username));
					try {
                        File.Delete(filename);
					} catch { }
					RutonyBotFunctions.FileAddString(filename, string.Format("{0}",currenthp));
					break;
				default:
					currenthp -= (str+2);
					break;
			}
			
			if (rndstr > 1 && rndstr <=4)
			{
				
				return;
			}
			
			RutonyBotFunctions.FileAddString(dragonusers, string.Format("{0} {1}", username, (str+2)*krit));
			if (currenthp > 0)
			{
				RutonyBot.BotSay(site, string.Format("{0} бьет дракона на {1} урона! У дракона осталось {2} здоровья! Поднажмите!", username, (str+2)*krit, currenthp));
				try {
                        File.Delete(filename);
                } catch { }
				RutonyBotFunctions.FileAddString(filename, string.Format("{0}",currenthp));
				
			}
			else
			{
				RutonyBot.BotSay(site, string.Format("{0} добивает дракона! Всем участники получают " + names.mnpads[0] + " из его сокровищницы!", username));
				string[] listUsers = File.ReadAllLines(dragonusers);
				int countPlayers = RutonyBotFunctions.FileLength(dragonusers);
				for (int j = 0; j < countPlayers; j++)
				{
					
					string[] arrString = listUsers[j].Split(' ');
							
					RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());
					cr_win.CreditsQty += Convert.ToInt32(int.Parse(arrString[1])*0.5);
					

				}
				
				try {
                        File.Delete(dragonusers);
				} catch { }

				try {
                        File.Delete(filename);
                } catch { }
				
			}
			
			
			
			
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