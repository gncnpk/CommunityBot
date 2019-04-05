using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WazeBotDiscord.ChannelSync
{
    class ChannelSyncService
    {
        readonly HttpClient _client;
        List<SyncChannelsRow> _syncdChannels;

        public ChannelSyncService()
        {
        }

        public async Task InitAsync()
        {
            using (var db = new WbContext())
            {
                _syncdChannels = await db.SyncChannels.ToListAsync();
            }
        }

        public SyncChannelsRow getSyncChannels(ulong channel) 
        {
            return _syncdChannels.Find(c => c.Channel1 == channel || c.Channel2 == channel);
        }
    }
}
