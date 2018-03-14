using System.Collections.Generic;

namespace Arg.Parser
{
    public class ArgsParserBuilder
    {
        List<Command> commands = new List<Command>();

        public ArgsParserBuilder AddFlagOption(){
            return this;
        }

        public Parser Build(){
            return new Parser(commands);
        }
    }
}
