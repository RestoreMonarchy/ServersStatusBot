using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RestoreMonarchy.ServersStatusBot.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestoreMonarchy.ServersStatusBot.Models;
using System.IO;
using YamlDotNet.Serialization;

namespace ServersStatusBot
{
    class Program
    {
        public Configuration Configuration { get; set; }
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult(); 
        }

        public async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ServersStatusBot by RestoreMonarchy.com");
            bool configLoaded = true;

            if (!File.Exists("configuration.yaml"))
            {
                using (StreamWriter configFile = new StreamWriter("configuration.yaml"))
                {
                    Serializer serializer = new SerializerBuilder().Build();
                    Configuration = new Configuration();
                    Configuration.LoadDefaults();
                    string configText = serializer.Serialize(Configuration);
                    configFile.Write(configText);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Configuration file generated. Change the bot's token!");                    
                }
            }
            else
            {
                try
                {
                    Deserializer deserializer = new DeserializerBuilder().Build();
                    Configuration = deserializer.Deserialize<Configuration>(File.ReadAllText("configuration.yaml"));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Configuration loaded successfully!");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[Config Exception] ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(e.Message);
                    Console.ResetColor();
                    configLoaded = false;
                }

            }
            Console.ResetColor();
            using (var services = ConfigureServices())
            {
                if (configLoaded)
                {
                    var client = services.GetRequiredService<DiscordSocketClient>();

                    client.Log += LogAsync;

                    try
                    {
                        await client.LoginAsync(TokenType.Bot, Configuration.Token);
                    } catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid bot token");
                        Console.ResetColor();
                        return;
                    }
                    
                    await client.StartAsync();

                    client.Ready += services.GetRequiredService<ServerService>().Client_Ready;

                    await Task.Delay(-1);
                } else
                {
                    Environment.Exit(0);
                }
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<ServerService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<Configuration>(Configuration)
                .BuildServiceProvider();
        }
    }
}
