using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arg.Parser
{
    public class ArgsParserBuilder
    {
        private readonly List<ArgFlag> argFlags = new List<ArgFlag>();

        public ArgsParserBuilder AddFlagOption(char shortName, string longName, string description){
            argFlags.Add(new ArgFlag(shortName, longName, description));
            return this;
        }
        
        public ArgsParserBuilder AddFlagOption(char shortName){
            argFlags.Add(new ArgFlag(shortName));
            return this;
        }
        
        public ArgsParserBuilder AddFlagOption(string longName){
            argFlags.Add(new ArgFlag(longName));
            return this;
        }

        public Parser Build(){
            return new Parser(new ReadOnlyCollection<ArgFlag>(argFlags));
        }
    }
}
