using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;


//by pasvitas twitch.tv/pasvitas

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {
			
			string file = ProgramProps.dir_scripts + @"\robbers.json";



			int credit;
			RankControl.ChatterRank cr = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());

			if (cr == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                return;
            }
			
			if (!Int32.TryParse(text.Substring(text.IndexOf(" ") + 1), out credit)) {
                RutonyBot.BotSay(site, "Количество кредитов должно быть больше 0!");
                return;
            }
            if (credit <= 0) {
                RutonyBot.BotSay(site, "Кредитов должно быть больше 0!");
                return;
            }
            if (cr.CreditsQty < credit) {
                RutonyBot.BotSay(site, string.Format("У вас всего {0} кредитов!", cr.CreditsQty));
                return;
            }

			cr.CreditsQty -= credit;
			
			

			if (!File.Exists(file))
			{
				AddRobber(username, credit, site);
                new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    Thread.Sleep(60000);

						int sum = 0;

                        Robbers players = GetListRobbers();
						foreach (Robber player in players.ListRobbers)
        				{
            				sum += player.amount;
        				}
			

						
						Random winrnd = new Random();
						int winrandom = winrnd.Next(1, 100);
						
						int sumrandom = (sum/10);
						if (sumrandom > 25) 
						{ 
							sumrandom = 25; 
						}
						
						if (winrandom+sumrandom > 80)
						{
							RutonyBot.BotSay(site, "Ограбление прошло успешно!");
								foreach (Robber player in players.ListRobbers)
        						{
									player.amount = player.amount*2;

									RankControl.ChatterRank cr_win = RankControl.ListChatters.Find(r => r.Nickname == player.name);
									cr_win.CreditsQty += player.amount;
									RutonyBot.BotSay(site, player.name + " получил " + player.amount + " кредитов!");
								}

						
						}
						else
						{
							RutonyBot.BotSay(site, "Ограбление не удалось, но грабителям удалось унести ноги");
						}
										
						try {
							File.Delete(file);
						} catch { }

					}).Start();
                return;
			}
			else
			{
				AddRobber(username, credit, site);
			}
        }

		

		public Robbers GetListRobbers()
		{
			string file = ProgramProps.dir_scripts + @"\robbers.json";

			Robbers players = new Robbers();

            if (File.Exists(file))
			{
                string[] filetexts = File.ReadAllLines(file);

			    players = JsonConvert.DeserializeObject<Robbers>(filetexts[0]);
				
			}

			return players;
		}

		public void AddRobber(string username, int vklad, string site)
		{
			string file = ProgramProps.dir_scripts + @"\robbers.json";
			Robbers players = GetListRobbers();

			Robber thisrobber = players.ListRobbers.Find(r => r.name == username.Trim().ToLower());

			if (thisrobber == null) {
                
                players.ListRobbers.Add(new Robber() {name=username.Trim().ToLower(), amount = vklad});
                thisrobber = players.ListRobbers.Find(r => r.name == username.Trim().ToLower());
				RutonyBot.BotSay(site, username + " спасибо за вклад, ждем других участников!");

                try {
                        File.Delete(file);
             	 } catch { }

            	string serialized = JsonConvert.SerializeObject(players);
            	RutonyBotFunctions.FileAddString(file, serialized);
            }
			else
			{
				RutonyBot.BotSay(site, username + " вы уже вложились в ограбление");

			}

		}

           

    }
	public class Robbers
    {
        public List<Robber> ListRobbers = new List<Robber>();
    }

    public class Robber
	{

		public string name {get; set;}
        public int amount {get; set;}

	}
}