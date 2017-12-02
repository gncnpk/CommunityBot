using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WazeBotDiscord.ServerLeave
{
    public class ServerLeaveService
    {
        readonly HttpClient _client;
        List<LeaveMessageChannel> _leaveChannels;

        public ServerLeaveService(HttpClient client)
        {
            _client = client;
        }

        public async Task InitServerLeaveServiceAsync()
        {
            using (var db = new WbContext())
            {
                _leaveChannels = await db.LeaveMessageChannels.ToListAsync();
            }
        }

        /// <summary>
        /// Adds a channel to the database for the user leave event on the server to notify of the user leaving
        /// </summary>
        /// <param name="guildID"></param>
        /// <param name="channelID"></param>
        /// <returns>Returns true if it is a new add, false if there was an entry and we are modifying</returns>
        public async Task<bool> AddChannelIDAsync(ulong guildID, ulong channelID)
        {
            var existing = GetExistingLeaveChannel(guildID);
            if (existing == null)
            {
                var dbSheet = new LeaveMessageChannel
                {
                    GuildId = guildID,
                    ChannelId = channelID
                };

                using (var db = new WbContext())
                {
                    db.LeaveMessageChannels.Add(dbSheet);
                    await db.SaveChangesAsync();
                }

                _leaveChannels.Add(dbSheet);
                return true;
            }

            existing.GuildId = guildID;
            existing.ChannelId = channelID;

            using (var db = new WbContext())
            {
                db.LeaveMessageChannels.Update(existing);
                await db.SaveChangesAsync();
            }

            return false;
        }

        /// <summary>
        /// Removes the set channel for the server
        /// </summary>
        /// <param name="guildID"></param>
        /// <returns>True if the channel was removed, false if there was no channel set</returns>
        public async Task<bool> RemoveServerChannelAsync(ulong guildID)
        {
            var existing = GetExistingLeaveChannel(guildID);
            if (existing == null)
                return false;

            _leaveChannels.Remove(existing);

            using (var db = new WbContext())
            {
                db.LeaveMessageChannels.Remove(existing);
                await db.SaveChangesAsync();
            }

            return true;
        }

        public LeaveMessageChannel GetExistingLeaveChannel(ulong guildId)
        {
            return _leaveChannels.Find(r => r.GuildId == guildId);
        }
    }
}
