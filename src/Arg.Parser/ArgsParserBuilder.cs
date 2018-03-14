using System.Collections.Generic;

namespace Arg.Parser
{
    public class ArgsParserBuilder
    {
        List<Command> commands = new List<Command>();

        public ArgsParserBuilder AddFlagOption(
            string commandFullName,
            string commandAbbrName,
            string description){

            Command command = new Command(commandFullName, commandAbbrName, description);
            commands.Add(command);
            return this;
        }

        public Parser Build(){
            return new Parser(commands);
        }
    }
}
