namespace Arg.Parser
{
    /// <summary>
    /// Flag Option Definition
    /// </summary>
    public interface IOptionDefinitionMetadata
    {
        /// <summary>
        /// Symbol Metadata
        /// </summary>
        IOptionSymbolMetadata SymbolMetadata { get; }
        /// <summary>
        /// Description
        /// </summary>
        string Description { get; }
    }
}