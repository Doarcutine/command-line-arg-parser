using System.Collections.Generic;
using System;
using System.Linq;

namespace Arg.Parser
{
    /// <summary>
    /// build from ArgParserBuilder,
    /// used for parse command line input flag argument
    /// </summary>
    public class Parser
    {
        // ReSharper disable once InconsistentNaming
        private readonly IReadOnlyCollection<FlagOption> supportArgFlags;
        private static readonly Func<string,bool> FullFormPrefix = arg => arg[0] == '-' && arg[1] == '-';
        private static readonly Func<string,bool> abbreviationFormPrefix = arg => arg[0] == '-';

        internal Parser(IReadOnlyCollection<FlagOption> supportArgFlags)
        {
            ValidateSupportFlag(supportArgFlags);
            this.supportArgFlags = supportArgFlags;
        }

        private static void ValidateSupportFlag(IReadOnlyCollection<FlagOption> argFlags)
        {
            if (argFlags.Any(f => string.IsNullOrEmpty(f.FullForm) && f.AbbreviationForm == null))
            {
                throw new ArgumentException("arg opt must have at least one form, full form or abbreviation form");
            }
            var fullFormArgs = argFlags.Where(f => !string.IsNullOrEmpty(f.FullForm)).ToList();
            var abbreviationFormArgs = argFlags.Where(f => f.AbbreviationForm.HasValue).ToList();

            if (fullFormArgs.Any(f => !FullFormArg.Requirment(f.FullForm)))
            {
                var argFlag = fullFormArgs.First(f => !FullFormArg.Requirment(f.FullForm));
                throw new ArgumentException("full form should only contain lower or upper letter," +
                                               $" number, dash and underscore, but get '{argFlag.FullForm}'");
            }
            if (abbreviationFormArgs.Any(f => f.AbbreviationForm.HasValue && !AbbreviationFormArg.Requirment(f.AbbreviationForm.Value)))
            {
                var argFlag = abbreviationFormArgs.First(f => f.AbbreviationForm.HasValue && !AbbreviationFormArg.Requirment(f.AbbreviationForm.Value));
                throw new ArgumentException($"abbreviation argument must and only have one lower or upper letter, but get: '{argFlag.AbbreviationForm}'");
            }

            if (argFlags.Where(f => f.AbbreviationForm.HasValue).GroupBy(f => f.AbbreviationForm.HasValue ? char.ToLower(f.AbbreviationForm.Value) : (char?) null).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("duplicate abbreviation name");
            }
            
            if (argFlags.Where(f => !string.IsNullOrEmpty(f.FullForm)).GroupBy(f => f.FullForm.ToLower()).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("duplicate full form");
            }
        }

        /// <summary>
        /// parse and match with supported flag option
        /// </summary>
        /// <param name="args">command line input args, like -f --version</param>
        /// <returns>parser result, maybe success or failed, check IsSuccess,
        ///          use GetFlagValue to get user input value if success,
        ///          field Error contains failed detail
        /// </returns>
        public ArgsParsingResult Parse(string[] args)
        {
            var parseResults = args.Select(Parse).ToList();

            if (parseResults.Any(a => !a.ParseSuccess))
            {
                var failedParse = parseResults.First(a => !a.ParseSuccess);
                return new ArgsParsingResult(
                    new Error(ParsingErrorCode.FlagSyntaxError, failedParse.ParseErrorReason, failedParse.OriginInput));
            }

            if (!parseResults.All(p => supportArgFlags.Any(s => p.Result.MatchArg(s))))
            {
                var notMatchArg = parseResults.First(p => !supportArgFlags.Any(s => p.Result.MatchArg(s)));
                return new ArgsParsingResult(new Error(ParsingErrorCode.NotSupportedFlag,
                    "input argument is not supported", notMatchArg.OriginInput));
            }

            return new ArgsParsingResult(parseResults.Select(x => x.Result).ToList(), supportArgFlags);
        }

        internal static IParseResult<IInputArg> Parse(string arg)
        {
            if (arg.Length < 2)
                return new FailedParse<IInputArg>("argument too short", arg);
            if (FullFormPrefix(arg))
            {
                return FullFormArg.Parse(arg);
            }
            if (abbreviationFormPrefix(arg))
            {
                return AbbreviationFormArg.Parse(arg);
            }
            return new FailedParse<IInputArg>("argument must start with - or --", arg);
        }

        /// <summary>
        /// get support flag help description
        /// </summary>
        /// <returns>help description</returns>
        public IEnumerable<string> HelpInfo()
        {
            return supportArgFlags.Select(af =>
                $"{af.AbbreviationForm?.ToString() ?? ""}    {af.FullForm ?? ""}    {af.Description.Replace('\n', ' ')}");
        }
    }
}
