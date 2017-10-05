using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Text.RegularExpressions;

namespace WazeBotDiscord.Modules
{
    public class FunModule : ModuleBase
    {
        [Command("/slap")]
        public async Task SlapUser([Remainder]string message = null)
        {
            string nickname = ((Discord.WebSocket.SocketGuildUser)Context.User).Nickname;
            if (nickname == null)
                nickname = Context.User.Username;
            await ReplyAsync($"{nickname} slaps {message} around a bit with a large trout.");
        }

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
    }
}
