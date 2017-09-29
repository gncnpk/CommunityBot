using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    public class RequireAdminAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            var guild = context.Guild as SocketGuild;
            var exists = Admin.Ids.TryGetValue(guild.Id, out var roleId);
            if (!exists)
                return PreconditionResult.FromError("This server is not configured for that command.");

            var adminRole = guild.GetRole(roleId);

            if (((SocketGuildUser)context.Message.Author).Hierarchy >= adminRole.Position)
                return PreconditionResult.FromSuccess();

            return PreconditionResult.FromError("You must be an administrator to use that command.");
        }
    }
}
