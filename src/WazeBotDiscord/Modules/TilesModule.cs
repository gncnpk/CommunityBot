using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using WazeBotDiscord.Tiles;

namespace WazeBotDiscord.Modules
{
    [Group("tiles")]
    class TilesModule : ModuleBase
    {

        readonly TilesService _tilesSvc;

        public TilesModule(TilesService tilesSvc)
        {
            _tilesSvc = tilesSvc;
        }

        [Command]
        public async Task Search()
        {
            var item = _tilesSvc.GetTileInfo();
            if (item == null)
            {
                await ReplyAsync($"Unable to get tile status.");
                return;
            }
            
            var embed = CreateEmbed(item);
            await ReplyAsync("", embed: embed);
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
