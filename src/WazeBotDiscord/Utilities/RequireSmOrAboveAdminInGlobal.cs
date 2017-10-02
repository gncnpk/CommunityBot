using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Linq;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    class RequireSmOrAboveAdminInGlobal: PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            var guild = context.Guild as SocketGuild;
            var exists = StateManager.Ids.TryGetValue(guild.Id, out var roleId);
            if (!exists)
                return PreconditionResult.FromError("This server is not configured for that command.");

            var cmRole = guild.GetRole(roleId);

            if ((context.Guild.Id != 347386780074377217 && ((SocketGuildUser)context.Message.Author).Hierarchy >= cmRole.Position) //if on any server but Global & role >= SM
                || (context.Guild.Id == 347386780074377217 && ((SocketGuildUser)context.Message.Author).Roles.Any(r => (r.Id == 361110624958152704)))) //or if on Global and role == admin
                return PreconditionResult.FromSuccess();

            if(context.Guild.Id == 347386780074377217) //if on Global, must be admin
                return PreconditionResult.FromError("You must be admin to use that command.");
            else
                return PreconditionResult.FromError("You must be SM or above to use that command.");
        }
    }
}
