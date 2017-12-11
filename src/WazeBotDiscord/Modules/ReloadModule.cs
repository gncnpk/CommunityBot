using Discord.Commands;
using System.Threading.Tasks;
using WazeBotDiscord.Lookup;
using WazeBotDiscord.Twitter;
using WazeBotDiscord.Scripts;
using WazeBotDiscord.ServerJoin;

namespace WazeBotDiscord.Modules
{
    [Group("reload")]
    [RequireOwner]
    public class ReloadModule : ModuleBase
    {
        readonly TwitterService _twitterSvc;
        readonly LookupService _lookupSvc;
        readonly ScriptsService _scriptsSvc;
        readonly ServerJoinService _serverJoinSvc;

        public ReloadModule(TwitterService twitterSvc, LookupService lookupSvc, ScriptsService scriptsSvc, ServerJoinService serverJoinSvc)
        {
            _twitterSvc = twitterSvc;
            _lookupSvc = lookupSvc;
            _scriptsSvc = scriptsSvc;
            _serverJoinSvc = serverJoinSvc;
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

        [Command("serverjoin")]
        public async Task ReloadServerJoin()
        {
            await _lookupSvc.ReloadSheetsAsync();
            await ReplyAsync("Lookup reloaded.");
        }
    }
}
