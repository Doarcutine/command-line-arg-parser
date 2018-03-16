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

    class ShortArg : IInputArg
    {
        public static readonly Func<char,bool> Requirment = c => IsLower(c) || IsUpper(c);
        public char Arg { get; }

        private ShortArg(char arg)
        {
            this.Arg = arg;
        }

        public bool MatchArg(FlagOption c)
        {
            return c.ShortName.HasValue && ToLower(c.ShortName.Value) == ToLower(this.Arg);
        }

        public bool Value => true;
        
        public static IParseResult<ShortArg> Parse(string input)
        {
            var arg = input.Substring(1);
            if (arg.Length != 1 || !Requirment(arg.First()))
                return new FailedParse<ShortArg>("short argument must and only have one " +
                                                 "lower or upper letter", input);
            return new SuccessParse<ShortArg>(new ShortArg(arg.First()), input);
        }
    }

    class LongArg : IInputArg
    {
        public static readonly Func<string,bool> Requirment = arg => arg.First() != '-' &&
            arg.All(c => IsLetterOrDigit(c) || c == '-' || c == '_');
        public string Arg { get; }

        private LongArg(string arg)
        {
            this.Arg = arg;
        }

        public bool MatchArg(FlagOption c)
        {
            return c.LongName?.ToLower() == this.Arg.ToLower();
        }
        
        public bool Value => true;

        public static IParseResult<LongArg> Parse(string input)
        {
            string arg = input.Substring(2);
            if (!Requirment(arg))
                return new FailedParse<LongArg>("long name should only contain lower or upper letter," +
                                                " number, dash and underscore", input);
            return new SuccessParse<LongArg>(new LongArg(arg), input);
        }
    }
}
