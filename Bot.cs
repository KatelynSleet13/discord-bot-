using Discord_Bot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        
        
        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("confg.json"))
           
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = ConfigJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
               MinimumLogLevel  = LogLevel.Debug,
            
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] {ConfigJson.Prefix},
                EnableDms= false,
                EnableMentionPrefix= true,
                DmHelp = true,
               

            };
            
            
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<FunCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
          {
            return Task.CompletedTask;
        }
        
    }
}
