using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using WazeBotDiscord.Tiles;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace WazeBotDiscord.Modules
{
    public class TilesModule : ModuleBase
    {
        TilesResult _tilesResult;

        [Command("tiles")]
        public async Task Tiles([Remainder]string unused = null)
        {
            await UpdateParseUpdatesAsync();
            var embed = CreateEmbed(_tilesResult);
            await ReplyAsync("", embed: embed);
        }

        async Task UpdateParseUpdatesAsync()
        {
            HttpClient httpClient = new HttpClient();
            string body = await httpClient.GetStringAsync("https://status.waze.com/feeds/posts/default");//("https://wazestatus.wordpress.com/category/main/feed");
            Regex regNA = new Regex(@"<published>(.{29})<\/published><updated>.{29}<\/updated><title type='text'>NA map tiles were successfully updated to: (.*?)<\/title>");//new Regex(" < title>NA map tiles were successfully updated to:(.*?)<\\/title>(.|\\n)*?<pubDate>(.*?) \\+0000<\\/pubDate>");
            Match matchNA = regNA.Match(body);
            Regex regINTL = new Regex(@"<published>(.{29})<\/published><updated>.{29}<\/updated><title type='text'>INTL map tiles were successfully updated to: (.*?)<\/title>");//new Regex(" < title>INTL map tiles were successfully updated to:(.*?)<\\/title>(.|\\n)*?<pubDate>(.*?) \\+0000<\\/pubDate>");
            Match matchINTL = regINTL.Match(body);
            DateTime NAPubDateTime = DateTime.Parse(matchNA.Groups[1].ToString());
            DateTime INTLPubDateTime = DateTime.Parse(matchINTL.Groups[1].ToString());

            _tilesResult = new TilesResult();
            _tilesResult.NATileDate = "NA: " + matchNA.Groups[2].ToString();
            _tilesResult.NAUpdatePerformed = $"*(performed: {NAPubDateTime.ToString()})*";
            _tilesResult.INTLTileDate = "INTL: " + matchINTL.Groups[2].ToString();
            _tilesResult.INTLUpdatePerformed = $"*(performed: {INTLPubDateTime.ToString()})*";
        }

        Embed CreateEmbed(TilesResult item)
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color(147, 196, 211),
                Title = "Waze Status",
                Url = $"https://wazestatus.wordpress.com",
                Description = item.NATileDate + Environment.NewLine + item.NAUpdatePerformed + Environment.NewLine + item.INTLTileDate + Environment.NewLine + item.INTLUpdatePerformed,

                /*Footer = new EmbedFooterBuilder
                {
                    //Text = $"Last updated on {item.ModifiedAt.Date.ToString("yyyy-MM-dd")}"
                }*/
            };

            return embed.Build();
        }
    }
}
