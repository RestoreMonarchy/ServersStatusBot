using System.Collections.Generic;

namespace RestoreMonarchy.ServersStatusBot.Models
{
    public class Configuration
    {
        public string Token { get; set; }
        public bool UseCategories { get; set; }
        public bool SpaceBetweenServers { get; set; }
        public bool ShowLastRefresh { get; set; }
        public int RefreshTime { get; set; }
        public ulong ChannelId { get; set; }
        public string ColorHex { get; set; }
        public string Title { get; set; }
        public string DescriptionTop { get; set; }
        public string DescriptionBottom { get; set; }        
        public string FooterIcon { get; set; }
        public string Footer { get; set; }
        public string CategoryFormat { get; set; }
        public string ServerFormat { get; set; }
        public List<Server> Servers { get; set; }
        public string AuthorText { get; set; }
        public string AuthorUrl { get; set; }
        public string AuthorIconUrl { get; set; }
        public bool UseAuthor { get; set; }

        public void LoadDefaults()
        {
            Token = "TOKEN";
            UseCategories = true;
            SpaceBetweenServers = true;
            ShowLastRefresh = true;
            RefreshTime = 5;
            ChannelId = 525027082644750347;
            ColorHex = "ffcc00";
            Title = "Restore Monarchy Servers";
            DescriptionTop = "These are our awesome servers, be sure to join them! \n\n **Players Online:** <totalplayers>/<totalmaxplayers>";
            DescriptionBottom = "[Website](https://restoremonarchy.com)  [Documentation](https://docs.restoremonarchy.com) [Discord](https://discord.gg/yBztk3w)";
            CategoryFormat = "__**<category>**__";
            FooterIcon = "https://i.imgur.com/rpBmHVE.png";
            Footer = "Made by RestoreMonarchy.com";
            ServerFormat = "**<name>** \n Players: `<players>/<maxplayers>` Map: `<map>` Address: `<address>:<port>`";
            Servers = new List<Server>() { new Server("RM1", "Rust in Unturned", "restoremonarchy.com", 27015), new Server("RM2", "Rust in Unturned", "restoremonarchy.com", 27025),
                new Server("RM3", "Semi-Vanilla", "restoremonarchy.com", 27045), new Server("RM4", "Semi-Vanilla", "restoremonarchy.com", 27055) };
            AuthorText = "Text";
            AuthorUrl = "URL";
            AuthorIconUrl = "https://i.imgur.com/rpBmHVE.png";
            UseAuthor = false;
        }
    }
}
