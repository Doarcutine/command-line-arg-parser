using System;

namespace Arg.Parser
{
    public class ArgFlag
    {
        public char? ShortName { get; }
        public string LongName { get; }
        public string Description { get; }
        
        public ArgFlag(char? shortName, string longName, string description)
        {
            this.ShortName = shortName;
            this.LongName = longName;
            this.Description = description;
        }

        public ArgFlag(char shortName)
        {
            this.ShortName = shortName;
        }

        public ArgFlag(string longName)
        {
            this.LongName = longName;
        }
    }
}
