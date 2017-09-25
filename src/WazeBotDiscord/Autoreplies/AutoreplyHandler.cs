using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WazeBotDiscord.Autoreplies
{
    public static class AutoreplyHandler
    {
        public static async Task HandleAutoreplyAsync(SocketMessage inMsg, AutoreplyService arService)//, IReadOnlyCollection<SocketGuild> guilds)
        {
            if (inMsg.Channel is SocketDMChannel)
                return;

            var msg = (SocketUserMessage)inMsg;
            var content = msg.Content.ToLowerInvariant();
            var channel = (SocketTextChannel)msg.Channel;

            //you can get the channel from the guild with GetTextChannelAsync then SendMessageAsync on it like any other channel
            /*if (msg.Channel.Id == 359327158944137228) {
                SocketGuild syncGuild = null;
                foreach(SocketGuild g  in guilds)
                {
                    if (g.Id == 360595895965843456)
                        syncGuild = g;
                }

                
                var syncChannel = syncGuild.GetTextChannel(361352875680595969);
                await syncChannel.SendMessageAsync($"**{inMsg.Author.Username}**: {inMsg.Content}");
            }*/
            
            var ar = arService.SearchForAutoreply(content, channel);
            if (ar == null)
                return;

            await channel.SendMessageAsync(ar.Reply);
        }
    }
}
