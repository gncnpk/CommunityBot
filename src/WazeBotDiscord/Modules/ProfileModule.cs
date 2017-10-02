using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using WazeBotDiscord.Profile;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System.Text;

namespace WazeBotDiscord.Modules
{
    public class ProfileModule : ModuleBase
    {
        /*
         * Links for justins83: 
            Editor profile
            https://www.waze.com/user/editor/justins83 (may not exist)

            Forum profile
            https://www.waze.com/forum/memberlist.php?mode=viewprofile&un=justins83

            Wiki profile
            https://wazeopedia.waze.com/wiki/USA/User:justins83
        */

        const string editorProfileBase = "https://www.waze.com/user/editor/";
        const string forumProfileBase = "https://www.waze.com/forum/memberlist.php?mode=viewprofile&un=";
        const string wikiProfileBase = "https://wazeopedia.waze.com/wiki/USA/User:";

        [Command("profile")]
        public async Task Tiles([Remainder]string editorName )
        {
            string editorProfile = editorProfileBase + editorName;
            string forumProfile = forumProfileBase + editorName;
            string wikiProfile = wikiProfileBase + editorName;

            forumProfile = await CheckProfile(forumProfile, "forum");
            wikiProfile = await CheckProfile(wikiProfile, "wiki");
            ProfileResult pr = new ProfileResult();
            pr.EditorName = editorName;
            pr.EditorProfile = $"<{editorProfile}>";
            pr.ForumProfile = forumProfile;
            if (forumProfile.StartsWith("http"))
                pr.ForumProfile = $"<{forumProfile}>";
            pr.WikiProfile = wikiProfile;
            if (wikiProfile.StartsWith("http"))
                pr.WikiProfile = $"<{wikiProfile}>";

            //var embed = CreateEmbed(pr);
            StringBuilder sr = new StringBuilder();
            
            sr.AppendLine($":bust_in_silhouette: **{pr.EditorName}**");
            sr.AppendLine("_Editor Profile_");
            sr.AppendLine(pr.EditorProfile);
            sr.AppendLine("_Forum Profile_");
            sr.AppendLine(pr.ForumProfile);
            sr.AppendLine("_Wiki Profile_");
            sr.AppendLine(pr.WikiProfile);
            await ReplyAsync(sr.ToString());
            //await ReplyAsync("", embed: embed);
        }

        async Task<string> CheckProfile(string url, string profileType)
        {
            try {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                //Returns TRUE if the Status code == 200

                if (!(response.StatusCode == HttpStatusCode.OK))
                    url = "No " + profileType + " profile";
                response.Close();
            }
            catch {
                url = "No " + profileType + " profile";
            }

            return url;
        }

        Embed CreateEmbed(ProfileResult item)
        {
            var users = Context.Guild.GetUsersAsync(CacheMode.AllowDownload);
            string avatarURL = "";
            foreach (var u in users.Result)
            {
                if (u.Username.ToLower().StartsWith(item.EditorName.ToLower()))
                {
                    avatarURL = u.GetAvatarUrl();
                    break;
                }
            }
            StringBuilder sr = new StringBuilder();
            sr.AppendLine("**Editor Profile**");
            sr.AppendLine(item.EditorProfile);
            sr.AppendLine("**Forum Profile**");
            sr.AppendLine(item.ForumProfile);
            sr.AppendLine("**Wiki Profile**");
            sr.AppendLine(item.WikiProfile);
            var embed = new EmbedBuilder()
            {
                Color = new Color(147, 196, 211),
                Title = "Profiles for " + item.EditorName,
                Description = sr.ToString(),
                ThumbnailUrl = avatarURL

                /*Footer = new EmbedFooterBuilder
                {
                    //Text = $"Last updated on {item.ModifiedAt.Date.ToString("yyyy-MM-dd")}"
                }*/
            };

            return embed.Build();
        }
    }
}
