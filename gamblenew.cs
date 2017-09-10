using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;

//Модифицированный скрипт Visteras-а
//http://steamcommunity.com/sharedfiles/filedetails/?id=918344254
//by pasvitas twitch.tv/pasvitas


namespace RutonyChat {
    public class Script {

        public void RunScript(string site, string username, string text, string param) {

            int credit;
            int status = 0;
            RankControl.ChatterRank cr = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());
            //TODO очень маловероятная ошибка, но вдруг? Механики работы чата пока не знаю, потому проверка пока пусть будет...
            if (cr == null) {
                RutonyBot.BotSay(site,
                    username + ", ошибка! Вашей записи нет в базе данных или она повреждена!");
                return;
            }

            string textsfile = ProgramProps.dir_scripts + @"\texts.json";

            string[] filetexts = File.ReadAllLines(textsfile);

            CreditName names = JsonConvert.DeserializeObject<CreditName>(filetexts[0]);
			
			
			
			string[] arrString = text.Split(' ');
			
            
            if (arrString.Length != 3) {
                RutonyBot.BotSay(site, "Используйте !ставка (кол-во " + names.mnpads[1] + ") (красное/черное/ноль)");
                return;
            }
			//int credit = 0;
			credit = int.Parse(arrString[1]);
            if (credit <= 0) {
                RutonyBot.BotSay(site, "Параметр должен быть положительным числом!");
                return;
            }
            if (cr.CreditsQty < credit) {
                RutonyBot.BotSay(site, string.Format("У вас всего {0} {1}!", cr.CreditsQty, names.mnpads[1]));
                return;
            }
			
			
			cr.CreditsQty -= credit;
            
			string stavka = arrString[2].Trim().ToLower();
			if (stavka != "красное" && stavka != "черное" && stavka != "ноль")
			{
				RutonyBot.BotSay(site, "Вам нужно указать, на что ставите (красное/черное/ноль)");
                return;
			}	
			
			Random rnd = new Random();
            int randomShoot = rnd.Next(1, 40);

            if (randomShoot <= 18) {
				if (stavka == "красное")
				{
					status = 1;
					cr.CreditsQty += credit*2;
				}
            } else if (randomShoot > 18 && randomShoot <= 38) {
				if (stavka == "черное")
				{
					status = 1;
					cr.CreditsQty += credit*2;
				}
            } else {
				if (stavka == "ноль")
				{
					status = 2;
					cr.CreditsQty += credit*5;
				}
            }
            RankControl.ChatterRank cr_change = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());
            switch (status) {
                case 0:
                    RutonyBot.BotSay(site, string.Format("Ставка не прошла! К сожалению {2} проиграл и потерял {1} {4}! Теперь у него {3} {4}", randomShoot, credit, username, cr_change.CreditsQty, names.mnpads[1]));
                    break;
                case 1:
                    RutonyBot.BotSay(site, string.Format("Хорошая ставка! {2} выигрывает и получает {1} {4}! Теперь у него {3} {4}", randomShoot, credit*2, username, cr_change.CreditsQty, names.mnpads[1]));
                    break;
                case 2:
                    RutonyBot.BotSay(site, string.Format("Ставка в ноль прошла! {2} получает {1} {4}! Теперь у него {3} {4}", randomShoot, credit*4, username, cr_change.CreditsQty, names.mnpads[1]));
                    break;
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