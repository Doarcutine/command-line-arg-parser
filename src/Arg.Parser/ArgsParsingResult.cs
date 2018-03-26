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
        private readonly IReadOnlyCollection<IInputArg> parsedArgs;
        private readonly IReadOnlyCollection<FlagOption> supportArgFlags;

        internal ArgsParsingResult(IReadOnlyCollection<IInputArg> parsedArgs, IReadOnlyCollection<FlagOption> supportArgFlags)
        {
            this.IsSuccess = true;
            this.parsedArgs = parsedArgs;
            this.supportArgFlags = supportArgFlags;
        }

        internal ArgsParsingResult(Error error)
        {
            this.IsSuccess = false;
            this.Error = error;
            this.supportArgFlags = new List<FlagOption>();
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
            
            FlagOption supportArg;
            switch (queryParseResult.Result.Single())
            {
                case AbbreviationFormArg abbreviationFormArgArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.AbbreviationForm.HasValue && ToLower(s.AbbreviationForm.Value) == ToLower(abbreviationFormArgArg.Arg));
                    if (supportArg == null) {
                        throw new ArgumentException();
                    }
                    break;
                case FullFormArg fullFormArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.FullForm?.ToLower() == fullFormArg.Arg?.ToLower());
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
                if (supportArg.AbbreviationForm == null) return false;
                return ToLower(abbreviationFormArgArg.Arg) == ToLower(supportArg.AbbreviationForm.Value);
            });
            var matchedFullFormArg = parsedArgs.SingleOrDefault(r => (r as FullFormArg)?.Arg?.ToLower() == supportArg.FullForm?.ToLower());
            return matchedAbbreviationFormArg?.Value ?? matchedFullFormArg?.Value ?? false;
        }
    }
}
