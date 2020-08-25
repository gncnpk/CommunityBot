using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WazeBotDiscord.Utilities;
using WazeBotDiscord.BotManagement;

namespace WazeBotDiscord.Modules
{
    public class BotManagementModule : ModuleBase
    {
        readonly BotManagementService _botmanagementSvc;

        public BotManagementModule(BotManagementService botmanagementSvc)
        {
            _botmanagementSvc = botmanagementSvc;
        }

        [Command("restartbot")]
        [RequireChampInNationalGuild]
        public async Task RestartBot()
        {
            await ReplyAsync("Bot restart triggered");
            var success = await _botmanagementSvc.ExecuteBotService("restart");
            if (!success)
                await ReplyAsync("Bot restart failed");
        }

        [Command("updatebot")]
        [RequireChampInNationalGuild]
        public async Task UpdateBot()
        {
            await ReplyAsync("Bot update triggered");
            var success = await _botmanagementSvc.ExecuteBotService("update");
            if (!success)
                await ReplyAsync("Bot update failed");
        }

        [Command("stopbot")]
        [RequireAppOwner]
        public async Task StopBot()
        {
            await ReplyAsync("Bot stop triggered");
            var success = await _botmanagementSvc.ExecuteBotService("stop");
            if (!success)
                await ReplyAsync("Bot stop failed");
        }
    }
}
