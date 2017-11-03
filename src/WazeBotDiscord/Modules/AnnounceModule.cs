using Discord.Commands;
using System.Threading.Tasks;
using System;
using WazeBotDiscord.Announce;
using WazeBotDiscord.Utilities;
using Discord.WebSocket;

namespace WazeBotDiscord.Modules
{
    [Group("announce")]
    [RequireChampInNationalAdminInGlobal]
    public class AnnounceModule : ModuleBase
    {
        readonly AnnounceService _announceSvc;

        public AnnounceModule(AnnounceService announceSvc)
        {
            _announceSvc = announceSvc;
        }
        
        [Command]
        public async Task SendMessage([Remainder] string message = null)
        {
            if(message == null || message.Trim() == "")
            {
                await ReplyAsync("Please specify a message.");
                return;
            }

            var _channels = _announceSvc.GetAnnounceChannels();
            var _guilds = _announceSvc.GetBotGuilds();
            SocketTextChannel announceChannel;
            foreach(var c in _channels)
            {
                foreach (SocketGuild g in _guilds)
                {
                    announceChannel = g.GetTextChannel(c.Channel);
                    if(announceChannel != null)
                        await announceChannel.SendMessageAsync(message);
                }


                
            }
        }
    }
}
