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
        public static readonly Func<char,bool> Requirment = c => char.IsLower(c) || char.IsUpper(c);
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
            if (arg.Length != 1 || !Requirment(arg.First()))
                return new FailedParse<ShortArg>($"short argument must and only have one " +
                                                 $"lower or upper letter, but get: '{arg}'");
            return new SuccessParse<ShortArg>(new ShortArg(arg.First()));
        }
    }

    public class LongArg : IInputArg
    {
        public static readonly Func<string,bool> Requirment = arg => arg.First() != '-' &&
            arg.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
        public string Arg { get; }

        private LongArg(string arg)
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
            if (!Requirment(arg))
                return new FailedParse<LongArg>($"long name should only contain lower or upper letter," +
                                                $" number, dash and underscore, but get '{arg}'");
            return new SuccessParse<LongArg>(new LongArg(arg));
        }
    }
}
