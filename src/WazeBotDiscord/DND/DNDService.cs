using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WazeBotDiscord.DND
{
    public class DNDService
    {
        readonly HttpClient _client;
        List<DNDListItem> _dnds;

        public DNDService(HttpClient client)
        {
            _client = client;
        }

        public async Task InitAsync()
        {
            using (var db = new WbContext())
            {
                _dnds = await db.DndList.ToListAsync();
            }
        }

        public async Task<string> GetDNDTime(ulong userID)
        {
            var dnd = _dnds.Find(s => s.UserId == userID);
            if (dnd == null || dnd.EndTime < DateTime.Now)
            {
                if (dnd != null)
                    await RemoveDND(userID);
                return "DND is not currently enabled";
            }

            TimeSpan timeLeft = dnd.EndTime - DateTime.Now;
            string returnVal = "";
            if (timeLeft.Days > 0)
                returnVal += $"{timeLeft.Days} days ";
            if (timeLeft.Hours > 0)
                returnVal += $"{timeLeft.Hours} hours ";
            if (timeLeft.Minutes > 0 || (timeLeft.Minutes == 0 && timeLeft.Seconds > 0))
                returnVal += $"{timeLeft.Minutes}.{Math.Round((double)timeLeft.Seconds / 60, 1).ToString().Substring(2)} minutes ";
            return $"{returnVal}left";
        }

        public async Task<bool> AddDND(ulong userID, DateTime endTime)
        {
            var existing = GetExistingDND(userID);
            if (existing == null)
            {
                var dbSheet = new DNDListItem
                {
                    UserId = userID,
                    EndTime = endTime
                };

                using (var db = new WbContext())
                {
                    db.DndList.Add(dbSheet);
                    await db.SaveChangesAsync();
                }

                _dnds.Add(dbSheet);
                return true;
            }

            existing.UserId = userID;
            existing.EndTime = endTime;

            using (var db = new WbContext())
            {
                db.DndList.Update(existing);
                await db.SaveChangesAsync();
            }

            return false;
        }

        public async Task<bool> RemoveDND(ulong userID)
        {
            var existing = GetExistingDND(userID);
            if (existing == null)
                return false;

            _dnds.Remove(existing);

            using (var db = new WbContext())
            {
                db.DndList.Remove(existing);
                await db.SaveChangesAsync();
            }

            return true;
        }

        public DNDListItem GetExistingDND(ulong userID)
        {
            return _dnds.Find(r => r.UserId == userID);
        }
    }
}
