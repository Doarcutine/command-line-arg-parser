namespace Arg.Parser
{
    class OriginInputAndParsedArg
    {
        internal IInputArg Arg { get; }
        internal string OriginInput { get; }

        internal OriginInputAndParsedArg(IInputArg arg, string originInput)
        {
            Arg = arg;
            OriginInput = originInput;
        }
    }
}