using Discord.WebSocket;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using WazeBotDiscord.ChannelSync;
using Discord;

namespace WazeBotDiscord.ChannelSync
{
    class ChannelSyncHandler
    {
        public static async Task HandleChannelSyncAsync(SocketMessage msg, ChannelSyncService service, DiscordSocketClient client)
        {
            if (msg.Author.Id == client.CurrentUser.Id) //don't sync bot messages
                return;

            var channels = service.getSyncChannels(msg.Channel.Id);
            ulong channelToSyncTo = channels.Channel1;

            if (msg.Channel.Id == channels.Channel1)
                channelToSyncTo = channels.Channel2;

            string nickname = msg.Author.Username;
            if (msg.Author is Discord.WebSocket.SocketGuildUser)
                nickname = ((Discord.WebSocket.SocketGuildUser)msg.Author).Nickname;

            var author = new EmbedAuthorBuilder().WithName(nickname).WithIconUrl(msg.Author.GetAvatarUrl());

            var embed = new EmbedBuilder().WithAuthor(author).WithDescription(msg.Content);

            var myChannel = client.GetChannel(channelToSyncTo) as IMessageChannel;
            await myChannel.SendMessageAsync("",false, embed);

            //DiscordSocketClient _client = new DiscordSocketClient(); // 2
            //ulong id = 123456789012345678; // 3
            //var chnl = _client.GetChannel(id) as IMessageChannel; // 4
            //await chnl.SendMessageAsync("Announcement!"); // 5

        }
    }
}
