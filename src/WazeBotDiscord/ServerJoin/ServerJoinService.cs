using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WazeBotDiscord.ServerJoin
{
    public class ServerJoinService
    {
        List<ServerJoinRecord> _serverJoinMessages;

        public async Task InitAsync()
        {
            using (var db = new WbContext())
            {
                _serverJoinMessages = await db.ServerJoinRecords.ToListAsync();
            }
        }

        /// <summary>
        /// Adds a message to the database for the server the command is run on, to be sent when a user joins a server
        /// </summary>
        /// <param name="guildID"></param>
        /// <param name="message"></param>
        /// <returns>Returns true if it is a new add, false if there was an entry and we are modifying</returns>
        public async Task<bool> AddServerMessage(ulong guildID, string message)
        {
            var existing = GetExistingJoinMessage(guildID);
            if (existing == null)
            {
                var dbSheet = new ServerJoinRecord
                {
                    GuildId = guildID,
                    JoinMessage = message
                };

                using (var db = new WbContext())
                {
                    db.ServerJoinRecords.Add(dbSheet);
                    await db.SaveChangesAsync();
                }

                _serverJoinMessages.Add(dbSheet);
                return true;
            }

            existing.GuildId = guildID;
            existing.JoinMessage = message;

            using (var db = new WbContext())
            {
                db.ServerJoinRecords.Update(existing);
                await db.SaveChangesAsync();
            }

            return false;
        }

        public async Task<bool> RemoveServerMessage(ulong guildID)
        {
            var existing = GetExistingJoinMessage(guildID);
            if (existing == null)
                return false;

            _serverJoinMessages.Remove(existing);

            using (var db = new WbContext())
            {
                db.ServerJoinRecords.Remove(existing);
                await db.SaveChangesAsync();
            }

            return true;
        }

        public ServerJoinRecord GetExistingJoinMessage(ulong guildId)
        {
            return _serverJoinMessages.Find(r => r.GuildId == guildId);
        }

        public async Task ReloadSheetsAsync()
        {
            _serverJoinMessages.Clear();

            await InitAsync();
        }
    }
}
