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
            var command = new List<string>() {"mkdir"};
            command.AddRange(parameters);
            return command.ToArray();
        }

		public string[] Gulp(params string[] parameters)
		{
            var command = new List<string>() {"gulp"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		public string[] Upload(params string[] parameters)
		{
			var command = new List<string>() {"ftp"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		public string[] Compress(params string[] parameters)
		{
			var command = new List<string>() {"zip", "-r"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		public string[] NavigateTo(params string[] parameters)
		{
			var command = new List<string>() {"cd"};
            command.AddRange(parameters);
            return command.ToArray();
		}

        public string[] Move(params string[] parameters)
		{
			var command = new List<string>() {"mv"};
            command.AddRange(parameters);
            return command.ToArray();
		}
	}
}
