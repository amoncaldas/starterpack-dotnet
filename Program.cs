using System.IO;
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
using System.Diagnostics;

namespace StarterPack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //execute deploy command here


            if(args.Length > 0 && args.First().Equals("sp")){

                CLI.Run(args);
            }
            else {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*:5000")
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
