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
            Regex regURL = new Regex(@"(?:http(?:s):\/\/)?(?:www\.|beta\.)?waze\.com\/(?:.*?\/)?editor[-a-zA-Z0-9@:%_\+.~#?&//=]*");

            foreach (Match itemMatch in regURL.Matches(message))
            {
                if (sb.Length > 0)
                    sb.Append(" | ");
                Regex rgx = new Regex(@"&[^&]*Filter=[^&]*|&s=(\d+)");
                string result = rgx.Replace(itemMatch.ToString(), "");
                sb.Append(result);
            }
            if (sb.ToString().Length == 0)
                sb.Append("No PLs found");
            await ReplyAsync(sb.ToString());
        }
    }
}
