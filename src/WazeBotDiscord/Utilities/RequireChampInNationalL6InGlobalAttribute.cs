using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    class RequireChampInNationalL6InGlobalAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            if (!(context.Guild.Id == 300471946494214146 || context.Guild.Id == 347386780074377217))
                return PreconditionResult.FromError("That command can only be used on the national or global servers.");

            //Check if champ on national server
            if (context.Guild.Id == 300471946494214146 && ((SocketGuildUser)context.Message.Author).Roles.Any(r => r.Id == 300494132839841792 || r.Id == 300494182403801088)) //Champ roles on national
                return PreconditionResult.FromSuccess();

            //Check if L6 on global server
            if (context.Guild.Id == 347386780074377217 && (((SocketGuildUser)context.Message.Author).Roles.Any(r => r.Id == Admin.Ids[347386780074377217])))
                return PreconditionResult.FromSuccess();

            return PreconditionResult.FromError($"{context.Message.Author.Mention}: " + "You must be a champ to use that command.");
        }
    }
}
