using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StarterPack.Core.Console.Deploy
{
    public class LinuxDeployCommands : IDeployCommands
    {
		public OSPlatform Platform()
		{
			return OSPlatform.Linux;
		}

		string[] IDeployCommands.Copy(params string[] parameters)
		{
			var command = new List<string>() {"cp", "-R"};
            command.AddRange(parameters);
            return command.ToArray();
		}

        string[] IDeployCommands.DeleteFile(params string[] parameters)
		{
			var command = new List<string>() {"rm", "-f"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		string[] IDeployCommands.DeleteFolder(params string[] parameters)
		{
			var command = new List<string>() {"rm", "-Rf"};
            command.AddRange(parameters);
            return command.ToArray();
		}

        string[] IDeployCommands.CreateFolder(params string[] parameters){
            var command = new List<string>() {"mkdir"};
            command.AddRange(parameters);
            return command.ToArray();
        }

		string[] IDeployCommands.Gulp(params string[] parameters)
		{
            var command = new List<string>() {"gulp"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		string[] IDeployCommands.Upload(params string[] parameters)
		{
			var command = new List<string>() {"ftp"};
            command.AddRange(parameters);
            return command.ToArray();
		}

		string[] IDeployCommands.Compress(params string[] parameters)
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

        string[] IDeployCommands.Move(params string[] parameters)
		{
			var command = new List<string>() {"mv"};
            command.AddRange(parameters);
            return command.ToArray();
		}
	}
}
