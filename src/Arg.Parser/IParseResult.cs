using System;

namespace Arg.Parser
{
    interface IParseResult<out T>
    {
        bool ParseSuccess { get; }
        T Result { get; }
        string ParseErrorReason { get; }
        string OriginInput { get; }
    }

    class SuccessParse<T> : IParseResult<T>
    {
        public bool ParseSuccess => true;
        public T Result { get; }
        public string ParseErrorReason => null;
        public string OriginInput { get; }

        public SuccessParse(T result, string originInput)
        {
            this.Result = result;
            this.OriginInput = originInput;
        }
    }

    class FailedParse<T> : IParseResult<T>
    {
        public bool ParseSuccess => false;

        public T Result => throw new NotImplementedException();

        public string ParseErrorReason { get; }
        public string OriginInput { get; }

        public FailedParse(string reason, string originInput)
        {
            this.ParseErrorReason = reason;
            this.OriginInput = originInput;
        }
    }
}
