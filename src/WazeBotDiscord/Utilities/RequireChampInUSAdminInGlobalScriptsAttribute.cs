using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Servers;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    public class RequireChampInUSAdminInGlobalScriptsAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            if(context.Guild.Id == Servers.WazeScripts || context.Guild.Id == Servers.GlobalMapraid)
            {
                var guild = context.Guild as SocketGuild;
                var exists = Admin.Ids.TryGetValue(guild.Id, out var roleId);
                var adminRole = guild.GetRole(roleId);

                if (((SocketGuildUser)context.Message.Author).Roles.Contains(adminRole))
                    return PreconditionResult.FromSuccess();

                return PreconditionResult.FromError("You must be an admin on this server to use that command.");
            }
            else
            {
                var guild = context.Guild as SocketGuild;
                ulong roleId;
                var exists = LocalChamp.Ids.TryGetValue(guild.Id, out roleId);
                if (!exists)
                    return PreconditionResult.FromError("This server is not configured for that command.");

                var champRole = guild.GetRole(roleId);

                if (((SocketGuildUser)context.Message.Author).Hierarchy >= champRole.Position)
                    return PreconditionResult.FromSuccess();


                exists = GlobalChamp.Ids.TryGetValue(guild.Id, out roleId);
                if (!exists)
                    return PreconditionResult.FromError("This server is not configured for that command.");

                champRole = guild.GetRole(roleId);

                if (((SocketGuildUser)context.Message.Author).Hierarchy >= champRole.Position)
                    return PreconditionResult.FromSuccess();



                return PreconditionResult.FromError("You must be a champ or above on this server to use that command.");
            }

        }
    }
}
