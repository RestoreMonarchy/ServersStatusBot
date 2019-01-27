using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using Bot;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;

namespace Bot.other
{
    public class Server
    {
        public static string[] servers()
        {

            var settings = ReadConfig.GetAppSettings();

            var myServers = settings.GetSection("servers").Get<string[]>();

            return myServers;

        }

        public static int GetPlayers(string url)
        {

            string json = new WebClient().DownloadString(url);

            
            
            ServerDetails myServer = JsonConvert.DeserializeObject<ServerDetails>(json);

            int players = int.Parse(myServer.players);

            return players;
        }

        public static int GetAllPlayers()
        {
            int players = 0;

            foreach (string server in servers())
            {
                var settings = ReadConfig.GetAppSettings();
                string getURL = settings["url"];
                string url = getURL + server;

                players = players + GetPlayers(url);
            }

            return players;
        }

        public static int GetAllSlots()
        {
            int slots = 0;

            var settings = ReadConfig.GetAppSettings();
            string getSlots = settings["slots"];
            int setSlots = int.Parse(getSlots);

            foreach (string server in servers())
            {
                slots = slots + setSlots;
            }

            return slots;
        }

        public static object Embed(EmbedBuilder builder)
        {

            foreach (string api in servers())
            {
                string url = "https://unturned-servers.net/api/?object=servers&element=detail&key=" + api;
                string json = new WebClient().DownloadString(url);
                ServerDetails myServer = JsonConvert.DeserializeObject<ServerDetails>(json);

                builder.AddField($"{myServer.hostname.Substring(1)}", $"`{myServer.address}:{myServer.port} • {myServer.map} • {myServer.players}/{myServer.maxplayers}`");
            }

            

            return builder;
        }

    }


    public class ServerDetails
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string port { get; set; }
        public string _private { get; set; }
        public string password { get; set; }
        public int query_port { get; set; }
        public string location { get; set; }
        public string hostname { get; set; }
        public string map { get; set; }
        public string is_online { get; set; }
        public string players { get; set; }
        public string maxplayers { get; set; }
        public string version { get; set; }
        public string platform { get; set; }
        public string uptime { get; set; }
        public string score { get; set; }
        public string rank { get; set; }
        public string votes { get; set; }
        public string favorited { get; set; }
        public string comments { get; set; }
        public string url { get; set; }
        public string last_check { get; set; }
        public string last_online { get; set; }
    }

}
