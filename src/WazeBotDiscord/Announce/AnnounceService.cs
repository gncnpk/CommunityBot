using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace WazeBotDiscord.Announce
{
    public class AnnounceService
    {
        List<AnnounceChannel> _AnnounceChannels = new List<AnnounceChannel>();
        DiscordSocketClient _client;

        public async Task InitAnnounceServiceAsync(DiscordSocketClient client)
        {
            _client = client;
            using (var db = new WbContext())
            {
                //_AnnounceChannels = await db.AnnounceList.ToListAsync();
                _AnnounceChannels = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(db.AnnounceList);
            }
        }

        public List<AnnounceChannel> GetAnnounceChannels()
        {
            return _AnnounceChannels;
        }

        public IReadOnlyCollection<SocketGuild> GetBotGuilds()
        {
            return _client.Guilds;
        }
    }
}
