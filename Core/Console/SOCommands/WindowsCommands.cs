using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace StarterPack.Core.Console.SOCommands
{
    public class WindowsDeployCommands : IOSCommand
    {
		public OSPlatform Platform()
		{
			return OSPlatform.Linux;
		}

		public string[] Copy(params string[] parameters)
		{
			parameters[0] = "\"" + parameters[0] + "\"";
			parameters[1] = "\"" + parameters[1] + "\"";
			var command = new List<string>() {"cmd.exe", "/c", "copy"};
            command.AddRange(parameters);
            return command.ToArray();
		}


        public string[] DeleteFolder(params string[] parameters)
		{
			var command = new List<string>() {"cmd.exe", "/c", "rmdir", "/S", "/Q"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		public string[] DeleteFile(params string[] parameters)
		{
			var command = new List<string>() {"cmd.exe", "/c", "del", "/F", "/Q"};
            command.AddRange(parameters);
            return command.ToArray();
		}

        public string[] CreateFolder(params string[] parameters){
            var command = new List<string>() {"cmd.exe", "/c", "mkdir"};
			parameters[0] = "\"" + parameters[0] + "\"";
            command.AddRange(parameters);
            return command.ToArray();
        }

		public string[] Gulp(params string[] parameters)
		{
            var command = new List<string>() {"cmd.exe", "/c", "gulp"};
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
