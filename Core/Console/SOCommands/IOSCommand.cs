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
        string[] Compress (params string[] parameters);
        string[] Upload(params string[] parameters);
        string[] NavigateTo(params string[] parameters);

        string[] Move(params string[] parameters);

        OSPlatform Platform();
    }
}
