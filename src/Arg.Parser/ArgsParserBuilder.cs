using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arg.Parser
{
    /// <summary>
    /// Used to build Parser
    /// </summary>
    public class ArgsParserBuilder
    {
        private Dictionary<ICommandDefinitionMetadata, ReadOnlyCollection<FlagOption>> commandWithArgFlags = new Dictionary<ICommandDefinitionMetadata, ReadOnlyCollection<FlagOption>>();
        
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
            return new Parser(new ReadOnlyDictionary<ICommandDefinitionMetadata,ReadOnlyCollection<FlagOption>>(commandWithArgFlags));
        }

        internal void AddCommand(string commandName, ReadOnlyCollection<FlagOption> argFlags)
        {
            var command = new CommandDefinitionMetadata(commandName);
            if (commandWithArgFlags.ContainsKey(command))
                throw new InvalidOperationException();
            commandWithArgFlags.Add(command, argFlags);
        }
    }
}
