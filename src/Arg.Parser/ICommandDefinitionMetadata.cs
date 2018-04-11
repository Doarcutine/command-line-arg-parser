namespace Arg.Parser
{
    /// <summary>
    /// Command definition metadata
    /// </summary>
    public interface ICommandDefinitionMetadata
    {
        /// <summary>
        /// command symbol
        /// </summary>
        string Symbol { get; }
    }

    class CommandDefinitionMetadata : ICommandDefinitionMetadata
    {
        public string Symbol { get; }

        public CommandDefinitionMetadata(string symbol)
        {
            Symbol = symbol;
        }

        bool Equals(CommandDefinitionMetadata other)
        {
            return string.Equals(Symbol, other.Symbol);
        }

        /// <summary>
        /// equals method, will compare by Symbol
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommandDefinitionMetadata) obj);
        }

        /// <summary>
        /// hashCode method, will decide by Symbol
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (Symbol != null ? Symbol.GetHashCode() : 0);
        }
    }
}