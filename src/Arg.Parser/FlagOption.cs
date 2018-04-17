namespace Arg.Parser
{
    class FlagOption : IOptionDefinitionMetadata
    {
        public IOptionSymbolMetadata SymbolMetadata { get; }
        public string Description { get; }
        
        internal FlagOption(char? abbreviationForm, string fullForm, string description)
        {
            this.SymbolMetadata = new OptionSymbolMetadata(abbreviationForm, fullForm);
            this.Description = description ?? string.Empty;
        }

        internal FlagOption(char abbreviationForm)
        {
            this.SymbolMetadata = new OptionSymbolMetadata(abbreviationForm, null);
            this.Description = string.Empty;
        }

        internal FlagOption(string fullForm)
        {
            this.SymbolMetadata = new OptionSymbolMetadata(null, fullForm);
            this.Description = string.Empty;
        }
    }
}
