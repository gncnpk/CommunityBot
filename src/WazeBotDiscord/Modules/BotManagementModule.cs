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
            var success = await _botmanagementSvc.ExecuteBotService("restart");
            if (string.IsNullOrEmpty(success))
                await ReplyAsync("Bot restart triggered");
            else
                await ReplyAsync(success);
        }

        [Command("updatebot")]
        [RequireChampInNationalGuild]
        public async Task UpdateBot()
        {
            var success = await _botmanagementSvc.ExecuteBotService("update");
            if (string.IsNullOrEmpty(success))
                await ReplyAsync("Bot update triggered");
            else
                await ReplyAsync("Bot update failed");
        }

        [Command("stopbot")]
        [RequireAppOwner]
        public async Task StopBot()
        {
            var success = await _botmanagementSvc.ExecuteBotService("stop");
            if (string.IsNullOrEmpty(success))
                await ReplyAsync("Bot stop triggered");
            else
                await ReplyAsync(success);
        }
    }
}
