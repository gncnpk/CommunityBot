using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.ServerLeave;

namespace WazeBotDiscord.Events
{
    public static class UserLeftEvent
    {
        public static async Task Alert(SocketGuildUser user, IDiscordClient client, ServerLeaveService serverLeaveService)
        {
            var serverLeft = user.Guild;

            LeaveMessageChannel result = serverLeaveService.GetExistingLeaveChannel(user.Guild.Id);

            if (result != null)
            {
                var syncChannel = serverLeft.GetTextChannel(result.ChannelId);
                string usernameString = user.Username;
                if (user.Nickname != null)
                    usernameString += $" ({user.Nickname})";
                await syncChannel.SendMessageAsync($"**{usernameString}** has left the server.");
            }
        }
    }
}
