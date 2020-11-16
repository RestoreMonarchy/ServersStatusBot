using Discord;
using Discord.WebSocket;
using RestoreMonarchy.ServersStatusBot.Helpers;
using RestoreMonarchy.ServersStatusBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace RestoreMonarchy.ServersStatusBot.Services
{
    public class ServerService
    {
        private readonly DiscordSocketClient client;
        private readonly Configuration configuration;

        private SocketTextChannel serversChannel;
        private IUserMessage serversMessage;
        public ServerService(DiscordSocketClient client, Configuration configuration)
        {
            this.client = client;
            this.configuration = configuration;
        }

        public async Task Client_Ready()
        {
            serversChannel = client.GetChannel(configuration.ChannelId) as SocketTextChannel;

            if (serversChannel == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Couldn't find channel {configuration.ChannelId}. Stopping the bot...");
                Console.ResetColor();
                await client.StopAsync();
                return;
            }

            await GetMessage();
            
            Timer timer = new Timer(configuration.RefreshTime * 1000);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public Embed GetEmbed()
        {
            EmbedBuilder eb = new EmbedBuilder();

            List<KeyValuePair<string, string>> servers = new List<KeyValuePair<string, string>>();

            int totalPlayers = 0;
            int totalMaxPlayers = 0;

            eb.WithColor(Convert.ToUInt32(configuration.ColorHex, 16));

            foreach (Server server in configuration.Servers)
            {
                SteamServerInfo info = new SteamServerInfo(server.Address + ':' + server.Port);

                if (!info.Online)
                {
                    Console.WriteLine($"Failed to connect to {server.ServerId} ({server.Address}:{server.Port})");
                    continue;
                }                    

                string serverText = configuration.ServerFormat;

                foreach (PropertyInfo property in info.GetType().GetProperties())
                {
                    serverText = serverText.Replace($"<{property.Name}>", property.GetValue(info).ToString(), StringComparison.OrdinalIgnoreCase);
                }
                serverText = serverText.Replace("<address>", server.Address, StringComparison.OrdinalIgnoreCase);

                servers.Add(new KeyValuePair<string, string>(server.Category, serverText));
                totalPlayers += info.Players;
                totalMaxPlayers += info.MaxPlayers;
            }

            if (!string.IsNullOrEmpty(configuration.Title))
                eb.WithTitle(configuration.Title);

            string serversText;

            if (configuration.UseCategories)
            {
                IEnumerable<string> distinctCategories = servers.Select(x => x.Key).Distinct();

                List<string> categoryServers = new List<string>();
                foreach (string category in distinctCategories)
                {
                    categoryServers.Add(configuration.CategoryFormat.Replace("<category>", category, StringComparison.OrdinalIgnoreCase) + "\n\n" + string.Join(configuration.SpaceBetweenServers ? "\n\n" : "\n",
                        servers.Where(x => x.Key == category).Select(x => x.Value)));
                }

                serversText = string.Join("\n\n", categoryServers);
            }
            else
            {
                serversText = string.Join(configuration.SpaceBetweenServers ? "\n\n" : "\n", servers.Select(x => x.Value));
            }

            string description = configuration.DescriptionTop + "\n\n" + serversText + "\n\n" + configuration.DescriptionBottom;

            eb.WithDescription(description.Replace("<totalplayers>", totalPlayers.ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("<totalmaxplayers>", totalMaxPlayers.ToString(), StringComparison.OrdinalIgnoreCase));


            if (!string.IsNullOrEmpty(configuration.Footer))
                eb.WithFooter(configuration.Footer, configuration.FooterIcon);

            if (configuration.UseAuthor)
                eb.WithAuthor(configuration.AuthorText, configuration.AuthorIconUrl, configuration.AuthorUrl);

            if (configuration.ShowLastRefresh)
                eb.WithCurrentTimestamp();

            return eb.Build();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                await serversMessage.ModifyAsync(x => { x.Content = string.Empty; x.Embed = GetEmbed(); });
            }
            catch
            {
                await GetMessage();
            }
        }

        private async Task GetMessage()
        {
            var messages = await serversChannel.GetMessagesAsync(5).FlattenAsync();
            var userMessage = messages.FirstOrDefault(x => x.Author.Id == client.CurrentUser.Id) as IUserMessage;

            if (userMessage != null)
            {
                await userMessage.ModifyAsync(x => x.Embed = GetEmbed());
            }
            else
            {                
                try
                {
                    userMessage = await serversChannel.SendMessageAsync(embed: GetEmbed());
                } catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e);
                    Console.ResetColor();
                    await client.StopAsync();
                    return;
                }
                this.serversMessage = userMessage;
            }            
        }
    }
}
