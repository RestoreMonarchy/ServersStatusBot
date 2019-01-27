using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bot
{
    public static class ReadConfig
    {
        public static IConfigurationRoot GetAppSettings()
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json");

            return builder.Build();
        }
    }
}
