using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Text;

namespace WazeBotDiscord.Modules
{
    public class PLModule : ModuleBase
    {
        [Command("pl")]
        public async Task Tiles([Remainder]string message = null)
        {
            StringBuilder sb = new StringBuilder();
            Regex regURL = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)");

            foreach (Match itemMatch in regURL.Matches(message))
            {
                if (sb.Length > 0)
                    sb.Append(" | ");
                Regex rgx = new Regex(@"&[^&]*Filter=[^&]*|&s=(\d+)");
                string result = rgx.Replace(itemMatch.ToString(), "");
                sb.Append(result);
            }
            await ReplyAsync(sb.ToString());
        }
    }
}
