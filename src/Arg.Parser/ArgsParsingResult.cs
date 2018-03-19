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
        }

        /// <summary>
        /// get {queryArg} value from parser result
        /// </summary>
        /// <param name="queryArg">flag which you want to get value</param>
        /// <returns>true if flag appear on input, otherwise false</returns>
        public bool GetFlagValue(string queryArg)
        {
            var queryParseResult = Parser.Parse(queryArg);
            if (!queryParseResult.ParseSuccess)
                return false;
            FlagOption supportArg;
            switch (queryParseResult.Result)
            {
                case ShortArg shortArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.AbbreviationForm.HasValue && ToLower(s.AbbreviationForm.Value) == ToLower(shortArg.Arg));
                    if (supportArg == null) {
                        return false;
                    }
                    break;
                case LongArg longArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.FullForm?.ToLower() == longArg.Arg?.ToLower());
                    if (supportArg == null)
                    {
                        return false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var matchedShortArg = parsedArgs.SingleOrDefault(r =>
            {
                if (!(r is ShortArg shortArg)) return false;
                if (supportArg.AbbreviationForm == null) return false;
                return ToLower(shortArg.Arg) == ToLower(supportArg.AbbreviationForm.Value);
            });
            var matchedLongArg = parsedArgs.SingleOrDefault(r => (r as LongArg)?.Arg?.ToLower() == supportArg.FullForm?.ToLower());
            return matchedShortArg?.Value ?? matchedLongArg?.Value ?? false;
        }
    }
}
