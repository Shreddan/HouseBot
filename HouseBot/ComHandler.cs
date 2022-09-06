using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Discord.Commands;
using Discord.WebSocket;

namespace HouseBot
{
    public class ComHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;

        public ComHandler(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commandService = commands;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            await _commandService.ExecuteAsync(context: context, argPos: argPos, services: null);
        }
    }
}
