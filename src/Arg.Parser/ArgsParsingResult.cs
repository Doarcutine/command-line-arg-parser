using System;
using System.Collections.Generic;
using System.Linq;
using static System.Char;
// ReSharper disable InconsistentNaming

namespace Arg.Parser
{
    /// <summary>
    /// Parsing result, maybe success or fail
    /// </summary>
    public class ArgsParsingResult
    {
        /// <summary>
        /// true means parse success, false means parse fail
        /// </summary>
        public bool IsSuccess { get; }
        /// <summary>
        /// when IsSuccess is false, this field will contain failure detail
        /// when IsSuccess is true, this field will be null
        /// </summary>
        public Error Error { get;  } 
        /// <summary>
        /// input command
        /// </summary>
        public ICommandDefinitionMetadata Command { get; }
        private readonly IReadOnlyCollection<IInputArg> parsedArgs;

        internal ArgsParsingResult(ICommandDefinitionMetadata command, IReadOnlyCollection<IInputArg> parsedArgs)
        {
            this.IsSuccess = true;
            this.Command = command;
            this.parsedArgs = parsedArgs;
        }

        internal ArgsParsingResult(Error error)
        {
            this.IsSuccess = false;
            this.Error = error;
            this.Command = null;
        }

        /// <summary>
        /// get {queryArg} value from parser result
        /// </summary>
        /// <param name="queryArg">flag which you want to get value</param>
        /// <returns>true if flag appear on input, otherwise false</returns>
        public bool GetFlagValue(string queryArg)
        {
            if (queryArg == null)
                throw new ArgumentNullException();
            if (!IsSuccess)
                throw new InvalidOperationException();
            
            var queryParseResult = Parser.Parse(queryArg);
            
            if (!queryParseResult.ParseSuccess)
                throw new ArgumentException();

            if (queryParseResult.Result.Count != 1)
                throw new ArgumentException();
            
            IOptionSymbolMetadata supportArg;
            switch (queryParseResult.Result.Single())
            {
                case AbbreviationFormArg abbreviationFormArgArg:
                    supportArg = Command.GetRegisteredOptionsMetadata().SingleOrDefault(s => s.SymbolMetadata.Abbreviation.HasValue && ToLower(s.SymbolMetadata.Abbreviation.Value) == ToLower(abbreviationFormArgArg.Arg))?.SymbolMetadata;
                    if (supportArg == null) {
                        throw new ArgumentException();
                    }
                    break;
                case FullFormArg fullFormArg:
                    supportArg = Command.GetRegisteredOptionsMetadata().SingleOrDefault(s => s.SymbolMetadata.FullForm?.ToLower() == fullFormArg.Arg?.ToLower())?.SymbolMetadata;
                    if (supportArg == null)
                    {
                        throw new ArgumentException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var matchedAbbreviationFormArg = parsedArgs.SingleOrDefault(r =>
            {
                if (!(r is AbbreviationFormArg abbreviationFormArgArg)) return false;
                if (supportArg.Abbreviation == null) return false;
                return ToLower(abbreviationFormArgArg.Arg) == ToLower(supportArg.Abbreviation.Value);
            });
            var matchedFullFormArg = parsedArgs.SingleOrDefault(r =>
            {
                if (!(r is FullFormArg fullFormArg)) return false;
                if (supportArg.FullForm == null) return false;
                return string.Equals(fullFormArg.Arg, supportArg.FullForm, StringComparison.OrdinalIgnoreCase);
            });
            return matchedAbbreviationFormArg?.Value ?? matchedFullFormArg?.Value ?? false;
        }
    }
}
