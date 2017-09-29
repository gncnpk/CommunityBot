using Discord.WebSocket;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WazeBotDiscord.Keywords
{
    public static class KeywordHandler
    {
        public static async Task HandleKeywordAsync(SocketMessage msg, KeywordService service, DiscordSocketClient client)
        {
            if (msg.Author.Id == client.CurrentUser.Id || msg.Channel is SocketDMChannel)
                return;

            var channel = msg.Channel as SocketTextChannel;

            foreach (var m in service.CheckForKeyword(msg.Content, msg.Author.Id, channel.Guild.Id, channel.Id))
            {
                if (msg.Author.Id == m.UserId
                    || !channel.Users.Any(u => u.Id == m.UserId))
                    continue;

                string nickname = ((Discord.WebSocket.SocketGuildUser)msg.Author).Nickname;
                var reply = new StringBuilder();
                reply.Append(msg.Author.Username);
                if(nickname != null)
                    reply.Append($" ({nickname})");
                reply.Append(" mentioned ");
                reply.Append(m.MatchedKeywords.Count);
                reply.Append(" of your keywords in ");
                reply.Append($"<#{msg.Channel.Id}>");
                //reply.Append(msg.Channel.Name);
                reply.Append(" on ");
                reply.Append(((SocketGuildChannel)msg.Channel).Guild.Name);
                reply.Append(".\n\nMatched keyword");
                if (m.MatchedKeywords.Count > 1)
                    reply.Append("s");
                reply.Append(":\n```\n");

                foreach (var k in m.MatchedKeywords)
                {
                    reply.Append(k);
                    reply.Append("\n");
                }

                reply.Append("```\n");
                reply.Append("Message:\n```\n");
                reply.Append(msg.Content);
                reply.Append("\n```");

                var dm = await client.GetUser(m.UserId).GetOrCreateDMChannelAsync();
                await dm.SendMessageAsync(reply.ToString());
            }
        }
    }
}
