namespace RestoreMonarchy.ServersStatusBot.Models
{
    public class Server
    {
        public Server(string serverId, string category, string address, int port)
        {
            this.ServerId = serverId;
            this.Category = category;
            this.Address = address;
            this.Port = port;
        }

        public Server() { }

        public string ServerId { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
    }
}
