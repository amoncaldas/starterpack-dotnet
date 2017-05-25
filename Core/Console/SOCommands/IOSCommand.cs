using System.Runtime.InteropServices;

namespace StarterPack.Core.Console.SOCommands
{
    public interface IOSCommand
    {
        string[] Copy(params string[] parameters);
        string[] DeleteFolder(params string[] parameters);

        string[] DeleteFile(params string[] parameters);
        string[] CreateFolder(params string[] parameters);
        string[] Gulp(params string[] parameters);

        OSPlatform Platform();
    }
}
