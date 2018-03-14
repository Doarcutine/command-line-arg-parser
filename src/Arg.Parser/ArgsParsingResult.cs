using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Arg.Parser
{
    public class ArgsParsingResult
    {
        public bool IsSuccess { get; }
        private IReadOnlyCollection<IInputArg> results;
        private IReadOnlyCollection<ArgFlag> supportArgFlags;

        public ArgsParsingResult(bool isSuccess, IReadOnlyCollection<IInputArg> parseResults, IReadOnlyCollection<ArgFlag> supportArgFlags)
        {
            this.IsSuccess = isSuccess;
            this.results = parseResults;
            this.supportArgFlags = supportArgFlags;
        }


        public bool GetFlagValue(string expectArg)
        {
            var queryParseResult = Parser.Parse(expectArg);
            if (!queryParseResult.ParseSuccess)
                return false;
            var x = queryParseResult.Result;
            ArgFlag supportArg = null;
            if (x is ShortArg)
            {
                supportArg = supportArgFlags.SingleOrDefault(s => s.ShortName == ((ShortArg)x).Arg);
                if (supportArg == null) {
                    return false;
                }
                return results.Any(r => (r as ShortArg)?.Arg == supportArg.ShortName)
                       || results.Any(r => (r as LongArg)?.Arg == supportArg.LongName);
            }
            if (x is LongArg)
            {
                supportArg = supportArgFlags.SingleOrDefault(s => s.LongName == ((LongArg)x).Arg);
                if (supportArg == null)
                {
                    return false;
                }
            }
            return results.Any(r => (r as ShortArg)?.Arg == supportArg.ShortName)
                    || results.Any(r => (r as LongArg)?.Arg == supportArg.LongName);
        }
    }

    class ParsedArg
    {
        public ParsedArg(char? argShortName, string argLongName, bool b)
        {
            throw new NotImplementedException();
        }

        public char? ShortName { get; }
        public string LongName { get; }
        public bool Value { get; } 
    }
}
