using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace HouseBot
{
    public class announceModule : ModuleBase<SocketCommandContext>
    {

        [Command("announce")]
        [Summary("makes an announcement")]
        public async Task AnnounceAsync([Remainder] [Summary("Text to Announce")] string announce)
        {
            await Context.Guild.GetTextChannel(876551668827848755).SendMessageAsync(announce);

        }


    }
}
