using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arg.Parser
{
    /// <summary>
    /// Used to build Parser
    /// </summary>
    public class ArgsParserBuilder
    {
        private readonly List<FlagOption> argFlags = new List<FlagOption>();

        /// <summary>
        /// add support flag option for parser, if abbreviationForm and fullForm both empty/null, will throw ArgumentException when Build()
        /// </summary>
        /// <param name="abbreviationForm">command line abbreviation form argument, like -f, if pass null means not set</param>
        /// <param name="fullForm">command line full form argument, like --version, if pass null means not set</param>
        /// <param name="description">command line help info, will show if need</param>
        /// <returns>return self, you can chain call</returns>
        public ArgsParserBuilder AddFlagOption(char? abbreviationForm, string fullForm, string description){
            argFlags.Add(new FlagOption(abbreviationForm, fullForm, description));
            return this;
        }
        
        /// <summary>
        /// add support flag option for parser
        /// </summary>
        /// <param name="abbreviationForm">command line abbreviation form argument, like -f</param>
        /// <returns>return self, you can chain call</returns>
        public ArgsParserBuilder AddFlagOption(char abbreviationForm){
            argFlags.Add(new FlagOption(abbreviationForm));
            return this;
        }
        
        /// <summary>
        /// add support flag option for parser
        /// </summary>
        /// <param name="fullForm">command line full form argument, like --version</param>
        /// <returns>return self, you can chain call</returns>
        public ArgsParserBuilder AddFlagOption(string fullForm){
            argFlags.Add(new FlagOption(fullForm));
            return this;
        }

        /// <summary>
        /// Build a parser from you defined flag options
        /// </summary>
        /// <returns>a parser will parse you defined flag options</returns>
        public Parser Build(){
            return new Parser(new ReadOnlyCollection<FlagOption>(argFlags));
        }
    }
}
