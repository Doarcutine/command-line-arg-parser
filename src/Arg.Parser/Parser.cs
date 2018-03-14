using System.Collections.Generic;
using System;
using System.Linq;

namespace Arg.Parser
{
    public class Parser
    {
        readonly IReadOnlyCollection<ArgFlag> supportArgFlags;
        private static readonly Func<string,bool> LongNamePrefix = arg => arg[0] == '-' && arg[1] == '-';
        private static readonly Func<string,bool> ShortNamePrefix = arg => arg[0] == '-';

        public Parser(IReadOnlyCollection<ArgFlag> supportArgFlags)
        {
            ValidateSupportFlag(supportArgFlags);
            this.supportArgFlags = supportArgFlags;
        }

        private static void ValidateSupportFlag(IReadOnlyCollection<ArgFlag> argFlags)
        {
            if (argFlags.Any(f => string.IsNullOrEmpty(f.LongName) && f.ShortName == null))
            {
                throw new ApplicationException("arg opt must have at least one name, long or short");
            }
            var longNameArgs = argFlags.Where(f => !string.IsNullOrEmpty(f.LongName));
            var shortNameArgs = argFlags.Where(f => f.ShortName.HasValue);

            if (longNameArgs.Any(f => !LongArg.Match(f.LongName)))
            {
                throw new ApplicationException("long name not match requirment");
            }
            if (shortNameArgs.Any(f => !ShortArg.Match(f.ShortName.Value)))
            {
                throw new ApplicationException("short name not match requirment");
            }

            if (argFlags.Count > 1)
            {
                throw new ApplicationException("only support one flag for now");
            }
        }

        public ArgsParsingResult Parse(string[] args)
        {
            var parseResults = args.Select(Parse).ToList();
            
            var isSuccess = parseResults.All(a => a.ParseSuccess) &&
                            parseResults.All(p => supportArgFlags.Any(s => p.Result.MatchArg(s)));
            
            if (!isSuccess)
                throw new ApplicationException(parseResults.First(a => !a.ParseSuccess).ParseErrorReason);
            
            return new ArgsParsingResult(true, parseResults);
        }

        public static IParseResult<IInputArg> Parse(string arg)
        {
            if (arg.Length < 2)
                return new FailedParse<IInputArg>("argument too short");
            if (arg[0] == '-' && arg[1] == '-')
            {
                string longName = arg.Substring(2);
                return LongArg.Parse(longName);
            }
            if (arg[0] == '-')
            {
                string shortName = arg.Substring(1);
                return ShortArg.Parse(shortName);
            }
            return new FailedParse<IInputArg>("argument must start with - or --");
        }
    }
}
