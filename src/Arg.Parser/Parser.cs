using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
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
        private readonly ReadOnlyCollection<ICommandDefinitionMetadata> commandDefinitions;
        private static readonly Func<string,bool> FullFormPrefix = arg => arg[0] == '-' && arg[1] == '-';
        private static readonly Func<string,bool> AbbreviationFormPrefix = arg => arg[0] == '-';

        internal Parser(ReadOnlyCollection<ICommandDefinitionMetadata> commandDefinitions)
        {
            foreach (var supportArgFlags in commandDefinitions)
            {
                ValidateSupportFlag(new ReadOnlyCollection<IOptionDefinitionMetadata>(supportArgFlags.GetRegisteredOptionsMetadata().ToList()));
            }
            this.commandDefinitions = commandDefinitions;
        }

        private static void ValidateSupportFlag(IReadOnlyCollection<IOptionDefinitionMetadata> argFlags)
        {
            if (argFlags.Any(f => string.IsNullOrEmpty(f.SymbolMetadata.FullForm) && f.SymbolMetadata.Abbreviation == null))
            {
                throw new ArgumentException("arg opt must have at least one form, full form or abbreviation form");
            }
            var fullFormArgs = argFlags.Where(f => !string.IsNullOrEmpty(f.SymbolMetadata.FullForm)).ToList();
            var abbreviationFormArgs = argFlags.Where(f => f.SymbolMetadata.Abbreviation.HasValue).ToList();

            if (fullFormArgs.Any(f => !FullFormArg.Requirment(f.SymbolMetadata.FullForm)))
            {
                var argFlag = fullFormArgs.First(f => !FullFormArg.Requirment(f.SymbolMetadata.FullForm));
                throw new ArgumentException("full form should only contain lower or upper letter," +
                                               $" number, dash and underscore, but get '{argFlag.SymbolMetadata.FullForm}'");
            }
            if (abbreviationFormArgs.Any(f => f.SymbolMetadata.Abbreviation.HasValue && !AbbreviationFormArg.Requirment(f.SymbolMetadata.Abbreviation.Value)))
            {
                var argFlag = abbreviationFormArgs.First(f => f.SymbolMetadata.Abbreviation.HasValue && !AbbreviationFormArg.Requirment(f.SymbolMetadata.Abbreviation.Value));
                throw new ArgumentException($"abbreviation argument must and only have one lower or upper letter, but get: '{argFlag.SymbolMetadata.Abbreviation}'");
            }

            if (argFlags.Where(f => f.SymbolMetadata.Abbreviation.HasValue).GroupBy(f => f.SymbolMetadata.Abbreviation.HasValue ? char.ToLower(f.SymbolMetadata.Abbreviation.Value) : (char?) null).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("duplicate abbreviation name");
            }
            
            if (argFlags.Where(f => !string.IsNullOrEmpty(f.SymbolMetadata.FullForm)).GroupBy(f => f.SymbolMetadata.FullForm.ToLower()).Any(g => g.Count() > 1))
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
            if (args == null)
                throw new ArgumentNullException();
            
            if (args.Any(a => a == null))
                throw new ArgumentException();
            
            var parseResults = args.Select(Parse).ToList();

            if (parseResults.Any(a => !a.ParseSuccess))
            {
                var failedParse = parseResults.First(a => !a.ParseSuccess);
                return new ArgsParsingResult(
                    new Error(ParsingErrorCode.FreeValueNotSupported, failedParse.ParseErrorReason, failedParse.OriginInput));
            }

            ICommandDefinitionMetadata command = commandDefinitions.Single();
            var supportArgFlags = command.GetRegisteredOptionsMetadata();
            
            var parsedFlags = parseResults.SelectMany(p => p.Result.Select(a => new OriginInputAndParsedArg(a, p.OriginInput))).ToList();

            if (!parsedFlags.All(p => supportArgFlags.Any(s => p.Arg.MatchArg(s.SymbolMetadata))))
            {
                var notMatchArg = parsedFlags.First(p => !supportArgFlags.Any(s => p.Arg.MatchArg(s.SymbolMetadata)));
                return new ArgsParsingResult(new Error(ParsingErrorCode.FreeValueNotSupported,
                    "input argument is not supported", notMatchArg.OriginInput));
            }

            var duplicateFlagInInput = parsedFlags.Select(p => new OriginInputAndSupportFlag(supportArgFlags.Single(s => p.Arg.MatchArg(s.SymbolMetadata)), p.OriginInput))
                .GroupBy(s => s.Flag).Where(g => g.Count() > 1).ToList();
            if (duplicateFlagInInput.Any())
            {
                var trigger = duplicateFlagInInput.First().Select(x => x.OriginInput).Aggregate((acc, x) => acc + " " + x);
                return new ArgsParsingResult(new Error(ParsingErrorCode.DuplicateFlagsInArgs,
                    "duplicate flag option in input arguments", trigger));
            }

            return new ArgsParsingResult(command, parsedFlags.Select(x => x.Arg).ToList());
        }

        internal static IParseResult<IReadOnlyCollection<IInputArg>> Parse(string arg)
        {
            if (arg.Length < 2)
                return new FailedParse<IReadOnlyCollection<IInputArg>>("argument too short", arg);
            if (FullFormPrefix(arg))
            {
                return FullFormArg.Parse(arg);
            }
            if (AbbreviationFormPrefix(arg))
            {
                return AbbreviationFormArg.Parse(arg);
            }
            return new FailedParse<IReadOnlyCollection<IInputArg>>("argument must start with - or --", arg);
        }

        /// <summary>
        /// get support flag help description
        /// </summary>
        /// <returns>help description</returns>
        public IEnumerable<string> HelpInfo()
        {
            return commandDefinitions.Single().GetRegisteredOptionsMetadata().Select(af =>
                $"{af.SymbolMetadata.Abbreviation?.ToString() ?? ""}    {af.SymbolMetadata.FullForm ?? ""}    {af.Description.Replace('\n', ' ')}");
        }
    }
}
