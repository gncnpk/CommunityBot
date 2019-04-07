using System;
using System.Collections.Generic;
using System.Text;

namespace WazeBotDiscord.ChannelSync
{
    public class SyncChannelsRow
    {
        public ulong Channel1 { get; set; }

        public ulong Channel2 { get; set; }

        public DateTime AddedAt { get; set; }

        public ulong AddedById { get; set; }

        public int Id { get; set; }

        public string AddedByName { get; set; }
    }
}
