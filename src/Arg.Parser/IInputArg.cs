using System;
using System.Linq;

namespace Arg.Parser
{
    public interface IInputArg
    {
        bool MatchArg(ArgFlag c);
        bool Value { get; }
    }

    public class ShortArg : IInputArg
    {
        public static readonly Func<char,bool> Match = c => char.IsLower(c) || char.IsUpper(c);
        public char Arg { get; }

        private ShortArg(char arg)
        {
            this.Arg = arg;
        }

        public bool MatchArg(ArgFlag c)
        {
            return c.ShortName == this.Arg;
        }

        public bool Value => true;

        public static IParseResult<ShortArg> Parse(string arg)
        {
            if (arg.Length != 1 || !Match(arg.First()))
                return new FailedParse<ShortArg>("short argument must and only have one a-zA-Z character, but get: " + arg.First());
            return new SuccessParse<ShortArg>(new ShortArg(arg.First()));
        }
    }

    public class LongArg : IInputArg
    {
        public static readonly Func<string,bool> Match = arg => arg.First() != '-' &&
            arg.All(c => char.IsLower(c) || char.IsUpper(c) || char.IsDigit(c) || c == '-' || c == '_');
        public string Arg { get; }

        public LongArg(string arg)
        {
            this.Arg = arg;
        }

        public bool MatchArg(ArgFlag c)
        {
            return c.LongName == this.Arg;
        }
        
        public bool Value => true;

        public static IParseResult<LongArg> Parse(string arg)
        {
            if (!Match(arg))
                return new FailedParse<LongArg>("argument full name contain invalid character");
            return new SuccessParse<LongArg>(new LongArg(arg));
        }
    }
}
