namespace Arg.Parser
{
    class FlagOption
    {
        internal char? AbbreviationForm { get; }
        internal string FullForm { get; }
        internal string Description { get; }
        
        internal FlagOption(char? abbreviationForm, string fullForm, string description)
        {
            this.AbbreviationForm = abbreviationForm;
            this.FullForm = fullForm;
            this.Description = description;
        }

        internal FlagOption(char abbreviationForm)
        {
            this.AbbreviationForm = abbreviationForm;
        }

        internal FlagOption(string fullForm)
        {
            this.FullForm = fullForm;
        }
    }
}
