using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;
using System.Text.RegularExpressions;
using System.Net;
using WazeBotDiscord.Fun;

namespace WazeBotDiscord.Modules
{
    public class FunModule : ModuleBase
    {

        readonly FunService _funSvc;

        public FunModule(FunService funSvc)
        {
            _funSvc = funSvc;
        }

        #region "Slap"
        [Command("/slap")]
        public async Task SlapUser([Remainder]string message = null)
        {
            string nickname = ((Discord.WebSocket.SocketGuildUser)Context.User).Nickname;
            if (nickname == null)
                nickname = Context.User.Username;
            await ReplyAsync($"{nickname} slaps {message} around a bit with a large trout.");
        }
        #endregion

        #region "Diceroll"
        [Command("diceroll")]
        public async Task Diceroll([Remainder]string message = null)
        {
            StringBuilder sb = new StringBuilder();
            Regex regURL = new Regex(@"(\d+)d(\d+)");

            if (message == null)
                message = "1d6";

            if (regURL.Matches(message).Count > 1)
            {
                int sum = 0;

                sb.Append($"`{message}` =");
                foreach (Match itemMatch in regURL.Matches(message))
                {
                    int numDie = Convert.ToInt32(itemMatch.Groups[1].ToString());
                    int sides = Convert.ToInt32(itemMatch.Groups[2].ToString());

                    var result = RollTheDice(numDie, sides);
                    if (sum > 0)
                        sb.Append("+");
                    sb.Append(result.Item2);
                    sum += result.Item1;
                }
                sb.Append($" = {sum}");
            }
            else
            {
                Match die = regURL.Match(message);
                int numDie = Convert.ToInt32(die.Groups[1].ToString());
                int sides = Convert.ToInt32(die.Groups[2].ToString());
                int sum = 0;

                sb.Append($"`{die.Groups[0].ToString()}`");
                var result = RollTheDice(numDie, sides);
                sum += result.Item1;
                if (numDie > 1)
                    sb.Append(" =");
                sb.Append(result.Item2);
                sb.Append($" = {sum}");
            }

            await ReplyAsync(sb.ToString());
        }

        private Tuple<int, string> RollTheDice(int numDie, int numSides)
        {
            Random rnd = new Random();
            int roll, sum = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numDie; i++)
            {
                roll = rnd.Next(1, numSides);
                if (numDie > 1)
                {
                    if (i > 0)
                        sb.Append("+");
                    else
                        sb.Append(" (");
                    sb.Append(roll);
                }
                sum += roll;
            }
            if (numDie > 1)
                sb.Append(")");
            return Tuple.Create(sum, sb.ToString());
        }
        #endregion

        #region "Dad jokes"
        [Command("dadjoke")]
        public async Task GetDadJoke([Remainder]string message = null)
        {
            var result = _funSvc.GetWebRequest("https://icanhazdadjoke.com/slack", "application/json; charset=utf-8").Result;

            var dadJoke = JsonConvert.DeserializeObject<TheDadJoke>(result);
            await ReplyAsync(dadJoke.attachments[0].text);
        }
        public class TheDadJoke
        {
            public List<Attachment> attachments { get; set; }
            public string response_type { get; set; }
            public string username { get; set; }
        }

        public class Attachment
        {
            public string fallback { get; set; }
            public string footer { get; set; }
            public string text { get; set; }
        }
        #endregion

        #region "Facts"
        [Command("dogfact")]
        public async Task GetDogFact([Remainder] string ignore = null)
        {
            var result = _funSvc.GetWebRequest("https://dog-api.kinduff.com/api/facts", "application/json; charset=utf-8").Result;

            var dogFact = JsonConvert.DeserializeObject<DogFacts>(result);
            await ReplyAsync(dogFact.facts[0]);
        }

        //{"facts":["An American Animal Hospital Association poll found that 33% of dog owners admit to talking to their dogs on the phone and leaving answering machine messages for them while away."],"success":true}
        public class DogFacts
        {
            public List<string> facts { get; set; }
            public Boolean success { get; set; }
        }

        [Command("numberfact")]
        public async Task getNumberFactor([Remainder] string ignore = null)
        {
            
            var result = _funSvc.GetWebRequest("http://numbersapi.com/random", "text/html").Result;

            await ReplyAsync(result);
        }

        #endregion
    }
}
