using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arg.Parser
{
    /// <summary>
    /// used to build command
    /// </summary>
    public class CommandBuilder
    {
        private readonly string commandName;
        private readonly ArgsParserBuilder argsParserBuilder;
        private readonly List<FlagOption> argFlags = new List<FlagOption>();

        internal CommandBuilder(string commandName, ArgsParserBuilder argsParserBuilder)
        {
            this.commandName = commandName;
            this.argsParserBuilder = argsParserBuilder;
        }

        /// <summary>
        /// add support flag option for parser, if abbreviationForm and fullForm both empty/null, will throw ArgumentException when Build()
        /// </summary>
        /// <param name="abbreviationForm">command line abbreviation form argument, like -f, if pass null means not set</param>
        /// <param name="fullForm">command line full form argument, like --version, if pass null means not set</param>
        /// <param name="description">command line help info, will show if need</param>
        /// <returns>return self, you can chain call</returns>
        public CommandBuilder AddFlagOption(char? abbreviationForm, string fullForm, string description){
            argFlags.Add(new FlagOption(abbreviationForm, fullForm, description));
            return this;
        }
        
        /// <summary>
        /// add support flag option for parser
        /// </summary>
        /// <param name="abbreviationForm">command line abbreviation form argument, like -f</param>
        /// <returns>return self, you can chain call</returns>
        public CommandBuilder AddFlagOption(char abbreviationForm){
            argFlags.Add(new FlagOption(abbreviationForm));
            return this;
        }
        
        /// <summary>
        /// add support flag option for parser
        /// </summary>
        /// <param name="fullForm">command line full form argument, like --version</param>
        /// <returns>return self, you can chain call</returns>
        public CommandBuilder AddFlagOption(string fullForm){
            argFlags.Add(new FlagOption(fullForm));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArgsParserBuilder EndCommand()
        {
            argsParserBuilder.AddCommand(commandName, new ReadOnlyCollection<FlagOption>(argFlags));
            return argsParserBuilder;
        }
    }
}