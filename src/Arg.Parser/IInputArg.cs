using System;
using System.Linq;
using static System.Char;

namespace Arg.Parser
{
    interface IInputArg
    {
        bool MatchArg(FlagOption c);
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

        public bool MatchArg(FlagOption c)
        {
            return c.AbbreviationForm.HasValue && ToLower(c.AbbreviationForm.Value) == ToLower(this.Arg);
        }

        public bool Value => true;
        
        public static IParseResult<AbbreviationFormArg> Parse(string input)
        {
            var arg = input.Substring(1);
            if (arg.Length != 1 || !Requirment(arg.First()))
                return new FailedParse<AbbreviationFormArg>("abbreviation form argument must and only have one " +
                                                 "lower or upper letter", input);
            return new SuccessParse<AbbreviationFormArg>(new AbbreviationFormArg(arg.First()), input);
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

        public bool MatchArg(FlagOption c)
        {
            return c.FullForm?.ToLower() == this.Arg.ToLower();
        }
        
        public bool Value => true;

        public static IParseResult<FullFormArg> Parse(string input)
        {
            string arg = input.Substring(2);
            if (!Requirment(arg))
                return new FailedParse<FullFormArg>("full form should only contain lower or upper letter," +
                                                " number, dash and underscore", input);
            return new SuccessParse<FullFormArg>(new FullFormArg(arg), input);
        }
    }
}
