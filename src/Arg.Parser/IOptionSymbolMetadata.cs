namespace Arg.Parser
{
    /// <summary>
    /// Flag Option Definition Metadata
    /// </summary>
    public interface IOptionSymbolMetadata
    {
        /// <summary>
        /// Abbreviation form
        /// </summary>
        char? Abbreviation { get; }
        /// <summary>
        /// Full form
        /// </summary>
        string FullForm { get; }
    }

    class OptionSymbolMetadata : IOptionSymbolMetadata
    {
        public char? Abbreviation { get; }
        public string FullForm { get; }

        internal OptionSymbolMetadata(char? abbreviationForm, string fullForm)
        {
            this.Abbreviation = abbreviationForm;
            this.FullForm = fullForm;
        }
    }
}