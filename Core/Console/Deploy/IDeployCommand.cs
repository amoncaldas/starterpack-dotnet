using System.Runtime.InteropServices;

namespace StarterPack.Core.Console.Deploy
{
    public interface IDeployCommands
    {
        string[] Copy(params string[] parameters);
        string[] Delete(params string[] parameters);
        string[] CreateFolder(params string[] parameters);
        string[] Gulp(params string[] parameters);
        string[] Compress (params string[] parameters);
        string[] Upload(params string[] parameters);
        string[] NavigateTo(params string[] parameters);

        string[] Move(params string[] parameters);

        OSPlatform Platform();
    }
}
