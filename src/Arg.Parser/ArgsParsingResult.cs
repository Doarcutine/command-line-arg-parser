using System;
using System.Collections.Generic;
using System.Linq;
using static System.Char;

namespace Arg.Parser
{
    public class ArgsParsingResult
    {
        public bool IsSuccess { get; }
        private readonly IReadOnlyCollection<IInputArg> parsedArgs;
        private readonly IReadOnlyCollection<ArgFlag> supportArgFlags;

        public ArgsParsingResult(bool isSuccess, IReadOnlyCollection<IInputArg> parsedArgs, IReadOnlyCollection<ArgFlag> supportArgFlags)
        {
            this.IsSuccess = isSuccess;
            this.parsedArgs = parsedArgs;
            this.supportArgFlags = supportArgFlags;
        }

        public bool GetFlagValue(string queryArg)
        {
            var queryParseResult = Parser.Parse(queryArg);
            if (!queryParseResult.ParseSuccess)
                return false;
            ArgFlag supportArg;
            switch (queryParseResult.Result)
            {
                case ShortArg shortArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.ShortName.HasValue && ToLower(s.ShortName.Value) == ToLower(shortArg.Arg));
                    if (supportArg == null) {
                        return false;
                    }
                    break;
                case LongArg longArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.LongName?.ToLower() == longArg.Arg?.ToLower());
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
                if (supportArg.ShortName == null) return false;
                return ToLower(shortArg.Arg) == ToLower(supportArg.ShortName.Value);
            });
            var matchedLongArg = parsedArgs.SingleOrDefault(r => (r as LongArg)?.Arg?.ToLower() == supportArg.LongName?.ToLower());
            return matchedShortArg?.Value ?? matchedLongArg?.Value ?? false;
        }
    }
}
