using Discord.Commands;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WazeBotDiscord.DND;

namespace WazeBotDiscord.Modules
{
    [Group("dnd")]
    public class DNDModule : ModuleBase
    {
        readonly DNDService _dndService;

        public DNDModule(DNDService dndSvc)
        {
            _dndService = dndSvc;
        }

        [Command]
        public async Task GetDNDTime()
        {
            string result = await _dndService.GetDNDTime(Context.User.Id);
            await ReplyAsync(result);
        }

        [Command]
        public async Task TaskAddDNDTime(string dndTime)
        {
            double hours = 0;
            try
            {
                hours = Convert.ToDouble(dndTime);
                if(hours <= 0)
                {
                    await ReplyAsync("DND hours must be greater than zero.");
                    return;
                }
            }
            catch
            {
                await ReplyAsync("Incorrect parameters specified.");
                return;
            }

            if(await _dndService.AddDND(Context.User.Id, DateTime.Now.AddHours(hours)))
            {
                await ReplyAsync($"DND enabled for {hours} hours.");
            }
            else
            {
                await ReplyAsync($"DND time changed to {hours} hours.");
            }
        }

        [Command("disable"), Priority(9)]
        [Alias("off")]
        public async Task Remove([Remainder]string unused = null)
        {
            var removed = await _dndService.RemoveDND(Context.User.Id);

            if (removed)
                await ReplyAsync("DND disabled.");
            else
                await ReplyAsync("DND was not enabled.");
        }
    }
}
