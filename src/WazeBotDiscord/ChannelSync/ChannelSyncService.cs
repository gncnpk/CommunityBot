using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WazeBotDiscord.ChannelSync
{
    public class ChannelSyncService
    {
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

        public async Task<bool> AddChannelSync(ulong channelID1, ulong channelID2,ulong AddedBy, DateTime AddedAt, string AddedByName)
        {
            var dbSheet = new SyncChannelsRow
            {
                Channel1 = channelID1,
                Channel2 = channelID2,
                AddedAt = AddedAt,
                AddedById = AddedBy,
                AddedByName = AddedByName
            };

            using (var db = new WbContext())
            {
                db.SyncChannels.Add(dbSheet);
                await db.SaveChangesAsync();

            }

            _syncdChannels.Add(dbSheet);
            return true;
        }

        public async Task<bool> RemoveChannelSync(ulong channelID)
        {
            var syncdChannels = getSyncChannels(channelID);
            if (syncdChannels == null)
                return false;

            _syncdChannels.Remove(syncdChannels);
            using (var db = new WbContext())
            {
                db.SyncChannels.Remove(syncdChannels);
                await db.SaveChangesAsync();
            }

            return true;
        }

        public async Task ReloadChannelSyncAsync()
        {
            _syncdChannels.Clear();
            await InitAsync();
        }
    }
}
