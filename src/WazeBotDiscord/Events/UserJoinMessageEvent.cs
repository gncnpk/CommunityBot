using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using WazeBotDiscord.ServerJoin;

namespace WazeBotDiscord.Events
{
    class UserJoinMessageEvent
    {
        public static async Task SendMessage(SocketGuildUser user, DiscordSocketClient client, ServerJoinService serverJoinService)
        {
            var serverLeft = user.Guild;

            var joinMessage = serverJoinService.GetExistingJoinMessage(user.Guild.Id);

            if (joinMessage != null)
            {
                var dm = await client.GetUser(user.Id).GetOrCreateDMChannelAsync();
                await dm.SendMessageAsync(joinMessage.JoinMessage);
            }
        }
    }
}
