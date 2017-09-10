using System.IO;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {

            string filename = ProgramProps.dir_scripts + @"\steal_" + username + ".txt";
            string[] arrString = text.Split(' ');
            
            if (arrString.Length != 2) {

                 RutonyBot.BotSay(site, username + ", недостаточно параметров");

                return;
            }

            RankControl.ChatterRank stealer = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());

			if (stealer == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                return;
            }

            RankControl.ChatterRank vicit = RankControl.ListChatters.Find(r => r.Nickname == arrString[1].Trim().ToLower());

			if (vicit == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Чаттера " + arrString[1] + " не существует!");
                return;
            }

            if (vicit.RankOrder == 0)
                    {
                        RutonyBot.BotSay(site,
                    username + ", не воруй у новичков Kappa");
                     return;   
                    }


            if (File.Exists(filename))
			{
                RutonyBot.BotSay(site, username + " вы уже крадете у кого-то!");
				return;
			}

            RutonyBotFunctions.FileAddString(filename, string.Format("{0}",arrString[1]));

            RutonyBot.BotSay(site, username + " начинает подготовку к краже...");
            
            new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    Thread.Sleep(60000);

                    string[] file = File.ReadAllLines(filename);

                    int cof = int.Parse(param)*(stealer.RankOrder - vicit.RankOrder);

                    if (cof < 0)
                    {
                        cof = int.Parse(param)*(stealer.RankOrder + vicit.RankOrder);
                    }

                    Random winrnd = new Random();
					int winrandom = winrnd.Next(0, 101);
                    
                    winrandom = winrandom+cof;

                    string filenamedef = ProgramProps.dir_scripts + @"\def_" + arrString[1] + ".txt";
                    if (File.Exists(filenamedef))
                    {
                        winrandom-=25;
                    }

                    //RutonyBot.BotSay(site, "Шанс " + winrandom);

                    if (winrandom > 70)
                    {
                        if (vicit.CreditsQty - winrandom > 10)
                        {
                            RutonyBot.BotSay(site, username + " кража произошла успешно!");

                            stealer.CreditsQty += winrandom;
                             vicit.CreditsQty -= winrandom;
                        }
                        else
                        {
                            RutonyBot.BotSay(site, username + " у него нечего красть!");
                        }
                    }
                    else
                    {
                        RutonyBot.BotSay(site, username + " вам не удалось украсть золотые!");
                    }


                     try {
                         File.Delete(filename);
			        } catch { }

			        try {
                        File.Delete(filenamedef);
                    } catch { }

            }).Start();
            return;


           


        }
    }
}