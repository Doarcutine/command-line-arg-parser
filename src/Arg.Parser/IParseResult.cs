using System;

namespace Arg.Parser
{
    public interface IParseResult<out T>
    {
        bool ParseSuccess { get; }
        T Result { get; }
        string ParseErrorReason { get; }
    }

    public class SuccessParse<T> : IParseResult<T>
    {
        public bool ParseSuccess => true;
        public T Result { get; }
        public string ParseErrorReason => null;

        public SuccessParse(T result)
        {
            this.Result = result;
        }
    }

    public class FailedParse<T> : IParseResult<T>
    {
        public bool ParseSuccess => false;

        public T Result => throw new NotImplementedException();

        public string ParseErrorReason { get; }

        public FailedParse(string reason)
        {
            this.ParseErrorReason = reason;
        }
    }
}