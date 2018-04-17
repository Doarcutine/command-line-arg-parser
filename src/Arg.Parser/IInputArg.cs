using System;
using System.Collections.Generic;
using System.Linq;
using static System.Char;

namespace Arg.Parser
{
    interface IInputArg
    {
        bool MatchArg(IOptionSymbolMetadata c);
        bool Value { get; }
    }

    class AbbreviationFormArg : IInputArg
    {
        public static readonly Func<char,bool> Requirment = c => IsLower(c) || IsUpper(c);
        public char Arg { get; }

        private AbbreviationFormArg(char arg)
        {
            this.Arg = arg;
        }

        public bool MatchArg(IOptionSymbolMetadata c)
        {
            return c.Abbreviation.HasValue && ToLower(c.Abbreviation.Value) == ToLower(this.Arg);
        }

        public bool Value => true;
        
        public static IParseResult<IReadOnlyCollection<AbbreviationFormArg>> Parse(string input)
        {
            var args = input.Substring(1);
            if (args.Any(arg => !Requirment(arg)))
                return new FailedParse<IReadOnlyCollection<AbbreviationFormArg>>("abbreviation form argument must only contains lower or upper letter", input);
            return new SuccessParse<IReadOnlyCollection<AbbreviationFormArg>>(
                args.Select(a => new AbbreviationFormArg(a)).ToList(), input);
        }
    }

    class FullFormArg : IInputArg
    {
        public static readonly Func<string,bool> Requirment = arg => arg.First() != '-' &&
            arg.All(c => IsLetterOrDigit(c) || c == '-' || c == '_');
        public string Arg { get; }

        private FullFormArg(string arg)
        {
            this.Arg = arg;
        }

        public bool MatchArg(IOptionSymbolMetadata c)
        {
            return c.FullForm?.ToLower() == this.Arg.ToLower();
        }
        
        public bool Value => true;

        public static IParseResult<IReadOnlyCollection<FullFormArg>> Parse(string input)
        {
            string arg = input.Substring(2);
            if (!Requirment(arg))
                return new FailedParse<IReadOnlyCollection<FullFormArg>>("full form should only contain lower or upper letter," +
                                                " number, dash and underscore", input);
            return new SuccessParse<IReadOnlyCollection<FullFormArg>>(new List<FullFormArg>{new FullFormArg(arg)}, input);
        }
    }
}
