using System;
using System.Collections.Generic;
using System.Linq;

namespace Arg.Parser
{
    public class ArgsParsingResult
    {
        public bool IsSuccess { get; }
        private IReadOnlyCollection<IInputArg> parsedArgs;
        private IReadOnlyCollection<ArgFlag> supportArgFlags;

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
                    supportArg = supportArgFlags.SingleOrDefault(s => s.ShortName == shortArg.Arg);
                    if (supportArg == null) {
                        return false;
                    }
                    break;
                case LongArg longArg:
                    supportArg = supportArgFlags.SingleOrDefault(s => s.LongName == longArg.Arg);
                    if (supportArg == null)
                    {
                        return false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var matchedShortArg = parsedArgs.SingleOrDefault(r => (r as ShortArg)?.Arg == supportArg.ShortName);
            var matchedLongArg = parsedArgs.SingleOrDefault(r => (r as LongArg)?.Arg == supportArg.LongName);
            return matchedShortArg?.Value ?? matchedLongArg?.Value ?? false;
        }
    }
}
