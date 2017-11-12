using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.Utilities;

namespace WazeBotDiscord.Modules
{
    public class AssignModule : ModuleBase
    {
        [Command("assign")]
        [RequireChampInNationalGuild]
        public async Task AssignRoles(IRole role, params IUser[] users)
        {
            foreach (SocketGuildUser myuser in users)
            {
                if (!myuser.Roles.Contains(role))
                    await myuser.AddRoleAsync(role);
            }

            await ReplyAsync("Assignment complete.");
        }
    }
}
