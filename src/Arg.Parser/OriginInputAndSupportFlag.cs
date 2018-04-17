namespace Arg.Parser
{
    class OriginInputAndSupportFlag
    {
        internal IOptionDefinitionMetadata Flag { get; }
        internal string OriginInput { get; }

        internal OriginInputAndSupportFlag(IOptionDefinitionMetadata flag, string originInput)
        {
            Flag = flag;
            OriginInput = originInput;
        }
    }
}