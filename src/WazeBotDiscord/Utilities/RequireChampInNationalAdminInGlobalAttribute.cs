using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    public class RequireChampInNationalAdminInGlobalAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            if (context.Guild.Id != 300471946494214146 && context.Guild.Id != 347386780074377217) //National and Global servers
                return PreconditionResult.FromError("That command can only be used on the national & global server.");

            //National server and global or local champ roles OR Global server and admin role
            if (((SocketGuildUser)context.Message.Author).Roles.Any(r => (context.Guild.Id == 300471946494214146 && (r.Id == 300494132839841792 || r.Id == 300494182403801088)) || (context.Guild.Id == 347386780074377217 && (r.Id == Admin.Ids[347386780074377217]))))
                return PreconditionResult.FromSuccess();

            if(context.Guild.Id == 300471946494214146)
                return PreconditionResult.FromError($"{context.Message.Author.Mention}: " + "You must be a champ to use that command.");
            else
                return PreconditionResult.FromError($"{context.Message.Author.Mention}: " + "You must be an admin to use that command.");
        }
    }
}
