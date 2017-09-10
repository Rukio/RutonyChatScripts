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
			string file = ProgramProps.dir_scripts + @"\dragonwarriors.json";
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
			AddWarrior(username, (str+2)*krit, site);
			
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
				RutonyBot.BotSay(site, string.Format("{0} добивает дракона! Всем участники получают кредиты из его сокровищницы!", username));
				RankControl.ChatterRank cr_lasthit = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());
				cr_lasthit.CreditsQty += 50;
				Warriors players = GetListWarriors();
				foreach (Warrior player in players.ListWarriors)
        		{
					

					RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == player.name);
					cr_win.CreditsQty += Convert.ToInt32(player.damage);
					//RutonyBot.BotSay(site, player.name + " получил " + player.amount + " кредитов!");
				}

				
				
				try {
                        File.Delete(file);
				} catch { }

				try {
                        File.Delete(filename);
                } catch { }
				
			}
			
			
			
			
		}
		public Warriors GetListWarriors()
		{
			string file = ProgramProps.dir_scripts + @"\dragonwarriors.json";

			Warriors players = new Warriors();

            if (File.Exists(file))
			{
                string[] filetexts = File.ReadAllLines(file);

			    players = JsonConvert.DeserializeObject<Warriors>(filetexts[0]);
				
			}

			return players;
		}

		public void AddWarrior(string username, int vklad, string site)
		{
			string file = ProgramProps.dir_scripts + @"\dragonwarriors.json";
			Warriors players = GetListWarriors();

			Warrior thiswarrior = players.ListWarriors.Find(r => r.name == username.Trim().ToLower());

			if (thiswarrior == null) {
                
                players.ListWarriors.Add(new Warrior() {name=username.Trim().ToLower(), damage = vklad});
                thiswarrior = players.ListWarriors.Find(r => r.name == username.Trim().ToLower());
				//RutonyBot.BotSay(site, username + " спасибо за вклад, ждем других участников!");

				try {
                        File.Delete(file);
             	 } catch { }

            	string serialized = JsonConvert.SerializeObject(players);
            	RutonyBotFunctions.FileAddString(file, serialized);


                
            }
			else
			{

				thiswarrior.damage += vklad;

				try {
                        File.Delete(file);
             	 } catch { }

            	string serialized = JsonConvert.SerializeObject(players);
            	RutonyBotFunctions.FileAddString(file, serialized);

			}

		}	
    }
	public class Warriors
    {
        public List<Warrior> ListWarriors = new List<Warrior>();
    }

    public class Warrior
	{

		public string name {get; set;}
        public int damage {get; set;}

	}
}