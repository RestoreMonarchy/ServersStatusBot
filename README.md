# ServersStatusBot
Discord.NET bot for displaying your Steam game server's status. Use the automatically generated 
`configuration.yml` file to set up and customize the message and bot.

### Preview (default configuration)
![Preview Image](https://i.imgur.com/YJhog9h.png)

### Default Configuration
```yml
Token: TOKEN
Icon: https://i.imgur.com/rpBmHVE.png
UseCategories: true
SpaceBetweenServers: true
RefreshTime: 5
ChannelId: 525027082644750347
ColorHex: ffcc00
Title: Restore Monarchy Servers
DescriptionTop: "These are our awesome servers, be sure to join them! \n\n **Players Online:** <totalplayers>/<totalmaxplayers>"
DescriptionBottom: '[Website](https://restoremonarchy.com)  [Documentation](https://docs.restoremonarchy.com) [Discord](https://discord.gg/yBztk3w)'
CategoryFormat: __**<category>**__
ServerFormat: "**<name>** \n Players: `<players>/<maxplayers>` Map: `<map>` Address: `<address>:<port>`"
Servers:
- ServerId: RM1
  Category: Rust in Unturned
  Address: restoremonarchy.com
  Port: 27015
- ServerId: RM2
  Category: Rust in Unturned
  Address: restoremonarchy.com
  Port: 27025
- ServerId: RM3
  Category: Semi-Vanilla
  Address: restoremonarchy.com
  Port: 27045
- ServerId: RM4
  Category: Semi-Vanilla
  Address: restoremonarchy.com
  Port: 27055
```
