namespace Arg.Parser
{
    class FlagOption
    {
        internal char? ShortName { get; }
        internal string LongName { get; }
        internal string Description { get; }
        
        internal FlagOption(char? shortName, string longName, string description)
        {
            this.ShortName = shortName;
            this.LongName = longName;
            this.Description = description;
        }

        internal FlagOption(char shortName)
        {
            this.ShortName = shortName;
        }

        internal FlagOption(string longName)
        {
            this.LongName = longName;
        }
    }
}
