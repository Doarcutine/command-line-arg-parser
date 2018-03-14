using System.Collections.Generic;
using System;
using System.Linq;

namespace Arg.Parser
{
    public class Parser
    {
        private readonly IReadOnlyCollection<ArgFlag> supportArgFlags;
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
            var longNameArgs = argFlags.Where(f => !string.IsNullOrEmpty(f.LongName)).ToList();
            var shortNameArgs = argFlags.Where(f => f.ShortName.HasValue).ToList();

            if (longNameArgs.Any(f => !LongArg.Requirment(f.LongName)))
            {
                var argFlag = longNameArgs.First(f => !LongArg.Requirment(f.LongName));
                throw new ApplicationException($"long name should only contain lower or upper letter," +
                                               $" number, dash and underscore, but get '{argFlag.LongName}'");
            }
            if (shortNameArgs.Any(f => !ShortArg.Requirment(f.ShortName.Value)))
            {
                var argFlag = shortNameArgs.First(f => !ShortArg.Requirment(f.ShortName.Value));
                throw new ApplicationException($"short argument must and only have one lower or upper letter, but get: '{argFlag.ShortName.Value}'");
            }

            if (argFlags.Count > 1)
            {
                throw new ApplicationException("only support one flag for now");
            }
        }

        public ArgsParsingResult Parse(string[] args)
        {
            var parseResults = args.Select(Parse).ToList();
            
            if (!parseResults.All(a => a.ParseSuccess))
                throw new ApplicationException(parseResults.First(a => !a.ParseSuccess).ParseErrorReason);

            if (!parseResults.All(p => supportArgFlags.Any(s => p.Result.MatchArg(s))))
            {
                var failedParse = parseResults.First(p => !supportArgFlags.Any(s => p.Result.MatchArg(s))).Result;
                string failedArgument = "";
                switch (failedParse)
                {
                    case LongArg longArg:
                        failedArgument = $"--{longArg.Arg}";
                        break;
                    case ShortArg shortArg:
                        failedArgument = $"-{shortArg.Arg}";
                        break;
                }
                throw new ApplicationException("input argument are not supported: " + failedArgument);
            }

            return new ArgsParsingResult(true, parseResults.Select(x => x.Result).ToList(), supportArgFlags);
        }

        public static IParseResult<IInputArg> Parse(string arg)
        {
            if (arg.Length < 2)
                return new FailedParse<IInputArg>("argument too short");
            if (LongNamePrefix(arg))
            {
                string longName = arg.Substring(2);
                return LongArg.Parse(longName);
            }
            if (ShortNamePrefix(arg))
            {
                string shortName = arg.Substring(1);
                return ShortArg.Parse(shortName);
            }
            return new FailedParse<IInputArg>("argument must start with - or --");
        }

        public IEnumerable<string> HelpInfo()
        {
            return supportArgFlags.Select(af =>
                $"{af.ShortName?.ToString() ?? ""}    {af.LongName ?? ""}    {af.Description.Replace('\n', ' ')}");
        }
    }
}
