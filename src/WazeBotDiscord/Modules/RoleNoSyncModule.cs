using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Roles;
using WazeBotDiscord.Utilities;

namespace WazeBotDiscord.Modules
{
    public class RoleNoSyncModule : ModuleBase
    {
        [Command("iosbeta", RunMode = RunMode.Async)]
        [RequireCmOrAbove]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ToggleCm(IUser user)
        {
            if (IsSelf(user))
            {
                await ReplyAsync("You can't change this role for yourself.");
                return;
            }

            var msg = await ReplyAsync($"{user.Mention}: Just a moment...");

            var result = await RoleAssignmentHelper.ToggleRoleAsync(user, iosbeta.Ids, Context);

            if (result == SyncedRoleStatus.Added)
            {
                await msg.ModifyAsync(m => m.Content = $"{user.Mention}: Added iOS beta role.");
            }
            else if (result == SyncedRoleStatus.Removed)
            {
                await msg.ModifyAsync(m => m.Content = $"{user.Mention}: Removed iOS beta role.");
            }
        }

        bool IsSelf(IUser target)
        {
            return Context.Message.Author == target;
        }
    }
}
