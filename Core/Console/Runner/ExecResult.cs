using System.Collections.Generic;

namespace StarterPack.Core.Console.Runner
{
    public class ExecResult
    {
        public List<string> Messages {get;set;}
        public bool Success {get;set;}

        public ExecResult(){
            Messages = new List<string>();
        }
    }
}
