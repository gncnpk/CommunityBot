using Discord.Commands;
using System.Threading.Tasks;
using WazeBotDiscord.Lookup;
using WazeBotDiscord.Twitter;
using WazeBotDiscord.Scripts;

namespace WazeBotDiscord.Modules
{
    [Group("reload")]
    [RequireOwner]
    public class ReloadModule : ModuleBase
    {
        readonly TwitterService _twitterSvc;
        readonly LookupService _lookupSvc;
        readonly ScriptsService _scriptsSvc;

        public ReloadModule(TwitterService twitterSvc, LookupService lookupSvc, ScriptsService scriptsSvc)
        {
            _twitterSvc = twitterSvc;
            _lookupSvc = lookupSvc;
            _scriptsSvc = scriptsSvc;
        }

        [Command("twitter")]
        public async Task ReloadTwitter()
        {
            _twitterSvc.StopAllStreams();
            await _twitterSvc.InitTwitterServiceAsync();

            await ReplyAsync("Twitter reloaded.");
        }

        [Command("lookup")]
        public async Task ReloadLookup()
        {
            await _lookupSvc.ReloadSheetsAsync();
            await ReplyAsync("Lookup reloaded.");
        }
    }
}
