﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarterPack.Core.Helpers;
using StarterPack.Core.Persistence;
using StarterPack.Core.Seeders;
using System.Linq;
using StarterPack.Core.Console;

namespace StarterPack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length > 0 && args.First().Equals("sp")){
                CLI.Run(args);
            }
            else {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseWebRoot("public")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();

                host.Run();

            }
        }
    }
}
