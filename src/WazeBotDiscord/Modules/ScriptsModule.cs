using Discord.Commands;
using System.Threading.Tasks;
using WazeBotDiscord.Scripts;
using WazeBotDiscord.Utilities;

namespace WazeBotDiscord.Modules
{
    [Group("scripts")]
    //[Alias("spreadsheet", "sheet")]
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

            await ReplyAsync(await _scriptsService.SearchSheetAsync(searchString));
        }

        [Command("List")]
        public async Task List([Remainder]string unused = null)
        {
            await ReplyAsync("Not yet implemented");
        }
        /*[Command("add"), Priority(10)]
        [RequireSmOrAbove]
        public async Task Add([Remainder]string sheetID = null)
        {
            if (sheetID == null)
            {
                await ReplyAsync($"{Context.Message.Author.Mention}: You must specify a sheet ID.");
                return;
            }
            var sheetInfo = sheetID.Split(" ");
            bool result;
            if (sheetInfo.Length > 2)
            {
                await ReplyAsync("Incorrect paramaters specified.");
                return;
            }
            else if (sheetInfo.Length == 2)
            {
                result = await _scriptsService.AddSheetIDAsync(Context.Guild.Id, Context.Channel.Id, sheetInfo[0], sheetInfo[1]);
            }
            else
                result = await _scriptsService.AddSheetIDAsync(Context.Guild.Id, Context.Channel.Id, sheetID);


            var reply = $"{Context.Message.Author.Mention}: sheet added.";
            if (result == false)
                reply = $"{Context.Message.Author.Mention}: sheet modified.";

            await ReplyAsync(reply);
        }*/

        /*[Command("remove"), Priority(9)]
        [RequireSmOrAbove]
        public async Task Remove([Remainder]string sheetID = null)
        {
            var removed = await _scriptsService.RemoveSheetIDAsync(Context.Guild.Id, Context.Channel.Id);

            if (removed)
                await ReplyAsync("Sheet removed.");
            else
                await ReplyAsync("No sheet was set for this channel.");
        }*/
    }
}
