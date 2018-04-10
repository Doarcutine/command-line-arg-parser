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
        private Dictionary<string, ReadOnlyCollection<FlagOption>> commandWithArgFlags = new Dictionary<string, ReadOnlyCollection<FlagOption>>();
        
        /// <summary>
        /// begin default command, format like 'rm -rf' is default command
        /// </summary>
        /// <returns></returns>
        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder("", this);
        }
        
        /// <summary>
        /// Build a parser from you defined flag options
        /// </summary>
        /// <returns>a parser will parse you defined flag options</returns>
        public Parser Build(){
            return new Parser(new ReadOnlyDictionary<string,ReadOnlyCollection<FlagOption>>(commandWithArgFlags));
        }

        internal void AddCommand(string commandName, ReadOnlyCollection<FlagOption> argFlags)
        {
            if (commandWithArgFlags.ContainsKey(commandName))
                throw new InvalidOperationException();
            commandWithArgFlags.Add(commandName, argFlags);
        }
    }
}
