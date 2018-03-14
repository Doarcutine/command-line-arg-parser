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

        public ArgsParsingResult(bool isSuccess, IReadOnlyCollection<IInputArg> parseResults)
        {
            this.IsSuccess = isSuccess;
            this.results = parseResults;
        }


        public bool GetFlagValue(string s)
        {
            var queryParseResult = Parser.Parse(s);
            if (!queryParseResult.ParseSuccess)
                return false;
            var x = queryParseResult.Result;
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
