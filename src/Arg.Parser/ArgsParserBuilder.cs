using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Arg.Parser
{
    /// <summary>
    /// Used to build Parser
    /// </summary>
    public class ArgsParserBuilder
    {
        private List<ICommandDefinitionMetadata> commands = new List<ICommandDefinitionMetadata>();
        
        /// <summary>
        /// begin default command, format like 'rm -rf' is default command
        /// </summary>
        /// <returns></returns>
        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(null, this);
        }
        
        /// <summary>
        /// Build a parser from you defined flag options
        /// </summary>
        /// <returns>a parser will parse you defined flag options</returns>
        public Parser Build(){
            return new Parser(new ReadOnlyCollection<ICommandDefinitionMetadata>(commands));
        }

        internal void AddCommand(string commandName, ReadOnlyCollection<FlagOption> argFlags)
        {
            var command = new CommandDefinitionMetadata(commandName, argFlags);
            if (commands.Any(c => c.Symbol == command.Symbol))
                throw new InvalidOperationException();
            commands.Add(command);
        }
    }
}
