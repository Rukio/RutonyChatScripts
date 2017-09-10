
using System;

namespace RutonyChat {
    public class Script {
        public void RunScript(string site, string username, string text, string param) {

            // !credits nickname 100
            string[] arrString = text.Split(' ');
            
            if (arrString.Length != 3) {

                switch(site) {
                    case "Goodgame":
                        RutonyBot.GoodgameBot.Say(username + ", Не хватает агрументов!");
                        break;
                    case "Twitch":
                        RutonyBot.TwitchBot.Say(username + ", Не хватает агрументов!");
                        break;
                }

                return;
            }

            int qtyCredits = 0;
            try {
                qtyCredits = int.Parse(arrString[2]);
            } catch {
                switch (site) {
                    case "goodgame":
                        RutonyBot.GoodgameBot.Say(username + ", Это не кредиты!");
                        break;
                    case "twitch":
                        RutonyBot.TwitchBot.Say(username + ", Это не кредиты!");
                        break;
                }
            }

            RankControl.ChatterRank cr = RankControl.ListChatters.Find(r => r.Nickname == arrString[1].Trim().ToLower());
            RankControl.ChatterRank crgiver = RankControl.ListChatters.Find(r => r.Nickname == username.Trim().ToLower());

            if (crgiver.CreditsQty < qtyCredits)
            {
                RutonyBot.BotSay(site, username + " у вас нет столько кредитов");
                return;
            }
            if (qtyCredits <= 0)
            {
                 RutonyBot.BotSay(site, username + " кредитов должно быть больше 0");
                return;
            }
            if (cr != null) {

                cr.CreditsQty += qtyCredits;
                crgiver.CreditsQty -= qtyCredits;
                switch (site) {
                    case "goodgame":
                        RutonyBot.GoodgameBot.Say(string.Format("{0}, {1} передал вам {2} кредитов и теперь у вас их {3}!", arrString[1], username, qtyCredits, cr.CreditsQty));
                        break;
                    case "twitch":
                        RutonyBot.TwitchBot.Say(string.Format("{0}, {1} передал вам {2} кредитов и теперь у вас их {3}!", arrString[1], username, qtyCredits, cr.CreditsQty));
                        break;
                }
            }

        }
    }
}