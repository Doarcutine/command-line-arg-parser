namespace Arg.Parser
{
    class OriginInputAndSupportFlag
    {
        internal FlagOption Flag { get; }
        internal string OriginInput { get; }

        internal OriginInputAndSupportFlag(FlagOption flag, string originInput)
        {
            Flag = flag;
            OriginInput = originInput;
        }
    }
}