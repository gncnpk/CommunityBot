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
        [Command("worldcup", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ToggleWorldCup()
        {
            var msg = await ReplyAsync($"{Context.Message.Author.Mention}: Just a moment...");
            var result = await RoleAssignmentHelper.ToggleRoleAsync(Context.Message.Author, WorldCup.Ids, Context);

            if (result == SyncedRoleStatus.Added)
            {
                await msg.ModifyAsync(m => m.Content = $"{Context.Message.Author.Mention}: Added worldcup role.  Join the discussion in <#448908730243743754>");
            }
            else if (result == SyncedRoleStatus.Removed)
            {
                await msg.ModifyAsync(m => m.Content = $"{Context.Message.Author.Mention}: Removed worldcup role.");
            }
        }

        [Command("iosbeta", RunMode = RunMode.Async)]
        [RequireAdminModeratorInGlobal]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ToggleiOSBeta(IUser user)
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

        [Command("wmebeta", RunMode = RunMode.Async)]
        [RequireAdminModeratorInGlobal]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ToggleWMEBeta(IUser user)
        {
            if (IsSelf(user))
            {
                await ReplyAsync("You can't change this role for yourself.");
                return;
            }

            var msg = await ReplyAsync($"{user.Mention}: Just a moment...");

            var result = await RoleAssignmentHelper.ToggleRoleAsync(user, WMEBeta.Ids, Context);

            if (result == SyncedRoleStatus.Added)
            {
                await msg.ModifyAsync(m => m.Content = $"{user.Mention}: Added WME beta role.");
            }
            else if (result == SyncedRoleStatus.Removed)
            {
                await msg.ModifyAsync(m => m.Content = $"{user.Mention}: Removed WME beta role.");
            }
        }

        [Command("androidbeta", RunMode = RunMode.Async)]
        [RequireAdminModeratorInGlobal]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ToggleAndroidBeta(IUser user)
        {
            if (IsSelf(user))
            {
                await ReplyAsync("You can't change this role for yourself.");
                return;
            }

            var msg = await ReplyAsync($"{user.Mention}: Just a moment...");

            var result = await RoleAssignmentHelper.ToggleRoleAsync(user, AndroidBeta.Ids, Context);

            if (result == SyncedRoleStatus.Added)
            {
                await msg.ModifyAsync(m => m.Content = $"{user.Mention}: Added Android beta role.");
            }
            else if (result == SyncedRoleStatus.Removed)
            {
                await msg.ModifyAsync(m => m.Content = $"{user.Mention}: Removed Android beta role.");
            }
        }

        bool IsSelf(IUser target)
        {
            return Context.Message.Author == target;
        }
    }
}
