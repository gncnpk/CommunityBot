using Discord.Commands;
using System.Threading.Tasks;
using WazeBotDiscord.Scripts;
using WazeBotDiscord.Utilities;

namespace WazeBotDiscord.Modules
{
    [Group("scripts")]
    [Alias("script")]
    public class ScriptsModule : ModuleBase
    {
        readonly ScriptsService _scriptsService;

        public ScriptsModule(ScriptsService lookupSvc)
        {
            _scriptsService = lookupSvc;
        }

        [Command]
        public async Task GetUrl()
        {
            await ReplyAsync(_scriptsService.GetChannelSheetUrl(Context.Channel.Id));
        }

        [Command(RunMode = RunMode.Async), Priority(5)]
        public async Task Search([Remainder]string searchString)
        {
            if (searchString.Length < 3)
            {
                await ReplyAsync("Your search term must be at least three characters long.");
                return;
            }

            await ReplyAsync(await _scriptsService.SearchSheetAsync(searchString, Context.Guild.Id));
        }
    }
}
