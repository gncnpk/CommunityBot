﻿using Discord.WebSocket;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using WazeBotDiscord.ChannelSync;
using Discord.Commands;
using WazeBotDiscord.Utilities;

namespace WazeBotDiscord.Modules
{

    [Group("channelsync")]
    [RequireAppOwner]
    public class ChannelSyncModule : ModuleBase
    {
        readonly ChannelSyncService _channelSyncSvc;

        public ChannelSyncModule(ChannelSyncService channelSyncSvc)
        {
            _channelSyncSvc = channelSyncSvc;
        }

        [Command("add"), Priority(10)]
        public async Task Add(string channelId1, string channelId2)
        {
            ulong channelID1;
            if (channelId1.StartsWith("<#") && channelId1.EndsWith(">"))
                channelID1 = Convert.ToUInt64(channelId1.TrimStart('<').TrimStart('#').TrimEnd('>'));
            else
                channelID1 = Convert.ToUInt64(channelId1);

            ulong channelID2;
            if (channelId2.StartsWith("<#") && channelId2.EndsWith(">"))
                channelID2 = Convert.ToUInt64(channelId2.TrimStart('<').TrimStart('#').TrimEnd('>'));
            else
                channelID2 = Convert.ToUInt64(channelId2);

            var channels = _channelSyncSvc.getSyncChannels(channelID1);
            if (channels != null) {
                await ReplyAsync($"<#{channelID1}>({channelID1}) is already syncing with a channel.  A channel can only sync to one other channel.");
                return;
            }
            channels = _channelSyncSvc.getSyncChannels(channelID2);
            if (channels != null)
            {
                await ReplyAsync($"<#{channelID2}>({channelID2}) is already syncing with a channel.  A channel can only sync to one other channel.");
                return;
            }

            if (channelId1 == channelId2)
            {
                await ReplyAsync("You cannot sync a channel to itself.");
                return;
            }

            var result = await _channelSyncSvc.AddChannelSync(channelID1, channelID2, Context.Message.Author.Id, DateTime.UtcNow, Context.Message.Author.Username);
            if (result)
            {
                await ReplyAsync($"<@{Context.Message.Author.Id}> Channels sync'd");
            }
        }

        [Command("remove"), Priority(9)]
        [Alias("delete")]
        public async Task Remove(string channelId)
        {
            ulong channelID;
            if (channelId.StartsWith("<#") && channelId.EndsWith(">"))
                channelID = Convert.ToUInt64(channelId.TrimStart('<').TrimStart('#').TrimEnd('>'));
            else
                channelID = Convert.ToUInt64(channelId);

            var channels = _channelSyncSvc.getSyncChannels(channelID);
            if (channels == null)
            {
                await ReplyAsync($"Channel {channelID} is not sync'd to any other channels.");
                return;
            }

            var result = await _channelSyncSvc.RemoveChannelSync(channelID);
            if (result)
                await ReplyAsync($"<@{Context.Message.Author.Id}> sync removed for channel <#{channelID}> ({channelID})");
            else
                await ReplyAsync($"<@{Context.Message.Author.Id}> failed to remove channel sync");
        }
    }
}
