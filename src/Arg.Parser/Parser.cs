using System.Collections.Generic;

namespace Arg.Parser
{
    public class Parser
    {
        List<Command> supportCommands = new List<Command>();
        List<Command> inputCommands = new List<Command>();

        public Parser(List<Command> supportCommands)
        {
            this.supportCommands = supportCommands;
        }
    }
}
