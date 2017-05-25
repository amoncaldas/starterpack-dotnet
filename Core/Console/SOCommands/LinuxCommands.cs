using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StarterPack.Core.Console.SOCommands
{
    public class LinuxDeployCommands : IOSCommand
    {
		public OSPlatform Platform()
		{
			return OSPlatform.Linux;
		}

		public string[] Copy(params string[] parameters)
		{
			var command = new List<string>() {"cp", "-R"};
            command.AddRange(parameters);
            return command.ToArray();
		}

        public string[] DeleteFile(params string[] parameters)
		{
			var command = new List<string>() {"rm", "-f"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		public string[] DeleteFolder(params string[] parameters)
		{
			var command = new List<string>() {"rm", "-Rf"};
            command.AddRange(parameters);
            return command.ToArray();
		}

        public string[] CreateFolder(params string[] parameters){
            var command = new List<string>() {"mkdir", "-p"};
            command.AddRange(parameters);
            return command.ToArray();
        }

		public string[] Gulp(params string[] parameters)
		{
            var command = new List<string>() {"gulp"};
            command.AddRange(parameters);
            return command.ToArray();
		}

	}
}
