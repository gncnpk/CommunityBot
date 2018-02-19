using Discord.Commands;
using System.Threading.Tasks;
using WazeBotDiscord.Lookup;
using WazeBotDiscord.Twitter;
using WazeBotDiscord.Scripts;
using WazeBotDiscord.ServerJoin;
using WazeBotDiscord.Autoreplies;
using WazeBotDiscord.Outreach;
using WazeBotDiscord.Utilities;
using WazeBotDiscord.Glossary;
using WazeBotDiscord.Keywords;

namespace WazeBotDiscord.Modules
{
    [Group("reload")]
    [RequireChampInUSAdminInGlobalScripts]
    public class ReloadModule : ModuleBase
    {
        readonly TwitterService _twitterSvc;
        readonly LookupService _lookupSvc;
        readonly ScriptsService _scriptsSvc;
        readonly ServerJoinService _serverJoinSvc;
        readonly AutoreplyService _autoreplySvc;
        readonly OutreachService _outreachSvc;
        readonly GlossaryService _glossarySvc;
        readonly KeywordService _keywordSvc;

        public ReloadModule(TwitterService twitterSvc, LookupService lookupSvc, ScriptsService scriptsSvc, ServerJoinService serverJoinSvc, AutoreplyService autoreplySvc, OutreachService outreachSvc,
            GlossaryService glossarySvc, KeywordService keywordSvc)
        {
            _twitterSvc = twitterSvc;
            _lookupSvc = lookupSvc;
            _scriptsSvc = scriptsSvc;
            _serverJoinSvc = serverJoinSvc;
            _autoreplySvc = autoreplySvc;
            _outreachSvc = outreachSvc;
            _glossarySvc = glossarySvc;
            _keywordSvc = keywordSvc;
        }

        [Command()]
        public async Task AvailableModules()
        {
            await ReplyAsync("Modules available to reload: twitter, lookup, outreach, serverjoin, autoreplies, glossary, keywords");
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

        [Command("outreach")]
        public async Task ReloadOutreach()
        {
            await _outreachSvc.ReloadOutreachAsync();
            await ReplyAsync("Outreach reloaded.");
        }

        [Command("serverjoin")]
        public async Task ReloadServerJoin()
        {
            await _serverJoinSvc.ReloadServerjoinAsync();
            await ReplyAsync("Serverjoin reloaded.");
        }

        [Command("autoreplies")]
        public async Task ReloadAutorepliesJoin()
        {
            await _autoreplySvc.ReloadAutorepliesAsync();
            await ReplyAsync("Autoreplies reloaded.");
        }

        [Command("glossary")]
        public async Task ReloadGlossary()
        {
            await _glossarySvc.ReloadGlossaryAsync();
            await ReplyAsync("Glossary reloaded.");
        }

        [Command("keywords")]
        public async Task ReloadKeywords()
        {
            await _keywordSvc.ReloadKeywordsAsync();
            await ReplyAsync("Keywords reloaded.");
        }
    }
}
