namespace Arg.Parser
{
    class ArgFlag
    {
        internal char? ShortName { get; }
        internal string LongName { get; }
        internal string Description { get; }
        
        internal ArgFlag(char? shortName, string longName, string description)
        {
            this.ShortName = shortName;
            this.LongName = longName;
            this.Description = description;
        }

        internal ArgFlag(char shortName)
        {
            this.ShortName = shortName;
        }

        internal ArgFlag(string longName)
        {
            this.LongName = longName;
        }
    }
}
