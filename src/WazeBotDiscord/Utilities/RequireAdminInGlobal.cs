using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    class RequireAdminInGlobal : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            if (context.Guild.Id != 347386780074377217) //Global server
                return PreconditionResult.FromError("That command can only be used on the global server.");

            //Global server and Admin
            if ((context.Guild.Id == 347386780074377217 && ((SocketGuildUser)context.Message.Author).Roles.Any(r => (r.Id == Admin.Ids[347386780074377217]))))
                return PreconditionResult.FromSuccess();

            return PreconditionResult.FromError($"{context.Message.Author.Mention}: " + "You must be an admin to use that command.");
        }
    }
}
