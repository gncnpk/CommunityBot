﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using WazeBotDiscord.Classes.Roles;

namespace WazeBotDiscord.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequireCmOrAboveAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var appInfo = await context.Client.GetApplicationInfoAsync();
            if (appInfo.Owner.Id == context.User.Id)
                return PreconditionResult.FromSuccess();

            var guild = context.Guild as SocketGuild;

            if (RestrictedRegion.Ids.Contains(guild.Id))
                return PreconditionResult.FromError(
                    "This command is currently disabled. More info here: <https://github.com/vaindil/wazebot-discord/issues/12>");

            var exists = CountryManager.Ids.TryGetValue(guild.Id, out var roleId);
            if (!exists)
                return PreconditionResult.FromError("This server is not configured for that command.");

            var cmRole = guild.GetRole(roleId);

            if (((SocketGuildUser)context.Message.Author).Hierarchy >= cmRole.Position)
                return PreconditionResult.FromSuccess();

            return PreconditionResult.FromError("You must be CM or above (which includes RC/ARC) to use that command.");
        }
    }
}
