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
        /// add support flag option for parser
        /// </summary>
        /// <param name="shortName">command line short argument, like -f</param>
        /// <param name="longName">command line long argument, like --version</param>
        /// <param name="description">command line help info, will show if need</param>
        /// <returns>return self, you can chain call</returns>
        public ArgsParserBuilder AddFlagOption(char shortName, string longName, string description){
            argFlags.Add(new FlagOption(shortName, longName, description));
            return this;
        }
        
        /// <summary>
        /// add support flag option for parser
        /// </summary>
        /// <param name="shortName">command line short argument, like -f</param>
        /// <returns>return self, you can chain call</returns>
        public ArgsParserBuilder AddFlagOption(char shortName){
            argFlags.Add(new FlagOption(shortName));
            return this;
        }
        
        /// <summary>
        /// add support flag option for parser
        /// </summary>
        /// <param name="longName">command line long argument, like --version</param>
        /// <returns>return self, you can chain call</returns>
        public ArgsParserBuilder AddFlagOption(string longName){
            argFlags.Add(new FlagOption(longName));
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
