﻿using Discord.Commands;
using System.Threading.Tasks;
using System;
using WazeBotDiscord.ServerJoin;
using WazeBotDiscord.Utilities;
using Discord.WebSocket;

namespace WazeBotDiscord.Modules
{
    [Group("serverjoin")]
    [RequireChampInNationalAdminInGlobal]
    public class ServerJoinModule : ModuleBase
    {
        readonly ServerJoinService _serverJoinSvc;

        public ServerJoinModule(ServerJoinService serverJoinSvc)
        {
            _serverJoinSvc = serverJoinSvc;
        }

        [Command("add")]
        public async Task Add([Remainder]string message)
        {
            if (message == null)
            {
                await ReplyAsync($"{Context.Message.Author.Mention}: You must specify a message to display.");
                return;
            }

            var result = await _serverJoinSvc.AddServerMessage(Context.Guild.Id, message);

            var reply = $"{Context.Message.Author.Mention}: server join message added.";
            if (result == false)
                reply = $"{Context.Message.Author.Mention}: server join message modified.";

            await ReplyAsync(reply);

        }

        [Command("remove")]
        public async Task Remove()
        {
            var removed = await _serverJoinSvc.RemoveServerMessage(Context.Guild.Id);

            if (removed)
                await ReplyAsync($"Removed server join message from {Context.Guild.Name}");
            else
                await ReplyAsync("No server join message was set for this server.");
        }
    }

}
