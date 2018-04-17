using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        /// <summary>
        /// command description
        /// </summary>
        string Description { get; }
        /// <summary>
        /// command support flag options
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata();
    }

    class CommandDefinitionMetadata : ICommandDefinitionMetadata
    {
        public string Symbol { get; }
        public string Description { get; }
        private readonly IReadOnlyCollection<IOptionDefinitionMetadata> registeredOptions;
        
        public IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata()
        {
            return registeredOptions;
        }

        public CommandDefinitionMetadata(string symbol, IReadOnlyCollection<IOptionDefinitionMetadata> registeredOptions)
        {
            Symbol = symbol;
            Description = string.Empty;
            this.registeredOptions = new ReadOnlyCollection<IOptionDefinitionMetadata>(registeredOptions.ToList());
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