using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Timers;
using Bot.other;

using Discord;
using Discord.Commands;
using System.Net;
using System.Threading;

namespace Bot.commands
{
    public class Servers : ModuleBase<SocketCommandContext>
    {
        [Command("servers"), Alias("status"), Summary("Servers Display")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MCrow()
        {

            var builder = new EmbedBuilder();
            builder.WithColor(0, 162, 255);
            Server.Embed(builder);

            

            var embed = builder.Build();
            

            var message = await Context.Channel.SendMessageAsync("", false, embed);

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            while (true) {
                Console.WriteLine("Updating info...");
                var builderUpdate = new EmbedBuilder();
                builderUpdate.WithColor(0, 162, 255);

                Server.Embed(builderUpdate);

                var embedUpdate = builderUpdate.Build();

                await message.ModifyAsync(x =>
                {
                    x.Embed = embedUpdate;
                });
                Thread.Sleep(60000);
            }


            


            


            
        }

    }
}